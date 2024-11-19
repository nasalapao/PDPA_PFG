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

        string taxid = "", pdaccdt = "";

        protected void Page_Load(object sender, EventArgs e)
        {

        }



        private void ShowReport()
        {
            try
            {
                //reset
                ReportViewer1.Reset();

                // data source

                DataTable dt = GetData();

                if (db.isError == false && db.isHasRow)
                {
                    ReportDataSource rds = new ReportDataSource("DataSet1", dt);
                    ReportViewer1.LocalReport.DataSources.Add(rds);
                    ReportViewer1.LocalReport.ReportPath = "ITS008_CheckList.rdlc";
                    ReportViewer1.LocalReport.Refresh();
                }
                else
                {
                    ShowError("ไม่พบข้อมูล");
                }


            }
            catch (Exception ex)
            {
                ShowError("ShowReport error: " + ex.Message);
               
            }


        }

        private DataTable GetData()
        {

            try
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

                //// taxid 
                if (string.IsNullOrEmpty(taxid) == false)
                {
                    sql.Append(string.Format(" WHERE PDTAXID = '{0}' ", taxid));
                    //where 
                }
                else
                {
                    sql.Append(" WHERE  1 = 1 ");

                }

                //date
                if (string.IsNullOrEmpty(pdaccdt) == false)
                {

                    sql.Append(string.Format("AND  PDACCDT = '{0}'", pdaccdt));

                }
                // type
                if (!ddlTYPEDOC.Text.Equals("3"))
                {
                    sql.Append(string.Format("AND  PDTYPEDOC = '{0}'", ddlTYPEDOC.Text));
                }

                sql.Append(" ORDER BY PDRGDT DESC , PDRGTM desc");

                dt = db.ExecuteDb2Query(sql.ToString());

                return dt;
            }
            catch (Exception ex)
            {
                ShowError("GetData error: " + ex.Message);
                return null;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            taxid = Request.Form["taxid"];
            pdaccdt = Request.Form["date_pdacc"];
            ShowError("");
            ShowReport();
        }

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


    }
}