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
    public class ProductOrderDetailsDAL
    {
        public bool addProductOrderDetails(Product_Order_Details pod)
        {
            string sql = "INSERT Product_Order_Details "
                 + "VALUES(@productId, @orderId, @orderQuantity, @price, @discountPercent, '')";
            SqlParameter productIdParam = new SqlParameter("@productId", pod.Product_ID);
            SqlParameter orderIdParam = new SqlParameter("@orderId", pod.Order_ID);
            SqlParameter quantityParam = new SqlParameter("@orderQuantity", pod.Order_Quantity);
            SqlParameter priceParam = new SqlParameter("@price", pod.Price);
            SqlParameter discountParam = new SqlParameter("@discountPercent", pod.Discount_Percent);            
            return DataProvider.ExecuteNonQuery(sql, CommandType.Text, productIdParam,  orderIdParam, quantityParam, priceParam, discountParam);
        }
    }
}
