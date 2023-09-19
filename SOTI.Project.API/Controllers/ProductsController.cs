using SOTI.Project.API.CustomFilters;
using SOTI.Project.DAL;
using SOTI.Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;

namespace SOTI.Project.API.Controllers
{
    [EnableCors(origins:"*", headers:"*", methods:"*")]
    [RoutePrefix("api/SOTI/Products")]
    // [BasicAuthentication]
    public class ProductsController : ApiController
    {
        private readonly IProduct _product = null;
        private readonly IProductAdditional _additional = null;

        public ProductsController(IProduct product, IProductAdditional additional)
        {
            _product = product;
            _additional = additional;
        }

        [HttpGet]
        [Route("AllProducts")]
        [AllowAnonymous]
        public IHttpActionResult GetProducts()
        {
            var dt = _product.GetAllProduct();
            if (dt == null)
            {
                return BadRequest();
            }
            return Ok(dt);
        }

        [HttpGet]
        [Route("{productId}", Name = "ById")]
        [AllowAnonymous]
        public IHttpActionResult GetProductById([FromUri] int productId)
        {
            var row = _product.GetProductById(productId);
            if (row == null)
            {
                return NotFound();
            }
            return Ok(row);
        }

        [HttpGet]
        public IHttpActionResult TestMethod(string name, int age)
        {
            return Json(new { name, age });
        }

        [HttpGet]
        [Route("ByPrice/{price?}")]
        public IHttpActionResult ProductByPrice(decimal? price)
        {
            if (price.HasValue)
            {
                var products = _additional.GetProducts(price.Value);

                if (products == null)
                {
                    return NotFound();
                }
                return Ok(products);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetProducts/{price}/{quantity}")]
        public IHttpActionResult GetProducts(decimal price, short quantity)
        {
            var products = _additional.GetProducts(price, quantity);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPost]
        [Route("AddProduct")]
        [Authorize(Roles ="admin, employee")]
        public IHttpActionResult AddProduct(Product product)
        {
            var result = _product.AddProduct(product.ProductName, product.UnitPrice.Value, product.UnitsInStock.Value);
            if (result)
            {
                return Created("api/SOTI/Products/" + product.ProductId, product);
                //return CreatedAtRoute("ById", new { productId = product.ProductId }, product);
            }
            return BadRequest();
        }

        [HttpPut]
        public IHttpActionResult UpdateProduct([FromUri] int id, [FromBody] Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }
            var result = _product.UpdatedProduct(id, product);
            if (result)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return BadRequest();
        }

        [HttpDelete]
        [Authorize(Roles ="admin")]
        public IHttpActionResult DeleteProduct([FromUri] int id)
        {
            var result = _product.DeleteProduct(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}

