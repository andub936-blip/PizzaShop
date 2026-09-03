using Microsoft.EntityFrameworkCore;
using Moq;
using PizzaShop.Data;
using PizzaShop.Domain.Model;
using PizzaShop.Interfaces;
using PizzaShop.Services;
using PizzaShop.Tests.Helpers;
using PizzaShop.ViewModels.OrderVM;

namespace PizzaShop.Tests
{
    public class OrderServiceTests
    {
        private OrderService CreateOrderService(
        Mock<ICartService> cartService,
        Mock<IPizzaService> pizzaService,
        PizzaShopDbContext dbContext)
        {
            return new OrderService(
                cartService.Object,
                pizzaService.Object,
                dbContext);
        }

        private static CheckoutViewModel CreateCheckout()
        {
            return new CheckoutViewModel
            {
                CustomerName = "John",
                Address = "Test Address"
            };
        }

        [Fact]
        public async Task CreateOrderAsync_EmptyCart_ThrowsException()
        {
            // Arrange
            var cartService = new Mock<ICartService>();
            var pizzaService = new Mock<IPizzaService>();

            cartService.Setup(s => s.GetCart())
            .Returns(new Cart());

            await using var database = await TestDatabase.CreateContextAsync();
            var orderService = CreateOrderService(
                cartService,
                pizzaService,
                database.Context);

            var checkout = CreateCheckout();

            // Act
            var act = () => orderService.CreateOrderAsync(checkout);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(act);
            Assert.Empty(database.Context.Orders);
        }

