using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class fareslab_list : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    BindFareSlabList();
                }
                else
                {
                    Response.Redirect("userloginpg.aspx");
                }
            }
        }
        protected void btnAddNewRoute_Click(object sender, EventArgs e)
        {
            Response.Redirect("add_fareslab.aspx");
        }
        private void BindFareSlabList()
        {
            DataTable fareSlabs = GetFareSlabs();
            gvFareSlabList.DataSource = fareSlabs;
            gvFareSlabList.DataBind();
        }

        private DataTable GetFareSlabs()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM FareSlab";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        protected void gvFareSlabList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFareSlabList.PageIndex = e.NewPageIndex;
            BindFareSlabList();
        }

        protected void gvFareSlabList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteDetails"))
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvFareSlabList.Rows[rowIndex];

                string fareSlabId = ((Label)row.FindControl("FareSlabID")).Text;
                int fareSlabIdValue;
                if (int.TryParse(fareSlabId, out fareSlabIdValue))
                {
                    DeleteFareSlab(fareSlabIdValue);
                    BindFareSlabList(); // Refresh the grid after deletion
                }
            }
            if (e.CommandName.Equals("EditDetails"))
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvFareSlabList.Rows[rowIndex];

                string fareSlabId = ((Label)row.FindControl("FareSlabID")).Text;
                Session["FareSlabID"] = fareSlabId;
                Response.Redirect("edit_fareslab.aspx");
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("admindashboard.aspx");
        }
        private void DeleteFareSlab(int fareSlabId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM FareSlab WHERE FareSlabID = @FareSlabID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FareSlabID", fareSlabId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
