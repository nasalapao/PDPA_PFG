using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFormApp
{
    public partial class DownloadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string filePath = Request.QueryString["filepath"];
            if (!string.IsNullOrEmpty(filePath))
            {
                string fullPath = ("\\\\172.16.33.37\\PDPA_Document\\" + filePath);
                if (System.IO.File.Exists(fullPath))
                {
                    string fileExtension = System.IO.Path.GetExtension(fullPath).ToLower();
                    string contentType;

                    // กำหนด Content-Type ตามประเภทไฟล์
                    switch (fileExtension)
                    {
                        case ".pdf":
                            contentType = "application/pdf";
                            break;
                        case ".jpg":
                        case ".jpeg":
                            contentType = "image/jpeg";
                            break;
                        case ".png":
                            contentType = "image/png";
                            break;
                        case ".gif":
                            contentType = "image/gif";
                            break;
                        case ".xlsx":
                            contentType = "application/xlsx";
                            break;
                        default:
                            contentType = "application/octet-stream";
                            break;
                    }

                    Response.ContentType = contentType;
                    Response.AppendHeader("Content-Disposition", "inline; filename=" + System.IO.Path.GetFileName(fullPath));
                    Response.TransmitFile(fullPath);
                    Response.End();
                }
                else
                {
                    Response.Write("File not found.");
                }
            }
        }
    }
}