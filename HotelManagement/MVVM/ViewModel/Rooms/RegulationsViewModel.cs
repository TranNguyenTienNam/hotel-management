﻿using HotelManagement.Core;
using HotelManagement.MVVM.Model;
using HotelManagement.Object;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotelManagement.MVVM.ViewModel
{
    class RegulationsViewModel : ObservableObject
    {
        public static RegulationsViewModel Instance => new RegulationsViewModel();

        private string _typeName;
        public string TypeName { get { return _typeName; } set { _typeName = value; OnPropertyChanged(); } }

        private ulong _price;
        public ulong Price { get { return _price; } set { _price = value; OnPropertyChanged(); } }

        private uint _maxPeople;
        public uint MaxPeople { get { return _maxPeople; } set { _maxPeople = value; OnPropertyChanged(); } }

        public ICommand AddRoomTypeCommand { get; set; }

        public RegulationsViewModel()
        {

            AddRoomTypeCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(TypeName) || Price <= 1000 || MaxPeople <= 0)
                    return false;
                return true;
            }, (p) =>
            {
                RegulationsModel model = new RegulationsModel();
                if (model.Insert_Type(TypeName, Price, MaxPeople))
                {
                    MessageBox.Show("Type of Room has been added!");
                }
            });
        }
    }
}
