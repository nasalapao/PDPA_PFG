using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace WebFormApp
{
   
    public partial class Login : System.Web.UI.Page
    {
         string Fname = "";
         string Userid = "";

        private dbConnection db = new dbConnection();

        protected void Page_Load(object sender, EventArgs e)
        {
            // ถ้ามี session อยู่แล้วให้ redirect ไปหน้า Default
            //if (Session["UserID"] != null)
            //{
            //    Response.Redirect("index.aspx");
            //}
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {

          
      
            string username = Request.Form["username"];
            string password = Request.Form["password"];

            

            // เช็คว่ากรอกข้อมูลครบไหม
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Please enter username and password");
                return;
            }

            DataTable result = CheckLogin(username, password);

            if (result.Rows.Count > 0)
            {
                // ล็อกอินสำเร็จ

                Fname = result.Rows[0]["FnameE"].ToString();
                Userid = result.Rows[0]["Personcode"].ToString();

                Session["FnameE"] = Fname;
                Session["Userid"] = Userid;

                FormsAuthentication.SetAuthCookie(username, false);
                ShowError("เข้าสู่ระบบสำเร็จ!");
                Response.Redirect("index.aspx"); // เปลี่ยนไปหน้าอื่นหลังจากล็อกอินสำเร็จ

            }
            else
            {
                // ล็อกอินล้มเหลว
                ShowError("ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง");
            }

            }
            catch (Exception ex)
            {
                ShowError("Login error: " + ex.Message);
            }

        }// end 

        private void ShowError(string message)
        {
            if (message=="")
            {
                lblMessage.Visible = false;
            }else
            {
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = message;
            }
            
        }//end


        public DataTable CheckLogin(string username, string password)
        {
            try
            {


                string query = "SELECT Personcode,FnameE,FnameT  FROM PersonDetail WHERE enddate IS NULL " +
                           "AND personcode = @username AND Pws = @password AND cmb2id = '22'";

            SqlParameter[] parameters = {
                new SqlParameter("@username", SqlDbType.VarChar) { Value = username.Trim() },
                new SqlParameter("@password", SqlDbType.VarChar) { Value = password.Trim() }
            };

            return db.ExecuteSqlQueryWithParams(query, parameters);

            }
            catch (Exception ex)
            {
                ShowError("Login error: " + ex.Message);
                return null;
            }
        }


    }
}