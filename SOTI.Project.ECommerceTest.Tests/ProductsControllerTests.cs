using AutoFixture;
using Castle.DynamicProxy;
using Moq;
using NUnit;
using NUnit.Framework;
using SOTI.Project.API.Controllers;
using SOTI.Project.DAL;
using SOTI.Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;

namespace SOTI.Project.ECommerceTest.Tests
{

    [TestFixture]
    public class ProductsControllerTests
    {
        private Mock<IProduct> _productMock;
        private Mock<IProductAdditional> _productAdditionalMock;
        private ProductsController _controller;
        private Fixture _fixture;

        public ProductsControllerTests()
        {
            _productMock = new Mock<IProduct>();
            _productAdditionalMock = new Mock<IProductAdditional>();
            _fixture = new Fixture();
        }

        [SetUp]
        public void Setup()
        {
            _controller = new ProductsController(_productMock.Object, _productAdditionalMock.Object)
            {
                Configuration = new HttpConfiguration(),
                Request = new System.Net.Http.HttpRequestMessage()
            };
        }

        [TearDown]
        public void Teardown()
        {
            _productMock.Reset();
            _productAdditionalMock.Reset();
            _fixture = null;
        }

        [Test]
        public void GetProducts_ShouldReturnListOfProduct_WhenProductExists()
        {
            //Arrange           
            var productList = _fixture.CreateMany<Product>(5).ToList();
            _productMock.Setup(p => p.GetAllProduct()).Returns(productList);//Setup Database Action

            //Act
            var actionResult = _controller.GetProducts();

            //Assert
            var response = actionResult as OkNegotiatedContentResult<List<Product>>;
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(HttpStatusCode.OK, response.ExecuteAsync(CancellationToken.None).Result.StatusCode);
        }

        [Test]
       // [Ignore("This method is obselete")]
        public void GetProducts_ShouldReturnBadRequest_WhenExceptionThrown()
        {
            //Arrange
            _productMock.Setup(p => p.GetAllProduct()).Throws<Exception>();

            //Act
            var actionResult = _controller.GetProducts();
            var response = actionResult as BadRequestErrorMessageResult;

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.ExecuteAsync(CancellationToken.None).Result.StatusCode);
        }

        //Add Product
        [Test]
        public void AddProduct_ShouldReturnCreated_WhenProductAddedSuccessfully()
        {
            // _productMock.Setup(p=>p.UpdatedProduct(It.IsAny<int>(),It.IsAny<Product>()))
            var product = _fixture.Create<Product>();
            _productMock.Setup(p => p.AddProduct(It.IsAny<Product>())).Returns(true);//Call DAL Method


            var actionResult = _controller.AddProduct(product);//Call Controller Method
            var response = actionResult as CreatedNegotiatedContentResult<Product>;

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.Created, response.ExecuteAsync(CancellationToken.None).Result.StatusCode);
        }

        [Test]
        public void AddProduct_ShouldReturnBadRequest_WhenNoProductExist()
        {
            _productMock.Setup(p => p.AddProduct(It.IsAny<Product>())).Returns(false);//Call DAL Method

            var actionResult = _controller.AddProduct(null);//Call Controller Method
            var response = actionResult as BadRequestErrorMessageResult;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.ExecuteAsync(CancellationToken.None).Result.StatusCode);
        }
    }
}
