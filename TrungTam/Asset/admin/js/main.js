function getEle(id) {
    return document.getElementById(id);
}
//Khởi tạo đối tượng DSNV từ lớp DanhSachNhanVien
var DSNV = new DanhSachNhanVien();

//Hàm hiển thị form
// status= 1 => nhân vào nút thêm => sửa lại tiêu đề Thêm, hiện nút thêm
// status = 2 = > nhấn vào nút sửa => sửa lại tiều đề cập nhật, hiện nút cập nhật
function hienThiModal(status) {
    switch (status) {
        case 1:
            getEle('header-title').innerHTML = 'Thêm Nhân Viên';
            getEle('btnThemNV').style.display = 'inline';
            getEle('btnCapNhatNV').style.display = 'none';
            break;
        case 2:
            getEle('header-title').innerHTML = 'Cập Nhật Nhân Viên';
            getEle('btnThemNV').style.display = 'none';
            getEle('btnCapNhatNV').style.display = 'inline';
            break;
        default: return 0;
    }
}
//Kiểm tra đăng kí
function dangKi() {
    var isValid = true;
    // kiểm tra testbox mã

    if (kiemTraText('txt_hoten', 'tbTen', 'vui lòng nhập lại họ tên phải là chữ ') == false
        || kiemTraNhap('txt_hoten', 'tbTen', 'vui lòng nhập tên ') == false) {
        isValid = false
    }

    if (kiemTraNhap('txt_ngaysinh', 'tbNgay', 'vui lòng nhập ngày sinh') == false) {
        isValid = false;
    }

    //if (kiemTraNhap('khoi', 'tbkhoi', 'vui lòng nhập khối') == false) {
    //    isValid = false;
    //}
    //if (kiemTraNhap('truong', 'tbtruong', 'vui lòng nhập Đầy đủ họ tên trường') == false
    //    || (kiemTraText('truong', 'tbtruong', 'vui lòng nhập lại  tên trường phải là chữ ') == false) {
    //    isValid = false;
    //}
    if (kiemTraText('txt_truong', 'tbTruongHoc', 'vui lòng nhập lại  tên trường phải là chữ ') == false
        || kiemTraNhap('txt_truong', 'tbTruongHoc', 'vui lòng nhập Đầy đủ họ tên trường') == false) {
        isValid = false;
    }
    if (kiemTraNhap('txt_noidung', 'tbNhuCauHoc', 'vui lòng nhập nội dung') == false) {
        isValid = false;
    }
    if (kiemTraNhap('txt_sdt', 'tbSDT', 'vui lòng nhập số điện thoại') == false || kiemTraDoDai('txt_sdt', 'tbSDT', 'vui lòng nhập số điện thoại',10,11) == false || kiemTraSo('txt_sdt', 'tbSDT', 'vui lòng nhập số điện thoại phải là số') == false) {
        isValid = false;
    }

    //if (kiemTraText('phuhuynh', 'tbphuhuynh', 'vui lòng nhập lại họ tên phải là chữ ') == false
    //    || kiemTraNhap('phuhuynh', 'tbphuhuynh', 'vui lòng nhập tên ') == false) {
    //    isValid = false;
    //}
    if (kiemTraNhap('txt_diachi', 'tbDC', 'vui lòng nhập địa chỉ') == false) {
        isValid = false;
    }
    ////if (kiemTraDoDai('txt_diachi', 'tbDC', 'vui lòng nhập địa chỉ',5,7) == false) {
    ////    isValid = false;
    ////}

    //if (kiemTraDoDai('txt_diachi','tbdiachi','vui lòng nhập địa chỉ',5,6) == false)
    //     {
    //    isValid = false;
    //}

    return isValid;
}
//hàm thêm học sinh
function themHV() {
    var isValid = true;
    // kiểm tra testbox mã

    if (kiemTraText('name', 'tbTen', 'vui lòng nhập lại họ tên phải là chữ ') == false
        || kiemTraNhap('name', 'tbTen', 'vui lòng nhập tên ') == false) {
        isValid = false
    }

    if (kiemTraNhap('datepicker', 'tbNgay', 'vui lòng nhập ngày sinh') == false)
         {
        isValid = false;
    }

    if (kiemTraNhap('khoi', 'tbkhoi', 'vui lòng nhập khối') == false)
         {
        isValid = false;
    }
    //if (kiemTraNhap('truong', 'tbtruong', 'vui lòng nhập Đầy đủ họ tên trường') == false
    //    || (kiemTraText('truong', 'tbtruong', 'vui lòng nhập lại  tên trường phải là chữ ') == false) {
    //    isValid = false;
    //}
    if (kiemTraText('truong', 'tbtruong', 'vui lòng nhập lại  tên trường phải là chữ ') == false
        || kiemTraNhap('truong', 'tbtruong', 'vui lòng nhập Đầy đủ họ tên trường') == false) {
        isValid = false;
    }
    //if (kiemTraNhap('sdt', 'tbSDT', 'vui lòng nhập điện thoại') == false|| kiemTraDoDai('sdt', 'tbsdt', 'vui lòng nhập số điện thoại phải có', 10, 11) == false)
    //     {
    //    isValid = false;
    //}
    if (kiemTraNhap('sdt', 'tbSDT', 'vui lòng nhập số điện thoại') == false || kiemTraDoDai('sdt', 'tbSDT', 'vui lòng nhập số điện thoại', 10, 11) == false || kiemTraSo('sdt', 'tbSDT', 'vui lòng nhập số điện thoại phải là số') == false) {
        isValid = false;
    }
    
    if (kiemTraText('phuhuynh', 'tbphuhuynh', 'vui lòng nhập lại họ tên phải là chữ ') == false
        || kiemTraNhap('phuhuynh', 'tbphuhuynh', 'vui lòng nhập tên ') == false) {
        isValid = false;
    }
    if (kiemTraNhap('diachi', 'tbdiachi', 'vui lòng nhập địa chỉ') == false)
         {
        isValid = false;
    }

    //if (kiemTraNhap('diachi', 'tbdiachi', 'vui lòng nhập địa chỉ') == false)
    //     {
    //    isValid = false;
    //}

    return isValid;
}
//hàm thêm khối
function themKhoi() {
    var isValid = true;
    // kiểm tra testbox mã  
    if (kiemTraNhap('tenkhoi', 'tbKhoi', 'vui lòng nhập khối') == false) {
        isValid = false;
    }
    return isValid;
}
//hàm thêm loại lớp
function themLop() {
    var isValid = true;
    // kiểm tra testbox mã  
    if (kiemTraNhap('tenloai', 'tbTenLop', 'vui lòng nhập lớp') == false) {
        isValid = false;
    }
    return isValid;
}
//hàm thêm loại lớp
function themMonHoc() {
    var isValid = true;
    // kiểm tra testbox mã  
    if (kiemTraNhap('tenmon', 'tbTenMon', 'vui lòng nhập loại lớp') == false) {
        isValid = false;
    }
    return isValid;
}
//hàm thêm luơng
function themLLuong() {
    var isValid = true;
    // kiểm tra testbox mã  
    if (kiemTraNhap('tenloai', 'tbTen', 'vui lòng nhập tên loại lương') == false) {
        isValid = false;
    }
    if (kiemTraNhap('min', 'tbMin', 'vui lòng nhập không được bỏ trống') == false) {
        isValid = false;
    }
    if (kiemTraNhap('max', 'tbMax', 'vui lòng nhập không được bỏ trống') == false) {
        isValid = false;
    }
    if (kiemTraNhap('dongia', 'tbDG', 'vui lòng nhập đơn giá') == false) {
        isValid = false;
    }
    return isValid;
}
//hàm thêm khuyến mãi
function themKM() {
    var isValid = true;
    // kiểm tra testbox mã  
    if (kiemTraNhap('name', 'tbTen', 'vui lòng nhập tên loại khuyến mãi') == false) {
        isValid = false;
    }
    if (kiemTraNhap('soMDK', 'tbsomon', 'vui lòng nhập số môn quy định') == false) {
        isValid = false;
    }
    if (kiemTraNhap('datepicker', 'tbNgay', 'vui lòng nhập phần trăm giảm giá') == false) {
        isValid = false;
    }
    return isValid;
}
function GiaSu() {
    var isValid = true;
    if (kiemTraNhap('tenlop', 'tbTen', 'vui lòng nhập không được bỏ trống') == false) {
        isValid = false;
    }
    if (kiemTraNhap('monhoc', 'tbMonHoc', 'vui lòng nhập không được bỏ trống') == false) {
        isValid = false;
    }
    if (kiemTraNhap('monhoc', 'tbMonHoc', 'vui lòng nhập không được bỏ trống') == false) {
        isValid = false;
    }
    if (kiemTraNhap('thoigian', 'tbTG', 'vui lòng nhập không được bỏ trống') == false) {
        isValid = false;
    }
    if (kiemTraNhap('dongia', 'tbDG', 'vui lòng nhập không được bỏ trống') == false) {
        isValid = false;
    }
    if (kiemTraNhap('diachi', 'tbDC', 'vui lòng nhập không được bỏ trống') == false) {
        isValid = false;
    }
    if (kiemTraNhap('yeucau', 'tbYC', 'vui lòng nhập không được bỏ trống') == false) {
        isValid = false;
    }
    if (kiemTraNhap('lienhe', 'tbLH', 'vui lòng nhập không được bỏ trống') == false) {
        isValid = false;
    }
    return isValid;

}
function LuongNG() {
    var isValid = true;
    if (kiemTraNhap('tenloai', 'tbTenLoai', 'vui lòng nhập không được bỏ trống') == false) {
        isValid = false;
    }
    if (kiemTraNhap('dongia', 'tbTendg', 'vui lòng nhập không được bỏ trống') == false) {
        isValid = false;
    }
    return isValid;
}
    function CTNgoai() {
        var isValid = true;
        if (kiemTraNhap('name', 'tbTen', 'vui lòng nhập không được bỏ trống') == false) {
            isValid = false;
        }
        if (kiemTraNhap('datepicker', 'tbNgay', 'vui lòng nhập không được bỏ trống') == false) {
            isValid = false;
        }
        if (kiemTraNhap('tien', 'tbTT', 'vui lòng nhập không được bỏ trống') == false) {
            isValid = false;
        }
        return isValid;
    }
