using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFormApp
{
    public partial class Requrest : System.Web.UI.Page
    {
        public string FnameE = "", Userid = "";
        private dbConnection db = new dbConnection();
        StringBuilder sql = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowData();
            }


            if (!User.Identity.IsAuthenticated)
            {
                // ถ้าผู้ใช้ยังไม่ล็อกอิน ให้เปลี่ยนเส้นทางไปยังหน้า Login.aspx
                toLogin();
            }
            else
            {
                if (Session["FnameE"] != null)
                {
                    FnameE = Session["FnameE"].ToString();
                    Userid = Session["Userid"].ToString();
                    // Response.Write(valueFromSession);
                }
                else
                {
                    toLogin();
                }
            }


        }//class

        private void toLogin()
        {
            // ถ้าผู้ใช้ยังไม่ล็อกอิน ให้เปลี่ยนเส้นทางไปยังหน้า Login.aspx
            FnameE = "";
            Userid = "";
            Response.Redirect("Login.aspx");
        }


        private void ShowData()
        {

            sql.Clear();
            sql.Append(" SELECT RETAXID AS PDTAXID,PDTAXNAME ,PDTAXADDR ,REDATE  , RESTATUS ");
            sql.Append(" FROM ITPROD.PDPAFILE A RIGHT JOIN  ITPROD.PDPAREQUREST B ");
            sql.Append(" ON	a.PDTAXID  = b.RETAXID ");
            //sql.Append(string.Format(" WHERE RETAXID='{0}' ", 0));
            sql.Append(" ORDER  BY REDATE DESC  ");

            DataTable result = db.ExecuteDb2Query(sql.ToString());

            try
            {
                GridView1.DataSource = result;
                GridView1.DataBind();
                ShowError("");
            }

            catch (Exception ex)
            {
                ShowError("Log error: " + ex.Message);
            }






        }


        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            ShowData();
        }//class

        private void ShowError(string message)
        {
            if (message == "")
            {
                lblMessage.Visible = false;
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = message;
            }

        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            ShowData();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {


            try
            {
                string retaxid = GridView1.DataKeys[e.RowIndex].Value.ToString();  //ต้องไปเพิ่ม DataKeys ที่ หัว gridview ด้วยน่ะ

                GridViewRow row = GridView1.Rows[e.RowIndex];
                DropDownList ddlStatus = (DropDownList)row.FindControl("DropDownListSTATUS");

                if (ddlStatus == null)
                {
                    lblMessage.Text = "DropDownListSTATUS ไม่พบในแถวที่แก้ไข";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }


                string newStatus = ddlStatus.SelectedValue;
                string sqlUpdate = "UPDATE ITPROD.PDPAREQUREST SET RESTATUS = ? WHERE RETAXID = ?";
                var parameters = new[]
                {
                    new System.Data.OleDb.OleDbParameter("@RESTATUS", OleDbType.SmallInt) { Value = newStatus },
                    new System.Data.OleDb.OleDbParameter("@RETAXID", OleDbType.VarChar) { Value = retaxid }
                };


                db.ExecuteDb2NonQuery(sqlUpdate, parameters);


                GridView1.EditIndex = -1;

                ShowData();

                ShowError("อัปเดตข้อมูลสำเร็จ!");

            }
            catch (Exception ex)
            {
                ShowError("อัปเดตข้อมูลสำเร็จ!");
            }

        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            ShowData();
        }//class


    }
}