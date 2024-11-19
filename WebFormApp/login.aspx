<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebFormApp.Login" %>
<!DOCTYPE html>
<html lang="en">

<head runat="server">
     <% Server.Execute("head.html"); %>
</head>

<body>

    <header>
                <% Server.Execute("menu.aspx"); %>
    </header>

     <div class="formpdpa">
        <div class="formpdpa-login">
            <h2>เข้าสู่ระบบ</h2>
            <form id="form1" runat="server">
                <label for="username">ชื่อผู้ใช้:</label>
                <input type="text" id="username" name="username" placeholder="ชื่อผู้ใช้" required>
        
                <label for="password">รหัสผ่าน:</label>
                <input type="password" id="password" name="password" placeholder="รหัสผ่าน" required>
        
              <%--  <button type="submit">เข้าสู่ระบบ</button>--%>
                <asp:Button ID="Button1" runat="server" Text="เข้าสู่ระบบ" CssClass="custom-button" OnClick="Button1_Click" />

                <asp:Label ID="lblMessage" runat="server" Text="Label"></asp:Label>

                <br>
                
      <div>   
     </div>

            </form>
        </div>
     
         
         </div>
   


 <!-- อ้างอิงไฟล์ JavaScript -->
    <script src="Scripts/menuToggle.js"></script>
   
</body>

</html>