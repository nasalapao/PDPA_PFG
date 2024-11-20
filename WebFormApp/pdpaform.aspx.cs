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
    public partial class pdpaform : System.Web.UI.Page
    {
        private dbConnection db = new dbConnection();
        StringBuilder sql = new StringBuilder();
        string pdtaxid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) // ตรวจสอบว่าหน้านี้โหลดครั้งแรกหรือไม่
            {

                






                pdtaxid = Session["pdtaxid"] as string;
                if (!string.IsNullOrEmpty(pdtaxid))
                {
                    loaddatafromPDPAfile(pdtaxid);
                    loadDataRequese(pdtaxid);
                  
                    
                }
                else
                {
                    ShowError("Page_Load ไม่พบข้อมูล TaxID");
                }
            }
        }//class

        protected void  loadDataRequese(string taxidto)
        {
           
            string txtSTATUS = "";
            try
            {
                sql.Clear();
                sql.Append(string.Format(" SELECT * FROM ITPROD.PDPAREQUREST WHERE RETAXID='{0}'", taxidto));

            DataTable result =  db.ExecuteDb2Query(sql.ToString());

                if (db.isError == false && db.isHasRow)
                {

                    chkAccess.Checked =  Convert.ToBoolean(result.Rows[0]["REACCESS"]);
                    chkEdit.Checked = Convert.ToBoolean(result.Rows[0]["REEDIT"]);
                    chkDelete.Checked = Convert.ToBoolean(result.Rows[0]["REDELETE"]);
                    chkSuspend.Checked = Convert.ToBoolean(result.Rows[0]["RESUSPEND"]);
                    chkOppose.Checked = Convert.ToBoolean(result.Rows[0]["REOPPOSE"]);
                    chkTransfer.Checked = Convert.ToBoolean(result.Rows[0]["RETRANSFER"]);
                    remark.Value = result.Rows[0]["REMARK"].ToString();


                    int STATUS = Convert.ToInt16(result.Rows[0]["RESTATUS"]);
                    if (STATUS == 0) 
                    {
                        txtSTATUS = "รอดำเนินการ";
                    }
                    else if (STATUS == 1)
                    {
                        txtSTATUS = "อยู่ระหว่างดำเนินการ";
                    }
                    else if (STATUS == 2)
                    {
                        txtSTATUS = "เสร็จสิ้น";
                    }
                    else if (STATUS == 3)
                    {
                        txtSTATUS = "ปฏิเสธ";
                    }

                    Button1.Enabled = false;
                    Button1.Text = string.Format("ระบบได้รับข้อมูลของคุณแล้ว เจ้าหน้าที่ : ({0})" , txtSTATUS);
                   
                }


               
            }
            catch (Exception ex)
            {
                ShowError("loadDataRequese error: " + ex.Message);
               
            }
        }

        protected void loaddatafromPDPAfile(string taxidto)
        {
            try
            {
                sql.Clear();
                sql.Append("SELECT pdtaxid,pdrfcode,pdtaxname,pdtaxaddr,pdphone  ");
                sql.Append(",pdsource    ,pdbddt , PDEMAIL    ,pdsexid ,pdnation  ");
                sql.Append(",pdrgdt  ,pdrgtm,pdaccdt FROM itprod.pdpafile  ");
                sql.Append(string.Format("WHERE PDTAXID =  '{0}' ", taxidto));




                DataTable result = db.ExecuteDb2Query(sql.ToString());

                if (db.isError == false && db.isHasRow == true)
                {

                    txtFirstName.Text = result.Rows[0]["pdtaxname"].ToString();
                    txtTel.Text = result.Rows[0]["pdphone"].ToString();
                    txtEmail.Text = result.Rows[0]["PDEMAIL"].ToString();
                    txtAddress.Text = result.Rows[0]["pdtaxaddr"].ToString();

                }


            }
            catch (Exception ex)
            {
                ShowError("loaddatafromPDPAfile error: " + ex.Message);
            }
        }//class



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

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                int REACCESS = 0, REEDIT = 0, REDELETE = 0, RESUSPEND = 0, REOPPOSE = 0, RETRANSFER = 0;
                string remark = "";
                // รับค่าจากฟอร์ม
                string fullName = txtFirstName.Text.Trim();
                string tel = txtTel.Text.Trim();
                string email = txtEmail.Text.Trim();
                string address = txtAddress.Text.Trim();
                string rights = Request.Form["rights"]; // รับค่าจาก checkbox group


                pdtaxid = Session["pdtaxid"] as string;


                if (string.IsNullOrEmpty(pdtaxid))
                {
                    ShowError("Error pdtaxid");
                    Response.Redirect("LoadData.aspx");
                    return;
                }

                // ตรวจสอบว่า Checkbox ใดถูกเลือกและตั้งค่าตัวแปร
                if (chkAccess.Checked)
                {
                    REACCESS = 1;
                }
                if (chkEdit.Checked)
                {
                    REEDIT = 1;
                }
                if (chkDelete.Checked)
                {
                    REDELETE = 1;
                }
                if (chkSuspend.Checked)
                {
                    RESUSPEND = 1;
                }
                if (chkOppose.Checked)
                {
                    REOPPOSE = 1;
                }
                if (chkTransfer.Checked)
                {
                    RETRANSFER = 1;
                }

                // ตรวจสอบว่าไม่มี Checkbox ใดถูกเลือก
                if (REACCESS == 0 && REEDIT == 0 && REDELETE == 0 && RESUSPEND == 0 && REOPPOSE == 0 && RETRANSFER == 0)
                {
                    ShowError("คุณต้องการใช้สิทธิเรื่องใด กรุณาเลือกอย่างน้อย 1 อัน");
                    return;
                }




                remark = Request.Form["remark"];

                if (string.IsNullOrEmpty(remark))
                {
                    remark = "";
                }



                sql.Clear();
                sql.AppendLine("INSERT INTO ITPROD.PDPAREQUREST ");
                sql.AppendLine(" (RETAXID , REDATE , REACCESS, REEDIT, REDELETE, RESUSPEND, REOPPOSE, RETRANSFER , REMARK ) ");
                sql.AppendLine(" VALUES (? , ? , ? , ? , ? , ? , ? , ? , ?) ");

                //, REACCESS, REEDIT, REDELETE, RESUSPEND, REOPPOSE, RETRANSFER, REMARK, RESTATUS)

                // กำหนดค่า Parameters สำหรับคำสั่ง SQL
                var parameters = new[]
                {
                            new OleDbParameter("@RETAXID", OleDbType.VarChar) { Value = pdtaxid } ,
                            new System.Data.OleDb.OleDbParameter("@REDATE", OleDbType.VarChar) { Value = db.SqlDateYYYY_MM_DD(DateTime.Now) },
                            new System.Data.OleDb.OleDbParameter("@REACCESS", OleDbType.SmallInt) { Value = REACCESS },
                            new System.Data.OleDb.OleDbParameter("@REEDIT", OleDbType.SmallInt) { Value = REEDIT },
                            new System.Data.OleDb.OleDbParameter("@REDELETE", OleDbType.SmallInt) { Value = REDELETE },
                            new System.Data.OleDb.OleDbParameter("@RESUSPEND", OleDbType.SmallInt) { Value = RESUSPEND },
                            new System.Data.OleDb.OleDbParameter("@REOPPOSE", OleDbType.SmallInt) { Value = REOPPOSE },
                            new System.Data.OleDb.OleDbParameter("@RETRANSFER", OleDbType.SmallInt) { Value = RETRANSFER },
                            new System.Data.OleDb.OleDbParameter("@REMARK", OleDbType.VarChar) { Value = remark }
                            //new System.Data.OleDb.OleDbParameter("@RESTATUS", OleDbType.SmallInt) { Value = 0 }
                        };



                int rowsAffected = db.ExecuteDb2NonQuery(sql.ToString(), parameters);


                if (rowsAffected > 0)
                {
                    lblMessage.Text = "บันทึกข้อมูลสำเร็จ!";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblMessage.Text = "เกิดข้อผิดพลาดในการบันทึกข้อมูล.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                // แสดงข้อผิดพลาด
                lblMessage.Text = "ข้อผิดพลาด: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }//class

    }
}