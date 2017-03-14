using System.Data;
using DemoEntity.DataProvider;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Model.DAL
{
    public class Province_Town_DistrictDAL
    {
        public DataSet getProvince()
        {
            DataSet dsProvince = new DataSet();
            string SQL = "SELECT * FROM Province";
            try
            {
                dsProvince = DataProvider.ExecuteQueryWithDataSet(SQL, CommandType.Text);
            }
            catch (SqlException se)
            {
                throw new Exception(se.Message);
            }
            return dsProvince;
        }
        //-------------------------------------------------------
        public DataSet getAllTown(int provinceId)
        {
            DataSet dsTown = new DataSet();
            string SQL = "SELECT * FROM Town WHERE Province_ID=@Province_ID";
            try
            {
                SqlParameter provinceID = new SqlParameter("@Province_ID", provinceId);
                dsTown = DataProvider.ExecuteQueryWithDataSet(SQL, CommandType.Text, provinceID);
            }
            catch (SqlException se)
            {
                throw new Exception(se.Message);
            }
            return dsTown;
        }
        //-------------------------------------------------------------
        public DataSet getAllDistrict(int dictrictId)
        {
            DataSet dsDistrict = new DataSet();
            string SQL = "SELECT * FROM District WHERE District_ID=@District_ID";
            try
            {
                SqlParameter dictrictID = new SqlParameter("@District_ID", dictrictId);
                dsDistrict = DataProvider.ExecuteQueryWithDataSet(SQL, CommandType.Text, dictrictID);
            }
            catch (SqlException se)
            {
                throw new Exception(se.Message);
            }
            return dsDistrict;
        }
    }
}

