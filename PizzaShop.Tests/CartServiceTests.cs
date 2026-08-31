using Microsoft.AspNetCore.Http;
using Moq;
using PizzaShop.Domain.Enums;
using PizzaShop.Domain.Model;
using PizzaShop.Interfaces;
using PizzaShop.Services;
using PizzaShop.Tests.Helpers;

namespace PizzaShop.Tests
{
    public class CartServiceTests
    {
        [Fact]
        public async Task Add_NewItem_AddsItemToCart()
        {
            // Arrange
            var httpAccessor = TestHttpContext.CreateAccessor();
            var pizzaService = new Mock<IPizzaService>();

            pizzaService.Setup(
                s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(
                new Pizza
                {
                    Id = 1,
                    Name = "Margherita",
                    Price = 15.50m
                });

            var cartService = new CartService(
                httpAccessor,
                pizzaService.Object);

            // Act
            var result = await cartService.AddAsync(
                new CartItem
                {
                    PizzaId = 1,
                    Quantity = 2
                });

            // Assert
            Assert.Equal(AddToCartResult.Success, result);

            var cart = cartService.GetCart();

            var cartItem = Assert.Single(cart.Items);
            Assert.Equal(1, cartItem.PizzaId);
            Assert.Equal(2, cartItem.Quantity);
        }

        [Fact]
        public async Task Add_ExistingItem_IncreasesQuantity()
        {
            // Arrange
            var httpAccessor = TestHttpContext.CreateAccessor();
            var pizzaService = new Mock<IPizzaService>();

            pizzaService.Setup(
                s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Pizza
                {
                    Id = 1,
                    Name = "Margherita",
                    Price = 15.50m
                });

            var cartService = new CartService(
                httpAccessor,
                pizzaService.Object);

            // Act
            var firstResult = await cartService.AddAsync(
                new CartItem
                {
                    PizzaId = 1,
                    Quantity = 2
                });

            var secondResult = await cartService.AddAsync(
                new CartItem
                {
                    PizzaId = 1,
                    Quantity = 3
                });

            // Assert
            Assert.Equal(AddToCartResult.Success, firstResult);
            Assert.Equal(AddToCartResult.Success, secondResult);

            var cart = cartService.GetCart();

            var cartItem = Assert.Single(cart.Items);

            Assert.Equal(1, cartItem.PizzaId);
            Assert.Equal(5, cartItem.Quantity);
        }

        [Fact]
        public async Task Add_ExistingItem_QuantityLimitExceeded_DoesNotChangeCart()
        {
            // Arrange
            var httpAccessor = TestHttpContext.CreateAccessor();
            var pizzaService = new Mock<IPizzaService>();


            pizzaService.Setup(
                s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Pizza
                {
                    Id = 1,
                    Name = "Margherita",
                    Price = 15.50m
                });

            var cartService = new CartService(
                httpAccessor,
                pizzaService.Object);

            var firstResult = await cartService.AddAsync(
                new CartItem
                {
                    PizzaId = 1,
                    Quantity = 18
                });

            // Act
            var secondResult = await cartService.AddAsync(
                new CartItem
                {
                    PizzaId = 1,
                    Quantity = 3
                });

            // Assert
            Assert.Equal(AddToCartResult.Success, firstResult);
            Assert.Equal(
                AddToCartResult.QuantityLimitExceeded,
                secondResult);

            var cart = cartService.GetCart();

            var cartItem = Assert.Single(cart.Items);

            Assert.Equal(1, cartItem.PizzaId);
            Assert.Equal(18, cartItem.Quantity);
        }

        [Fact]
        public async Task Add_PizzaNotFound_DoesNotChangeCart()
        {
            // Arrange
            var httpAccessor = TestHttpContext.CreateAccessor();
            var pizzaService = new Mock<IPizzaService>();


            pizzaService.Setup(
                s => s.GetByIdAsync(1))
                .ReturnsAsync(new Pizza
                {
                    Id = 1,
                    Name = "Margherita",
                    Price = 15.50m
                });

            pizzaService.Setup(
                s => s.GetByIdAsync(999))
                .ReturnsAsync((Pizza?)null);

            var cartService = new CartService(
                httpAccessor,
                pizzaService.Object);

            var firstResult = await cartService.AddAsync(
                new CartItem
                {
                    PizzaId = 1,
                    Quantity = 2
                });

            // Act
            var result = await cartService.AddAsync(
                new CartItem
                {
                    PizzaId = 999,
                    Quantity = 3
                });

            // Assert
            Assert.Equal(AddToCartResult.Success, firstResult);
            Assert.Equal(AddToCartResult.PizzaNotFound, result);

            var cart = cartService.GetCart();

            var cartItem = Assert.Single(cart.Items);

            Assert.Equal(1, cartItem.PizzaId);
            Assert.Equal(2, cartItem.Quantity);
        }

        [Fact]
        public async Task UpdateQuantity_ValidQuantity_UpdatesItem()
        {
            // Arrange
            var httpAccessor = TestHttpContext.CreateAccessor();
            var pizzaService = new Mock<IPizzaService>();

            pizzaService.Setup(
                s => s.GetByIdAsync(1))
                .ReturnsAsync(new Pizza
                {
                    Id = 1,
                    Name = "Margherita",
                    Price = 15.50m
                });

            var cartService = new CartService(
                httpAccessor,
                pizzaService.Object);

            await cartService.AddAsync(
                new CartItem
                {
                    PizzaId = 1,
                    Quantity = 2
                });

            // Act
            cartService.UpdateQuantity(1, 5);

            // Assert
            var cart = cartService.GetCart();

            var cartItem = Assert.Single(cart.Items);

            Assert.Equal(1, cartItem.PizzaId);
            Assert.Equal(5, cartItem.Quantity);
        }

        [Fact]
        public async Task UpdateQuantity_QuantityLessThanOne_DoesNotChangeItem()
        {
            // Arrange
            var httpAccessor = TestHttpContext.CreateAccessor();
            var pizzaService = new Mock<IPizzaService>();

            pizzaService.Setup(
            s => s.GetByIdAsync(1))
            .ReturnsAsync(new Pizza
            {
                Id = 1,
                Name = "Margherita",
                Price = 15.50m
            });

            var cartService = new CartService(
                httpAccessor,
                pizzaService.Object);

            await cartService.AddAsync(
                new CartItem
                {
                    PizzaId = 1,
                    Quantity = 5
                });

            // Act
            cartService.UpdateQuantity(1, 0);

            // Assert
            var cart = cartService.GetCart();

            var cartItem = Assert.Single(cart.Items);

            Assert.Equal(5, cartItem.Quantity);
        }

        [Fact]
        public async Task UpdateQuantity_QuantityGreaterThanTwenty_DoesNotChangeItem()
        {
            // Arrange
            var httpAccessor = TestHttpContext.CreateAccessor();
            var pizzaService = new Mock<IPizzaService>();

            pizzaService.Setup(
            s => s.GetByIdAsync(1))
            .ReturnsAsync(new Pizza
            {
                Id = 1,
                Name = "Margherita",
                Price = 15.50m
            });

            var cartService = new CartService(
                httpAccessor,
                pizzaService.Object);

            await cartService.AddAsync(
                new CartItem
                {
                    PizzaId = 1,
                    Quantity = 5
                });

            // Act
            cartService.UpdateQuantity(1, 21);

            // Assert
            var cart = cartService.GetCart();

            var cartItem = Assert.Single(cart.Items);

            Assert.Equal(5, cartItem.Quantity);
        }

        [Fact]
        public void UpdateQuantity_ItemDoesNotExist_DoesNothing()
        {
            // Arrange
            var httpAccessor = TestHttpContext.CreateAccessor();
            var pizzaService = new Mock<IPizzaService>();

            var cartService = new CartService(
                httpAccessor,
                pizzaService.Object);

            // Act
            cartService.UpdateQuantity(999, 5);

            // Assert
            var cart = cartService.GetCart();

            Assert.Empty(cart.Items);
        }

        [Fact]
        public async Task Remove_ExistingItem_RemovesItem()
        {
            // Arrange
            var httpAccessor = TestHttpContext.CreateAccessor();
            var pizzaService = new Mock<IPizzaService>();

            pizzaService.Setup(
                s => s.GetByIdAsync(1))
                .ReturnsAsync(new Pizza
                {
                    Id = 1,
                    Name = "Margherita",
                    Price = 15.50m
                });

            var cartService = new CartService(
                httpAccessor,
                pizzaService.Object);

            await cartService.AddAsync(
                new CartItem
                {
                    PizzaId = 1,
                    Quantity = 2
                });

            // Act
            cartService.Remove(1);

            // Assert
            var cart = cartService.GetCart();

            Assert.Empty(cart.Items);
        }

        [Fact]
        public async Task Remove_ItemDoesNotExist_DoesNothing()
        {
            // Arrange
            var httpAccessor = TestHttpContext.CreateAccessor();
            var pizzaService = new Mock<IPizzaService>();

            pizzaService.Setup(
                s => s.GetByIdAsync(1))
                .ReturnsAsync(new Pizza
                {
                    Id = 1,
                    Name = "Margherita",
                    Price = 15.50m
                });

            var cartService = new CartService(
                httpAccessor,
                pizzaService.Object);

            await cartService.AddAsync(
                new CartItem
                {
                    PizzaId = 1,
                    Quantity = 2
                });

            // Act
            cartService.Remove(999);

            // Assert
            var cart = cartService.GetCart();

            var cartItem = Assert.Single(cart.Items);

            Assert.Equal(1, cartItem.PizzaId);
            Assert.Equal(2, cartItem.Quantity);
        }

        [Fact]
        public async Task Clear_RemovesAllItemsFromCart()
        {
            // Arrange
            var httpAccessor = TestHttpContext.CreateAccessor();
            var pizzaService = new Mock<IPizzaService>();

            pizzaService.Setup(
                s => s.GetByIdAsync(1))
                .ReturnsAsync(new Pizza
                {
                    Id = 1,
                    Name = "Margherita",
                    Price = 15.50m
                });

            var cartService = new CartService(
                httpAccessor,
                pizzaService.Object);

            await cartService.AddAsync(
                new CartItem
                {
                    PizzaId = 1,
                    Quantity = 2
                });

            // Act
            cartService.Clear();

            // Assert
            var cart = cartService.GetCart();

            Assert.Empty(cart.Items);
        }

        [Fact]
        public async Task GetCartViewModelAsync_PizzaDoesNotExist_ExcludesItem()
        {
            // Arrange
            var httpAccessor = TestHttpContext.CreateAccessor();
            var pizzaService = new Mock<IPizzaService>();

            pizzaService.Setup(
                s => s.GetByIdAsync(1))
                .ReturnsAsync(new Pizza
                {
                    Id = 1,
                    Name = "Margherita",
                    Price = 15.50m
                });

            // Pizza has been removed from the database
            pizzaService.Setup(
                s => s.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(Array.Empty<Pizza>()); 

            var cartService = new CartService(
                httpAccessor,
                pizzaService.Object);

            var addResult = await cartService.AddAsync(
                new CartItem
                {
                    PizzaId = 1,
                    Quantity = 2
                });

            Assert.Equal(AddToCartResult.Success, addResult);

            // Act
            var viewModel = await cartService.GetCartViewModelAsync();

            // Assert
            Assert.Empty(viewModel.Items);
        }

        [Fact]
        public async Task GetCartViewModelAsync_ValidCart_ReturnsCartViewModel()
        {
            // Arrange
            var httpAccessor = TestHttpContext.CreateAccessor();
            var pizzaService = new Mock<IPizzaService>();

            pizzaService.Setup(
                s => s.GetByIdAsync(1))
                .ReturnsAsync(new Pizza
                {
                    Id = 1,
                    Name = "Margherita",
                    Price = 15.50m
                });

            pizzaService.Setup(
                s => s.GetByIdAsync(2))
                .ReturnsAsync(new Pizza
                {
                    Id = 2,
                    Name = "Pepperoni",
                    Price = 18.00m
                });

            pizzaService.Setup(
                s => s.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(new[]
                {
                    new Pizza
                    {
                        Id = 1,
                        Name = "Margherita",
                        Price = 15.50m
                    },
                    new Pizza
                    {
                        Id = 2,
                        Name = "Pepperoni",
                        Price = 18.00m
                    }
                });

            var cartService = new CartService(
                httpAccessor,
                pizzaService.Object);

            await cartService.AddAsync(
                new CartItem
                {
                    PizzaId = 1,
                    Quantity = 2
                });

            await cartService.AddAsync(
                new CartItem
                {
                    PizzaId = 2,
                    Quantity = 3
                });

            // Act
            var viewModel = await cartService.GetCartViewModelAsync();

            // Assert
            Assert.Equal(2, viewModel.Items.Count);

            var margherita = Assert.Single(
                viewModel.Items.Where(i => i.PizzaId == 1));

            Assert.Equal("Margherita", margherita.PizzaName);
            Assert.Equal(15.50m, margherita.Price);
            Assert.Equal(2, margherita.Quantity);
            Assert.Equal(31.00m, margherita.Total);

            var pepperoni = Assert.Single(
                viewModel.Items.Where(i => i.PizzaId == 2));

            Assert.Equal("Pepperoni", pepperoni.PizzaName);
            Assert.Equal(18.00m, pepperoni.Price);
            Assert.Equal(3, pepperoni.Quantity);
            Assert.Equal(54.00m, pepperoni.Total);

            Assert.Equal(85.00m, viewModel.Total);
        }
    }
}
