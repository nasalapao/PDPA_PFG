<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="menu.aspx.cs" Inherits="WebFormApp.menu" %>

           
<nav class="navbar">
    <div class="logo"> <a href="#">PDPA</a></div>
    <ul class="links">
        <li><a href="index.aspx">Home</a></li>
        <li><a href="privacy.aspx">Privacy</a></li>
        <li><a href="LoadData.aspx">Services</a></li>

         <% 
            if (valueFromSession == "") {
                Response.Write(" <li><a href='login.aspx'>Login</a></li>");
            }else
            {
                Response.Write(@"
                    <div class='dropdown'>
                        <li><a href=''>PDPA Menu</a></li>
                        <div class='dropdown-content'>
                            <a href='LoadData.aspx'>Load</a>
                            <a href='View.aspx'>View</a>
                            <a href='Report.aspx'>Report</a>
                        </div>
                    </div>
                    <li><a href='Logout.aspx'>Logout</a></li>
                ");

            }

             %>
        
        
            
       
    </ul>

   <!-- <a href="#" class="action_btn">Get Started</a>-->

    <div class="toggle_btn">
        <i class="fa-solid fa-bars"></i>
    </div>

    <div class="dropdown_menu">
        <li><a href="index.aspx">Home</a></li>
        <li><a href="privacy.aspx">Privacy</a></li>
        <li><a href="LoadData.aspx">Services</a></li>

        <% 
            if (valueFromSession == "") {
                Response.Write("<li><a href='Logout.aspx'>Logout</a></li>");
            }else
            {
                Response.Write(@"
                   
                        <li><a href=''>**PDPA Menu**</a></li>
                        <li><a href=''>Load</a></li>
                        <li><a href=''>Verify</a></li>
                        <li><a href=''>Upload</a></li>
                        <li><a href=''>Report</a></li>
                        <li><a href='Logout.aspx'>Logout</a></li>
                ");
            }
            
            
             %>


        
        <li> <a href="#" class="action_btn">Get Started</a></li>
    </div>
</nav>