using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFormApp
{
    public partial class View : System.Web.UI.Page
    {
        public string FnameE = "", Userid = "";
        private string Filename = "";
        private dbConnection db = new dbConnection();
        StringBuilder sql = new StringBuilder();


        protected void Page_Load(object sender, EventArgs e)
        {
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
            }


            ShowData();





        }//end

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
            sql.Append("SELECT pdtaxid,pdrfcode,pdtaxname,pdtaxaddr,pdphone  ");
            sql.Append(",pdsource ,pdbddt ,pdsexid ,pdnation ,pdrgdt ,pdrgtm ");
            sql.Append(",pdaccdt FROM itprod.pdpafile  ");
            sql.Append(string.Format("WHERE PDRGDT IS NOT NULL ", 0));
            sql.Append("ORDER BY PDRGDT DESC , PDRGTM desc");

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

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }//end

     

        
        private bool CheckIfFileUploaded(string taxid)
        {

            try
            {
                string sql = string.Format("SELECT PDAFILE FROM itprod.pdpafile WHERE PDTAXID = '{0}'", taxid);
                DataTable result = db.ExecuteDb2Query(sql);
                if (db.isError == false || db.isHasRow)
                {

                    Filename = result.Rows[0]["PDAFILE"].ToString();
                    if (string.IsNullOrEmpty(Filename))
                    {
                        return false;
                    }
                    return true; // มีไฟล์อัปโหลดแล้ว
                }

                return false;
            }
            catch (Exception ex)
            {
                ShowError("Log error: " + ex.Message);
                return false;
            }

        }

        // ฟังก์ชันที่ให้ URL ของไฟล์ที่อัปโหลดแล้ว
        private string GetUploadedFileUrl()
        {
           // return   ("\\\\172.16.33.37\\PDPA_Document\\" + Filename); // เปลี่ยนเป็น URL ของไฟล์จริง

           // return "http://172.16.33.37/PDPA_Document/" + Filename;

            return "DownloadFile.aspx?filepath=" + Filename;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string taxid = DataBinder.Eval(e.Row.DataItem, "pdtaxid").ToString();
                bool hasUploadedFile = CheckIfFileUploaded(taxid); // ฟังก์ชันที่ตรวจสอบว่ามีไฟล์หรือไม่

                HyperLink uploadLink = (HyperLink)e.Row.FindControl("HyperLink2");
                HyperLink editfile = (HyperLink)e.Row.FindControl("HyperLink3");

                if (hasUploadedFile)
                {
                    uploadLink.Text = "View";
                    uploadLink.NavigateUrl = GetUploadedFileUrl(); // ฟังก์ชันที่ให้ URL ของไฟล์
                    uploadLink.Target = "_blank";
                    editfile.Text = "Edit";
                    editfile.NavigateUrl = "UploadDoc.aspx?pdtaxid=" + taxid;
                   
                }
                else
                {
                    uploadLink.Text = "Upload";
                    uploadLink.NavigateUrl = "UploadDoc.aspx?pdtaxid=" + taxid;
                    editfile.Visible = false;
                   
                }

                uploadLink.Visible = true;
            }
        }





    }//class
}//namespa