using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Models;

namespace ShoppingCartServiceTests.BusinessLogic.Validation
{
    public class AddressValidatorTests
    {
        [Fact]
        public void TestIsValid_ValidAddress_ReturnsTrue()
        {
            // Assign
            var address = new Address
            {
                Country = "The Netherlands",
                City = "Amsterdam",
                Street = "Kaasstraat 1"
            };
            var validator = new AddressValidator();

            // Act
            bool result = validator.IsValid(address);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void TestIsValid_NullAddress_ReturnsFalse()
        {
            // Assign
            Address address = null;
            var validator = new AddressValidator();

            // Act
            bool result = validator.IsValid(address);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TestIsValid_NullCountry_ReturnsFalse()
        {
            // Assign
            var address = new Address
            {
                Country = null,
                City = "Amsterdam",
                Street = "Kaasstraat 1"
            };
            var validator = new AddressValidator();

            // Act
            bool result = validator.IsValid(address);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TestIsValid_EmptyCountry_ReturnsFalse()
        {
            // Assign
            var address = new Address
            {
                Country = "",
                City = "Amsterdam",
                Street = "Kaasstraat 1"
            };
            var validator = new AddressValidator();

            // Act
            bool result = validator.IsValid(address);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TestIsValid_NullCity_ReturnsFalse()
        {
            // Assign
            var address = new Address
            {
                Country = "The Netherlands",
                City = null,
                Street = "Kaasstraat 1"
            };
            var validator = new AddressValidator();

            // Act
            bool result = validator.IsValid(address);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TestIsValid_EmptyCity_ReturnsFalse()
        {
            // Assign
            var address = new Address
            {
                Country = "The Netherlands",
                City = "",
                Street = "Kaasstraat 1"
            };
            var validator = new AddressValidator();

            // Act
            bool result = validator.IsValid(address);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TestIsValid_NullStreet_ReturnsFalse()
        {
            // Assign
            var address = new Address
            {
                Country = "The Netherlands",
                City = "Amsterdam",
                Street = null
            };
            var validator = new AddressValidator();

            // Act
            bool result = validator.IsValid(address);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TestIsValid_EmptyStreet_ReturnsFalse()
        {
            // Assign
            var address = new Address
            {
                Country = "The Netherlands",
                City = "Amsterdam",
                Street = ""
            };
            var validator = new AddressValidator();

            // Act
            bool result = validator.IsValid(address);

            // Assert
            Assert.False(result);
        }
    }
}
