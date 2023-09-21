using AutoFixture;
using Moq;
using NUnit.Framework;
using SOTI.Project.API.Controllers;
using SOTI.Project.DAL;
using SOTI.Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;
using TechTalk.SpecFlow;

namespace SOTI.Project.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class ProductsStepDefinitions
    {
        private Mock<IProduct> _productMock;
        private Mock<IProductAdditional> _productAdditionalMock;
        private ProductsController _controller;
        private Fixture _fixture;
        private IHttpActionResult _actionResult;
        private Product _product;

        public ProductsStepDefinitions()
        {
            _productMock = new Mock<IProduct>();
            _productAdditionalMock = new Mock<IProductAdditional>();
            _fixture = new Fixture();
        }

        [BeforeScenario]
        public void Setup()
        {
            _controller = new ProductsController(_productMock.Object, _productAdditionalMock.Object)
            {
                Configuration = new HttpConfiguration(),
                Request = new System.Net.Http.HttpRequestMessage()
            };
        }

        [AfterScenario]
        public void Teardown()
        {
            _productMock.Reset();
            _productAdditionalMock.Reset();
            _fixture = null;
            _controller.Dispose();
        }

        [Given(@"I have a mocked product controllter")]
        public void GivenIHaveAMockedProductControllter()
        {
            var productList = _fixture.CreateMany<Product>(10).ToList();
            _productMock.Setup(p => p.GetAllProduct()).Returns(productList);
        }

        [When(@"I sent a GET Request to endpoint")]
        public void WhenISentAGETRequestToEndpoint()
        {
            _actionResult = _controller.GetProducts();
        }

        [Then(@"the response should contain list of products")]
        public void ThenTheResponseShouldContainListOfProducts()
        {
            var response = _actionResult as OkNegotiatedContentResult<List<Product>>;
            Assert.IsNotNull(response);
        }

        [Then(@"the response should be OK result")]
        public void ThenTheResponseShouldBeOKResult()
        {
            var response = _actionResult as OkNegotiatedContentResult<List<Product>>;
            Assert.AreEqual(HttpStatusCode.OK, response.ExecuteAsync(CancellationToken.None).Result.StatusCode);
        }

        //Insert Product
        [Given(@"I have a mocked the ProductDetails for insert")]
        public void GivenIHaveAMockedTheProductDetailsForInsert()
        {
            _productMock.Setup(p => p.AddProduct(It.IsAny<Product>())).Returns(true);
        }

        [Given(@"I have a product to insert")]
        public void GivenIHaveAProductToInsert()
        {
            _product = _fixture.Create<Product>();
        }

        [When(@"I send a POST request to api/products/addproduct endpoint")]
        public void WhenISendAPOSTRequestToApiProductsAddproductEndpoint()
        {
            _actionResult = _controller.AddProduct(_product);
        }

        [Then(@"the response status should be (.*)")]
        public void ThenTheResponseStatusShouldBe(int expectedStatusCode)
        {
            Assert.AreEqual(expectedStatusCode, (int)_actionResult.ExecuteAsync(CancellationToken.None).Result.StatusCode);
        }

        [Then(@"the response should contain the inserted product")]
        public void ThenTheResponseShouldContainTheInsertedProduct()
        {
            var result = _actionResult as CreatedNegotiatedContentResult<Product>;
            Assert.IsNotNull(result);
        }


        //Update Product
        Product _existingProduct = null;

        [Given(@"I have a mocked the ProductDetails for Update")]
        public void GivenIHaveAMockedTheProductDetailsForUpdate()
        {
            _productMock.Setup(p => p.UpdatedProduct(It.IsAny<int>(), It.IsAny<Product>())).Returns(true);
        }

        [Given(@"I have an existing product")]
        public void GivenIHaveAnExistingProduct()
        {
            _existingProduct = _fixture.Create<Product>();
        }

        [When(@"I send a PUT Request to endpoint")]
        public void WhenISendAPUTRequestToEndpoint()
        {
            _actionResult = _controller.UpdateProduct(_existingProduct.ProductId, _existingProduct);
        }

        [Then(@"the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int expectedStatusCode)
        {
            Assert.AreEqual(expectedStatusCode, (int)_actionResult.ExecuteAsync(CancellationToken.None).Result.StatusCode);
        }

        [Then(@"the response should contain the update the product")]
        public void ThenTheResponseShouldContainTheUpdateTheProduct()
        {
            Assert.IsNotNull(_actionResult);
        }
    }
}
