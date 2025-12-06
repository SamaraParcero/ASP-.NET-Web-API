using APICatalog.Controllers;
using APICatalog.DTOs;
using APICatalogoxUnitTest.NewFolder;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;


namespace APICatalogoxUnitTest.UnitTests
{
    public class GetProductUnitTests : IClassFixture<ProductUnitTestController>
    {
        private readonly ProductsController _controller;

        public GetProductUnitTests(ProductUnitTestController controller)
        {
            _controller = new ProductsController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task GetProductById_OkResult()
        {
            //Arrange
            var prodId = 2;

            //Act
            var data = await _controller.GetProduct(prodId);

            //Assert(xUnit)
            //  var okResult = Assert.IsType<OkObjectResult>(data.Result);
            // Assert.Equal(200, okResult.StatusCode);

            //Assert (fluenbtAssertions)
            //verifica se é object Result
            data.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);// Se é do tipo 200
        }

        [Fact]
        public async Task GetProductById_Return_NotFound()
        {
            //Arrange
            var prodId = 999;

            //Act
            var data = await _controller.GetProduct(prodId);

            //Assert (fluenbtAssertions)
            //verifica se é object Result
            data.Result.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should().Be(404);// Se é do tipo 404
        }

        [Fact]
        public async Task GetProductById_Return_BadRequest()
        {
            //Arrange
            var prodId = -1;

            //Act
            var data = await _controller.GetProduct(prodId);

            //Assert (fluenbtAssertions)
            //verifica se é object Result
            data.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be(400);// Se é do tipo 400
        }


        [Fact]
        public async Task GetProducts_Return_ListOfProductsDTO()
        {


            //Act
            var data = await _controller.Get();

            //Assert (fluenbtAssertions)
            //verifica se é object Result
            data.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeAssignableTo<IEnumerable<ProductDTO>>().And.NotBeNull();// Se é do tipo 400
        }

        [Fact]
        public async Task GetProducts_Return_BadRequestResult()
        {


            //Act
            var data = await _controller.Get();

            //Assert (fluenbtAssertions)
            //verifica se é object Result
            data.Result.Should().BeOfType<BadRequestResult>();
        }

    }
}
