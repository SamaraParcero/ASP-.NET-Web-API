using APICatalog.Controllers;
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
    public class DeleteProductUnitTests : IClassFixture<ProductUnitTestController>
    {
        private readonly ProductsController _controller;
        public DeleteProductUnitTests(ProductUnitTestController controller)
        {
            _controller = new ProductsController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task DeleteProductById_Return_OkResult()
        {
            var prodId = 13;
            var result = await _controller.Delete(prodId);
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        
        [Fact]
        public async Task DeleteProductById_Return_NotFound()
        {
            var prodId = 1000;
            var result = await _controller.Delete(prodId);
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
