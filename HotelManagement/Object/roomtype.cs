﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Object
{
    class roomtype
    {
        int maLoaiPhong;
        string tenLoaiPhong;
        int donGia;
        int soNgToiDa;

        public int MaLoaiPhong { get => maLoaiPhong; set => maLoaiPhong = value; }
        public string TenLoaiPhong { get => tenLoaiPhong; set => tenLoaiPhong = value; }
        public int DonGia { get => donGia; set => donGia = value; }
        public int SoNgToiDa { get => soNgToiDa; set => soNgToiDa = value; }
    }
}