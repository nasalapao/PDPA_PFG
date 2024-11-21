using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private string  taxid = "";
        private string Filename = "";
       

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

                    taxid = Request.QueryString["pdtaxid"];
                    bool hasUploadedFile = GetFileNameUploaded(taxid);

                    if (hasUploadedFile)
                    {
                        lblMessage.Text = "Uploaded file: " + Filename;
                        btnDelete.Visible = true;
                        btnView.Visible = true;
                    }
                    else
                    {
                        btnDelete.Visible = false;
                        btnView.Visible = false;
                    }
                   ///dteddd
                    
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

        private void checkfileupload(){
            try
            {
                sql.Clear();
                sql.Append(string.Format("SELECT  PDAFILE FROM ITPROD.PDPAFILE WHERE PDTAXID ='{0}' " , taxid));
                sql.Append("AND PDAFILE IS NOT NULL AND PDAFILE <> ''");

                DataTable result = db.ExecuteDb2Query(sql.ToString());

                if (db.isError == false && db.isHasRow)
                {
                    btnDelete_Click(null, EventArgs.Empty);
                }

                
            }
            catch (Exception ex)
            {
                ShowError("checkfileupload: " + ex.Message);
               
            }

           
         }


        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            //เช็คก่อนว่ามี upload มาหรือยัง หากมีแล้วให้ลบก่อน 


            checkfileupload();

            if (FileUpload1.HasFile)
            {
                string fileName = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
                // string filePath = Server.MapPath("~/Uploads/" + fileName);
                string fileaddr = db.SqlDateYYYYMMDDHHMMSS(DateTime.Now)+ "-" + fileName;

                // แยกชื่อไฟล์และนามสกุลไฟล์
                string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileaddr);
                string extension = Path.GetExtension(fileaddr);

                // ลบอักขระต้องห้ามรวมทั้งจุด ยกเว้นตัวอักษร ตัวเลข และขีดกลาง
                string modifiedName = Regex.Replace(nameWithoutExtension, @"[^\w\-]", "");

                // รวมชื่อไฟล์ที่ถูกแก้ไขกับนามสกุลไฟล์
                string result = modifiedName + extension;

                string filePath = ("\\\\172.16.33.37\\PDPA_Document\\" + result);

                try
                {
                    if (updateDb2File(result, taxid))
                    {
                         FileUpload1.SaveAs(filePath);
                         ShowError ("ไฟล์ถูกอัปโหลดสำเร็จ: " + fileName);
                         btnDelete.Visible = true;
                         btnView.Visible = true;
                    }
                    else
                    {
                        btnDelete.Visible = false;
                        btnView.Visible = false;
                    }
                   
                }
                catch (Exception ex)
                {
                    ShowError ("เกิดข้อผิดพลาดในการอัปโหลด: " + ex.Message);
                }
            }
            else
            {
                ShowError ("กรุณาเลือกไฟล์ก่อนอัปโหลด");
            }
        }


        protected bool updateDb2File(string fileaddr, string taxid)
        {
            try
            {
                string sql = string.Format("UPDATE itprod.pdpafile SET PDAFILE= '{0}' WHERE  PDTAXID =  '{1}' ", fileaddr, taxid);


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

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            bool hasUploadedFile = GetFileNameUploaded(taxid);

            if (hasUploadedFile == false)
            {
                return;
            }

            string filePath = ("\\\\172.16.33.37\\PDPA_Document\\" + Filename);

            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    ShowError("File deleted successfully.");
                    btnDelete.Visible = false; // ซ่อนปุ่ม Delete หลังจากลบไฟล์
                    btnView.Visible = false;
                    updateDb2File("",taxid);
                }
                else
                {
                    ShowError("File not found.");
                }
            }
            catch (Exception ex)
            {
                ShowError("Error deleting file: " + ex.Message);
            }
        }//end

        private bool GetFileNameUploaded(string taxid)
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

        protected void btnView_Click(object sender, EventArgs e)
        {
            bool hasUploadedFile = GetFileNameUploaded(taxid);

            if (hasUploadedFile == false)
            {
                return;
            }

            string filePath = ("\\\\172.16.33.37\\PDPA_Document\\" + Filename);

            try
            {
               
                string fileUrl = "DownloadFile.aspx?filepath=" + Filename;

                string script = "window.open('" + fileUrl + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "OpenFile", script, true);
            }
            catch (Exception ex)
            {
                ShowError("Log error: " + ex.Message);
            }

        }



    }
}