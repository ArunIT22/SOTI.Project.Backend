using SOTI.Project.DAL.Models;
using System.Collections.Generic;

namespace SOTI.Project.DAL
{
    public interface IProduct
    {
        List<Product> GetAllProduct();

        Product GetProductById(int id);

        bool AddProduct(Product product);

        bool DeleteProduct(int id);

        bool UpdatedProduct(int id,Product product);

    }

    public interface IProductAdditional
    {
        List<Product> GetProducts(decimal price);
        List<Product> GetProducts(decimal price, short quantity);
        
    }
}
