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
        public DataSet getAllTown(int districtId)
        {
            DataSet dsTown = new DataSet();
            string SQL = "SELECT * FROM Town WHERE District_ID=@District_ID";
            try
            {
                SqlParameter districtID = new SqlParameter("@District_ID", districtId);
                dsTown = DataProvider.ExecuteQueryWithDataSet(SQL, CommandType.Text, districtID);
            }
            catch (SqlException se)
            {
                throw new Exception(se.Message);
            }
            return dsTown;
        }
        //-------------------------------------------------------------
        public DataSet getAllDistrict(int provinceId)
        {
            DataSet dsDistrict = new DataSet();
            string SQL = "SELECT * FROM District WHERE Province_ID=@Province_ID";
            try
            {
                SqlParameter provinceID = new SqlParameter("@Province_ID", provinceId);
                dsDistrict = DataProvider.ExecuteQueryWithDataSet(SQL, CommandType.Text, provinceID);
            }
            catch (SqlException se)
            {
                throw new Exception(se.Message);
            }
            return dsDistrict;
        }
    }
}

