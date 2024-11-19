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
    public partial class LoadData : System.Web.UI.Page
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



                if (Check_duplicate(taxid) == false) //เช็คก่อนว่ามีข้อมูลไหม 
                {
                    //ถึงข้อมูลมาจาก Cyber
                    Calldata = 1;

                    ShowError(string.Format("บัตรเลขที่ :{0} ยังไม่มีในระบบ กรูณาตรวจสอบข้อมูลว่าถูกต้องหรือไม่", taxid));
                    if (loaddatafromHRIS(taxid) == true)
                    {
                        return;
                    }

                    if (DataCustomer_NFT(taxid) == true)
                    {
                        return;
                    }

                    if (DataSupplier_NFT(taxid) == true)
                    {
                        return;
                    }

                    if (DataCustomer_PFT(taxid) == true)
                    {
                        return;
                    }
                    if (DataSupplier_PFT(taxid) == true)
                    {
                        return;
                    }

                    ShowError(string.Format("บัตรเลขที่ :{0} ไม่มีข้อมูลใด ๆ เลย ตรวจสอบอีกครั้งว่าถูกต้อง", taxid));

                    taxid = "";
                    nameF = "";
                    addr = "";
                    tel = "";
                    pdsource = "";
                    bddt = "";
                    sexid = "";
                    currentNation = "";
                    DateN = "";
                    TimeN = "";
                    Calldata = 0;


                }
                else
                {
                    Calldata = 2;

                   

                    if (Session["FnameE"] != null)
                    {
                        ShowError(string.Format("บัตรเลขที่ :{0} มีอยู่ในระบบ สามารถแก้ไข และ ลบข้อมูลได้ ", taxid));
                        loaddatafromPDPAfile(taxid);
                    }
                    else
                    {

                        Calldata = 0 ;
                        ShowError(string.Format("บัตรเลขที่ :{0} มีอยู่ในระบบ กรุณารอสักครู่", taxid));
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






            }
            catch (Exception ex)
            {
                ShowError("Log error: " + ex.Message);
            }
        }//end 

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


        private bool DataSupplier_PFT(string taxidto)
        {
            try
            {
                if (taxidto == "")
                {
                    return false;
                }
                sql.Clear();
                sql.Append("SELECT idvrno, idsuno, ");
                sql.Append("CASE WHEN sasunm IS NOT NULL THEN TRIM(sasunm) || TRIM(saadr1) ELSE idsunm END AS saname, ");
                sql.Append("TRIM(saadr2) || TRIM(saadr3) || TRIM(saadr4) AS address, ");
                sql.Append("idphno, 'M3-PFT' AS pdsource, ");
                sql.Append("VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') AS DateN, ");
                sql.Append("REPLACE(SUBSTR(CHAR(CURRENT TIME), 1, 8), ':', '') AS TimeN, 0 ");
                sql.Append("FROM mvxcdtprod.cidmas ");
                sql.Append("LEFT JOIN ( ");
                sql.Append("    SELECT sacono, sasuno, sasunm, saadr1, saadr2, saadr3, saadr4, saadte, saadid ");
                sql.Append("    FROM MVXcdtprod.cidadr ");
                sql.Append("    WHERE saadte = '1' AND saadid = 'ADDR' ");
                sql.Append(") cidadr ON sacono = idcono AND sasuno = idsuno ");
                sql.Append("WHERE idcono = 100 ");
                sql.Append("AND idstat = '20' ");
                sql.Append("AND idcfi5 = 'N' ");
                sql.Append("AND idvrno <> '' ");
                sql.Append("AND idvrno NOT IN (SELECT pdtaxid FROM itprod.pdpafile) ");
                sql.Append(string.Format("AND idvrno = '{0}' ", taxidto));



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



        protected void loaddatafromPDPAfile(string taxidto)
        {
            try
            {
                sql.Clear();
                sql.Append("SELECT pdtaxid,pdrfcode,pdtaxname,pdtaxaddr,pdphone  ");
                sql.Append(",pdsource    ,pdbddt      ,pdsexid ,pdnation  ");
                sql.Append(",pdrgdt  ,pdrgtm,pdaccdt FROM itprod.pdpafile  ");
                sql.Append(string.Format("WHERE PDTAXID =  '{0}' ", taxidto));




                DataTable result = db.ExecuteDb2Query(sql.ToString());

                if (db.isError == false && db.isHasRow == true)
                {
                    taxid = result.Rows[0]["pdtaxid"].ToString();
                    Userid = result.Rows[0]["pdrfcode"].ToString();
                    nameF = result.Rows[0]["pdtaxname"].ToString();
                    addr = result.Rows[0]["pdtaxaddr"].ToString();
                    tel = result.Rows[0]["pdphone"].ToString();
                    pdsource = result.Rows[0]["pdsource"].ToString();
                    bddt = result.Rows[0]["pdbddt"].ToString();
                    sexid = result.Rows[0]["pdsexid"].ToString();
                    currentNation = result.Rows[0]["pdnation"].ToString();
                    DateN = db.SqlDateYYYYMMDD(DateTime.Now);
                    TimeN = DateTime.Now.ToString("HHmmss");//result.Rows[0]["pdrgtm"].ToString();
                }
                else
                {
                    taxid = "";
                    nameF = "";
                    addr = "";
                    tel = "";
                    pdsource = "";
                    bddt = "";
                    sexid = "";
                    currentNation = "";
                    DateN = "";
                    TimeN = "";
                    Calldata = 0;
                    ShowError(string.Format("บัตรเลขที่ :{0} ไม่มีข้อมูลใด ๆ เลย ตรวจสอบอีกครั้งว่าถูกต้อง", taxidto));
                }

            }
            catch (Exception ex)
            {
                ShowError("Log error: " + ex.Message);
            }
        }





        private bool loaddatafromHRIS(string taxidto)
        { //โหลดข้อมูลมาจาก PNT_Person Cyber 

            try
            {
                sql.Clear();
                sql.Append("SELECT taxid, PersonCode, ");
                sql.Append("RTRIM(FNameT) + ' ' + RTRIM(LNameT) AS sname, ");
                sql.Append("ISNULL(RTRIM(CurrentAddress), '') + ' ' + ISNULL(RTRIM(CURRENTROAD), '') + ' ' ");
                sql.Append("+ ISNULL(RTRIM(DISTRICTT), '') + ' ' + ISNULL(RTRIM(AmphurT), '') + ' ' ");
                sql.Append("+ ISNULL(RTRIM(ProveNameT), '') AS ADDRESS, ");
                sql.Append("CurrentTel, 'HRIS' AS pdsource, ");
                sql.Append("CONVERT(CHAR(8), BirthDate, 112) AS BDDT, "); //yyyyMMdd
                sql.Append("SexId, currentNation, ");
                sql.Append("CONVERT(VARCHAR, GETDATE(), 112) AS DateN, ");  // yyyyMMdd
                sql.Append("FORMAT(GETDATE(), 'HHmmss') AS TimeN ");        // HHmmss
                sql.Append("FROM PNT_Person ");
                sql.Append("LEFT JOIN PNM_District ON DistrictID = CardDistric ");
                sql.Append("LEFT JOIN PNM_Amphur ON AmphurID = CardAmphur ");
                sql.Append("LEFT JOIN PNM_Province ON ProvID = CardProvince ");
                sql.Append("WHERE enddate IS NULL ");
                sql.Append("AND taxid <> '' ");
                sql.Append(string.Format("AND taxid = '{0}'", taxidto));


                DataTable result = db.ExecuteSqlQuery(sql.ToString(), "Cyber");

                if (db.isError == false && db.isHasRow == true)
                {
                    taxid = result.Rows[0]["taxid"].ToString();
                    nameF = result.Rows[0]["sname"].ToString();
                    addr = result.Rows[0]["ADDRESS"].ToString();
                    tel = result.Rows[0]["CurrentTel"].ToString();
                    pdsource = result.Rows[0]["pdsource"].ToString();
                    bddt = result.Rows[0]["BDDT"].ToString();
                    sexid = result.Rows[0]["SexId"].ToString();
                    currentNation = result.Rows[0]["currentNation"].ToString();
                    DateN = result.Rows[0]["DateN"].ToString();
                    TimeN = result.Rows[0]["TimeN"].ToString();
                    return true;
                }
                else
                {
                    return false;

                    taxid = "";
                    nameF = "";
                    addr = "";
                    tel = "";
                    pdsource = "";
                    bddt = "";
                    sexid = "";
                    currentNation = "";
                    DateN = "";
                    TimeN = "";
                    Calldata = 0;
                    ShowError(string.Format("บัตรเลขที่ :{0} ไม่มีข้อมูลใด ๆ เลย ตรวจสอบอีกครั้งว่าถูกต้อง", taxidto));

                }



            }
            catch (Exception ex)
            {
                ShowError("Log error: " + ex.Message);
                return false;
            }



        } // end 

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