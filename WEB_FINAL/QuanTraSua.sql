--Tạo database
CREATE DATABASE QLQuanTraSua
GO

USE QLQuanTraSua
GO

--Tạo bảng Quản lý
Create table dbo.QUANLY(
MaQL int primary key,
TenQL nvarchar(50) not NULL,
Email varchar(50) not NULL,
Tuoi int not NULL,
DiaChi nvarchar(100) not NULL,
SDT char(10) not NULL,
Luong int not NULL,
HinhAnh nvarchar(50) NULL,
)

GO
insert into QUANLY values(18110203,N'Huỳnh Nhựt Thiên','18110203@gmail.com',20,N'Quận Thủ Đức','0968892926',150000000,N'avatar.png')
insert into QUANLY values(18110205,N'Đinh Minh Thiện','18110205@gmail.com',20,N'Quận 9','0900000000',150000000,N't1.jpg')
--Tạo bảng Chi Nhánh
Create table dbo.CHINHANH(
MaCN int primary key,
TenCN nvarchar(50) not NULL,
DiaChi nvarchar(100) not NULL,
SDT char(10) not NULL,
MaQL int references QUANLY(MaQL)
)
GO

insert into CHINHANH values(101,N'Milk Tea DoubleTee Cở Sở 1',N'Quận 1','0111222333',18110203)
insert into CHINHANH values(102,N'Milk Tea DoubleTee Cở Sở 2',N'Quận 2','0222111333',18110203)
insert into CHINHANH values(103,N'Milk Tea DoubleTee Cở Sở 3',N'Quận 3','0333222111',18110205)

--Tạo bảng loại menu
Create table dbo.LOAIMENU(
MaLoai int primary key,
TenLoai nvarchar(100) not NULL
)

insert into LOAIMENU values(1,N'Trà sữa')
insert into LOAIMENU values(2,N'Đá xay')
insert into LOAIMENU values(3,N'Topping')

--Tạo bảng Menu
Create table dbo.MENU(
MaMon nvarchar(10) primary key,
MaLoai int references LOAIMENU(MaLoai),
TenMon nvarchar(100) not NULL,
Gia int not NULL,
SoLuongDaBan int not NULL default(0),
HinhAnh nvarchar(MAX) NULL
)
GO

insert into MENU values('TS01',1,N'Trà sữa Okinawa',35000,12,N'Hinh-Web-OKINAWA-TRÀ-SỮA.png')
insert into MENU values('TS02',1,N'Trà sữa Xoài',35000,5,N'Mango-Milktea.png')
insert into MENU values('TS03',1,N'Trà sữa cà phê Okinawa',38000,9,N'Okinawa-Coffee-Milk-Tea.png')
insert into MENU values('TS04',1,N'Trà sữa Okinawa (bọt sữa mịn)',38000,4,N'Okinawa-Milk-Foam-Smoothie.png')
insert into MENU values('TS05',1,N'Trà sữa Cà phê',35000,7,N'Trà-sữa-cà-phê.png')
insert into MENU values('TS06',1,N'Trà sữa Chocolate',38000,8,N'Trà-sữa-Chocolate-2.png')
insert into MENU values('TS07',1,N'Trà sữa Earl-Grey',38000,2,N'Trà-sữa-Earl-Grey-2.png')
insert into MENU values('TS08',1,N'Trà sữa Hokkaido',38000,16,N'Trà-sữa-Hokkaido-2.png')
insert into MENU values('TS09',1,N'Trà sữa Khoai môn',35000,2,N'Trà-sữa-Khoai-môn-2.png')
insert into MENU values('TS10',1,N'Trà sữa Matcha đậu đỏ',38000,8,N'Trà-sữa-Matcha-đậu-đỏ-2.png')
insert into MENU values('TS11',1,N'Trà sữa Oolong',35000,3,N'Trà-sữa-Oolong-2.png')
insert into MENU values('TS12',1,N'Trà sữa Oolong 3J',38000,2,N'Trà-sữa-Oolong-3J-2.png')
insert into MENU values('TS13',1,N'Trà sữa Pudding đậu đỏ',38000,7,N'Trà-sữa-Pudding-đậu-đỏ-2.png')
insert into MENU values('TS14',1,N'Trà sữa Sương Sáo',35000,6,N'Trà-sữa-sương-sáo.png')
insert into MENU values('TS15',1,N'Trà sữa Trà đen',32000,5,N'Trà-sữa-trà-đen-3.png')
insert into MENU values('TS16',1,N'Trà sữa Trân châu đen',32000,18,N'Trà-sữa-Trân-châu-đen-1.png')
insert into MENU values('TS17',1,N'Trà sữa Trà xanh',32000,3,N'Trà-sữa-trà-xanh-1.png')

