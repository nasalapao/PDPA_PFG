<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Requrest.aspx.cs" Inherits="WebFormApp.Requrest" %>


<!DOCTYPE html>
<html lang="en">

<head runat="server">
      <% Server.Execute("head.html"); %>
      <style type="text/css">
          .blue-grid {
              margin-top: 19px;
          }
      </style>
</head>

<body>

    <header>
           <% Server.Execute("menu.aspx"); %> 
    </header>

    
    <div class="formpdpa-container">
        <div class="formpdpa-gridview">

         <asp:Label ID="lblMessage" runat="server" Text="Label"></asp:Label>

         <form id="mainForm" runat="server">
            <asp:GridView ID="GridView1" runat="server" DataKeyNames="pdtaxid"  CssClass="blue-grid" AutoGenerateColumns="False" GridLines="None" AllowPaging="True" PageSize="100" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" OnRowEditing="GridView1_RowEditing" OnRowDeleting="GridView1_RowDeleting" OnRowUpdating="GridView1_RowUpdating" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnDataBound="GridView1_DataBound">
                <Columns>
                    <asp:BoundField DataField="pdtaxid" HeaderText="TaxID" ReadOnly="True" />
                    <asp:BoundField DataField="pdtaxname" HeaderText="Name" ReadOnly="True" />
                    <asp:BoundField DataField="pdtaxaddr" HeaderText="Address" ReadOnly="True" />
                    <asp:BoundField DataField="REDATE" HeaderText="Requrest Date" ReadOnly="True" />
                   

                    <asp:TemplateField HeaderText="Action" ShowHeader="true">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" runat="server" Text="Delete Data" 
                                NavigateUrl='<%# "LoadData.aspx?pdtaxid=" + Eval("pdtaxid") %>' 
                                CssClass="linkid">
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="STATUS">
                        <EditItemTemplate>
                            <asp:DropDownList ID="DropDownListSTATUS" runat="server">
                                <asp:ListItem Text="รอดำเนินการ" Value="0" />
                                <asp:ListItem Text="อยู่ระหว่างดำเนินการ" Value="1" />
                                <asp:ListItem Text="เสร็จสิ้น" Value="2" />
                                <asp:ListItem Text="ปฏิเสธ" Value="3" />
                                <asp:ListItem Text="ถอนคำร้อง" Value="4" />
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="RESTATUS" runat="server" Text='<%# Bind("RESTATUS") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="True" />



                    <%--<asp:TemplateField HeaderText="Document" ShowHeader="true">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink2" runat="server" Text="Upload" CssClass="linkid"></asp:HyperLink>
                            <asp:HyperLink ID="HyperLink3" runat="server" Text="Upload" CssClass="linkid"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                   


                </Columns>
            </asp:GridView>
        </form>
        </div>
 </div>




 <!-- อ้างอิงไฟล์ JavaScript -->
    <script src="Scripts/menuToggle.js"></script>
</body>

</html>