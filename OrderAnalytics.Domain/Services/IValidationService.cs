namespace OrderAnalytics.Domain.Services
{
	using OrderAnalytics.Domain.Entities;
	using System.Collections.Generic;

	/// <summary>
	/// Интерфейс для сервиса валидации.
	/// </summary>
	public interface IValidationService
	{
		#region Public Methods

		/// <summary>
		/// Валидирует заказы.
		/// </summary>
		/// <param name="Orders">Список заказов.</param>
		/// <returns>Результат валидации.</returns>
		OrderValidationResult Validate(List<Order> Orders);

		#endregion Public Methods
	}
}