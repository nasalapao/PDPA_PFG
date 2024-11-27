<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="WebFormApp.Report" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>



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
            <div class="formpdpa-loaddataReport">
                <form id="form1" runat="server">
                    <div class="form-group">
                             <div class="input-row" >
                                    
                            </div>
                            <div class="input-row">
                                <input type="text" id="taxid" placeholder="เลขที่บัตร" name="taxid" class="input" title="กรุณากรอกตัวเลขเท่านั้น" style="width: 30%;"/>
                                <input type="text" id="date_pdacc" placeholder="วันที่ยอมรับ yyyymmdd" name="date_pdacc" class="input" title="กรุณากรอกตัวเลขเท่านั้น" style="width: 30%;"/>
                                 <asp:DropDownList ID="ddlTYPEDOC" runat="server" CssClass="input" >
                                    <asp:ListItem Value="1">จัดเก็บรวบรวม ใช้ ทั่วไป</asp:ListItem>
                                    <asp:ListItem Value="2">ใช้เพื่อวัตถุประสงค๋ทางการตลาด</asp:ListItem>
                                    <asp:ListItem Value="3" Selected="True">ALL</asp:ListItem>
                                </asp:DropDownList>


                                <asp:Button ID="Button1"  runat="server" Text="DATA" CssClass="custom-buttonwidth"  OnClick="Button1_Click" />
                               
                               
                            </div>
                         <asp:Label ID="lblMessage" runat="server" Text="Label" Visible="False"></asp:Label>
                    </div>

                   <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                   <rsweb:reportviewer id ="ReportViewer1" runat="server" Width ="100%"  Style="height: 100vh;">

                   </rsweb:reportviewer>
                  
                </form>
           </div>
    </div>
    


   
</body>
 <!-- อ้างอิงไฟล์ JavaScript -->
    <script src="Scripts/menuToggle.js"></script>
</html>
