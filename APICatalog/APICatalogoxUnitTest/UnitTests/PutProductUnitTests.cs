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
    public class PutProductUnitTests : IClassFixture<ProductUnitTestController>
    {
        private readonly ProductsController _controller;
        public PutProductUnitTests(ProductUnitTestController controller)
        {
            _controller = new ProductsController(controller.repository, controller.mapper);
        }
        [Fact]
        public async Task PutProduct_Return_OkResult()
        {
            //Arrange 
            var prodId = 2;

            var updatedProductDto = new ProductDTO
            {
                ProductId = prodId,
                Name = "New Productss",
                Description = "minha Descricaoss",
                Price = 10,
                ImageUrl = "images.png",
                CategoryId = 2
            };

            //Act
            var result = await _controller.Put(prodId, updatedProductDto);

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();

        }

        [Fact]
        public async Task PutProduct_Return_BadRequestResult()
        {
            //Arrange 
            var prodId = 1000;

            var updatedProductDto = new ProductDTO
            {
                ProductId = prodId,
                Name = "New Productss",
                Description = "minha Descricaoss",
                Price = 10,
                ImageUrl = "images.png",
                CategoryId = 2
            };

            //Act
            var data = await _controller.Put(prodId, updatedProductDto);

            //Assert
            data.Result.Should().BeOfType<BadRequestResult>().Which.StatusCode.Should().Be(400);

        }
    }
}
