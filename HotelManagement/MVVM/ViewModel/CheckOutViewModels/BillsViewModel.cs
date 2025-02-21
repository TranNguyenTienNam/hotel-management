﻿using System;
using System.Collections.ObjectModel;
using HotelManagement.Core;
using System.Data;
using System.Windows.Input;
using HotelManagement.MVVM.Model.CheckOut;
using ListView = System.Windows.Controls.ListView;
using HotelManagement.Object;
using HotelManagement.MVVM.View.CheckOutViews;
using System.Windows.Forms;

namespace HotelManagement.MVVM.ViewModel
{
    class BillsViewModel : ObservableObject
    {
        private ObservableCollection<Receipt> _items;
        public ObservableCollection<Receipt> Items { get { return _items; } set { _items = value; OnPropertyChanged("Items"); } }

        private string _searchText = "";
        public string SearchText { get { return _searchText; } set { _searchText = value; OnPropertyChanged(); } }
        // thuộc tính hóa đơn đầy đủ
        private int _maHoaDon;
        public int MaHoaDon { get { return _maHoaDon; } set { _maHoaDon = value; OnPropertyChanged(); } }

        private int _phuThu;
        public int PhuThu { get { return _phuThu; } set { _phuThu = value; OnPropertyChanged(); } }

        private int _tongTien;
        public int TongTien { get { return _tongTien; } set { _tongTien = value; OnPropertyChanged(); } }

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

        private int _soNgayThue;
        public int SoNgayThue { get { return _soNgayThue; } set { _soNgayThue = value; OnPropertyChanged(); } }

        private int _tongTienPhong;
        public int TongTienPhong { get { return _tongTienPhong; } set { _tongTienPhong = value; OnPropertyChanged(); } }

        private string _visibilityAdmin;
        public string VisibilityAdmin { get { return _visibilityAdmin; } set { _visibilityAdmin = value; OnPropertyChanged(); } }

        //command
        public ICommand SearchCommand { get; set; }
        public ICommand SelectRowCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand ExportReceiptCommand { get; set; }


        public BillsViewModel(int position)
        {
            //biến phụ đểc chứa item được chọn từ listview
            Receipt currentReceipt = new Receipt();
            VisibilityAdmin = (position == 2) ? "Collapsed" : "Visible";
            Items = new ObservableCollection<Receipt>();
            LoadListBills();

            ExportReceiptCommand = new RelayCommand<object>((p) =>
            {
                return !string.IsNullOrEmpty(CMND);
            }, (p) =>
            {
                ExportReceipt exportReceipt = new ExportReceipt();
                exportReceipt.DataContext = new ExportReceiptViewModel(currentReceipt);
                exportReceipt.ShowDialog();
            });

            SelectRowCommand = new RelayCommand<ListView>((p) =>
            {
                return !p.Items.IsEmpty;
            }, (p) =>
            {
                Receipt Item = p.SelectedItem as Receipt;
                MaHoaDon = Item.MaHoaDon;
                PhuThu = Item.PhuThu;
                TongTien = Item.TongTien;
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
                //gán item được chọn hiện tại cho current receipt
                currentReceipt = Item;
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
                    LoadListBills();
                }
                else
                {
                    LoadSearchByCMND();
                }
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(TenKH)) return false;
                return true;
            }, (p) =>
            {
                string message = "Are you sure to delete this receipt ?";
                string title = "Carefully!";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = System.Windows.Forms.MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    DeleteBill(MaHoaDon);
                    MessageBox.Show("Delete bill successful!");
                    //refresh lại list
                    Items.Clear();
                    ClearInfo();
                    LoadListBills();
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
                LoadListBills();
            });
        }

        private void DeleteBill(int maHoaDon)
        {
            BillsModel model = new BillsModel();
            model.Delete_Bills(maHoaDon);
        }

        private void LoadSearchByCMND()
        {
            if (Items.Count > 0)
            {
                Items.Clear();
            }
            BillsModel model = new BillsModel();
            DataTable data = new DataTable();
            data = model.Load_Search_CMND(SearchText);
            SetPropsFromData(data);
        }

        private int GetDays(DateTime ngayBD, DateTime ngayTP)
        {
            DateTime bd = ngayBD.Date;
            DateTime tp = ngayTP.Date;
            string re = tp.Subtract(bd).TotalDays.ToString();
            return int.Parse(re);
        }

        private void LoadListBills()
        {
            DataTable data = new DataTable();
            BillsModel model = new BillsModel();
            data = model.Load_List_Bills();
            SetPropsFromData(data);
        }

        public void SetPropsFromData(DataTable data)
        {
            foreach (DataRow row in data.Rows)
            {
                var obj = new Receipt()
                {
                    MaHoaDon = (int)row["MaHoaDon"],
                    PhuThu = (row["PhuThu"] == DBNull.Value) ? 0 : (int)row["PhuThu"],
                    TongTien = (row["TongTien"] == DBNull.Value ? 0 : (int)row["TongTien"]),
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
        private void ClearInfo()
        {
            SearchText = "";
            MaHoaDon = 0;
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
    }
}
