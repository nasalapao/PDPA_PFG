using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFormApp
{
    public partial class UploadDoc : System.Web.UI.Page
    {
        private dbConnection db = new dbConnection();
        StringBuilder sql = new StringBuilder();

        public string FnameE = "", Userid = "";

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
                else
                {
                    toLogin();
                }
            }


        }

        private void toLogin()
        {
            // ถ้าผู้ใช้ยังไม่ล็อกอิน ให้เปลี่ยนเส้นทางไปยังหน้า Login.aspx
            FnameE = "";
            Userid = "";
            Response.Redirect("Login.aspx");
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string fileName = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
                // string filePath = Server.MapPath("~/Uploads/" + fileName);
                string fileaddr = db.SqlDateYYYYMMDDHHMMSS(DateTime.Now)+ "-" + fileName;

                string filePath = ("\\\\172.16.33.37\\PDPA_Document\\" + fileaddr);

                try
                {
                    if (updateDb2File(fileaddr))
                    {
                         FileUpload1.SaveAs(filePath);
                         lblMessage.Text = "ไฟล์ถูกอัปโหลดสำเร็จ: " + fileName;
                    }
                   
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "เกิดข้อผิดพลาดในการอัปโหลด: " + ex.Message;
                }
            }
            else
            {
                lblMessage.Text = "กรุณาเลือกไฟล์ก่อนอัปโหลด";
            }
        }


        protected bool updateDb2File(string fileaddr)
        {
            try
            {
                string sql = string.Format("UPDATE itprod.pdpafile SET PDAFILE= '{0}' WHERE  PDTAXID =  '1319900087050' ", fileaddr);


                db.ExecuteDb2Query(sql);

                if (db.isError == false)
                {
                    return true;
                }


                return false;
            }
            catch (Exception ex)
            {
                ShowError("Log error: " + ex.Message);
                return false;
            }
        }//end 

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

        }//end


    }
}