insert into MENU values('DX01',2,N'Chocolate đá xay',30000,3,N'Chocolate-đá-xay-2.png')
insert into MENU values('DX02',2,N'Khoai môn đá xay',30000,2,N'Khoai-môn-đá-xay-2.png')
insert into MENU values('DX03',2,N'Matcha đá xay',30000,6,N'Matcha-đá-xay-2.png')
insert into MENU values('DX04',2,N'Okinawa Oreo kem đá xay',32000,8,N'Okinawa-Oreo-Cream-Milk-Tea.png')
insert into MENU values('DX05',2,N'Oreo Dâu đá xay',30000,3,N'Strawberry-Oreo-Smoothie.png')
insert into MENU values('DX06',2,N'Xoài đá xay',30000,1,N'Xoài-đá-xay-2.png')
insert into MENU values('DX07',2,N'Yakult đá xay',30000,2,N'Yakult-đá-xay-2.png')

insert into MENU values('TP01',3,N'Combo 2 loại hạt',10000,3,N'Combo2loaihat-2.png')
insert into MENU values('TP02',3,N'Combo 3 loại hạt',15000,6,N'Combo-3-loại-hạt.png')
insert into MENU values('TP03',3,N'Đậu đỏ',10000,4,N'Đậu-Đỏ.png')
insert into MENU values('TP04',3,N'Hạt é',10000,8,N'Hạt-É.png')
insert into MENU values('TP05',3,N'Kem sữa',10000,9,N'Kem-Sữa.png')
insert into MENU values('TP06',3,N'Nha đam',10000,3,N'Nha-Đam.png')
insert into MENU values('TP07',3,N'Sương sáo',10000,11,N'pudding.png')
insert into MENU values('TP08',3,N'Trân châu trắng vị dâu',10000,14,N'Sương-sáo.png')
insert into MENU values('TP09',3,N'Thạch Ai-yu',10000,5,N'Tc-trang-vi-dau.png')
insert into MENU values('TP10',3,N'Thạch dừa',10000,9,N'Thạch-Ai-yu.png')
insert into MENU values('TP11',3,N'Thạch trái cây',10000,18,N'Thạch-Dừa.png')
insert into MENU values('TP12',3,N'Trân châu đen',10000,17,N'Thạch-trái-cây.png')
insert into MENU values('TP13',3,N'Trân châu trắng',10000,10,N'Trân-Châu-Đen.png')
insert into MENU values('TP14',3,N'Pudding',15000,9,N'Trân-Châu-Trắng.png')
GO

--Tạo bảng Nhân Viên
Create table dbo.NHANVIEN(
MaNV int primary key,
TenNV nvarchar(50) not NULL,
Email varchar(50) not NULL,
Tuoi int not NULL,
ChucVu nvarchar(20) NULL,
SDT char(10) not NULL,
DiaChi nvarchar(100) not NULL,
MaCN int references CHINHANH(MACN),
Luong int not NULL
)
GO
insert into NHANVIEN values(1001,N'Nguyễn Văn Chiến','chiennv@gmail.com',32,N'Pha Chế','0901010101',N'Quận 1',101,15000000)
insert into NHANVIEN values(1002,N'Đoàn Thị Mai Hương','huongdtm@gmail.com',31,N'Pha Chế','0902020202',N'Quận 2',102,12000000)
insert into NHANVIEN values(1003,N'Trần Hoàng Nhân','nhanth@gmail.com',35,N'Pha Chế','0903030303',N'Quận 3',103,12000000)
insert into NHANVIEN values(1004,N'Võ Hà Uyên Thư','thuvhu@gmail.com',25,N'Thu Ngân','0904040404',N'Quận 1',101,10000000)
insert into NHANVIEN values(1005,N'Võ Quang Tuấn','tuanvq@gmail.com',27,N'Thu Ngân','0905050505',N'Quận 2',102,10000000)
insert into NHANVIEN values(1006,N'Nguyễn Đức Chánh','chanhnd@gmail.com',20,N'Bảo vệ','0975724427',N'Quận 1',101,8000000)
insert into NHANVIEN values(1007,N'Hư Trúc','hut@gmail.com',25,N'Tạp Vụ','0907070707',N'Quận 3',103,7000000)
insert into NHANVIEN values(1008,N'Vương Ngữ Yên','yenvn@gmail.com',20,N'Tạp Vụ','0908080808',N'Quận 2',102,7000000)
insert into NHANVIEN values(1009,N'Đoàn Dự','doand@gmail.com',20,N'Tạp Vụ','0909090909',N'Quận 1',101,7000000)
insert into NHANVIEN values(1010,N'Mộ Dung Phục','phucmd@gmail.com',20,N'Tạp Vụ','0910101010',N'Quận 1',101,7000000)
insert into NHANVIEN values(1011,N'Tiêu Phong','tieup@gmail.com',27,N'Tạp Vụ','0901010111',N'Quận 2',102,7000000)
insert into NHANVIEN values(1012,N'Quách Tĩnh','tinhq@gmail.com',35,N'Part time','0901010112',N'Quận 2',102,7000000)
insert into NHANVIEN values(1013,N'Hoàng Dung','dungh@gmail.com',32,N'Part time','0901010113',N'Quận 1',101,7000000)
insert into NHANVIEN values(1014,N'Dương Quá','quad@gmail.com',20,N'Part time','0901011114',N'Quận 3',103,7000000)
GO

