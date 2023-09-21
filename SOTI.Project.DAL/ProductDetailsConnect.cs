using SOTI.Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SOTI.Project.DAL
{
    public class ProductDetailsConnect : IProduct
    {
        private SqlConnection _connection = null;
        private SqlCommand _command = null;
        private SqlDataReader _reader = null;

        public List<Product> GetAllProduct()
        {
            List<Product> products = new List<Product>();
            using (_connection = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (_command = new SqlCommand("Select * from Products", _connection))
                {
                    if (_connection.State != ConnectionState.Open)
                    {
                        _connection.Open();
                    }
                    using (_reader = _command.ExecuteReader())
                    {
                        if (_reader.HasRows)
                        {
                            while (_reader.Read())
                            {
                                products.Add(new Product
                                {
                                    ProductId = Convert.ToInt32(_reader.GetValue(0)),
                                    ProductName = _reader.GetValue(1).ToString(),
                                    UnitPrice = Convert.ToDecimal(_reader.GetValue(5)),
                                    UnitsInStock = Convert.ToInt16(_reader.GetValue(6))
                                });
                            }
                        }
                    }
                }
            }
            return products;
        }

        public Product GetProductById(int id)
        {
            Product product = null;
            using (_connection = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (_command = new SqlCommand("Select * from Products Where ProductId = @ProductId", _connection))
                {
                    _command.Parameters.Add(new SqlParameter("@ProductId", SqlDbType.Int, 4));
                    //value for SqlParameter ProductId
                    _command.Parameters["@ProductId"].Value = id;
                    if (_connection.State != ConnectionState.Open)
                    {
                        _connection.Open();
                    }
                    using (_reader = _command.ExecuteReader())
                    {
                        if (_reader.HasRows)
                        {
                            product = new Product();
                            _reader.Read();
                            product.ProductId = Convert.ToInt32(_reader.GetValue(0));
                            product.ProductName = _reader.GetValue(1).ToString();
                            product.UnitPrice = Convert.ToDecimal(_reader.GetValue(5));
                            product.UnitsInStock = Convert.ToInt16(_reader.GetValue(6));
                        }
                    }
                }
            }
            return product;
        }

        public bool AddProduct(Product product)
        {
            using (_connection = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (_command = new SqlCommand("usp_AddProduct", _connection))
                {
                    _command.CommandType = CommandType.StoredProcedure;
                    if (_connection.State != ConnectionState.Open)
                    {
                        _connection.Open();
                    }
                    //Provide Datatype for Stored Procedure Parameters
                    _command.Parameters.Add(new SqlParameter("@ProductName", SqlDbType.NVarChar, 40));
                    _command.Parameters.Add(new SqlParameter("@UnitPrice", SqlDbType.Money, 8));
                    _command.Parameters.Add(new SqlParameter("@UnitsInStock", SqlDbType.SmallInt, 2));

                    //Value for the Stored Procedure Parameters
                    _command.Parameters["@ProductName"].Value = product.ProductName;
                    _command.Parameters["@UnitPrice"].Value = product.UnitPrice.Value;
                    _command.Parameters["@UnitsInStock"].Value = product.UnitsInStock.Value;

                    //Execute the Command
                    var res = _command.ExecuteNonQuery();
                    return res > 0;
                }
            }
        }

        public bool DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }


        public bool UpdatedProduct(int id, Product product)
        {
            throw new NotImplementedException();
        }
    }
}
