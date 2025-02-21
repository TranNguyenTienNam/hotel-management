﻿using HotelManagement.Core;
using HotelManagement.MVVM.Model;
using HotelManagement.Object;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotelManagement.MVVM.ViewModel
{
    /// <summary>
    /// Interaction logic for EditRoomView.xaml
    /// </summary>
    class EditRoomViewModel : ObservableObject
    {
        //danh sach loai phong
        private int indexOfTypes { get; set; }
        public ObservableCollection<roomtype> RoomTypes { get; set; }
        public ObservableCollection<string> TypeNames { get; set; }

        //Textbox Room ID
        private int _id;
        public int ID { get { return _id; } set { _id = value; OnPropertyChanged(); } }

        //Textbox Room name
        private string _rname;
        public string RoomName { get { return _rname; } set { _rname = value; OnPropertyChanged(); } }

        //Textbox Notes
        private string _notes;
        public string Notes { get { return _notes; } set { _notes = value; OnPropertyChanged(); } }

        //Selected value of combobox
        private string _type;
        public string Type { get { return _type; } set { _type = value; OnPropertyChanged(); } }

        //Textbox Price/Day
        private int _price;
        public int Price { get { return _price; } set { _price = value; OnPropertyChanged(); } }

        //Textbox Max People
        private int _maxPeople;
        public int MaxPeople { get { return _maxPeople; } set { _maxPeople = value; OnPropertyChanged(); } }

        public ICommand SaveEditCommand { get; set; }
        public ICommand ClickExitCommand { get; set; }
        public ICommand RoomTypeSelectionChangedCommand { get; set; }

        public EditRoomViewModel(int RoomID)
        {
            RoomTypes = new ObservableCollection<roomtype>();
            TypeNames = new ObservableCollection<string>();
            loadRoomTypes();
            loadTypes();
            ID = RoomID;
            loadRoom(RoomID);

            SaveEditCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(RoomName) || Notes == null)
                    return false;
                else
                    return true;
            }, (p) =>
            {
                //MessageBox.Show(ID + RoomName + Type + Price + MaxPeople + Notes);
                saveRoomEdited();
            });

            ClickExitCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                p.Close();
                EventSystem.Publish<Message>(new Message { message = "refresh" });
            });

            RoomTypeSelectionChangedCommand = new RelayCommand<ComboBox>((p) =>
            {
                return true;
            }, (p) =>
            {
                try
                {
                    indexOfTypes = p.SelectedIndex;
                    if (indexOfTypes >= 0 && indexOfTypes < RoomTypes.Count)
                    {
                        Price = RoomTypes[indexOfTypes].DonGia;
                        MaxPeople = RoomTypes[indexOfTypes].SoNgToiDa;
                    }    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("EditRoomViewModel RoomTypeSelectionChangedCommand\n" + ex.Message);
                }
            });
        }

        void saveRoomEdited()
        {
            try
            {
                RoomListModel model = new RoomListModel();

                if (model.Save_RoomEdited(ID, RoomName, RoomTypes[indexOfTypes].MaLoaiPhong, Notes))
                {
                    MessageBox.Show("Room has been edited.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("EditRoomViewModel saveRoomEdited\n" + ex.Message);
            }
        }

        public void loadRoom(int MaPhong)
        {
            RoomListModel model = new RoomListModel();
            DataTable dataTable = new DataTable();

            dataTable = model.GetRoom(MaPhong);

            foreach (DataRow row in dataTable.Rows)
            {
                RoomName = (string)row["TenPhong"];
                Type = (string)row["TenLoaiPhong"];
                Price = (int)row["DonGia"];
                MaxPeople = (int)row["SoNgToiDa"];
                foreach(var rt in RoomTypes)
                {
                    if (Type == rt.TenLoaiPhong && Price == rt.DonGia && MaxPeople == rt.SoNgToiDa)
                        indexOfTypes = RoomTypes.IndexOf(rt);
                }    
                Notes = (row["GhiChu"] == DBNull.Value) ? "" : (string)row["GhiChu"];
            }    
        }

        void loadRoomTypes()
        {
            if (RoomTypes.Count > 0)
                RoomTypes.Clear();

            DataTable dataTable = new DataTable();
            RoomsModel model = new RoomsModel();
            dataTable = model.Load_RoomType();

            foreach (DataRow row in dataTable.Rows)
            {
                var obj = new roomtype()
                {
                    MaLoaiPhong = (int)row["MaLoaiPhong"],
                    TenLoaiPhong = (string)row["TenLoaiPhong"],
                    DonGia = (int)row["DonGia"],
                    SoNgToiDa = (int)row["SoNgToiDa"]
                };
                RoomTypes.Add(obj);
            }
        }

        void loadTypes()
        {
            try
            {
                if (TypeNames.Count > 0)
                    TypeNames.Clear();

                foreach (roomtype rt in RoomTypes)
                {
                    string type = rt.TenLoaiPhong;
                    TypeNames.Add(type);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("RoomsViewModel LoadTypes\n" + ex.Message);
            }
        }
    }
}