--Tạo bảng Phân Ca
Create table dbo.PHANCA(
MaNV int references NHANVIEN(MaNV),
MaCN int references CHINHANH(MaCN),
NgayLamViec datetime NULL,
GioLamViec int NULL,
primary key(MaNV, MaCN)
)
GO

--Tạo bảng khách hàng
Create table dbo.KHACHHANG(
MaKH int primary key,
TenKH nvarchar(50) not NULL,
Email varchar(50) not NULL,
Tuoi int NULL,
SDT char(10) NULL,
DiaChi nvarchar(100) NULL,
TongChiTieu int NULL default(0),
CapBac nvarchar(50) NULL default('Nước'),
)
GO
insert into KHACHHANG values(1, N'Tiểu Long Nữ', 'coco@gmail.com',20,'0912312312', N'Cổ Mộ, núi Toàn Chân', 2102000, N'Trà Sữa')
insert into KHACHHANG values(2, N'Đoãn Chí Bình','yeumakhongdamnoi@gmail.com',22,'0912312333', N'Toàn Chân Giáo, núi Toàn Chân', 1560000, N'Đường')
go

--Tạo bảng khách hàng lạ
CREATE TABLE dbo.KHACHHANGLA(
MaKHLA int primary key,
HoTen nvarchar(50) NOT NULL,
EmailKH varchar(50) NOT NULL,
SoDienThoai char(10) NULL,
DiaChiKH nvarchar(100) NULL
)
GO




--Tạo bảng Tài khoản
Create table dbo.TAIKHOANQL(
UserName varchar(50) not NULL,
Pass varchar(20) not null default(0),
MaQL int references QUANLY(MaQL),
primary key(MaQL)
)
GO
insert into TAIKHOANQL values('thienhn','12345',18110203)
insert into TAIKHOANQL values('thiendm','12345',18110205)


Create table dbo.TAIKHOANKH(
UserName varchar(50) not NULL,
Pass varchar(20) not null default(0),
MaKH int references KHACHHANG(MaKH) not null,
primary key (MaKH)
)
GO

insert into TAIKHOANKH values('yeutrasua01','12345',1)
insert into TAIKHOANKH values('thichsuachua02','12345',2)
GO

Create table dbo.TAIKHOANNV(
UserName varchar(50) not NULL,
Pass varchar(20) not null default(0),
MaNV int references NHANVIEN(MaNV),
primary key(MaNV)
)
GO
insert into TAIKHOANNV values('nv01','1',1001)
insert into TAIKHOANNV values('nv02','1',1002)
GO
--Tạo bảng Hóa đơn
Create table dbo.HOADON(
MaHD int primary key,
MaKH int references KHACHHANG(MaKH),
MaCN int references CHINHANH(MaCN),
NgayXuatHD datetime not NULL,
TongGia int not NULL,
TinhTrangHD nvarchar(100) NULL
)
GO

--Tạo bảng Chi Tiết Hóa Đơn
Create table dbo.CHITIETHD(
MaHD int references HOADON(MaHD),
MaCN int references CHINHANH(MaCN),
MaMon nvarchar(10) references MENU(MaMon),
Ban char(10) NULL,
Gia int not NULL,
SoLuong int not NULL,
primary key(MaHD, MaCN, MaMon)
)
GO

--Tạo bảng Khuyến Mãi
Create table dbo.KHUYENMAI(
MaKM char(10) primary key,
TenKM nvarchar(100) not NULL,
DieuKienCapBac nvarchar(50) not NULL,
GhiChu varchar(200) NULL
)
GO

