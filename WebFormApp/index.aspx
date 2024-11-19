<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="WebFormApp.index" %>



<!DOCTYPE html>
<html lang="en">

<head runat="server">
      <% Server.Execute("head.html"); %>
</head>

<body>

    <header>
           <% Server.Execute("menu.aspx"); %> 
    </header>


    <main>
        <section id = "hero">
            <h1>Welcome</h1>
             <p> K.<%= Userid %></p>
            <p> K.<%= FnameE %></p>
            
          
        </section>
    </main>


 <!-- อ้างอิงไฟล์ JavaScript -->
    <script src="Scripts/menuToggle.js"></script>
</body>

</html>