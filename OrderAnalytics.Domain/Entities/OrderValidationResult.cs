namespace OrderAnalytics.Domain.Entities
{
	using System.Collections.Generic;

	/// <summary>
	/// Результат валидации.
	/// </summary>
	/// <param name="Orders">Список заказов.</param>
	/// <param name="Errors">Список ошибок.</param>
	public record OrderValidationResult(List<Order> Orders, List<ValidationError> Errors);
}