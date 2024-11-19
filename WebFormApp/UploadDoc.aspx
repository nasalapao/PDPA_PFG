<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadDoc.aspx.cs" Inherits="WebFormApp.UploadDoc" %>

<!DOCTYPE html>
<html lang="en">

<head runat="server">
      <% Server.Execute("head.html"); %>

       <style>
        /* CSS ที่เพิ่มเข้ามาตามที่กล่าวไว้ข้างต้น */
        .input-row {
             align-items: center; /* จัดให้อยู่กลาง Rows*/
        }

        .custom-file-upload {
            position: relative;
            display: inline-block;
            background-color: #2879f9;
            color: #fff;
            padding: 10px 20px;
            border-radius: 4px;
            width:100%
        }

    </style>

</head>

<body>

    <header>
           <% Server.Execute("menu.aspx"); %> 

    </header>


     <div class="formpdpa-container">
            <div class="formpdpa-loaddata">
                 <form id="form1" runat="server">

                     <div class="form-group">
                        <label for="taxid">กรุณาเลือกเอกสารที่ upload:</label>
                            <div class="input-row">
                            

                                <div class="custom-file-upload">
                                    <asp:FileUpload ID="FileUpload1" runat="server"  />
                                </div>
                                
                                
                                <asp:Button ID="btnsubmit" runat="server" Text="Save" CssClass="custom-button" OnClick="btnsubmit_Click" />

                            </div>
                        </div>
                 
                    <asp:Label ID="lblMessage" runat="server" Text="Label"></asp:Label>
                    <br>
                </form>
            </div>
           </div>


 <!-- อ้างอิงไฟล์ JavaScript -->
    <script src="Scripts/menuToggle.js"></script>
  
</body>

</html>