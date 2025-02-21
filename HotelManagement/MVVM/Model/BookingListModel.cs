﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace HotelManagement.MVVM.Model
{
    class BookingListModel
    {
        #region LoadBooking by (...) Listview from Database
        public DataTable LoadBooking()
        {
            string sql_select = "select * from KHACHHANG k,PHIEUTHUEPHONG p where k.CMND = p.CMND";

            return Process.createTable(sql_select);
        }
        public DataTable LoadBookingbyRoomId(string RoomID)
        {
            string sql_select = "select * from KHACHHANG k,PHIEUTHUEPHONG p " +
                "where k.CMND = p.CMND  and CHARINDEX(N'" + RoomID + "', MaPhong) != 0";

            return Process.createTable(sql_select);
        }
        public DataTable LoadBookingbyString(string String)
        {
            string sql_select = "select * from KHACHHANG k,PHIEUTHUEPHONG p " +
                "where k.CMND = p.CMND  and ( CHARINDEX(N'" + String + "', MaPhong) != 0 " +
                "or CHARINDEX(N'" + String + "', p.CMND) != 0  " +
                "or CHARINDEX(N'" + String + "', MaPhieuThue) != 0  " +
                "or CHARINDEX(N'" + String + "', TenKH) != 0  " +
                "or CHARINDEX(N'" + String + "', SoDienThoai) != 0  )";

            return Process.createTable(sql_select);
        }
        public DataTable LoadBookingbyCitizentID(string CitizentID)
        {
            string sql_select = "select * from KHACHHANG k,PHIEUTHUEPHONG p " +
                "where k.CMND = p.CMND  and CHARINDEX(N'" + CitizentID + "', p.CMND) != 0";

            return Process.createTable(sql_select);
        }
        public DataTable LoadBookingbyRentalId(string ID)
        {
            string sql_select = "select * from KHACHHANG k,PHIEUTHUEPHONG p " +
                "where k.CMND = p.CMND  and CHARINDEX(N'" + ID + "', MaPhieuThue) != 0";

            return Process.createTable(sql_select);
        }
        public DataTable LoadBookingbyClientName(string ClientName)
        {
            string sql_select = "select * from KHACHHANG k,PHIEUTHUEPHONG p " +
                "where k.CMND = p.CMND  and CHARINDEX(N'" + ClientName + "', TenKH) != 0";

            return Process.createTable(sql_select);
        }
        public DataTable LoadBookingbyPhone(string Phone)
        {
            string sql_select = "select * from KHACHHANG k,PHIEUTHUEPHONG p " +
                "where k.CMND = p.CMND  and CHARINDEX(N'" + Phone + "', SoDienThoai) != 0";

            return Process.createTable(sql_select);
        }
        #endregion

        #region Info Rental from Database
        public DataTable LoadClientInformation(string CMND)
        {
            string sql_select = "select lkh.TenLoaiKhach as TenLoaiKhach, kh.SoDienThoai as SoDienThoai, kh.GioiTinh as GioiTinh, kh.DiaChi as DiaChi "
                + "from KHACHHANG kh, LOAIKHACHHANG lkh where kh.MaLoaiKhach = lkh.MaLoaiKhach and kh.CMND = '" + CMND + "'";

            return Process.createTable(sql_select);
        }

        public DataTable LoadRentalInformation(int MaPhieuThue)
        {
            string sql_select = "select p.MaPhong as MaPhong, lp.TenLoaiPhong as TenLoaiPhong, lp.DonGia as DonGia, " +
                "ptp.SoLuongKhach as SoLuongKhach, ptp.NgayTraPhong as NgayTraPhong, ttng.MaNgDung as MaNgDung, ttng.Ten as Ten ,ptp.TinhTrang as TinhTrang " +
                "from PHIEUTHUEPHONG ptp, PHONG p, LOAIPHONG lp, TTNguoiDung ttng " +
                "where ptp.MaPhong = p.MaPhong and p.MaLoaiPhong = lp.MaLoaiPhong and ttng.MaNgDung = ptp.NguoiLapPhieu " +
                "and ptp.MaPhieuThue = " + MaPhieuThue;
            return Process.createTable(sql_select);
        }

        #endregion


        #region Modify Rental
        public bool Update_Rental(int RentalID,int Deposit,int Amount)
        {
            string sql_update = "UPDATE PHIEUTHUEPHONG " +
                "SET TienCoc ="+ Deposit +" , SoLuongKhach = " + Amount +
                "WHERE MaPhieuThue =" + RentalID;
            if (Process.ExecutiveNonQuery(sql_update) > 0)
                return true;
            return false;
        }
        public bool Delete_Rental(int RentalID)
        {
            string sql_update = "DELETE FROM PHIEUTHUEPHONG " +
                                "WHERE MaPhieuThue =" + RentalID;
            if (Process.ExecutiveNonQuery(sql_update) > 0)
                return true;
            return false;
        }
        public bool Checkin_Rental(int RentalID)
        {
            string sql_update = "UPDATE PHIEUTHUEPHONG " +
                                "SET TinhTrang ='Check-in'" +
                                "WHERE MaPhieuThue = " + RentalID;
            if (Process.ExecutiveNonQuery(sql_update) > 0)
                return true;
            return false;
        }
        #endregion

        //Get Username by Id
        public int GetPermision(int MaNgDung)
        {
            string sql_select = "SELECT QuyenHan FROM TTNguoiDung WHERE MaNgDung=" + MaNgDung;

            return Process.getNumber(sql_select);
        }


    }
}