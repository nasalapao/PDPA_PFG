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
    public partial class EditData : System.Web.UI.Page
    {
        private dbConnection db = new dbConnection();
        StringBuilder sql = new StringBuilder();

        string Filename = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack)
            {
                return;
            }


            string action = "";
            string taxid = "";
            string firstName = "";
            string address = "";
            string phoneNumber = "";
            string dataSource = "";
            string birthDate = "";
            string gender = "";
            string currentNation = "";
            string DateN = "";
            string TimeN = "";


            if (!User.Identity.IsAuthenticated)
            {
                // ถ้าผู้ใช้ยังไม่ล็อกอิน ให้เปลี่ยนเส้นทางไปยังหน้า Login.aspx
                Response.Redirect("Login.aspx");
            }
            else
            {
                if (Request.HttpMethod == "POST")
                {
                    // Retrieve form values sent via POST
                    action = Request.Form["action"];
                    taxid = Request.Form["taxid"];
                    firstName = Request.Form["first_name"];
                    address = Request.Form["addr"];
                    phoneNumber = Request.Form["phonenumber"];
                    dataSource = Request.Form["pdsource"];
                    birthDate = Request.Form["bddt"];
                    gender = Request.Form["sexid"];
                    currentNation = Request.Form["currentNation"];
                    DateN = Request.Form["DateN"];
                    TimeN = Request.Form["TimeN"];

                    if (action == "edit")
                    {
                        bool isSaved = EditDataInDatabase(taxid, firstName, address, phoneNumber, dataSource, birthDate, gender, currentNation, DateN, TimeN);


                        if (isSaved)
                        {
                            lblResult.Text = "<span style='color: green;'>Data has been edited successfully! Redirecting in <span id='countdown'>5</span> seconds...</span>";
                            string script = @"
                                                    <script type='text/javascript'>
                                                        var countdown = 5;
                                                        var countdownElement = document.getElementById('countdown');
                                                        var interval = setInterval(function() {
                                                            countdown--;
                                                            countdownElement.textContent = countdown;
                                                            if (countdown <= 0) {
                                                                clearInterval(interval);
                                                                window.location.href = 'View.aspx';
                                                            }
                                                        }, 1000);
                                                    </script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "CountdownRedirect", script);
                        }
                        else
                        {
                            lblResult.Text = "<span style='color: red;' > An error occurred while saving data.";
                        }
                    }
                    else if (action == "delete")
                    {
                        //เช็คก่อนว่ามีไฟล์ upload ไหม ให้ลบออกก่อน 

                        deleltefile(taxid);


                        string sql = string.Format("DELETE itprod.pdpafile WHERE PDTAXID =  '{0}'", taxid);
                        db.ExecuteDb2Query(sql);

                        if (db.isError == false)
                        {
                            lblResult.Text = "<span style='color: green;'>Data has been Deleted successfully! Redirecting in <span id='countdown'>5</span> seconds...</span>";
                            string script = @"
                                                    <script type='text/javascript'>
                                                        var countdown = 5;
                                                        var countdownElement = document.getElementById('countdown');
                                                        var interval = setInterval(function() {
                                                            countdown--;
                                                            countdownElement.textContent = countdown;
                                                            if (countdown <= 0) {
                                                                clearInterval(interval);
                                                                window.location.href = 'View.aspx';
                                                            }
                                                        }, 1000);
                                                    </script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "CountdownRedirect", script);
                        }
                    }//end if 

                }
            }
        }

        private void deleltefile(string taxid)
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
                    Console.WriteLine("File deleted successfully.");
                    
                }
                else
                {
                    Console.WriteLine("File not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting file: " + ex.Message);
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
                Console.WriteLine("Log error: " + ex.Message);
                return false;
            }

        }//end

        private bool EditDataInDatabase(string taxid, string firstName, string address, string phoneNumber, string dataSource, string birthDate, string gender, string currentNation, string DateN, string TimeN)
        {
            try
            {
                // สร้างรายการพารามิเตอร์ที่ต้องการใช้
                var parameters = new[]
                    {
                        new OleDbParameter("@firstName", OleDbType.VarChar) { Value = firstName },
                        new OleDbParameter("@address", OleDbType.VarChar) { Value = address },
                        new OleDbParameter("@phoneNumber", OleDbType.VarChar) { Value = phoneNumber },
                        new OleDbParameter("@dataSource", OleDbType.VarChar) { Value = dataSource },
                        new OleDbParameter("@birthDate", OleDbType.VarChar) { Value = birthDate },
                        new OleDbParameter("@gender", OleDbType.VarChar) { Value = gender },
                        new OleDbParameter("@currentNation", OleDbType.VarChar) { Value = currentNation },
                        new OleDbParameter("@DateN", OleDbType.VarChar) { Value = DateN },
                        new OleDbParameter("@TimeN", OleDbType.VarChar) { Value = TimeN },
                        new OleDbParameter("@pdaccdt", OleDbType.VarChar) { Value = 0 },
                        new OleDbParameter("@taxid", OleDbType.VarChar) { Value = taxid } // ใช้ taxid เป็นเงื่อนไขในการระบุข้อมูลที่จะอัปเดต
                    };

                // สร้างคำสั่ง SQL สำหรับอัปเดตข้อมูล
                var sql = new StringBuilder();
                sql.Append("UPDATE itprod.pdpafile ");
                sql.Append("SET pdtaxname = ?, pdtaxaddr = ?, pdphone = ?, pdsource = ?, pdbddt = ?, pdsexid = ?, pdnation = ?, pdrgdt = ?, pdrgtm = ?, pdaccdt = ? ");
                sql.Append("WHERE pdtaxid = ?");

                // เรียกใช้คำสั่งเพื่ออัปเดตข้อมูลลงในฐานข้อมูล
                int result = db.ExecuteDb2NonQuery(sql.ToString(), parameters);

                // ตรวจสอบผลลัพธ์ของการอัปเดตข้อมูล
                return result > 0; // คืนค่า true หากอัปเดตสำเร็จ
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // คืนค่า false หากเกิดข้อผิดพลาด
            }
        }


    }
}