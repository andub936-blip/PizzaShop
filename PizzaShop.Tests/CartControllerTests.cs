using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using PizzaShop.Controllers;
using PizzaShop.Domain.Enums;
using PizzaShop.Domain.Model;
using PizzaShop.Interfaces;
using PizzaShop.ViewModels.CartVM;

namespace PizzaShop.Tests
{
    public class CartControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewWithCartViewModel()
        {
            // Arrange

            var cartService = new Mock<ICartService>();

            var viewModel = new CartViewModel
            {
                Items = new List<CartItemViewModel>()
            };

            cartService.Setup(
                s => s.GetCartViewModelAsync())
                .ReturnsAsync(viewModel);
            
            var controller = new CartController(cartService.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(viewModel, viewResult.Model);

            cartService.Verify(
                s => s.GetCartViewModelAsync(),
                Times.Once);
        }

        [Fact]
        public async Task Add_Success_RedirectsToCart()
        {
            // Arrange
            var cartService = new Mock<ICartService>();

            cartService
                .Setup(s => s.AddAsync(It.IsAny<CartItem>()))
                .ReturnsAsync(AddToCartResult.Success);

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var controller = new CartController(cartService.Object)
            {
                TempData = tempData
            };

            var viewModel = new AddToCartViewModel
            {
                PizzaId = 1,
                Quantity = 2
            };

            // Act
            var result = await controller.Add(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Cart", redirectResult.ControllerName);


            Assert.Equal(
                "Pizza added to cart.",
                controller.TempData["Success"]);

            cartService.Verify(
                s => s.AddAsync(It.Is<CartItem>(item =>
                    item.PizzaId == 1 &&
                    item.Quantity == 2)),
                Times.Once);
        }

        [Fact]
        public async Task Add_PizzaNotFound_RedirectsToPizza()
        {
            // Arrange
            var cartService = new Mock<ICartService>();

            cartService
                .Setup(s => s.AddAsync(It.IsAny<CartItem>()))
                .ReturnsAsync(AddToCartResult.PizzaNotFound);

            var httpContext = new DefaultHttpContext();

            var tempData = new TempDataDictionary(
                httpContext,
                Mock.Of<ITempDataProvider>());

            var controller = new CartController(cartService.Object)
            {
                TempData = tempData
            };

            var viewModel = new AddToCartViewModel
            {
                PizzaId = 999,
                Quantity = 2
            };

            // Act
            var result = await controller.Add(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Pizza", redirectResult.ControllerName);

            Assert.Equal(
                "This pizza is no longer available.",
                controller.TempData["Error"]);

            cartService.Verify(
                s => s.AddAsync(It.Is<CartItem>(item =>
                    item.PizzaId == 999 &&
                    item.Quantity == 2)),
                Times.Once);
        }

        [Fact]
        public async Task Add_QuantityLimitExceeded_RedirectsToPizza()
        {
            // Arrange
            var cartService = new Mock<ICartService>();

            cartService
                .Setup(s => s.AddAsync(It.IsAny<CartItem>()))
                .ReturnsAsync(AddToCartResult.QuantityLimitExceeded);

            var httpContext = new DefaultHttpContext();

            var tempData = new TempDataDictionary(
                httpContext,
                Mock.Of<ITempDataProvider>());

            var controller = new CartController(cartService.Object)
            {
                TempData = tempData
            };

            var viewModel = new AddToCartViewModel
            {
                PizzaId = 1,
                Quantity = 3
            };

            // Act
            var result = await controller.Add(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Pizza", redirectResult.ControllerName);

            Assert.Equal(
                "You cannot have more than 20 of the same pizza.",
                controller.TempData["Error"]);

            cartService.Verify(
                s => s.AddAsync(It.Is<CartItem>(item =>
                    item.PizzaId == 1 &&
                    item.Quantity == 3)),
                Times.Once);
        }

        [Fact]
        public async Task Add_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var cartService = new Mock<ICartService>();
            var controller = new CartController(cartService.Object);

            controller.ModelState.AddModelError(
                "Quantity",
                "Quantity must be between 1 and 20.");

            var viewModel = new AddToCartViewModel
            {
                PizzaId = 1,
                Quantity = 0
            };

            // Act
            var result = await controller.Add(viewModel);

            // Assert
            var badRequestResult =
                Assert.IsType<BadRequestObjectResult>(result);

            var errors = Assert.IsType<SerializableError>(
                badRequestResult.Value);

            Assert.True(errors.ContainsKey("Quantity"));

            cartService.Verify(
                s => s.AddAsync(It.IsAny<CartItem>()),
                Times.Never);
        }

        [Fact]
        public void Update_ValidModelState_RedirectsToCart()
        {
            // Arrange
            var cartService = new Mock<ICartService>();
            var controller = new CartController(cartService.Object);

            var viewModel = new UpdateCartItemViewModel
            {
                PizzaId = 1,
                Quantity = 5
            };

            // Act
            var result = controller.Update(viewModel);

            // Assert
            var redirectResult =
                Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(redirectResult.ControllerName);

            cartService.Verify(
                s => s.UpdateQuantity(1, 5),
                Times.Once);
        }

        [Fact]
        public void Update_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var cartService = new Mock<ICartService>();
            var controller = new CartController(cartService.Object);

            controller.ModelState.AddModelError(
                "Quantity",
                "Quantity must be between 1 and 20.");

            var viewModel = new UpdateCartItemViewModel
            {
                PizzaId = 1,
                Quantity = 0
            };

            // Act
            var result = controller.Update(viewModel);

            // Assert
            var badRequestResult =
                Assert.IsType<BadRequestObjectResult>(result);

            var errors = Assert.IsType<SerializableError>(
                badRequestResult.Value);

            Assert.True(errors.ContainsKey("Quantity"));

            cartService.Verify(
                s => s.UpdateQuantity(
                    It.IsAny<int>(),
                    It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public void Update_ItemDoesNotExist_CallsServiceAndRedirects()
        {
            // Arrange
            var cartService = new Mock<ICartService>();
            var controller = new CartController(cartService.Object);

            var viewModel = new UpdateCartItemViewModel
            {
                PizzaId = 999,
                Quantity = 5
            };

            // Act
            var result = controller.Update(viewModel);

            // Assert
            var redirectResult =
                Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(redirectResult.ControllerName);

            cartService.Verify(
                s => s.UpdateQuantity(999, 5),
                Times.Once);
        }

        [Fact]
        public void Remove_ValidPizzaId_RedirectsToCart()
        {
            // Arrange

            var cartService = new Mock<ICartService>();
            var controller = new CartController(cartService.Object);

            // Act 

            var result = controller.Remove(1);

            // Assert

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(redirectResult.ControllerName);

            cartService.Verify(
                s => s.Remove(1),
                Times.Once);
        }

        [Fact]
        public void Remove_ItemDoesNotExist_RedirectsToCart()
        {
            // Arrange

            var cartService = new Mock<ICartService>();
            var controller = new CartController(cartService.Object);

            // Act 

            var result = controller.Remove(999);

            // Assert

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(redirectResult.ControllerName);

            cartService.Verify(
                s => s.Remove(999),
                Times.Once);
        }
    }
}
