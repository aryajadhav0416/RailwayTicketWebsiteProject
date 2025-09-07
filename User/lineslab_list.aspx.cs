using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class lineslab_list : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    BindLineSlabList();
                }
                else
                {
                    Response.Redirect("userloginpg.aspx");
                }
            }
        }

        private void BindLineSlabList()
        {
            DataTable lineSlabs = GetLineSlabs();
            gvLineSlabList.DataSource = lineSlabs;
            gvLineSlabList.DataBind();
        }

        private DataTable GetLineSlabs()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Line"; // Replace with your actual query
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

        protected void gvLineSlabList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLineSlabList.PageIndex = e.NewPageIndex;
            BindLineSlabList();
        }

        protected void gvLineSlabList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("EditDetails"))
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvLineSlabList.Rows[rowIndex];

                string lineSlabId = ((Label)row.FindControl("LineID")).Text;
                int lineSlabIdValue;
                if (int.TryParse(lineSlabId, out lineSlabIdValue))
                {
                    Session["SelectedLineId"] = lineSlabIdValue;
                }
                Response.Redirect($"edit_lineslab.aspx?LineID={lineSlabId}");
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("admindashboard.aspx");
        }
        protected void btnAddNewRoute_Click(object sender, EventArgs e)
        {
            Response.Redirect("add_lineslab.aspx");
        }
    }
}