//hàm thêm chấm công ngoài giờ.
function themgio() {
    var isValid = true;
    // kiểm tra testbox mã  
    if (kiemTraNhap('soGio', 'tbSoGio', 'vui lòng nhập số giờ') == false) {
        isValid = false;
    }
    if (kiemTraNhap('datepicker', 'tbNgay', 'vui lòng ngày') == false) {
        isValid = false;
    }
    return isValid;
}
//hàm thêm GV
function themGV() {
    var isValid = true;
    // kiểm tra testbox mã

    if (kiemTraText('name', 'tbTen', 'vui lòng nhập lại họ tên phải là chữ ') == false
        || kiemTraNhap('name', 'tbTen', 'vui lòng nhập tên ') == false) {
        isValid = false
    }

    if (kiemTraEmail('email', 'tbEmail', 'vui lòng nhập đúng email ') == false
        || kiemTraNhap('email', 'tbEmail', 'vui lòng nhập email ') == false) {
        isValid = false;
    }

    //if (kiemTraNhap('sdt', 'tbsdt', 'vui lòng nhập số điện thoại') == false
    //    || kiemTraDoDai('sdt', 'tbsdt', 'vui lòng nhập số điện thoại phải có', 10, 11) == false) {
    //    isValid = false;
    //}
    if (kiemTraNhap('sdt', 'tbsdt', 'vui lòng nhập số điện thoại') == false || kiemTraDoDai('sdt', 'tbsdt', 'vui lòng nhập số điện thoại', 10, 11) == false || kiemTraSo('sdt', 'tbsdt', 'vui lòng nhập số điện thoại phải là số') == false) {
        isValid = false;
    }
    //if (kiemTraNhap('gioitinh', 'tbgioitinh', 'vui lòng nhập giới tính') == false) {
    //    isValid = false;
    //}
    if (kiemTraNhap('datepicker', 'tbNgay', 'vui lòng nhập ngày sinh') == false) {
        isValid = false;
    }
    

    return isValid;

}
//Hàm Thêm Nhân Viên
function themNhanVien() {
    var isValid = true;
    // kiểm tra testbox mã

    if (kiemTraNhap('name','tbTen','vui lòng nhập mã nhân ') == false 
        || kiemTraDoDai('name','tbTen','vui lòng nhập mã nhân viên phải có',5,10) == false) {
       isValid = false;
   }

    if(kiemTraText('name','tbTen','vui lòng nhập lại họ tên phải là chữ ') == false 
    || kiemTraNhap('name','tbTen','vui lòng nhập mã nhân ') == false){
        isValid = false
    }
    if(kiemTraEmail('email','tbEmail','*email không hợp lệ') == false){
        isValid = false;
    }
    if(!isValid )
    {
        return;
    }
    var maNV = getEle('msnv').value;
    var tenNV = getEle('name').value;
    var email = getEle('email').value;
    var matKhau = getEle('password').value;
    var ngayBatDau = getEle('datepicker').value;
    var chucVu = getEle('chucvu').value;

    var nhanVien = new NhanVien(maNV, tenNV, email, matKhau, ngayBatDau, chucVu);
    //Gọi phương thức tinhLuong => có được tổng lương
    nhanVien.TinhLuong();

    DSNV.ThemNhanVien(nhanVien);
    //=> DSNV.MangNhanVien.push(nhanVien);

    //tạo bảng
    taoBang(DSNV.MangNhanVien);
    
    //Đóng modal trigger
    getEle('btnDong').click();
    //clear form
    var inputFields = document.getElementsByClassName('input-sm');
    for(var i =0 ; i < inputFields.length;i++){
        inputFields[i].value = '';
    }
}
//-------VALIDATION--------
function kiemTraEmail(idInput, idSpan, content) {
    var value = getEle(idInput).value;
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if( re.test(String(value).toLowerCase())){
        getEle(idSpan).style.display = "none"
        return true;
    }else
    {
        getEle(idSpan).style.display = "block";
        getEle(idSpan).innerHTML = content;
        return false;
    }
}
function kiemTraNhap(idInput,idspan,content ){
    var value = getEle(idInput).value;
    if(value.length == 0){
        getEle(idspan).style.display = "block"
        getEle(idspan).innerHTML = content;
        return false;
    }
    else
    {
        getEle(idspan).style.display = "none"
        return true;
    }
   
}
function kiemTraSo(idInput, idSpan, content) {
    var value = getEle(idInput).value;
    if (isNaN(value)) {
        getEle(idSpan).style.display = "block"
        getEle(idSpan).innerHTML = `${content}`;
        return false;
    }
    else {
        getEle(idSpan).style.display = "none"
        return true;
    }

}
function kiemTraDoDai(idInput, idSpan, content, min, max){
    var value = getEle(idInput).value;
    if (value.length > max || value.length < min) {
        getEle(idSpan).style.display = "block"
        getEle(idSpan).innerHTML = `${content} từ ${min} đến ${max} kí tự`;
        return false;
    }
    else
    {
        getEle(idSpan).style.display = "none"
        return true;
    }

}
function kiemTraText(idinput, idspan, content) {
    var value = getEle(idinput).value;
    var pattern = new RegExp("^[a-zA-Z_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶ" +
        "ẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆẾỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤÚỦỨỪễệỉịọỏốồổỗộớờởỡợ" +
        "ụúủứừÚỬỮỰỲỴÝỶỸửữựỳýỵỷỹ\\s]+$")
    if (!pattern.test(value)) {
        getEle(idspan).style.display = "block"
        getEle(idspan).innerHTML = content;
        return false;

    }
    else
        getEle(idspan).style.display = "none"
    return true;


}
//---end--------
//hàm tạo bảng
function taoBang(mangNhanVien){
    var content = '';
    for(var i = 0 ; i <mangNhanVien.length;i++){
        var nhanVien = mangNhanVien[i];
        content +=`
            <tr>
                <td>${nhanVien.MaNV}</td>
                <td>${nhanVien.TenNhanVien}</td>
                <td>${nhanVien.Email}</td>
                <td>${nhanVien.NgayBatDau}</td>
                <td>${nhanVien.ChucVu}</td>
                <td>${nhanVien.TongLuong}</td>
                <td>${nhanVien.MatKhau}</td>
                <td>
                    <button data-manv="${nhanVien.MaNV}" class="btn btn-success btnXoa">Xóa</button>
                    <button 
                    data-manv="${nhanVien.MaNV}"
                    data-tennv="${nhanVien.TenNhanVien}"
                    data-email="${nhanVien.Email}"
                    data-ngaybatdau="${nhanVien.NgayBatDau}"
                    data-chucvu="${nhanVien.ChucVu}"
                    data-matkhau="${nhanVien.MatKhau}"
                     onclick="layThongTinCapNhat(this)"
                     data-toggle = "modal"
                     data-target= "#myModal" class="btn btn-info btnSua">Sửa</button>
                </td>
            </tr>
        `
    }
    getEle('tableDanhSach').innerHTML = content;
    xoaNhanVien();
}
//hàm xóa nhân viên
function xoaNhanVien(){
    var btnDeletes = document.getElementsByClassName('btnXoa');
    for (var i = 0; i < btnDeletes.length; i++) {
        btnDeletes[i].addEventListener('click', function () {
            //code chạy khi click vào nút xóa
            var maNV = this.getAttribute('data-manv');
            DSNV.XoaNhanVien(maNV);
            taoBang(DSNV.MangNhanVien)
        })
    }

}
//Tìm nhân viên theo Mã
// function timNhanVienTheoMa(){
//     var keyword = getEle('searchName').value;
//     console.log(keyword);
//     if(keyword.length > 0){
//         taoBang(DSNV.timNhanVienTheoMa(keyword));
//     }
//     else{
//         taoBang(DSNV.MangNhanVien);
//     }
// }
function timNhanVienTheoTen(){
    var keyword = getEle('searchName').value;
    var nhanVientheoMa = DSNV.timNhanVienTheoMa(keyword);
    if (nhanVientheoMa.length > 0) {
        taoBang(nhanVientheoMa);
    } else
        taoBang(DSNV.timNhanVienTheoTen(keyword));
}
function layThongTinCapNhat(self) {

    var maNV = self.getAttribute('data-manv');
    var tenNV = self.getAttribute('data-tennv');
    var Email = self.getAttribute('data-email');
    var ngayBatDau = self.getAttribute('data-ngaybatdau');
    var ChucVu = self.getAttribute('data-chucvu');
    var MatKhau = self.getAttribute('data-matkhau');

    // đẩy thông tin lên form
    console.log(self);
    getEle('msnv').value = maNV;
    getEle('name').value = tenNV;
    getEle('email').value = Email;
    getEle('password').value = MatKhau;
    getEle('datepicker').value = ngayBatDau;
    getEle('chucvu').value = ChucVu;
    //đổi tiê đề
    hienThiModal(2);
    // var nhanVien = new NhanVien(maNV, tenNV, email, matKhau, ngayBatDau, chucVu);
    getEle('msnv').setAttribute('readonly',true);


}
function CapNhatND(){
    var maNV = getEle('msnv').value;
    var tenNV = getEle('name').value;
    var email = getEle('email').value;
    var matKhau = getEle('password').value;
    var ngayBatDau = getEle('datepicker').value;
    var chucVu = getEle('chucvu').value;

    var nhanVienEdit = new NhanVien(maNV, tenNV, email, matKhau, ngayBatDau, chucVu);
    nhanVienEdit.TinhLuong();
    DSNV.capNhatNhanVien(nhanVienEdit);
    taoBang(DSNV.MangNhanVien);
     //Đóng modal trigger
     getEle('btnDong').click();
     //gắng thuộc tính read only


}
//----------------GẮN SỰ KIỆN---------------------------
getEle('btnThem').addEventListener('click', function () {
    getEle('msnv').removeAttribute('readonly');
    hienThiModal(1);
})
getEle('btnThemNV').addEventListener('click', themGV);
// getEle('btnTimNV').addEventListener('click',timNhanVienTheoMa);
getEle('searchName').addEventListener('keyup',timNhanVienTheoTen);
getEle('btnCapNhatNV').addEventListener('click',CapNhatND);
