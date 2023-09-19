using SOTI.Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SOTI.Project.DAL
{
    public class ProductDetails : IProduct, IProductAdditional
    {
        private SqlConnection con = null;
        private SqlDataAdapter adapter = null;
        private DataSet ds = null;

        private DataSet GetProduct()
        {
            using (con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (adapter = new SqlDataAdapter("Select * from Products", con))
                {
                    using (ds = new DataSet())
                    {
                        adapter.Fill(ds, "Products");
                        return ds;
                    }
                }
            }
        }

        public List<Product> GetAllProduct()
        {
            var products = GetProduct().Tables["Products"].AsEnumerable()
              .Select(x => new Product
              {
                  ProductId = x.Field<int>("ProductId"),
                  ProductName = x.Field<string>("ProductName"),
                  UnitPrice = x.Field<decimal?>("UnitPrice"),
                  UnitsInStock = x.Field<short?>("UnitsInStock")
              }).ToList();
            return products;
        }

        public Product GetProductById(int id)
        {
            var product = GetProduct().Tables["Products"].AsEnumerable()
               .Select(x => new Product
               {
                   ProductId = x.Field<int>("ProductId"),
                   ProductName = x.Field<string>("ProductName"),
                   UnitPrice = x.Field<decimal?>("UnitPrice"),
                   UnitsInStock = x.Field<short?>("UnitsInStock")
               }).FirstOrDefault(x => x.ProductId == id);
            return product;
        }

        public bool AddProduct(string productName, decimal? unitPrice, short? unitsInStock)
        {
            using (con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (adapter = new SqlDataAdapter("Select ProductName, UnitPrice, UnitsInStock from Products", con))
                {
                    using (ds = new DataSet())
                    {
                        adapter.Fill(ds, "Products");
                        SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                        adapter.InsertCommand = builder.GetInsertCommand();
                        DataRow dr = ds.Tables["Products"].NewRow();
                        dr["ProductName"] = productName;
                        dr["UnitPrice"] = unitPrice;
                        dr["UnitsInStock"] = unitsInStock;
                        ds.Tables["Products"].Rows.Add(dr);
                        //Update to Database
                        var res = adapter.Update(ds, "Products");
                        return res > 0;
                    }
                }
            }
        }


        public bool DeleteProduct(int id)
        {
            using (con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (adapter = new SqlDataAdapter("Select ProductId from Products", con))
                {
                    using (ds = new DataSet())
                    {
                        adapter.Fill(ds, "Products");
                        SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                        adapter.UpdateCommand = builder.GetUpdateCommand();
                        var product = ds.Tables["Products"].AsEnumerable().FirstOrDefault(x => x.Field<int>("ProductId") == id);
                        if (product != null)
                        {
                            product.Delete();
                            //Update to Database
                            var res = adapter.Update(ds, "Products");
                            return res > 0 ? true : false;
                        }
                        return false;
                    }
                }
            }
        }

        public bool UpdatedProduct(int id, Product prd)
        {
            using (con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (adapter = new SqlDataAdapter("Select ProductId, ProductName, UnitPrice, UnitsInStock from Products", con))
                {
                    using (ds = new DataSet())
                    {
                        adapter.Fill(ds, "Products");
                        SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                        adapter.UpdateCommand = builder.GetUpdateCommand();
                        var product = ds.Tables["Products"].AsEnumerable().FirstOrDefault(x => x.Field<int>("ProductId") == id);
                        if (product != null)
                        {
                            product.BeginEdit();
                            product["ProductName"] = prd.ProductName;
                            product["UnitPrice"] = prd.UnitPrice == null ? product["UnitPrice"] : prd.UnitPrice.Value;
                            product["UnitsInStock"] = prd.UnitsInStock == null ? product["UnitsInStock"] : prd.UnitsInStock.Value;
                            product.EndEdit();

                            //Update to Database
                            var res = adapter.Update(ds, "Products");
                            return res > 0 ? true : false;
                        }
                        return false;
                    }
                }
            }
        }

        public List<Product> GetProducts(decimal price)
        {
            var ds = GetProduct();
            var products = ds.Tables[0].AsEnumerable()
                .Where(x => x.Field<decimal>("UnitPrice") > price)
                .Select(x => new Product
                {
                    ProductId = x.Field<int>("ProductId"),
                    ProductName = x.Field<string>("ProductName"),
                    UnitPrice = x.Field<decimal?>("UnitPrice"),
                    UnitsInStock = x.Field<short?>("UnitsInStock")
                }).ToList();
            return products;
        }

        public List<Product> GetProducts(decimal price, short quantity)
        {
            var ds = GetProduct();
            var products = ds.Tables[0].AsEnumerable()
                .Where(x => x.Field<decimal>("UnitPrice") >= price && x.Field<short>("UnitsInStock") >= quantity)
                .Select(x => new Product
                {
                    ProductId = x.Field<int>("ProductId"),
                    ProductName = x.Field<string>("ProductName"),
                    UnitPrice = x.Field<decimal?>("UnitPrice"),
                    UnitsInStock = x.Field<short?>("UnitsInStock")
                }).ToList();
            return products;
        }

    }
}
