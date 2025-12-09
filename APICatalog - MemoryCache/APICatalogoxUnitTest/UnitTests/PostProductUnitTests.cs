using APICatalog.Controllers;
using APICatalog.DTOs;
using APICatalogoxUnitTest.NewFolder;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICatalogoxUnitTest.UnitTests
{
    public class PostProductUnitTests : IClassFixture<ProductUnitTestController>
    {
        private readonly ProductsController _controller;
        public PostProductUnitTests(ProductUnitTestController controller)
        {
            _controller = new ProductsController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task PostProduct_Return_CreatedStatusCode()
        {
            var newProductDto = new ProductDTO
            {
                Name = "New Product",
                Description = "Descricao",
                Price = 10,
                ImageUrl = "image.png",
                CategoryId = 2
            };

            //Act
            var data = await _controller.Post(newProductDto);

            //Assert
            var createdResult = data.Result.Should().BeOfType<CreatedAtRouteResult>();
            createdResult.Subject.StatusCode.Should().Be(201);

        }

        [Fact]
        public async Task PostProduct_Return_BadRequest()
        {
            ProductDTO prod = null;

            //Act
            var data = await _controller.Post(prod);

            //Assert
            var badRequestResult = data.Result.Should().BeOfType<BadRequestResult>();
            badRequestResult.Subject.StatusCode.Should().Be(400);

        }
    }
}