--Tạo bảng Giao Hàng
Create table dbo.GIAOHANG(
MaGH char(10) primary key,
MaHD int references HOADON(MaHD),
DiaChiGiao nvarchar(100) not NULL,
MaKH int references KHACHHANG(MaKH),
PhuongThucGiao nvarchar(50) not NULL,
KieuThanhToan nvarchar(50) not NULL,
BenChuyenPhat nvarchar(50) not NULL
)
GO

--Tạo bảng Giỏ Hàng
Create table dbo.GIOHANG(
MaGio int primary key,
MaMon nvarchar(10) references MENU(MaMon),
MaKH int references KHACHHANG(MaKH),
MaHD int references HOADON(MaHD),
MaGH char(10) references GIAOHANG(MaGH),
MaKM char(10) references KHUYENMAI(MaKM)
)
GO

--trigger bảng TAIKHOANQL
create trigger KiemtraTKQL on TAIKHOANQL
after insert
as
declare @newtk varchar(50), @maql int
select @newtk = UserName, @maql = MaQL
from inserted
begin
	declare @ma int
	set @ma = 0

	select @ma = MaKH
	from TAIKHOANKH
	where UserName = @newtk

	select @ma = MaNV
	from TAIKHOANNV
	where UserName = @newtk

	if(@ma != 0)
	begin
		delete from TAIKHOANQL where MaQL = @maql
		delete from QUANLY where MaQL = @maql
	end
end
go

--test
select * from TAIKHOANQL
select * from QUANLY

insert into QUANLY values(123,N'noname','18110205@gmail.com',20,N'Quận 9','0900000000',150000000)
insert into TAIKHOANQL values('thiendm','12345', 123)

drop trigger KiemtraTKQL

delete from TAIKHOANQL where MaQL = 123
go
--

--trigger bảng TAIKHOANNV
create trigger KiemtraTKNV on TAIKHOANNV
after insert
as
declare @newtk varchar(50), @manv int
select @newtk = UserName, @manv = MaNV
from inserted
begin
	declare @ma int
	set @ma = 0

	select @ma = MaQL
	from TAIKHOANQL
	where UserName = @newtk

	select @ma = MaKH
	from TAIKHOANKH
	where UserName = @newtk

	if(@ma != 0)
	begin
		delete from TAIKHOANNV where MaNV = @manv
		delete from NHANVIEN where MaNV = @manv
	end
end
go

--trigger bảng TAIKHOANKH
create trigger KiemtraTKKH on TAIKHOANKH
after insert
as
declare @newtk varchar(50), @makh int
select @newtk = UserName, @makh = MaKH
from inserted
begin
	declare @ma int
	set @ma = 0

	select @ma = MaQL
	from TAIKHOANQL
	where UserName = @newtk

	select @ma = MaNV
	from TAIKHOANNV
	where UserName = @newtk

	if(@ma != 0)
	begin
		delete from TAIKHOANKH where MaKH = @makh
		delete from KHACHHANG where MaKH = @makh
	end
end
go

--để không trùng tài khoản trên cùng 1 bảng
--bảng TAIKHOANQL
ALTER TABLE TAIKHOANQL
ADD CONSTRAINT MyUniqueConstraint UNIQUE(UserName)
GO
--bảng TAIKHOANKH
ALTER TABLE TAIKHOANKH
ADD CONSTRAINT MyUniqueConstraintKH UNIQUE(UserName)
GO
--bảng TAIKHOANNV
ALTER TABLE TAIKHOANNV
ADD CONSTRAINT MyUniqueConstraintNV UNIQUE(UserName)
GO

Create trigger TangBacKH_nvarchar on KHACHHANG
after update
as
declare @MaKH int, @newCT int, @oldCT int
select @MaKH = ne.MaKH, @newCT = ne.TongChiTieu, @oldCT = ol.TongChiTieu
from inserted as ne, deleted as ol
where ne.MaKH = ol.MaKH
BEGIN
	if(@newCT - @oldCT > 0)
	begin
		if(@newCT >= 0 and @newCT < 500000)
		begin
			update KHACHHANG set CapBac = N'Nước' where MaKH = @MaKH
		end
		else if(@newCT >= 500000 and @newCT < 1000000)
		begin
			update KHACHHANG set CapBac = N'Trà' where MaKH = @MaKH
		end
		else if(@newCT >= 1000000 and @newCT < 1500000)
		begin
			update KHACHHANG set CapBac = N'Sữa' where MaKH = @MaKH
		end
		else if(@newCT >= 1500000)
		begin
			update KHACHHANG set CapBac = N'Trà Sữa' where MaKH = @MaKH
		end
	end
END
go