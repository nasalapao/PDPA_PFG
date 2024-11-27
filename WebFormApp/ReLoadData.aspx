<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReLoadData.aspx.cs" Inherits="WebFormApp.ReLoadData" %>


<!DOCTYPE html>
<html lang="en">

<head runat="server">
      <% Server.Execute("head.html"); %>
</head>

<body>

    <header>
           <% Server.Execute("menu.aspx"); %> 
    </header>

     <div class="formpdpa-container">
        <div class="formpdpa-loaddata">
             <form id="form1" runat="server">

                 <div class="form-group">
                    <label for="taxid">เลขที่บัตร:</label>
                        <div class="input-row">
                            <input type="text" id="taxid" placeholder="" name="taxid" class="input" title="กรุณากรอกตัวเลขเท่านั้น" />
                            <asp:Button ID="btnsubmit" runat="server" Text="ค้นหา" CssClass="custom-buttonfind" OnClick="btnsubmit_Click" />

                        </div>
                    </div>
                <asp:Label ID="lblMessage" runat="server" Text="Label"></asp:Label>
                <br>
            </form>
        </div>

         

     </div>
  
    <script>
        document.getElementById("taxid").addEventListener("input", function (event) {
            // ตรวจสอบว่าค่าที่กรอกเป็นตัวเลขเท่านั้น
            this.value = this.value.replace(/[^0-9]/g, '');
        });
    </script>






 <!-- อ้างอิงไฟล์ JavaScript -->
    <script src="Scripts/menuToggle.js"></script>
</body>

</html>