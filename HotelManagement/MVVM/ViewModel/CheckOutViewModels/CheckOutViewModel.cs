﻿using System;
using System.Collections.ObjectModel;
using HotelManagement.Core;
using System.Data;
using HotelManagement.MVVM.Model;
using System.Windows.Controls;
using System.Windows.Input;
using HotelManagement.MVVM.View.CheckOutViews;
using HotelManagement.MVVM.Model.CheckOut;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using ListView = System.Windows.Controls.ListView;
using HotelManagement.Object;

namespace HotelManagement.MVVM.ViewModel
{
    class CheckOutViewModel : ObservableObject
    {
        private ObservableCollection<Rental> _items;
        public ObservableCollection<Rental> Items { get { return _items; } set { _items = value; OnPropertyChanged("Items"); } }

        private string _searchText = "";
        public string SearchText { get { return _searchText; } set { _searchText = value; OnPropertyChanged(); } }

        //thuộc tính phiếu thuê đầy đủ
        private int _maPhieuThue;
        public int MaPhieuThue { get { return _maPhieuThue; } set { _maPhieuThue = value; OnPropertyChanged(); } }

        private int _maphong;
        public int MaPhong { get { return _maphong; } set { _maphong = value; OnPropertyChanged(); } }

        private DateTime _ngayBatDau;
        public DateTime NgayBatDau { get { return _ngayBatDau; } set { _ngayBatDau = value; OnPropertyChanged(); } }

        private DateTime _ngayTraPhong;
        public DateTime NgayTraPhong { get { return _ngayTraPhong; } set { _ngayTraPhong = value; OnPropertyChanged(); } }

        private int _soLuongKhach;
        public int SoLuongKhach { get { return _soLuongKhach; } set { _soLuongKhach = value; OnPropertyChanged(); } }

        private String _tinhTrang;
        public String TinhTrang { get { return _tinhTrang; } set { _tinhTrang = value; OnPropertyChanged(); } }

        private string _nguoiLapPhieu;
        public string NguoiLapPhieu { get { return _nguoiLapPhieu; } set { _nguoiLapPhieu = value; OnPropertyChanged(); } }

        private int _tiencoc;
        public int TienCoc { get { return _tiencoc; } set { _tiencoc = value; OnPropertyChanged(); } }

        private DateTime _ngayLapPhieu;
        public DateTime NgayLapPhieu { get { return _ngayLapPhieu; } set { _ngayLapPhieu = value; OnPropertyChanged(); } }

        private String _tenKhachHang;
        public String TenKH { get { return _tenKhachHang; } set { _tenKhachHang = value; OnPropertyChanged(); } }

        private String _CMND;
        public String CMND { get { return _CMND; } set { _CMND = value; OnPropertyChanged(); } }

        private String _soDienThoai;
        public String SoDienThoai { get { return _soDienThoai; } set { _soDienThoai = value; OnPropertyChanged(); } }

        private String _diaChi;
        public String DiaChi { get { return _diaChi; } set { _diaChi = value; OnPropertyChanged(); } }

        private String _gioiTinh;
        public String GioiTinh { get { return _gioiTinh; } set { _gioiTinh = value; OnPropertyChanged(); } }

        int _maLoaiKhach;
        public int MaLoaiKhach { get { return _maLoaiKhach; } set { _maLoaiKhach = value; OnPropertyChanged(); } }

        private String _tenLoaiKhach;
        public String TenLoaiKhach { get { return _tenLoaiKhach; } set { _tenLoaiKhach = value; OnPropertyChanged(); } }

        private String _tenPhong;
        public String TenPhong { get { return _tenPhong; } set { _tenPhong = value; OnPropertyChanged(); } }

        private int _donGia;
        public int DonGia { get { return _donGia; } set { _donGia = value; OnPropertyChanged(); } }

        private int _soNguoiToiDa;
        public int SoNgToiDa { get { return _soNguoiToiDa; } set { _soNguoiToiDa = value; OnPropertyChanged(); } }

        private String _ghiChu;
        public String GhiChu { get { return _ghiChu; } set { _ghiChu = value; OnPropertyChanged(); } }

        private int _maLoaiPhong;
        public int MaLoaiPhong { get { return _maLoaiPhong; } set { _maLoaiPhong = value; OnPropertyChanged(); } }

        private String _tenLoaiPhong;
        public String TenLoaiPhong { get { return _tenLoaiPhong; } set { _tenLoaiPhong = value; OnPropertyChanged(); } }

        // thuộc tính dựa vào phiếu thuê tính được tính được 
        private int _soNgayThue;
        public int SoNgayThue { get { return _soNgayThue; } set { _soNgayThue = value; OnPropertyChanged(); } }

        private int _phuThu;
        public int PhuThu { get { return _phuThu; } set { _phuThu = value; OnPropertyChanged(); } }

        private int _tongTienPhong;
        public int TongTienPhong { get { return _tongTienPhong; } set { _tongTienPhong = value; OnPropertyChanged(); } }

