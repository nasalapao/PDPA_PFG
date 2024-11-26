<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadData.aspx.cs" Inherits="WebFormApp.LoadData" %>


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
                            <%--<asp:Button ID="btnsubmit" runat="server" Text="ค้นหา" CssClass="custom-button" OnClick="btnsubmit_Click" OnClientClick="return ShowHideCyber(); return false;" />--%>
                            <asp:Button ID="btnsubmit" runat="server" Text="ค้นหา" CssClass="custom-buttonfind" OnClick="btnsubmit_Click" />

                        </div>
                    </div>
              <%--  <button type="submit">เข้าสู่ระบบ</button>--%>
                <asp:Label ID="lblMessage" runat="server" Text="Label"></asp:Label>
                <br>
            </form>
        </div>

         
         





          <% if (Calldata==1)
                        {
                            Response.Write(@"
                                        <div class='formpdpa-loaddata' id='formCyberData'>

                                         <form id='saveForm' method='post' action='SaveData.aspx'>
                                            <div class='form-group'>
                                                <label for='taxid'>เลขที่บัตร</label>
                                                <div class='input-row'>
                                                    <input type='text' placeholder='เลขที่บัตร' name='taxid' class='input' value='" + taxid + @"' readonly>
                                                </div>
                                            </div>
                                            <div class='form-group'>
                                                <label for='taxid'>ชื่อ-นามสกุล:</label>
                                                <div class='input-row'>
                                                    <input type='text' placeholder='ชื่อ' name='first_name' class='input' value='" + nameF + @"' readonly>
                                                </div>
                                            </div>
                                            <div class='form-group'>
                                                <label for='addr'>ที่อยู่:</label>
                                                <textarea name='addr' placeholder='ที่อยู่' rows='2' style='width:100%;' readonly>" + addr + @"</textarea>
                                            </div>
                                            <div class='form-group'>
                                                <label>โทร:ข้อมูลจาก:วันเดือนปีเกิด:เพศ</label>
                                                <div class='input-row'>
                                                    <input type='text' id='phonenumber' placeholder='เบอร์โทร' name='phonenumber' class='input' value='" + tel + @"' readonly title='เบอร์โทร'>
                                                    <input type='text' placeholder='ข้อมูลจาก' name='pdsource' class='input' value='" + pdsource + @"' readonly title='ข้อมูลจาก'>
                                                    <input type='text' placeholder='วันเดือนปีเกิด' name='bddt' class='input' value='" + bddt + @"' readonly title='วันเดือนปีเกิด'>
                                                    <input type='text' placeholder='เพศ' name='sexid' class='input' value='" + sexid + @"' readonly title='1=ชาย , 2=หญิง'>
                                                </div>
                                            </div>
                                                <input type='hidden' name='currentNation' value='" + currentNation + @"'>  
                                                <input type='hidden' name='DateN' value='" + DateN + @"'>
                                                <input type='hidden' name='TimeN' value='" + TimeN + @"'>   
                                                <input type='hidden' name='Userid' value='" + Userid + @"'>  
                                            <button type='button' class='custom-button' onclick='document.getElementById(""saveForm"").submit();'>Save</button>
                                        </form>
                                    </div>

                                    ");

                             }
             else if (Calldata == 2)
                             {
                                 Response.Write(@"
                                                        <div class='formpdpa-loaddata' id='formCyberData'>
                                                            <form id='editForm' method='post' action='EditData.aspx'>
                                                                <div class='form-group'>
                                                                    <label for='taxid'>เลขที่บัตร</label>
                                                                    <div class='input-row'>
                                                                        <input type='text' placeholder='เลขที่บัตร' name='taxid' class='input' value='" + taxid + @"' >
                                                                    </div>
                                                                </div>
                                                                <div class='form-group'>
                                                                    <label for='first_name'>ชื่อ-นามสกุล:</label>
                                                                    <div class='input-row'>
                                                                        <input type='text' placeholder='ชื่อ' name='first_name' class='input' value='" + nameF + @"' >
                                                                    </div>
                                                                </div>
                                                                <div class='form-group'>
                                                                    <label for='addr'>ที่อยู่:</label>
                                                                    <textarea name='addr' placeholder='ที่อยู่' rows='2' style='width:100%;' >" + addr + @"</textarea>
                                                                </div>
                                                                <div class='form-group'>
                                                                    <label>โทร:ข้อมูลจาก:วันเดือนปีเกิด:เพศ</label>
                                                                    <div class='input-row'>
                                                                        <input type='text' id='phonenumber' placeholder='เบอร์โทร' name='phonenumber' class='input' value='" + tel + @"'  title='เบอร์โทร'>
                                                                        <input type='text' placeholder='ข้อมูลจาก' name='pdsource' class='input' value='" + pdsource + @"'  title='ข้อมูลจาก'>
                                                                        <input type='text' placeholder='วันเดือนปีเกิด' name='bddt' class='input' value='" + bddt + @"'  title='วันเดือนปีเกิด'>
                                                                        <input type='text' placeholder='เพศ' name='sexid' class='input' value='" + sexid + @"'  title='1=ชาย , 2=หญิง'>
                                                                    </div>
                                                                </div>

                                                            <input type='hidden' name='currentNation' value='" + currentNation + @"'>  
                                                            <input type='hidden' name='DateN' value='" + DateN + @"'>
                                                            <input type='hidden' name='TimeN' value='" + TimeN + @"'>   
                                                            <input type='hidden' name='Userid' value='" + Userid + @"'>  


                                                              <div class='form-group'>
                                                                    <div class='input-row'>
                                                                     <button type='submit' class='custom-button'  name='action'  value='edit'>EditData</button>
                                                                     <button type='submit' class='custom-button'  name='action'  value='delete'>DeleteData</button>
                                                                    </div>   
                                                               </div>       
                                                               
                                                            </form>
                                                        </div>
                                                    ");

                 
                 
                             } //end if 
                                     
          %>

         









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