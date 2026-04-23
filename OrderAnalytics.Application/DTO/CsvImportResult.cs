namespace OrderAnalytics.Application.DTO
{
	using OrderAnalytics.Domain.Entities;
	using System.Collections.Generic;

	/// <summary>
	/// Результат импорта CSV файла.
	/// </summary>
	/// <param name="Orders"></param>
	/// <param name="Errors"></param>
	public record CsvImportResult(List<Order> Orders, List<ValidationError> Errors);
}