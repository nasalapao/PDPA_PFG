using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Text;

namespace WebFormApp
{
    public partial class ReLoadData : System.Web.UI.Page
    {
        public string FnameE = "", Userid = "";
        public int Calldata = 0; //0 = not show ,1 = found data show, 2 = not found data show
        public string taxid = "", nameF = "", sername = "", addr = "", tel = "", pdsource = "", bddt = "", sexid = "";
        public string currentNation = "", DateN = "", TimeN = "";
        string pdtaxid = "";

        private dbConnection db = new dbConnection();
        StringBuilder sql = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {

            pdtaxid = Request.QueryString["pdtaxid"];
            if (!string.IsNullOrEmpty(pdtaxid))
            {
                btnsubmit_Click(sender, e);
            }

            if (!User.Identity.IsAuthenticated)
            {
                // ถ้าผู้ใช้ยังไม่ล็อกอิน ให้เปลี่ยนเส้นทางไปยังหน้า Login.aspx
                //toLogin();
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
                    //toLogin();
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



            try
            {
                string fotaxid = Request.Form["taxid"];


                if (!string.IsNullOrEmpty(fotaxid))
                {
                    taxid = fotaxid;
                }
                if (!string.IsNullOrEmpty(pdtaxid))
                {
                    taxid = pdtaxid;
                }

                //ขั้นตอนการร้องขอ 

                //1. เช็คก่อนว่าเคยมีการ้องขอไปรียัง หากยังไปข้อ2 
                    //1.1 แสดง สถาน่ะว่า คำร้องถึงไหนแล้ว  
                    //1.2 




                //2. หากมีข้อมูลในระบบ ให้ redirec ไปที่ pdpaform
                    //2.1 ในหน้านี้หากเข้ามาอีกครั้ง ให้แสดงสถาน่ะ
                    //



                if (CheckDataRequrest(taxid))
                {
                    //มีข้อมูล ที่ร้องขอ

                    Calldata = 0;
                    ShowError(string.Format("บัตรเลขที่ :{0}  กรุณารอสักครู่", taxid));
                    Session["pdtaxid"] = taxid;
                    Response.Cookies["pdtaxid"].Expires = DateTime.Now.AddMinutes(60);
                    string script = string.Format(@"
                                                        <script type='text/javascript'>
                                                            setTimeout(function() {{
                                                                window.location.href = 'pdpaform.aspx';
                                                            }}, 5000);
                                                        </script>", taxid);
                    ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script);
                    return;
                }




                if (Check_duplicate(taxid) == false) //เช็คก่อนว่ามีข้อมูลไหม 
                {
                    //ไม่มีข้อมูล pdpa


                    ShowError(string.Format("บัตรเลขที่ :{0} ยังไม่มีในระบบ ", taxid));
                   

                }
                else //มีข้อมูลใน pdpa แล้ว
                {
                    Calldata = 2;



                        Calldata = 0 ;
                        ShowError(string.Format("บัตรเลขที่ :{0}  กรุณารอสักครู่", taxid));
                        Session["pdtaxid"] = taxid;
                        Response.Cookies["pdtaxid"].Expires = DateTime.Now.AddMinutes(60); 
                        string script = string.Format(@"
                                                        <script type='text/javascript'>
                                                            setTimeout(function() {{
                                                                window.location.href = 'pdpaform.aspx';
                                                            }}, 5000);
                                                        </script>", taxid);
                            ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script);


          


                }






            }
            catch (Exception ex)
            {
                ShowError("Log error: " + ex.Message);
            }
        }//end 


        private bool CheckDataRequrest(string taxidto)
        {
            try
            {
                if (string.IsNullOrEmpty(taxidto))
                {
                    ShowError("TaxID ไม่ถูกต้อง");
                    return false;
                }
                string sql = string.Format(
                    "SELECT 1 FROM PFGMVX.ITPROD.PDPAREQUREST WHERE RETAXID = '{0}'",
                    taxidto.Replace("'", "''") //  SQL injection
                );

                DataTable result = db.ExecuteDb2Query(sql);

                if (result != null && result.Rows.Count > 0)
                {
                    return true; // พบข้อมูล
                }

                return false; // ไม่มีข้อมูล
            }
            catch (Exception ex)
            {
                ShowError("Log error: " + ex.Message);
                return false;
            }
        }//end method



        private bool DataCustomer_NFT(string taxidto)  // โหลดข้อมูลจาก M3
        {
            try
            {
                if (taxidto == "")
                {
                    return false;
                }

                sql.Clear();
                sql.Append(" select OKVRNO AS TAXID ,okcuno,okcunm ");
                sql.Append("  ,trim(okcua1) || trim(okcua2) || trim(okcua3) || trim(okcua4) as address ");
                sql.Append(" ,okphno,'M3-NFT' as pdsource,VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') AS DateN ");
                sql.Append(" ,REPLACE(SUBSTR(CHAR(CURRENT TIME), 1, 8), ':', '') AS TimeN ,0  ");
                sql.Append(" from affcdtprod.ocusma where okcono = 200 and okstat = '20' and okcfc0 = 'N'   ");
                sql.Append(" and okvrno <> ''  and okvrno not in(select pdtaxid from itprod.pdpafile)  ");
                sql.Append(string.Format(" and okvrno = '{0}' ", taxidto));



                DataTable result = db.ExecuteDb2Query(sql.ToString());

                if (db.isError == false && db.isHasRow == true)
                {
                    taxid = result.Rows[0]["TAXID"].ToString();
                    Userid = result.Rows[0]["okcuno"].ToString();
                    nameF = result.Rows[0]["okcunm"].ToString();
                    addr = result.Rows[0]["address"].ToString();
                    tel = result.Rows[0]["okphno"].ToString();
                    pdsource = result.Rows[0]["pdsource"].ToString();
                    bddt = "";
                    sexid = "";
                    currentNation = "";
                    DateN = db.SqlDateYYYYMMDD(DateTime.Now); //result.Rows[0]["pdrgdt"].ToString();  // fix null data
                    TimeN = DateTime.Now.ToString("HHmmss");//result.Rows[0]["pdrgtm"].ToString();

                    return true;
                }
                else
                {
                    return false;

                }
            }
            catch (Exception ex)
            {
                ShowError("Log error: " + ex.Message);
                return false;
            }
        }//end

        private bool DataSupplier_NFT(string taxidto)  // โหลดข้อมูลจาก M3
        {

            try
            {
                if (taxidto == "")
                {
                    return false;
                }

                sql.Clear();
                sql.Append(" select idvrno,idsuno ");
                sql.Append(" ,case when sasunm is not null then trim(sasunm) || trim(saadr1) else idsunm end as saname ");
                sql.Append(" ,trim(saadr2) || trim(saadr3) || trim(saadr4) as address,idphno,'M3-NFT' as pdsource ");
                sql.Append(" ,VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') AS DateN  ");
                sql.Append(" ,REPLACE(SUBSTR(CHAR(CURRENT TIME), 1, 8), ':', '') AS TimeN ,0 ");
                sql.Append(" from affcdtprod.cidmas ");
                sql.Append(" left join  ");
                sql.Append(" (select sacono,sasuno,sasunm,saadr1,saadr2,saadr3,saadr4,saadte,saadid from affcdtprod.cidadr where saadte = '1' and saadid = 'ADDR') ");
                sql.Append(" cidadr on sacono = idcono and sasuno = idsuno ");
                sql.Append(" where idcono = 200 and idstat = '20' and idcfi5 = 'N'  ");
                sql.Append(" and  idvrno <> ''  ");
                sql.Append(" and idvrno not in(select pdtaxid from itprod.pdpafile) ");
                sql.Append(string.Format("  and idvrno = '{0}' ", taxidto));



                DataTable result = db.ExecuteDb2Query(sql.ToString());

                if (db.isError == false && db.isHasRow == true)
                {
                    taxid = result.Rows[0]["idvrno"].ToString();
                    Userid = result.Rows[0]["idsuno"].ToString();
                    nameF = result.Rows[0]["saname"].ToString();
                    addr = result.Rows[0]["address"].ToString();
                    tel = result.Rows[0]["idphno"].ToString();
                    pdsource = result.Rows[0]["pdsource"].ToString();
                    bddt = "";
                    sexid = "";
                    currentNation = "";
                    DateN = db.SqlDateYYYYMMDD(DateTime.Now); //result.Rows[0]["pdrgdt"].ToString();  // fix null data
                    TimeN = DateTime.Now.ToString("HHmmss");//result.Rows[0]["pdrgtm"].ToString();

                    return true;
                }
                else
                {
                    return false;

                }



            }
            catch (Exception ex)
            {
                ShowError("Log error: " + ex.Message);
                return false;
            }



        }//end method


        private bool DataCustomer_PFT(string taxidto)  //M3
        {
            try
            {
                if (taxidto == "")
                {
                    return false;
                }
                sql.Clear();
                sql.Append("SELECT OKVRNO AS TAXID, okcuno, okcunm, ");
                sql.Append("TRIM(okcua1) || TRIM(okcua2) || TRIM(okcua3) || TRIM(okcua4) AS address, ");
                sql.Append("okphno, 'M3-PFT' AS pdsource, ");
                sql.Append("VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') AS DateN, ");
                sql.Append("REPLACE(SUBSTR(CHAR(CURRENT TIME), 1, 8), ':', '') AS TimeN, 0 ");
                sql.Append("FROM mvxcdtprod.ocusma ");
                sql.Append("WHERE okcono = 100 ");
                sql.Append("AND okstat = '20' ");
                sql.Append("AND okcfc0 = 'N' ");
                sql.Append("AND okvrno <> '' ");
                sql.Append("AND okvrno NOT IN (SELECT pdtaxid FROM itprod.pdpafile) ");
                sql.Append(string.Format("AND okvrno = '{0}' ", taxidto));



                DataTable result = db.ExecuteDb2Query(sql.ToString());

                if (db.isError == false && db.isHasRow == true)
                {
                    taxid = result.Rows[0]["TAXID"].ToString();
                    Userid = result.Rows[0]["okcuno"].ToString();
                    nameF = result.Rows[0]["okcunm"].ToString();
                    addr = result.Rows[0]["address"].ToString();
                    tel = result.Rows[0]["okphno"].ToString();
                    pdsource = result.Rows[0]["pdsource"].ToString();
                    bddt = "";
                    sexid = "";
                    currentNation = "";
                    DateN = db.SqlDateYYYYMMDD(DateTime.Now); //result.Rows[0]["pdrgdt"].ToString();  // fix null data
                    TimeN = DateTime.Now.ToString("HHmmss");//result.Rows[0]["pdrgtm"].ToString();

                    return true;
                }
                else
                {
                    return false;

                }



            }
            catch (Exception ex)
            {
                ShowError("Log error: " + ex.Message);
                return false;
            }



        }//end method








        public bool Check_duplicate(string taxid)
        {
            try
            {
                string sql_find = string.Format("select * from itprod.pdpafile  where pdtaxid = '{0}'", taxid);

                DataTable result = db.ExecuteDb2Query(sql_find);

                if (db.isError == false && db.isHasRow == true)
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
    } //class
} // namespace