        private int _tongTien;
        public int TongTien { get { return _tongTien; } set { _tongTien = value; OnPropertyChanged(); } }

        //command
        public ICommand BillsCommand { get; set; }
        public ICommand SurchargeCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand CheckOutCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand SelectRowCommand { get; set; }
        public ICommand PickCheckOutDateCommand { get; set; }
        public ICommand ExportBillCommand { get; set; }

        public CheckOutViewModel(int Position)
        {
            //biến phụ để chứa item được chọn của listview
            Rental currentRental = new Rental();

            Items = new ObservableCollection<Rental>();
            LoadListRent();

            ExportBillCommand = new RelayCommand<object>((p) =>
            {
                return !string.IsNullOrEmpty(CMND);
            }, (p) =>
            {
                ExportBill exportBill = new ExportBill();
                exportBill.DataContext = new ExportBillViewModel(currentRental);
                exportBill.ShowDialog();
            });

            CheckOutCommand = new RelayCommand<object>((p) =>
            {
                return !string.IsNullOrEmpty(CMND);
            }, (p) =>
            {
                string message = "Are you sure to check-out now ?";
                string title = "Carefully!";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = System.Windows.Forms.MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    Checkout();
                    MessageBox.Show("Check-out successful!");
                    Items.Clear();
                    LoadListRent();
                    ClearInfo();
                }
                else
                {
                    //không làm gì cả
                }
            });

            RefreshCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                Items.Clear();
                ClearInfo();
                LoadListRent();
            });

            BillsCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                BillsView bv = new BillsView();
                bv.DataContext = new BillsViewModel(Position);
                bv.ShowDialog();
            });

            SurchargeCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                SurchargeView sv = new SurchargeView();
                sv.ShowDialog();
            });

            SearchCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (SearchText == "")
                {
                    Items.Clear();
                    ClearInfo();
                    LoadListRent();
                }
                else
                {
                    LoadSearchRoomName();
                    ClearInfo();
                }
            });

            SelectRowCommand = new RelayCommand<ListView>((p) =>
            {
                return !p.Items.IsEmpty;
            }, (p) =>
            {
                Rental Item = p.SelectedItem as Rental;
                MaPhieuThue = Item.MaPhieuThue;
                TenKH = Item.TenKH;
                GioiTinh = Item.GioiTinh;
                MaLoaiKhach = Item.MaLoaiKhach;
                TenLoaiKhach = Item.TenLoaiKhach;
                DiaChi = Item.DiaChi;
                CMND = Item.CMND;
                SoDienThoai = Item.SoDienThoai;
                TenPhong = Item.TenPhong;
                TenLoaiPhong = Item.TenLoaiPhong;
                DonGia = Item.DonGia;
                SoNgToiDa = Item.SoNgToiDa;
                SoLuongKhach = Item.SoLuongKhach;
                NgayLapPhieu = Item.NgayLapPhieu;
                NgayBatDau = Item.NgayBatDau;
                NgayTraPhong = Item.NgayTraPhong;
                TienCoc = Item.TienCoc;
                SoNgayThue = GetDays(Item.NgayBatDau, Item.NgayTraPhong);
                TongTienPhong = Item.DonGia * SoNgayThue;
                PhuThu = GetSurchargeMoney(SoLuongKhach, SoNgayThue, TongTienPhong, SoNgToiDa);
                TongTien = TongTienPhong + PhuThu - TienCoc;
                NguoiLapPhieu = Item.NguoiLapPhieu;
                // gán item hiện tại để truyền vào export bill
                currentRental = Item;
            });

            PickCheckOutDateCommand = new RelayCommand<DatePicker>((p) =>
            {
                if (p.SelectedDate <= NgayBatDau)
                {
                    MessageBox.Show("Ngày Check-out không được nhỏ hơn ngày Check-in !");
                    p.SelectedDate = NgayTraPhong;
                    // tính lại số ngày thuê, tiền thuê...
                    NgayTraPhong = p.SelectedDate.HasValue ? p.SelectedDate.Value.Date : NgayBatDau;
                    SoNgayThue = GetDays(NgayBatDau, NgayTraPhong);
                    TongTienPhong = DonGia * SoNgayThue;
                    PhuThu = GetSurchargeMoney(SoLuongKhach, SoNgayThue, TongTienPhong, SoNgToiDa);
                    TongTien = TongTienPhong + (int)PhuThu - TienCoc;
                    //set lại ngày trả phòng cho current rental
                    currentRental.NgayTraPhong = NgayTraPhong;

                    return false;
                }
                return !string.IsNullOrEmpty(CMND);
            }, (p) =>
            {
                // tính lại số ngày thuê, tiền thuê...
                NgayTraPhong = p.SelectedDate.HasValue ? p.SelectedDate.Value.Date : NgayBatDau;
                SoNgayThue = GetDays(NgayBatDau, NgayTraPhong);
                TongTienPhong = DonGia * SoNgayThue;
                PhuThu = GetSurchargeMoney(SoLuongKhach, SoNgayThue, TongTienPhong, SoNgToiDa);
                TongTien = TongTienPhong + (int)PhuThu - TienCoc;
                //set lại ngày trả phòng cho current rental
                currentRental.NgayTraPhong = NgayTraPhong;
            });
        }

        private void ClearInfo()
        {
            SearchText = "";
            MaPhieuThue = 0;
            TenKH = "";
            GioiTinh = "";
            MaLoaiKhach = 0;
            TenLoaiKhach = "";
            DiaChi = "";
            CMND = "";
            SoDienThoai = "";
            TenPhong = "";
            TenLoaiPhong = "";
            DonGia = 0;
            SoNgToiDa = 0;
            SoLuongKhach = 0;
            NgayLapPhieu = new DateTime();
            NgayBatDau = new DateTime();
            NgayTraPhong = new DateTime();
            TienCoc = 0;
            SoNgayThue = 0;
            TongTienPhong = 0;
            PhuThu = 0;
            TongTien = 0;
        }

        private void Checkout()
        {
            // set lại ngày trả phòng, tình trạng thành 'Checkout'
            CheckOutModel checkOutModel = new CheckOutModel();
            checkOutModel.Change_Checkout_Date_And_Set_Checkout(NgayTraPhong, MaPhieuThue);
            // tạo hóa đơn xuống database
            BillsModel billsModel = new BillsModel();
            billsModel.Insert_Bill(MaPhieuThue, PhuThu, TongTien);
        }

        private int GetDays(DateTime ngayBD, DateTime ngayTP)
        {
            DateTime bd = ngayBD.Date;
            DateTime tp = ngayTP.Date;
            string re = tp.Subtract(bd).TotalDays.ToString();
            return int.Parse(re);
        }

        private int GetSurchargeMoney(int soLuongkhach, int soNgayThue, int tongTienPhong, int soNguoiToiDa)
        {
            SurchargeModel surchargeModel = new SurchargeModel();
            int tyLePhuThu = surchargeModel.Get_surcharge_more_client();
            if (soLuongkhach > soNguoiToiDa)
            {
                return (soLuongkhach - soNguoiToiDa) * tyLePhuThu * tongTienPhong / 100;
            }
            return 0;
        }

        private void LoadSearchRoomName()
        {
            if (Items.Count > 0)
            {
                Items.Clear();
            }
            CheckOutModel model = new CheckOutModel();
            DataTable data = new DataTable();
            data = model.Load_List_Rent_By_Room(SearchText);
            //gán giá trị cho các item listview từ datatable load được
            SetPropsFromData(data);
        }

        private void LoadListRent()
        {
            DataTable data = new DataTable();
            CheckOutModel model = new CheckOutModel();
            data = model.Load_List_Rent();
            SetPropsFromData(data);
        }

        private void SetPropsFromData(DataTable data)
        {
            foreach (DataRow row in data.Rows)
            {
                var obj = new Rental()
                {
                    MaPhieuThue = (int)row["MaPhieuThue"],
                    NgayLapPhieu = (DateTime)row["NgayLapPhieu"],
                    NgayBatDau = (DateTime)row["NgayBatDau"],
                    NgayTraPhong = (row["NgayTraPhong"] == DBNull.Value ? DateTime.Now.Date : (DateTime)row["NgayTraPhong"]),
                    SoLuongKhach = (row["SoLuongKhach"] == DBNull.Value ? 0 : (int)row["SoLuongKhach"]),
                    TinhTrang = (string)row["TinhTrang"],
                    NguoiLapPhieu = (row["Ten"] == DBNull.Value ? string.Empty : (string)row["Ten"]),
                    TienCoc = (row["TienCoc"] == DBNull.Value ? 0 : (int)row["TienCoc"]),
                    TenKH = (string)row["TenKH"],
                    CMND = (string)row["CMND"],
                    SoDienThoai = (string)row["SoDienThoai"],
                    DiaChi = (string)row["DiaChi"],
                    GioiTinh = (string)row["GioiTinh"],
                    MaLoaiKhach = (int)row["MaLoaiKhach"],
                    TenLoaiKhach = (row["TenLoaiKhach"] == DBNull.Value ? string.Empty : (string)row["TenLoaiKhach"]),
                    MaPhong = (int)row["MaPhong"],
                    TenPhong = (string)row["TenPhong"],
                    GhiChu = (row["GhiChu"] == DBNull.Value) ? string.Empty : (string)row["GhiChu"],
                    MaLoaiPhong = (int)row["MaLoaiPhong"],
                    TenLoaiPhong = (string)row["TenLoaiPhong"],
                    DonGia = (int)row["DonGia"],
                    SoNgToiDa = (int)row["SoNgToiDa"]
                };
                Items.Add(obj);
            }
        }
    }
}
