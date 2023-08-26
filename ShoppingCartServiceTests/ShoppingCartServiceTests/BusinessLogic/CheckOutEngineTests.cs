using AutoMapper;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Mapping;
using ShoppingCartService.Models;

namespace ShoppingCartServiceTests.BusinessLogic
{
    public class CheckOutEngineTests
    {
        [Theory]
        [InlineData(CustomerType.Standard, 3.0, 0.0, 7.0)]
        [InlineData(CustomerType.Premium, 3.0, 10, 6.3)]
        public void TestCalculateTotals_DifferentCustomerTypes(
            CustomerType customerType, 
            double expectedShippingCost, 
            double expectedDiscount, 
            double expectedTotal)
        {
            // Assign
            var shippingAddress = new Address
            {
                Country = "The Netherlands",
                City = "Amsterdam",
                Street = "Cheese street 1"
            };
            var office = new Address
            {
                Country = "The Netherlands",
                City = "Amsterdam",
                Street = "Windmill street 1"
            };
            var items = new List<Item>()
            {
                new Item { Quantity = 2, Price = 1.5 },
                new Item { Quantity = 1, Price = 1.0 }
            };
            var cart = new Cart
            {
                CustomerType = customerType,
                ShippingMethod = ShippingMethod.Standard,
                ShippingAddress = shippingAddress,
                Items = items
            };
            var mapperConfigProvider = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper().ConfigurationProvider;
            var engine = new CheckOutEngine(new ShippingCalculator(office), new Mapper(mapperConfigProvider));

            // Act
            var result = engine.CalculateTotals(cart);

            // Assert
            Assert.NotNull(result.ShoppingCart);
            Assert.Equal(expectedShippingCost, result.ShippingCost);
            Assert.Equal(expectedDiscount, result.CustomerDiscount);
            Assert.Equal(expectedTotal, result.Total);
        }

        [Fact]
        public void TestCalculateTotals_MapShoppingCartToDto()
        {
            // Assign
            var shippingAddress = new Address
            {
                Country = "The Netherlands",
                City = "Amsterdam",
                Street = "Cheese street 1"
            };
            var items = new List<Item>()
            {
                new Item 
                { 
                    ProductId = "A",
                    ProductName = "Product A",
                    Quantity = 2, 
                    Price = 1.5 
                },
                new Item 
                {
                    ProductId = "B",
                    ProductName = "Product B",
                    Quantity = 1, 
                    Price = 1.0 
                }
            };
            var cart = new Cart
            {
                Id = "1",
                CustomerId = "2",
                CustomerType = CustomerType.Premium,
                ShippingMethod = ShippingMethod.Express,
                ShippingAddress = shippingAddress,
                Items = items
            };
            var mapperConfigProvider = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper().ConfigurationProvider;
            var engine = new CheckOutEngine(new ShippingCalculator(), new Mapper(mapperConfigProvider));

            // Act
            var checkoutResult = engine.CalculateTotals(cart);

            // Assert cart
            var cartResult = checkoutResult.ShoppingCart;
            Assert.Equal("1", cartResult.Id);
            Assert.Equal("2", cartResult.CustomerId);
            Assert.Equal(CustomerType.Premium, cartResult.CustomerType);
            Assert.Equal(ShippingMethod.Express, cartResult.ShippingMethod);
            Assert.Equal(2, cartResult.Items.Count());

            // Assert address
            Assert.Equal("The Netherlands", cartResult.ShippingAddress.Country);
            Assert.Equal("Amsterdam", cartResult.ShippingAddress.City);
            Assert.Equal("Cheese street 1", cartResult.ShippingAddress.Street);

            // Assert first item
            var itemsResult = cartResult.Items.OrderBy(x => x.ProductId);
            var firstItemResult = itemsResult.First();
            Assert.Equal("A", firstItemResult.ProductId);
            Assert.Equal("Product A", firstItemResult.ProductName);
            Assert.Equal((uint)2, firstItemResult.Quantity);
            Assert.Equal(1.5, firstItemResult.Price);

            // Assert second item
            var secondItemResult = itemsResult.Last();
            Assert.Equal("B", secondItemResult.ProductId);
            Assert.Equal("Product B", secondItemResult.ProductName);
            Assert.Equal((uint)1, secondItemResult.Quantity);
            Assert.Equal(1.0, secondItemResult.Price);
        }
    }
}
