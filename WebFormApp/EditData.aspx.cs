using System;
using System.Collections.Generic;
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