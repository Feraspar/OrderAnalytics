namespace OrderAnalytics.Application.DTO
{
	using OrderAnalytics.Domain.Entities;
	using System.Collections.Generic;

	/// <summary>
	/// DTO для хранения результата работы сервисов.
	/// </summary>
	/// <param name="Orders">Заказы.</param>
	/// <param name="ImportErrors">Ошибки импорта.</param>
	/// <param name="ValidationErrors">Ошибки валидации.</param>
	public record ImportWorkflowResult(List<Order> Orders, List<ValidationError> ImportErrors, List<ValidationError> ValidationErrors);
}