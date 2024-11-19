using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.OleDb;

namespace WebFormApp
{
    public partial class SaveData : System.Web.UI.Page
    {
        private dbConnection db = new dbConnection();
        StringBuilder sql = new StringBuilder();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // ถ้าผู้ใช้ยังไม่ล็อกอิน ให้เปลี่ยนเส้นทางไปยังหน้า Login.aspx
                Response.Redirect("Login.aspx");
            }


            if (Request.HttpMethod == "POST")
            {
                // Retrieve form values sent via POST
                string taxid = Request.Form["taxid"];
                string Userid = Request.Form["Userid"];
                string firstName = Request.Form["first_name"];
                string address = Request.Form["addr"];
                string phoneNumber = Request.Form["phonenumber"];
                string dataSource = Request.Form["pdsource"];
                string birthDate = Request.Form["bddt"];
                string gender = Request.Form["sexid"];
                string currentNation = Request.Form["currentNation"];
                string DateN = Request.Form["DateN"];
                string TimeN = Request.Form["TimeN"];


                       


                // Process and save the data (e.g., insert into a database)
                bool isSaved = SaveDataToDatabase(taxid, firstName, address, phoneNumber, dataSource, birthDate, gender, currentNation, DateN, TimeN, Userid);

                // Display the result message
                if (isSaved)
                {
                    lblResult.Text = "<span style='color: green;'>Data has been saved successfully! Redirecting in <span id='countdown'>5</span> seconds...</span>";
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
        }//end 


        private bool SaveDataToDatabase(string taxid, string firstName, string address, string phoneNumber, string dataSource, string birthDate, string gender, string currentNation, string DateN, string TimeN, string Userid)
        {
            try
            {
                // สร้างรายการพารามิเตอร์ที่ต้องการใช้
                var parameters = new[]
                {
                    new OleDbParameter("@taxid", OleDbType.VarChar) { Value = taxid },
                    new OleDbParameter("@Userid", OleDbType.VarChar) { Value = Userid },
                    new OleDbParameter("@firstName", OleDbType.VarChar) { Value = firstName },
                    new OleDbParameter("@address", OleDbType.VarChar) { Value = address },
                    new OleDbParameter("@phoneNumber", OleDbType.VarChar) { Value = phoneNumber },
                    new OleDbParameter("@dataSource", OleDbType.VarChar) { Value = dataSource },
                    new OleDbParameter("@birthDate", OleDbType.VarChar) { Value = birthDate },
                    new OleDbParameter("@gender", OleDbType.VarChar) { Value = gender },
                    new OleDbParameter("@currentNation", OleDbType.VarChar) { Value = currentNation },
                    new OleDbParameter("@DateN", OleDbType.VarChar) { Value = DateN },
                    new OleDbParameter("@TimeN", OleDbType.VarChar) { Value = TimeN },
                    new OleDbParameter("@pdaccdt", OleDbType.VarChar) { Value = DateN },
                    new OleDbParameter("@pdaccdt2", OleDbType.VarChar) { Value = DateN },
                    new OleDbParameter("@PDTYPEDOC", OleDbType.VarChar) { Value = 1} ,  // 1 ยินยอม 2 ไม่ยินยอม
                    new OleDbParameter("@PDTYPEDOC2", OleDbType.VarChar) { Value = 1} ,  // 1 ยินยอม 2 ไม่ยินยอม
                    new OleDbParameter("@PDMTHDOC", OleDbType.VarChar) { Value = 2}    // 1 ผ่านเว็บ 2 เอกสาร 

   
                };

                // สร้างคำสั่ง SQL สำหรับบันทึกข้อมูล
                var sql = new StringBuilder();
                sql.Append("insert into itprod.pdpafile(pdtaxid,pdrfcode,pdtaxname,pdtaxaddr,pdphone,pdsource,pdbddt,pdsexid ,pdnation,pdrgdt,pdrgtm,pdaccdt,pdaccdt2,PDTYPEDOC ,PDTYPEDOC2 , PDMTHDOC) ");
                sql.Append("VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ? , ? , ? , ?)");

                // เรียกใช้คำสั่งเพื่อบันทึกข้อมูลลงในฐานข้อมูล
                int result = db.ExecuteDb2NonQuery(sql.ToString(), parameters);

                // ตรวจสอบผลลัพธ์ของการบันทึกข้อมูล
                return result > 0; // คืนค่า true หากบันทึกสำเร็จ
            }
            catch (Exception ex)
            {
               
                Console.WriteLine("Error: " + ex.Message);
                return false; // Return false if an error occurs
            }
        }// end 

    }
}