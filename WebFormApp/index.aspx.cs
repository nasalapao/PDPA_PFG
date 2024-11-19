using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFormApp
{
    public partial class index : System.Web.UI.Page
    {
        public string FnameE = "" , Userid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // ถ้าผู้ใช้ยังไม่ล็อกอิน ให้เปลี่ยนเส้นทางไปยังหน้า Login.aspx
                toLogin();
            }else
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
    }
}