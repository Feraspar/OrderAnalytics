namespace OrderAnalytics.Tests
{
    using OrderAnalytics.Application.DTO;
    using OrderAnalytics.Domain.Entities;
    using OrderAnalytics.Infrastructure.Services;
    using System;

    public class OrderImportParserTests
    {
        /// <summary>
        /// Тестируемый сервис парсера строки.
        /// </summary>
        private readonly OrderImportParser _sut;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public OrderImportParserTests()
        {
            _sut = new OrderImportParser();
        }

        [Theory]
        [InlineData(";2026-04-01;Ivan Petrov;North;Laptop;2;55000;0.10;Completed", "OrderId")]
        [InlineData("ORD-001;2026-04-01;Ivan Petrov;North;Laptop;2;55000;0.10;", "Status")]
        [InlineData("ORD-001;2026-04-01;Ivan Petrov;North;;2;55000;0.10;Completed", "Product")]
        public void TryParseLine_EmptyFields_ReturnsErrors(string line, string errorField)
        {
            // Arrange
            int lineNumber = 1;

            // Act
            OrderImportParseResult result = _sut.TryParseLine(line, lineNumber);

            // Assert
            ValidationError error = Assert.Single(result.Errors);
            Assert.Null(result.Order);
            Assert.Equal($"{errorField} must not be empty.", error.Message);
        }

        [Theory]
        [InlineData("ORD-001;123456;Ivan Petrov;North;Laptop;2;55000;0.10;Completed")]
        [InlineData("ORD-001;2026-04-01;Ivan Petrov;North;Laptop;abc;55000;0.10;Completed")]
        [InlineData("ORD-001;2026-04-01;Ivan Petrov;North;Laptop;2;abc;0.10;Completed")]
        [InlineData("ORD-001;2026-04-01;Ivan Petrov;North;Laptop;2;55000;abc;Completed")]
        public void TryParseLine_InvalidNumericFields_ReturnsErrors(string line)
        {
            // Arrange
            int lineNumber = 1;

            // Act
            OrderImportParseResult result = _sut.TryParseLine(line, lineNumber);

            // Assert
            ValidationError error = Assert.Single(result.Errors);
            Assert.Null(result.Order);
            Assert.Equal("Invalid numeric value in line.", error.Message);
        }

        [Fact]
        public void TryParseLine_ValidLine_ReturnsOrder()
        {
            // Arrange
            string line = "ORD-001;2026-04-01;Ivan Petrov;North;Laptop;2;55000;0.10;Completed";
            int lineNumber = 1;

            // Act
            OrderImportParseResult result = _sut.TryParseLine(line, lineNumber);

            // Assert
            Assert.NotNull(result.Order);
            Assert.Empty(result.Errors);

            Order order = result.Order;

            Assert.Equal("ORD-001", order.OrderId);
            Assert.Equal(lineNumber, order.LineNumber);
            Assert.Equal(new DateTime(2026, 4, 1), order.CreatedAt);
            Assert.Equal("Ivan Petrov", order.Manager);
            Assert.Equal("North", order.Region);
            Assert.Equal("Laptop", order.Product);
            Assert.Equal(2, order.Quantity);
            Assert.Equal(55000m, order.UnitPrice);
            Assert.Equal(0.10m, order.Discount);
            Assert.Equal("Completed", order.Status);
            Assert.False(order.IsProblematic);
        }
    }
}