﻿using DemoEntity.DataProvider;
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
    public class CatalogueDAL
    {
        public List<Catalogue> getAllCatalogues()
        {
            List<Catalogue> catalogues = new List<Catalogue>();
            string sql = "SELECT * FROM Catalogue";
            try
            {
                SqlDataReader reader = DataProvider.ExecuteQueryWithDataReader(sql, CommandType.Text);
                while (reader.Read())
                {
                    Catalogue catalogue = new Catalogue
                    {
                        Catalogue_ID = reader.GetInt32(0),
                        Catalogue_Name = reader.GetString(1),
                    };
                    catalogues.Add(catalogue);
                }
            } catch( SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return catalogues;
        }

        public void getAllProductsByCatalogue(Catalogue catalogue)
        {
            String sql = "SELECT *"
                + " FROM Product"
                + " WHERE Catalogue_ID = " + catalogue.Catalogue_ID;
            try
            {
                SqlDataReader reader = DataProvider.ExecuteQueryWithDataReader(sql, CommandType.Text);
                while (reader.Read())
                {
                    Product product = new Product
                    {
                        Product_ID = reader.GetInt32(0),
                        Catalogue_ID = reader.GetInt32(1),
                        Is_Sale = reader.GetBoolean(2),
                        Product_Name = reader.GetString(3),
                        Price = reader.GetFloat(4),
                        Level_Trending = reader.GetInt32(5),
                        Description = reader.GetString(6),
                        Products_Available = reader.GetInt32(7),
                        Total_Sold = reader.GetInt32(8),
                        Created_Date = reader.GetDateTime(9),
                        Created_Username = reader.GetString(10),
                        Guarantee_Description = reader.GetString(11),
                        Title_Image = reader.GetString(12),
                        Tax_Percent = reader.GetFloat(13),
                        Manufacturer = reader.GetString(14),
                    };
                    ProductDAL productDal = new ProductDAL();
                    productDal.getAllPromotions(product);
                    catalogue.Products.Add(product);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void getProductsByCatalogue(Catalogue catalogue, int quantity)
        {
            String sql = "SELECT *"
                + " FROM Product"
                + " WHERE Catalogue_ID = " + catalogue.Catalogue_ID;
            List<Product> products = new List<Product>();
            try
            {
                SqlDataReader reader = DataProvider.ExecuteQueryWithDataReader(sql, CommandType.Text);
                while (reader.Read())
                {
                    Product product = new Product
                    {
                        Product_ID = reader.GetInt32(0),
                        Catalogue_ID = reader.GetInt32(1),
                        Is_Sale = reader.GetBoolean(2),
                        Product_Name = reader.GetString(3),
                        Price = reader.GetFloat(4),
                        Level_Trending = reader.GetInt32(5),
                        Description = reader.GetString(6),
                        Products_Available = reader.GetInt32(7),
                        Total_Sold = reader.GetInt32(8),
                        Created_Date = reader.GetDateTime(9),
                        Created_Username = reader.GetString(10),
                        Guarantee_Description = reader.GetString(11),
                        Title_Image = reader.GetString(12),
                        Tax_Percent = reader.GetFloat(13),
                        Manufacturer = reader.GetString(14),
                    };
                    ProductDAL productDal = new ProductDAL();
                    productDal.getAllPromotions(product);
                    products.Add(product);
                }
                var random = new Random();
                var shuffledList = products.OrderBy(item => random.Next());
                int count = 0;
                foreach (var item in shuffledList)
                {
                    catalogue.Products.Add(item);
                    count++;
                    if (count >= quantity)
                        break;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Catalogue getCatalogueById(int id)
        {
            string sql = "SELECT * FROM Catalogue WHERE Catalogue_ID = " + id;
            try
            {
                SqlDataReader reader = DataProvider.ExecuteQueryWithDataReader(sql, CommandType.Text);
                if (reader.Read())
                {
                    Catalogue catalogue = new Catalogue
                    {
                        Catalogue_ID = reader.GetInt32(0),
                        Catalogue_Name = reader.GetString(1),
                    };
                    return catalogue;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }
    }
}
