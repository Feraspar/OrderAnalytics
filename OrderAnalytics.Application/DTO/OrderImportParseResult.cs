namespace OrderAnalytics.Application.DTO
{
	using OrderAnalytics.Domain.Entities;
	using System.Collections.Generic;

	/// <summary>
	/// Результат парсинга строки CSV.
	/// </summary>
	/// <param name="Order">Заказ.</param>
	/// <param name="Errors">Ошибки.</param>
	public record OrderImportParseResult(Order? Order, List<ValidationError> Errors);
}