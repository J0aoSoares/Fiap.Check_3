using Fiap.Check_3.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace Fiap.Check_3.Controller
{
    public class ProductController
    {
        public static void AddProduct(Product product)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO products (name, price, quantity) VALUES (:name, :price, :quantity)";
                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("name", product.Name));
                    cmd.Parameters.Add(new OracleParameter("price", product.Price));
                    cmd.Parameters.Add(new OracleParameter("quantity", product.Quantity));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<Product> GetAllProducts()
        {
            var list = new List<Product>();
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM products";
                using (var cmd = new OracleCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Product
                        {
                            Name = reader["name"].ToString(),
                            Price = Convert.ToDecimal(reader["price"]),
                            Quantity = Convert.ToInt32(reader["quantity"])
                        });
                    }
                }
            }
            return list;
        }
    }
}
