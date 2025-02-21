﻿using HotelManagement.Object;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Core;
using System.Data;
using HotelManagement.MVVM.Model;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;

namespace HotelManagement.MVVM.ViewModel
{
    /// <summary>
    /// Interaction logic for RoomListView.xaml
    /// </summary>
    public class RoomListViewModel : ObservableObject
    {
        public static RoomListViewModel Instance => new RoomListViewModel();

        //Mỗi phần tử của Items là 1 hàng của ListView
        private ObservableCollection<RoomListItemViewModel> _items;
        public ObservableCollection<RoomListItemViewModel> Items { get { return _items; } set { _items = value; OnPropertyChanged("Items"); } }

        public ICommand RefreshListRoom { get; set; }

        public RoomListViewModel()
        {
            Items = new ObservableCollection<RoomListItemViewModel>();
            loadListRoom();

            RefreshListRoom = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                loadListRoom();
            });

            //Nhận message từ RoomsViewModel
            EventSystem.Subscribe<Message>(getMessages);
        }

        public void getMessages(Message message)
        {
            try
            {
                if (message.message == "refresh")
                    loadListRoom();
                else if (message.message.Contains("ID|"))
                {
                    string[] vs = message.message.Split('|');
                    loadSearchRoombyId(vs[1]);
                }
                else if (message.message.Contains("Name|"))
                {
                    string[] vs = message.message.Split('|');
                    loadSearchRoombyName(vs[1]);
                }
                else if (message.message.Contains("RemoveRoom|"))
                {
                    string[] vs = message.message.Split('|');
                    Items.Remove(Items.Where(X => X.MaPhong == Convert.ToInt32(vs[1])).Single());
                }    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void loadSearchRoombyId(string MaPhong)
        {
            if (Items.Count > 0)
                Items.Clear();
            RoomListModel model = new RoomListModel();
            DataTable data = new DataTable();
            data = model.Search_RoomID(MaPhong);

            foreach (DataRow row in data.Rows)
            {
                var obj = new RoomListItemViewModel()
                {
                    MaPhong = (int)row["MaPhong"],
                    TenPhong = (string)row["TenPhong"],
                    LoaiPhong = (string)row["TenLoaiPhong"],
                    DonGia = (int)row["DonGia"],
                    SoNgToiDa = (int)row["SoNgToiDa"],
                    GhiChu = (row["GhiChu"] == DBNull.Value) ? "" : (string)row["GhiChu"]
                };
                Items.Add(obj);
            }
        }

        void loadSearchRoombyName(string TenPhong)
        {
            if (Items.Count > 0)
                Items.Clear();
            RoomListModel model = new RoomListModel();
            DataTable data = new DataTable();
            data = model.Search_RoomName(TenPhong);

            foreach (DataRow row in data.Rows)
            {
                var obj = new RoomListItemViewModel()
                {
                    MaPhong = (int)row["MaPhong"],
                    TenPhong = (string)row["TenPhong"],
                    LoaiPhong = (string)row["TenLoaiPhong"],
                    DonGia = (int)row["DonGia"],
                    SoNgToiDa = (int)row["SoNgToiDa"],
                    GhiChu = (row["GhiChu"] == DBNull.Value) ? "" : (string)row["GhiChu"]
                };
                Items.Add(obj);
            }
        }

        public void loadListRoom()
        {
            if (Items.Count > 0)
                Items.Clear();
            RoomListModel model = new RoomListModel();
            DataTable data = new DataTable();
            data = model.Load_On();
            foreach (DataRow row in data.Rows)
            {
                var obj = new RoomListItemViewModel()
                {
                    MaPhong = (int)row["MaPhong"],
                    TenPhong = (string)row["TenPhong"],
                    LoaiPhong = (string)row["TenLoaiPhong"],
                    DonGia = (int)row["DonGia"],
                    SoNgToiDa = (int)row["SoNgToiDa"],
                    GhiChu = (row["GhiChu"] == DBNull.Value) ? "" : (string)row["GhiChu"]
                };
                Items.Add(obj);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case "Items":
                    {
                        loadListRoom();
                    }
                    break;
                default:
                    break;
            }
        }
    } 
}
