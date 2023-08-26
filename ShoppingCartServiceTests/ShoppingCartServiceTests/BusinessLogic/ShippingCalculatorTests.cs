using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;

namespace ShoppingCartServiceTests.BusinessLogic
{
    public class ShippingCalculatorTests
    {
        [Fact]
        public void TestCalculateShippingCost_SameCitySameCountry_Rate_1()
        {
            // Assign
            var office = new Address
            {
                Country = "The Netherlands",
                City = "Amsterdam",
                Street = "Cheese street 1"
            };
            var shippingAddress = new Address
            {
                Country = "The Netherlands",
                City = "Amsterdam",
                Street = "Windmill street 1"
            };
            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                ShippingAddress = shippingAddress,
                Items = new List<Item> { new Item { Quantity = 1 } }
            };
            var calculator = new ShippingCalculator(office);

            // Act
            double result = calculator.CalculateShippingCost(cart);

            // Assert
            Assert.Equal(1.0, result);
        }

        [Fact]
        public void TestCalculateShippingCost_OtherCitySameCountry_Rate_2()
        {
            // Assign
            var office = new Address
            {
                Country = "The Netherlands",
                City = "Amsterdam",
                Street = "Cheese street 1"
            };
            var shippingAddress = new Address
            {
                Country = "The Netherlands",
                City = "Rotterdam",
                Street = "Windmill street 1"
            };
            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                ShippingAddress = shippingAddress,
                Items = new List<Item> { new Item { Quantity = 1 } }
            };
            var calculator = new ShippingCalculator(office);

            // Act
            double result = calculator.CalculateShippingCost(cart);

            // Assert
            Assert.Equal(2.0, result);
        }

        [Fact]
        public void TestCalculateShippingCost_OtherCityOtherCountry_Rate_15()
        {
            // Assign
            var office = new Address
            {
                Country = "The Netherlands",
                City = "Amsterdam",
                Street = "Cheese street 1"
            };
            var shippingAddress = new Address
            {
                Country = "Canada",
                City = "Sim City",
                Street = "123 West Hill"
            };
            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                ShippingAddress = shippingAddress,
                Items = new List<Item> { new Item { Quantity = 1 } }
            };
            var calculator = new ShippingCalculator(office);

            // Act
            double result = calculator.CalculateShippingCost(cart);

            // Assert
            Assert.Equal(15.0, result);
        }

        [Theory]
        // Standard customer type
        [InlineData(ShippingMethod.Standard, CustomerType.Standard, 1.0)]
        [InlineData(ShippingMethod.Expedited, CustomerType.Standard, 1.2)]
        [InlineData(ShippingMethod.Priority, CustomerType.Standard, 2.0)]
        [InlineData(ShippingMethod.Express, CustomerType.Standard, 2.5)]
        // Premium customer type
        [InlineData(ShippingMethod.Standard, CustomerType.Premium, 1.0)]
        [InlineData(ShippingMethod.Expedited, CustomerType.Premium, 1.0)] /* no increase in rate */
        [InlineData(ShippingMethod.Priority, CustomerType.Premium, 1.0)] /* no increase in rate */
        [InlineData(ShippingMethod.Express, CustomerType.Premium, 2.5)]
        public void TestCalculateShippingCost_ShippingMethod_CustomerType_CorrespondingRate(ShippingMethod shippingMethod, CustomerType customerType, double expectedRate)
        {
            // Assign
            var office = new Address
            {
                Country = "The Netherlands",
                City = "Amsterdam",
                Street = "Cheese street 1"
            };
            var shippingAddress = new Address
            {
                Country = "The Netherlands",
                City = "Amsterdam",
                Street = "Windmill street 1"
            };
            var cart = new Cart
            {
                CustomerType = customerType,
                ShippingMethod = shippingMethod,
                ShippingAddress = shippingAddress,
                Items = new List<Item> { new Item { Quantity = 1 } }
            };
            var calculator = new ShippingCalculator(office);

            // Act
            double result = calculator.CalculateShippingCost(cart);

            // Assert
            Assert.Equal(expectedRate, result);
        }

        [Theory]
        [InlineData(0, 0.0)]
        [InlineData(1, 1.0)]
        [InlineData(2, 2.0)]
        [InlineData(101, 101.0)]
        public void TestCalculateShippingCost_IncreasingQuantity_CorrespondingRate(uint numberOfItems, double expectedRate)
        {
            // Assign
            var office = new Address
            {
                Country = "The Netherlands",
                City = "Amsterdam",
                Street = "Cheese street 1"
            };
            var shippingAddress = new Address
            {
                Country = "The Netherlands",
                City = "Amsterdam",
                Street = "Windmill street 1"
            };
            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                ShippingAddress = shippingAddress,
                Items = new List<Item> { new Item { Quantity = numberOfItems } }
            };
            var calculator = new ShippingCalculator(office);

            // Act
            double result = calculator.CalculateShippingCost(cart);

            // Assert
            Assert.Equal(expectedRate, result);
        }

        [Theory]
        [InlineData(0, 0.0)]
        [InlineData(1, 1.0)]
        [InlineData(2, 2.0)]
        [InlineData(101, 101.0)]
        public void TestCalculateShippingCost_MultipleItems_CorrespondingRate(int numberOfItems, double expectedRate)
        {
            // Assign
            var office = new Address
            {
                Country = "The Netherlands",
                City = "Amsterdam",
                Street = "Cheese street 1"
            };
            var shippingAddress = new Address
            {
                Country = "The Netherlands",
                City = "Amsterdam",
                Street = "Windmill street 1"
            };
            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                ShippingAddress = shippingAddress,
                Items = Enumerable.Range(0, numberOfItems).Select(_ => new Item { Quantity = 1 }).ToList()
            };
            var calculator = new ShippingCalculator(office);

            // Act
            double result = calculator.CalculateShippingCost(cart);

            // Assert
            Assert.Equal(expectedRate, result);
        }

        [Theory]
        [InlineData(CustomerType.Standard, 16.0)] // 2 * (1 + 3) * 2
        [InlineData(CustomerType.Premium, 8.0)] // 2 * (1 + 3) * 1
        public void TestCalculateShippingCost_StackingDifferentRates(CustomerType customerType, double expectedRate)
        {
            // Assign
            var office = new Address
            {
                Country = "The Netherlands",
                City = "Amsterdam",
                Street = "Cheese street 1"
            };
            var shippingAddress = new Address
            {
                Country = "The Netherlands",
                City = "Rotterdam",
                Street = "Windmill street 1"
            };
            var items = new List<Item>
                {
                    new Item { Quantity = 1 },
                    new Item { Quantity = 3 }
                };
            var cart = new Cart
            {
                CustomerType = customerType,
                ShippingMethod = ShippingMethod.Priority,
                ShippingAddress = shippingAddress,
                Items = items
            };
            var calculator = new ShippingCalculator(office);

            // Act
            double result = calculator.CalculateShippingCost(cart);

            // Assert
            Assert.Equal(expectedRate, result);
        }

        [Fact]
        public void TestCalculateShippingCost_DefaultOffice()
        {
            // Assign
            var shippingAddress = new Address
            {
                Country = "USA", /* same countray as main office */ 
                City = "Dallas", /* same city as main office */ 
                Street = "123 right lane." /* street is different from main office */
            };
            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                ShippingAddress = shippingAddress,
                Items = new List<Item> { new Item { Quantity = 1 } }
            };
            var calculator = new ShippingCalculator();

            // Act
            double result = calculator.CalculateShippingCost(cart);

            // Assert
            Assert.Equal(1.0, result);
        }
    }
}
