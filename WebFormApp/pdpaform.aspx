<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pdpaform.aspx.cs" Inherits="WebFormApp.pdpaform" %>


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
        <div class="formpdpa-info">
            <h1>ใช้สิทธิภายใต้ <br>พ.ร.บ. คุ้มครองข้อมูลส่วนบุคคล</h1>
            <p>ส่งคำขอด้านข้อมูลของคุณไปยัง Pataya food ผ่านฟอร์มด้านล่าง
                คำขอของคุณจะถูกส่งไปยังผู้ดูแลด้านข้อมูลของ <br> Pataya food</p>
            <hr>
                <form class="pdpa-form">

                    <div class="form-group">
                    <label>ชื่อและนามสกุลที่ต้องการส่งคำขอ</label>
                        <div class="input-row">
                            <input type="text" placeholder="ชื่อ" name="first_name" class="input">
                            <input type="text" placeholder="นามสกุล" name="last_name" class="input">
                        </div>
                    </div>
    
                      <div class="form-group">
                        <label>เบอร์โทรศัพท์</label> 
                        <input type="tel" placeholder="เบอร์โทรศัพท์" name="telnumber" class="input">
                    </div>

                    <div class="form-group">
                        <label>อีเมลของคุณ</label> 
                        <input type="email" placeholder="อีเมล" name="email" class="input">
                    </div>

                     <div class="form-group">
                        <label>ที่อยู่</label> 
                        <textarea name="addr" placeholder="ที่อยู่"  rows="4" style="width:100%;"></textarea>
                    </div>

                    <div class="form-group">
                    <label for="type">คุณต้องการใช้สิทธิเรื่องใด:</label>
                        <div class="checkbox-group">
                            <label><input type="checkbox" name="rights" value="access"> สิทธิในการเข้าถึงข้อมูล</label>
                            <label><input type="checkbox" name="rights" value="edit"> สิทธิในการแก้ไขข้อมูลให้ถูกต้อง</label>
                            <label><input type="checkbox" name="rights" value="delete"> สิทธิในการลบข้อมูล</label>
                            <label><input type="checkbox" name="rights" value="suspend"> สิทธิในการระงับการใช้ข้อมูล</label>
                            <label><input type="checkbox" name="rights" value="object"> สิทธิในการคัดค้านการประมวลผล</label>
                            <label><input type="checkbox" name="rights" value="transfer"> สิทธิในการโอนย้ายข้อมูล</label>
                        </div>
                    </div>

                    <div class="form-group">
                        <label>(ไม่จำเป็น) โปรดระบุเหตุผลหรือความจำเป็นในการใช้สิทธิของท่านโดยสังเขป</label> 
                        <textarea name="remark" placeholder="เหตุผล"  rows="4" style="width:100%;"></textarea>
                    </div>


                     <div class="form-group">
                         <label>โปรดอัปโหลดหลักฐานยืนยันตน 
                             <i class="fas fa-camera tooltip">
                                <span class="tooltiptext">กรุณาลบหรือปกปิดข้อมูลศาสนาของท่านก่อนอัปโหลดภาพบัตรประจำตัวประชาชน เนื่องจากเราไม่มีความจำเป็นต้องเก็บรวบรวมหรือใช้ข้อมูลส่วนบุคคลอ่อนไหวของท่าน</span>
                             </i>
                         </label>
                        <div class="file-upload">
                            <input type="text" placeholder="เลือกรูปภาพ" class="file-input-text" readonly>
                            <label class="file-button">
                                เลือก
                                <input type="file" class="button" hidden>
                            </label>
                        </div>
                    </div>






                    <br>
                    <button type="submit">ส่งข้อมูล </button>
                </form>


          </div>
             
       

        

        
      
       
    </div>

    
  



  

 <!-- อ้างอิงไฟล์ JavaScript -->
    <script src="Scripts/menuToggle.js"></script>
</body>

</html