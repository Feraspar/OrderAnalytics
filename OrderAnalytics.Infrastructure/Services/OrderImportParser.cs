namespace OrderAnalytics.Infrastructure.Services
{
	using OrderAnalytics.Application.Abstractions;
	using OrderAnalytics.Application.DTO;
	using OrderAnalytics.Domain.Entities;
	using OrderAnalytics.Domain.Enums;
	using System;
	using System.Collections.Generic;
	using System.Globalization;

	/// <summary>
	/// Сервис парсинга строки в заказ.
	/// </summary>
	public class OrderImportParser : IOrderImportParser
	{
		#region Private Fields

		/// <summary>
		/// Ожидаемое число колонок в файле.
		/// </summary>
		private const int EXPECTED_COLUMN_COUNT = 9;

		#endregion Private Fields

		#region Public Methods

		/// <inheritdoc />
		public OrderImportParseResult TryParseLine(string line, int lineNumber)
		{
			List<ValidationError> errors = new List<ValidationError>();

			string[] parts = line.Split(';');

			if (parts.Length != EXPECTED_COLUMN_COUNT)
			{
				errors.Add(new ValidationError(lineNumber, string.Empty, "Line", $"Invalid column count. Expected {EXPECTED_COLUMN_COUNT}, but got {parts.Length}.", ErrorSeverity.Error));

				return new OrderImportParseResult(null, errors);
			}

			string orderId = parts[0].Trim();
			string createdAtString = parts[1].Trim();
			string manager = parts[2].Trim();
			string region = parts[3].Trim();
			string product = parts[4].Trim();
			string quantityString = parts[5].Trim();
			string unitPriceString = parts[6].Trim();
			string discountString = parts[7].Trim();
			string status = parts[8].Trim();

			ValidateRequiredField(orderId, lineNumber, "OrderId", errors);
			ValidateRequiredField(createdAtString, lineNumber, "CreatedAt", errors, orderId);
			ValidateRequiredField(manager, lineNumber, "Manager", errors, orderId);
			ValidateRequiredField(region, lineNumber, "Region", errors, orderId);
			ValidateRequiredField(product, lineNumber, "Product", errors, orderId);
			ValidateRequiredField(quantityString, lineNumber, "Quantity", errors, orderId);
			ValidateRequiredField(unitPriceString, lineNumber, "UnitPrice", errors, orderId);
			ValidateRequiredField(discountString, lineNumber, "Discount", errors, orderId);
			ValidateRequiredField(status, lineNumber, "Status", errors, orderId);

			if (errors.Count > 0)
			{
				return new OrderImportParseResult(null, errors);
			}

			bool isCreatedAtParsed = DateTime.TryParseExact(createdAtString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime createdAt);

			bool isQuantityParsed = int.TryParse(quantityString, NumberStyles.Integer, CultureInfo.InvariantCulture, out int quantity);

			bool isUnitPriceParsed = decimal.TryParse(unitPriceString, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal unitPrice);

			bool isDiscountParsed = decimal.TryParse(discountString, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal discount);

			if (!isCreatedAtParsed || !isQuantityParsed || !isUnitPriceParsed || !isDiscountParsed)
			{
				errors.Add(new ValidationError(lineNumber, orderId, "Line", "Invalid numeric value in line.", ErrorSeverity.Error));
			}

			if (errors.Count > 0)
			{
				return new OrderImportParseResult(null, errors);
			}

			Order order = new Order
			{
				OrderId = orderId,
				LineNumber = lineNumber,
				CreatedAt = createdAt,
				Manager = manager,
				Region = region,
				Product = product,
				Quantity = quantity,
				UnitPrice = unitPrice,
				Discount = discount,
				Status = status,
				IsProblematic = false
			};

			return new OrderImportParseResult(order, errors);
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Добавляет ошибку если поле пустое.
		/// </summary>
		/// <param name="value">Проверяемое значение.</param>
		/// <param name="lineNumber">Номер строки.</param>
		/// <param name="fieldName">Наименование поля.</param>
		/// <param name="errors">Список ошибок.</param>
		/// <param name="orderId">Id заказа.</param>
		private static void ValidateRequiredField(string value, int lineNumber, string fieldName, List<ValidationError> errors, string orderId = "")
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				errors.Add(new ValidationError(lineNumber, orderId, fieldName, $"{fieldName} must not be empty.", ErrorSeverity.Error));
			}
		}

		#endregion Private Methods
	}
}