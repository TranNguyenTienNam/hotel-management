﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace HotelManagement.MVVM.Model.CheckOut
{
    class SurchargeModel
    {
        public static string con_string = ConfigurationManager.ConnectionStrings["con"].ToString();

        public int Get_surcharge_more_client()
        {
            int re = 0;
            SqlConnection con = new SqlConnection(con_string);
            con.Open();
            String query = "select * from PHUTHU";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                re = (int)dr["KhachThu3"];
            }
            con.Close();
            return re;
        }
        public void Update_surcharge(int tyLePhuThu)
        {
            SqlConnection con = new SqlConnection(con_string);
            con.Open();
            String query = "update PHUTHU " +
                "set KhachThu3 = " + tyLePhuThu;

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteReader();
        }
    }
}
