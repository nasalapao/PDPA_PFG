using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFormApp
{
    public partial class Report : System.Web.UI.Page
    {
        private dbConnection db = new dbConnection();
        StringBuilder sql = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {

        }



        private void ShowReport()
        {

            //reset
            ReportViewer1.Reset();

            // data source

            DataTable dt = GetData();
            ReportDataSource rds = new ReportDataSource("DataSet1", dt);

            ReportViewer1.LocalReport.DataSources.Add(rds);

            // path
            ReportViewer1.LocalReport.ReportPath = "ITS008_CheckList.rdlc";


            ReportViewer1.LocalReport.Refresh();


        }

        private DataTable GetData()
        {
            DataTable dt = new DataTable();

            sql.Clear();

            sql.Append("SELECT pdtaxid, pdrfcode, pdprefix, pdtaxname, ");
            sql.Append("pdtaxaddr, pdcuraddr, pdworkaddr, ");
            sql.Append("CASE WHEN pdsexid <> '' THEN CASE WHEN pdsexid = '1' THEN 'Male' ELSE 'Female' END END AS pdsexid, ");
            sql.Append("pdbddt, pdphone, pdemail, pdlineid, pdbanknm, pdbankno, ");
            sql.Append("CASE WHEN pdtypedoc <> '' THEN CASE WHEN pdtypedoc = '1' THEN 'ยินยอม' ELSE 'ไม่ยินยอม' END END AS pdtypedoc, pdaccdt, ");
            sql.Append("CASE WHEN pdtypedoc2 <> '' THEN CASE WHEN pdtypedoc2 = '1' THEN 'ยินยอม' ELSE 'ไม่ยินยอม' END END AS pdtypedoc2, pdaccdt2, ");
            sql.Append("CASE WHEN pdmthdoc <> '' THEN CASE WHEN pdmthdoc = '1' THEN 'ผ่าน Web' ELSE 'เอกสาร แบบฟอร์ม' END END AS pdmthdoc ");
            sql.Append("FROM ITPROD.PDPAFILE ");

            //where 

            ////18-11-2024

            //where 






            sql.Append(" ORDER BY PDRGDT DESC , PDRGTM desc");
           


            
            //  "    WHERE PDSOURCE IN('HRIS') " +
            //  "     AND PDRFCODE <> '' and substr(pdrfcode,1,2) in('84','99','62','85','91','25','24')  " +
            //  "    AND PDRFCODE >= '16311023' AND PDRFCODE <= '16311038'   " +
            //  " ORDER BY PDTAXID ";



            dt = db.ExecuteDb2Query(sql.ToString());



            return dt;

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string taxid = Request.Form["taxid"];
            string password = Request.Form["password"];


            ShowReport();
        }

       


    }
}