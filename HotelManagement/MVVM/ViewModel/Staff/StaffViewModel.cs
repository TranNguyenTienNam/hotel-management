﻿using HotelManagement.Object;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using HotelManagement.Core;
using System.Data;
using HotelManagement.MVVM.Model;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using HotelManagement.MVVM.View;

namespace HotelManagement.MVVM.ViewModel
{
    class StaffViewModel : ObservableObject
    {
        private ObservableCollection<StaffItemViewModel> staff;
        public ObservableCollection<StaffItemViewModel> Staff { get { return staff; } set { staff = value; OnPropertyChanged("Staff"); } }

        private string selectedMode;
        public string SelectedMode { get { return selectedMode; } set { selectedMode = value; OnPropertyChanged("mode"); } }

        private List<string> _itemsSearch;
        public List<string> ItemsSearch { get { return _itemsSearch; } set { _itemsSearch = value; OnPropertyChanged(); } }

        #region Search box element
        //Item search
        private string selectedSearchItem;
        public string SelectedSearchItem { get { return selectedSearchItem; } set { selectedSearchItem = value; OnPropertyChanged(); } }
        //Search text
        private string searchText;
        public string SearchText { get { return searchText; } set { searchText = value; OnPropertyChanged(); } }
        #endregion

        private int Position { get; set;}

        private int _widthHeaderEmail;
        public int WidthHeaderEmail { get { return _widthHeaderEmail; } set { _widthHeaderEmail = value; OnPropertyChanged(); } }

        public ICommand RefreshCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public StaffViewModel(int position)
        {
            WidthHeaderEmail = (position == 0) ? 145 : 188;
            Position = position;
            Staff = new ObservableCollection<StaffItemViewModel>();
            LoadAllAcounts();

            ItemsSearch = new List<string>();
            ItemsSearch.Add("Staff ID");
            ItemsSearch.Add("User Name");
            ItemsSearch.Add("First Name");
            ItemsSearch.Add("Phone");

            SearchCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(SelectedSearchItem))
                    return false;
                return true;
            }, (p) =>
            {
                SearchStaff();
            });

            RefreshCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (Staff.Count > 0)
                    Staff.Clear();
                SelectedSearchItem = "";
                SearchText = "";
                LoadAllAcounts();
            });
        }

        public DelegateCommand ModeRadCommand
        {
            get
            {
                return new DelegateCommand((p) =>
                {
                    SelectedMode = (string)p;
                });
            }
        }

        void SearchStaff()
        {
            switch (SelectedSearchItem)
            {
                case "Staff ID":
                    LoadSearchStaffById(SearchText);
                    break;
                case "User Name":
                    LoadSearchStaffByUserName(SearchText);
                    break;
                case "First Name":
                    LoadSearchStaffByFirstName(SearchText);
                    break;
                case "Phone":
                    LoadSearchStaffByPhone(SearchText);
                    break;
                default:
                    break;
            }
        }

        void LoadData(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                var obj = new StaffItemViewModel()
                {
                    MaNguoiDung = (int)row["MaNguoiDung"],
                    TenTaiKhoan = (string)row["TenTaiKhoan"],
                    TenNhanVien = (string)row["Ten"],
                    SoDienThoai = (row["SoDienThoai"] == DBNull.Value) ? "" : (string)row["SoDienThoai"],
                    GioiTinh = (row["GioiTinh"] == DBNull.Value) ? "" : (string)row["GioiTinh"],
                    Email = (string)row["Email"],
                    NgaySinh = (row["NgaySinh"] == DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : ((DateTime)row["NgaySinh"]).ToString("dd/MM/yyyy"),
                    QuyenHan = definePosition((int)row["QuyenHan"]),
                    IsPromoted = ((int)row["QuyenHan"] == 2) ? false : true,
                    IsBlocked = ((int)row["TinhTrang"] == 0) ? true : false,
                    WidthColumnEmail = (Position == 0) ? 145 : 188,
                    VisibilityPromoteButton = (Position == 0) ? "Visible" : "Collapsed"
                };
                Staff.Add(obj);
            }
        }

        void LoadAllAcounts()
        {
            if (Staff.Count > 0)
                Staff.Clear();
            DataTable dataTable = new DataTable();
            StaffModel model = new StaffModel();

            dataTable = model.Load_Accounts(SelectedMode, Position);

            LoadData(dataTable);
        }

        void LoadSearchStaffById(string MaNgDung)
        {
            if (Staff.Count > 0)
                Staff.Clear();
            StaffModel model = new StaffModel();
            DataTable data = new DataTable();
            data = model.Search_StaffID(MaNgDung, SelectedMode, Position);

            LoadData(data);
        }

        void LoadSearchStaffByUserName(string TenTK)
        {
            if (Staff.Count > 0)
                Staff.Clear();
            StaffModel model = new StaffModel();
            DataTable data = new DataTable();
            data = model.Search_StaffUsername(TenTK, SelectedMode, Position);

            LoadData(data);
        }

        void LoadSearchStaffByFirstName(string Ten)
        {
            if (Staff.Count > 0)
                Staff.Clear();
            StaffModel model = new StaffModel();
            DataTable data = new DataTable();
            data = model.Search_StaffFirstName(Ten, SelectedMode, Position);

            LoadData(data);
        }

        void LoadSearchStaffByPhone(string sdt)
        {
            if (Staff.Count > 0)
                Staff.Clear();
            StaffModel model = new StaffModel();
            DataTable data = new DataTable();
            data = model.Search_StaffPhone(sdt, SelectedMode, Position);

            LoadData(data);
        }

        string definePosition(int posNumber)
        {
            string position = "";
            switch (posNumber)
            {
                case 0:
                    position = "Admin";
                    break;
                case 1:
                    position = "Manager";
                    break;
                case 2:
                    position = "Staff";
                    break;
                default:
                    break;
            }

            return position;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (SelectedSearchItem == "" || SelectedSearchItem == null) {
                LoadAllAcounts();
            } 
            else
            {
                SearchStaff();
            }
        }
    }
}
