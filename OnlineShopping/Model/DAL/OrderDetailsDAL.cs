using DemoEntity.DataProvider;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public class OrderDetailsDAL
    {
        public int addOrder(Order_Details order)
        {
            string sql = "INSERT Order_Details OUTPUT Inserted.Order_ID " +
                "VALUES(@Email, @Ordered_Date, @Delivered, @Cancelled_Order, @Reason_Cancel, @Order_Name, @Exact_Address, @Order_Phone, @Province_ID, @District_ID, @Town_ID)";
            SqlParameter emailParam = new SqlParameter("@Email", order.Email);
            SqlParameter ordereddateParam = new SqlParameter("@Ordered_Date", order.Ordered_Date);
            SqlParameter deliveredParam = new SqlParameter("@Delivered", order.Delivered);
            SqlParameter cancelOrderParam = new SqlParameter("@Cancelled_Order", order.Cancelled_Order);
            SqlParameter reasonCancelParam = new SqlParameter("@Reason_Cancel", order.Reason_Cancel);
            SqlParameter orderNameParam = new SqlParameter("@Order_Name", order.Order_Name);
            SqlParameter addressParam = new SqlParameter("@Exact_Address", order.Exact_Address);
            SqlParameter phoneParam = new SqlParameter("@Order_Phone", order.Order_Phone);
            SqlParameter provinceParam = new SqlParameter("@Province_ID", order.Province_ID);
            SqlParameter districtParam = new SqlParameter("@District_ID", order.District_ID);
            SqlParameter townParam = new SqlParameter("@Town_ID", order.Town_ID);
            int result = DataProvider.ExecuteScalarQuery(sql, CommandType.Text, emailParam, ordereddateParam, deliveredParam, cancelOrderParam,
               reasonCancelParam, orderNameParam, addressParam, phoneParam, provinceParam, districtParam, townParam);
            return result;
        }
    }
}