        [Fact]
        public async Task CreateOrderAsync_PizzaDoesNotExist_ThrowsException()
        {
            // Arrange
            var cartService = new Mock<ICartService>();
            var pizzaService = new Mock<IPizzaService>();

            var cart = new Cart();

            cart.Items.Add(
                new CartItem
                {
                    PizzaId = 999,
                    Quantity = 2
                });

            cartService.Setup(s => s.GetCart())
                .Returns(cart);

            pizzaService.Setup(
                s => s.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(Array.Empty<Pizza>());

            await using var database = await TestDatabase.CreateContextAsync();
            var orderService = CreateOrderService(
                cartService,
                pizzaService,
                database.Context);

            var checkout = CreateCheckout();

            // Act
            var act = () => orderService.CreateOrderAsync(checkout);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(act);

            Assert.Empty(database.Context.Orders);
        }

        [Fact]
        public async Task CreateOrderAsync_ValidCart_CreatesOrder()
        {
            // Arrange
            var cartService = new Mock<ICartService>();
            var pizzaService = new Mock<IPizzaService>();

            var cart = new Cart();

            cart.Items.Add(
                new CartItem
                {
                    PizzaId = 1,
                    Quantity = 2
                });

            cartService.Setup(s => s.GetCart())
                .Returns(cart);

            pizzaService.Setup(
                s => s.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(new Pizza[]
                {
                    new Pizza()
                    {
                        Id = 1,
                        Name = "Margherita",
                        Price = 15.50m
                    }
                });

            await using var database = await TestDatabase.CreateContextAsync();

            var orderService = CreateOrderService(
                cartService,
                pizzaService,
                database.Context);

            var checkout = CreateCheckout();

            // Act
            var orderId = await orderService.CreateOrderAsync(checkout);

            // Assert
            Assert.Equal(1, orderId);

            var order = await database.Context.Orders
                .Include(o => o.Items)
                .SingleAsync();

            Assert.Equal("John", order.CustomerName);
            Assert.Equal("Test Address", order.Address);
            Assert.Equal(31, order.Total);

            var orderItem = Assert.Single(order.Items);

            Assert.Equal(1, orderItem.PizzaId);
            Assert.Equal("Margherita", orderItem.PizzaName);
            Assert.Equal(15.50m, orderItem.Price);
            Assert.Equal(2, orderItem.Quantity);

            cartService.Verify(
                s => s.Clear(),
                Times.Once);
        }

        [Fact]
        public async Task CreateOrderAsync_InvalidQuantity_ThrowsException()
        {
            // Arrange
            var cartService = new Mock<ICartService>();
            var pizzaService = new Mock<IPizzaService>();

            var cart = new Cart();

            cart.Items.Add(new CartItem
            {
                PizzaId = 1,
                Quantity = 21
            });

            cartService.Setup(
                s => s.GetCart())
                .Returns(cart);

            await using var database = await TestDatabase.CreateContextAsync();
            var orderService = CreateOrderService(
                cartService,
                pizzaService,
                database.Context);

            var checkout = CreateCheckout();

            // Act
            var act = () => orderService.CreateOrderAsync(checkout);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(act);

            pizzaService.Verify(
                s => s.GetByIdsAsync(It.IsAny<IEnumerable<int>>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateOrderAsync_SavesPizzaPriceSnapshot()
        {
            // Arrange
            var cartService = new Mock<ICartService>();
            var pizzaService = new Mock<IPizzaService>();

            var cart = new Cart();

            cart.Items.Add(new CartItem
            {
                PizzaId = 1,
                Quantity = 2
            });

            cartService
                .Setup(s => s.GetCart())
                .Returns(cart);

            pizzaService
                .Setup(s => s.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(new[]
                {
                    new Pizza
                    {
                        Id = 1,
                        Name = "Margherita",
                        Price = 20.00m
                    }
                });

            await using var database = await TestDatabase.CreateContextAsync();

            var orderService = CreateOrderService(
                cartService,
                pizzaService,
                database.Context);

            var checkout = CreateCheckout();

            // Act
            var orderId = await orderService.CreateOrderAsync(checkout);

            // Assert
            var order = await database.Context.Orders
                .Include(o => o.Items)
                .SingleAsync(o => o.Id == orderId);

            var orderItem = Assert.Single(order.Items);

            Assert.Equal(20.00m, orderItem.Price);
            Assert.Equal(40.00m, orderItem.Total);
            Assert.Equal(40.00m, order.Total);
        }

        [Fact]
        public async Task CreateOrderAsync_MultipleItems_CreatesOrderWithCorrectTotal()
        {
            // Arrange
            var cartService = new Mock<ICartService>();
            var pizzaService = new Mock<IPizzaService>();

            var cart = new Cart();

            cart.Items.AddRange(new[]
            {
                new CartItem
                {
                    PizzaId = 1,
                    Quantity = 2
                },
                new CartItem
                {
                    PizzaId = 2,
                    Quantity = 3
                }
            });

            cartService
                .Setup(s => s.GetCart())
                .Returns(cart);

            pizzaService
                .Setup(s => s.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
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
                        Price = 12.99m
                    }
                });

            await using var database = await TestDatabase.CreateContextAsync();

            var orderService = CreateOrderService(
                cartService,
                pizzaService,
                database.Context);

            var checkout = CreateCheckout();

            // Act
            var orderId = await orderService.CreateOrderAsync(checkout);

            // Assert
            var order = await database.Context.Orders
                .Include(o => o.Items)
                .SingleAsync(o => o.Id == orderId);

            Assert.Equal(2, order.Items.Count);
            Assert.Equal(69.97m, order.Total);

            var margherita = order.Items
                .Single(i => i.PizzaId == 1);

            Assert.Equal("Margherita", margherita.PizzaName);
            Assert.Equal(15.50m, margherita.Price);
            Assert.Equal(2, margherita.Quantity);
            Assert.Equal(31.00m, margherita.Total);

            var pepperoni = order.Items
                .Single(i => i.PizzaId == 2);

            Assert.Equal("Pepperoni", pepperoni.PizzaName);
            Assert.Equal(12.99m, pepperoni.Price);
            Assert.Equal(3, pepperoni.Quantity);
            Assert.Equal(38.97m, pepperoni.Total);

            cartService.Verify(
                s => s.Clear(),
                Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingOrder_ReturnsOrderWithItems()
        {
            // Arrange
            await using var database = await TestDatabase.CreateContextAsync();

            var order = new Order
            {
                CustomerName = "John",
                Address = "Test Address",
                CreatedAt = DateTime.UtcNow,
                Total = 31.00m,
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        PizzaId = 1,
                        PizzaName = "Margherita",
                        Price = 15.50m,
                        Quantity = 2
                    }
                }
            };

            await database.Context.Orders.AddAsync(order);
            await database.Context.SaveChangesAsync();

            var cartService = new Mock<ICartService>();
            var pizzaService = new Mock<IPizzaService>();

            var orderService = CreateOrderService(
                cartService,
                pizzaService,
                database.Context);

            // Act
            var result = await orderService.GetByIdAsync(order.Id);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(order.Id, result.OrderId);
            Assert.Equal("John", result.CustomerName);
            Assert.Equal("Test Address", result.Address);
            Assert.Equal(31.00m, result.Total);

            var item = Assert.Single(result.Items);

            Assert.Equal(1, item.PizzaId);
            Assert.Equal("Margherita", item.PizzaName);
            Assert.Equal(15.50m, item.Price);
            Assert.Equal(2, item.Quantity);
        }

        [Fact]
        public async Task GetByIdAsync_OrderDoesNotExist_ReturnsNull()
        {
            await using var database = await TestDatabase.CreateContextAsync();

            var cartService = new Mock<ICartService>();
            var pizzaService = new Mock<IPizzaService>();

            var orderService = CreateOrderService(
                cartService,
                pizzaService,
                database.Context);

            var result = await orderService.GetByIdAsync(999);

            Assert.Null(result);
        }
    }
} 