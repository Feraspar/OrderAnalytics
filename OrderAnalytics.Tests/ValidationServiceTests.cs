namespace OrderAnalytics.Tests
{
    using OrderAnalytics.Domain.Entities;
    using OrderAnalytics.Domain.Enums;
    using OrderAnalytics.Domain.Services;
    using System;

    /// <summary>
    /// Тесты для сервиса валидации.
    /// </summary>
    public class ValidationServiceTests
    {
        /// <summary>
        /// Тестируемый сервис валидации.
        /// </summary>
        private readonly ValidationService _sut;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public ValidationServiceTests()
        {
            _sut = new ValidationService();
        }

        /// <summary>
        /// Создает коллекцию для проверки подозрительных полей.
        /// </summary>
        public static IEnumerable<object[]> WarningCases()
        {
            return [
                    new object[] { DateTime.UtcNow.AddDays(1), 1000, 0.9m },
                    new object[] { "2026-01-01", 1001, 0.9m },
                    new object[] { "2026-01-01", 1000, 1.0m }
                    ];
        }

        [Fact]
        public void Validate_CorrectOrder_ReturnsNoError()
        {
            // Arrange

            Order order = CreateValidOrder();

            // Act
            OrderValidationResult result = _sut.Validate([order]);

            //Assert
            Assert.False(order.IsProblematic);
            Assert.Empty(result.Errors);
            Assert.Single(result.Orders);
        }

        [Fact]
        public void Validate_MultipleInvalidFields_ReturnsErrors()
        {
            // Arrange
            Order order = CreateValidOrder();
            order.Status = "UnknownStatus";
            order.Region = "UnknownRegion";
            order.Quantity = 0;
            order.Discount = 2m;

            // Act
            OrderValidationResult result = _sut.Validate([order]);

            // Assert
            Assert.Equal(5, result.Errors.Count);
            Assert.True(order.IsProblematic);
            Assert.Contains(result.Errors, x => x.Field == nameof(order.Status));
            Assert.Contains(result.Errors, x => x.Field == nameof(order.Region));
            Assert.Contains(result.Errors, x => x.Field == nameof(order.Quantity));
            Assert.Contains(result.Errors, x => x.Field == nameof(order.Discount));
            Assert.Contains(result.Errors, x => x.Field == nameof(order.NetAmount));
            Assert.Single(result.Errors.Where(x => x.ErrorSeverity == ErrorSeverity.Warning));
        }

        [Theory]
        [InlineData("Unknown status")]
        [InlineData("Done")]
        public void Validate_UnknownOrderStatus_ReturnsErrors(string status)
        {
            // Arrange
            Order order = CreateValidOrder();
            order.Status = status;

            // Act
            OrderValidationResult result = _sut.Validate([order]);

            //Assert
            ValidationError error = Assert.Single(result.Errors);
            Assert.True(order.IsProblematic);
            Assert.Single(result.Orders);
            Assert.Equal(nameof(order.Status), error.Field);
            Assert.Equal(ErrorSeverity.Error, error.ErrorSeverity);
        }

        [Theory]
        [MemberData(nameof(WarningCases))]
        public void Validate_WarningFields_ReturnsWarnings(DateTime date, int quantity, decimal discount)
        {
            // Arrange
            Order order = CreateValidOrder();
            order.CreatedAt = date;
            order.Quantity = quantity;
            order.Discount = discount;

            // Act
            OrderValidationResult result = _sut.Validate([order]);

            //Assert
            Assert.NotEmpty(result.Errors);
            Assert.True(order.IsProblematic);
            Assert.All(result.Errors, x => Assert.Equal(ErrorSeverity.Warning, x.ErrorSeverity));
        }

        /// <summary>
        /// Создает корректный заказ.
        /// </summary>
        /// <returns>Корректный заказ.</returns>
        private Order CreateValidOrder()
        {
            return new Order
            {
                LineNumber = 1,
                OrderId = "ORD-001",
                CreatedAt = new DateTime(2026, 4, 1),
                Manager = "Ivan Petrov",
                Region = "North",
                Product = "Laptop",
                Quantity = 2,
                UnitPrice = 55000m,
                Discount = 0.10m,
                Status = "Completed",
                IsProblematic = false
            };
        }
    }
}