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
    public class Product_CartDAL
    {
        public bool addProductCart(Product_Cart productCart)
        {
            string sql = "INSERT Product_Cart VALUES(@Email, @ProductId, @Quantity)";
            SqlParameter emailParam = new SqlParameter("@Email", productCart.Email);
            SqlParameter productIdParam = new SqlParameter("@ProductId", productCart.Product_ID);
            SqlParameter quantityParam = new SqlParameter("@Quantity", productCart.Quantity);
            bool result = DataProvider.ExecuteNonQuery(sql, CommandType.Text, emailParam, productIdParam, quantityParam);
            return result;
        }

        public List<Product_Cart> getProductCartsByEmail(string email)
        {
            List<Product_Cart> result = new List<Product_Cart>();
            string sql = "SELECT * FROM Product_Cart WHERE Email = N'" + email + "'";
            SqlDataReader reader = DataProvider.ExecuteQueryWithDataReader(sql, CommandType.Text);
            while (reader.Read())
            {
                int productId = reader.GetInt32(1);
                ProductDAL productDal = new ProductDAL();
                Product product = productDal.getProductById(productId);

                Product_Cart productCart = new Product_Cart()
                {
                    Email = reader.GetString(0),
                    Product_ID = reader.GetInt32(1),
                    Quantity = reader.GetInt32(2),
                    Product = product,
                };

                result.Add(productCart);
            }
            return result;
        }

        public bool updateQuantity(Product_Cart productCart)
        {
            string sql = "UPDATE Product_Cart"
                + " SET Quantity = @Quantity"
                + " WHERE Email = @Email AND Product_ID = @ProductId";
            SqlParameter quantityParam = new SqlParameter("@Quantity", productCart.Quantity);
            SqlParameter emailParam = new SqlParameter("@Email", productCart.Email);
            SqlParameter productIdParam = new SqlParameter("@ProductId", productCart.Product_ID);
            bool result = DataProvider.ExecuteNonQuery(sql, CommandType.Text, quantityParam, emailParam, productIdParam);
            return result;
        }

        public bool deleteRecord(int productId, string email)
        {
            string sql = "DELETE FROM Product_Cart WHERE Product_ID = @productId AND Email = @email";
            SqlParameter productIdParam = new SqlParameter("@productId", productId);
            SqlParameter emailParam = new SqlParameter("@email", email);
            bool result = DataProvider.ExecuteNonQuery(sql, CommandType.Text, productIdParam, emailParam);
            return result;
        }
    }
}
