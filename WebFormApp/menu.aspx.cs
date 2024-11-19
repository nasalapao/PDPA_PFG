using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFormApp
{
    public partial class menu : System.Web.UI.Page
    {
        
        public string valueFromSession = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // ถ้าผู้ใช้ยังไม่ล็อกอิน ให้เปลี่ยนเส้นทางไปยังหน้า Login.aspx
                valueFromSession = "";
                //Response.Redirect("Login.aspx");
            }
            else
            {
                if (Session["FnameE"] != null)
                {
                    valueFromSession = Session["FnameE"].ToString();
                    //Response.Write(valueFromSession);
                }
            }
        }
    }
}