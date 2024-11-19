<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="WebFormApp.View" %>



<!DOCTYPE html>
<html lang="en">

<head runat="server">
      <% Server.Execute("head.html"); %>
</head>

<body>

            <header>
                   <% Server.Execute("menu.aspx"); %>
                   <asp:Label ID="lblMessage" runat="server" Text="Label"></asp:Label>
            </header>

    <div class="formpdpa-container">
        <div class="formpdpa-gridview">

         <form id="mainForm" runat="server">
            <asp:GridView ID="GridView1" runat="server" CssClass="blue-grid" AutoGenerateColumns="False" GridLines="None" AllowPaging="True" PageSize="100" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="pdtaxid" HeaderText="TaxID" />
                    <asp:BoundField DataField="pdtaxname" HeaderText="Name" />
                    <asp:BoundField DataField="pdtaxaddr" HeaderText="Address" />
                    <asp:BoundField DataField="pdphone" HeaderText="Phone" />
                    <asp:BoundField DataField="pdsource" HeaderText="Source" />
                    <asp:BoundField DataField="pdbddt" HeaderText="Birthday" />

                    
                    <asp:TemplateField HeaderText="Action" ShowHeader="true">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" runat="server" Text="Edit Data" 
                                NavigateUrl='<%# "LoadData.aspx?pdtaxid=" + Eval("pdtaxid") %>' 
                                CssClass="linkid">
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Document" ShowHeader="true">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink2" runat="server" Text="Upload" CssClass="linkid"></asp:HyperLink>
                            <asp:HyperLink ID="HyperLink3" runat="server" Text="Upload" CssClass="linkid"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                   


                </Columns>
            </asp:GridView>
        </form>
        </div>
 </div>


 <!-- อ้างอิงไฟล์ JavaScript -->
    <script src="Scripts/menuToggle.js"></script>
</body>

</html>