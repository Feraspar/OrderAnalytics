namespace OrderAnalytics.Domain.Services
{
	using OrderAnalytics.Domain.Entities;
	using OrderAnalytics.Domain.Enums;
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Cервис валидации.
	/// </summary>
	public class ValidationService : IValidationService
	{
		#region Public Methods

		/// <inheritdoc />
		public OrderValidationResult Validate(List<Order> orders)
		{
			List<ValidationError> errors = new List<ValidationError>();

			foreach (Order order in orders)
			{
				ValidateDiscount(order, errors);
				ValidateNetAmount(order, errors);
				ValidateQuantity(order, errors);
				ValidateRegion(order, errors);
				ValidateStatus(order, errors);
				ValidateUnitPrice(order, errors);
				ValidateWarnings(order, errors);
			}

			return new OrderValidationResult(orders, errors);
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Добавляет ошибку валидации.
		/// </summary>
		/// <param name="order">Заказ.</param>
		/// <param name="errors">Список ошибок.</param>
		/// <param name="field">Проблемное поле.</param>
		/// <param name="message">Текст ошибки.</param>
		/// <param name="severity">Тип ошибки.</param>
		private void AddError(Order order, List<ValidationError> errors, string field, string message, ErrorSeverity severity)
		{
			errors.Add(new ValidationError(order.LineNumber, order.OrderId, field, message, severity));
		}

		/// <summary>
		/// Валидирует скидку заказа.
		/// </summary>
		/// <param name="order">Заказ.</param>
		/// <param name="errors">Список ошибок.</param>
		private void ValidateDiscount(Order order, List<ValidationError> errors)
		{
			if (order.Discount < 0 || order.Discount > 1)
			{
				order.IsProblematic = true;

				AddError(order, errors, nameof(order.Discount), "Discount must be between 0 and 1.", ErrorSeverity.Error);
			}
		}

		/// <summary>
		/// Валидирует стоимость всего заказа.
		/// </summary>
		/// <param name="order">Заказ.</param>
		/// <param name="errors">Список ошибок.</param>
		private void ValidateNetAmount(Order order, List<ValidationError> errors)
		{
			if (order.NetAmount < 0)
			{
				order.IsProblematic = true;

				AddError(order, errors, nameof(order.NetAmount), "NetAmount must not be negative.", ErrorSeverity.Error);
			}
		}

		/// <summary>
		/// Валидирует количество заказа.
		/// </summary>
		/// <param name="order">Заказ.</param>
		/// <param name="errors">Список ошибок.</param>
		private void ValidateQuantity(Order order, List<ValidationError> errors)
		{
			if (order.Quantity <= 0)
			{
				order.IsProblematic = true;

				AddError(order, errors, nameof(order.Quantity), "Quantity must be greater than 0.", ErrorSeverity.Error);
			}
		}

		/// <summary>
		/// Валидирует регион заказа.
		/// </summary>
		/// <param name="order">Заказ.</param>
		/// <param name="errors">Список ошибок.</param>
		private void ValidateRegion(Order order, List<ValidationError> errors)
		{
			if (!Enum.TryParse<OrderRegion>(order.Region, true, out OrderRegion region))
			{
				order.IsProblematic = true;

				AddError(order, errors, nameof(order.Region), $"Unknown status: {order.Region}", ErrorSeverity.Error);
			}
		}

		/// <summary>
		/// Валидирует статус заказа.
		/// </summary>
		/// <param name="order">Заказ.</param>
		/// <param name="errors">Список ошибок.</param>
		private void ValidateStatus(Order order, List<ValidationError> errors)
		{
			if (!Enum.TryParse<OrderStatus>(order.Status, true, out OrderStatus status))
			{
				order.IsProblematic = true;

				AddError(order, errors, nameof(order.Status), $"Unknown status: {order.Status}", ErrorSeverity.Error);
			}
		}

		/// <summary>
		/// Валидирует стоимость единицы товара в заказе.
		/// </summary>
		/// <param name="order">Заказ.</param>
		/// <param name="errors">Список ошибок.</param>
		private void ValidateUnitPrice(Order order, List<ValidationError> errors)
		{
			if (order.UnitPrice < 0)
			{
				order.IsProblematic |= true;

				AddError(order, errors, nameof(order.UnitPrice), "UnitPrice must not be negative.", ErrorSeverity.Error);
			}
		}

		/// <summary>
		/// Валидирует предупреждения в заказе.
		/// </summary>
		/// <param name="order">Заказ.</param>
		/// <param name="errors">Список ошибок.</param>
		private void ValidateWarnings(Order order, List<ValidationError> errors)
		{
			if (order.CreatedAt > DateTime.UtcNow)
			{
				order.IsProblematic = true;

				AddError(order, errors, nameof(order.CreatedAt), "CreatedAt is in the future.", ErrorSeverity.Warning);
			}

			if (order.NetAmount == 0)
			{
				order.IsProblematic = true;

				AddError(order, errors, nameof(order.NetAmount), "NetAmount equals zero.", ErrorSeverity.Warning);
			}

			if (order.Quantity > 1000)
			{
				order.IsProblematic = true;

				AddError(order, errors, nameof(order.Quantity), "Quantity is too large", ErrorSeverity.Warning);
			}

			if (order.Discount == 1)
			{
				order.IsProblematic = true;

				AddError(order, errors, nameof(order.Discount), "Discount is 100%", ErrorSeverity.Warning);
			}
		}

		#endregion Private Methods
	}
}