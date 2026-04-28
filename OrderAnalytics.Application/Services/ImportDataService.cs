namespace OrderAnalytics.Application.Services
{
	using OrderAnalytics.Domain.Entities;
	using System.Collections.Generic;

	/// <summary>
	/// Сервис для хранения импортированных данных.
	/// </summary>
	public class ImportDataService
	{
		#region Public Properties

		/// <summary>
		/// Ошибки импорта.
		/// </summary>
		public List<ValidationError> ImportErrors { get; private set; } = [];

		/// <summary>
		/// Заказы.
		/// </summary>
		public List<Order> Orders { get; private set; } = [];

		/// <summary>
		/// Ошибки валидации.
		/// </summary>
		public List<ValidationError> ValidationErrors { get; private set; } = [];

		#endregion Public Properties

		#region Public Methods

		/// <summary>
		/// Записывает данные в коллекции.
		/// </summary>
		/// <param name="orders">Заказы.</param>
		/// <param name="validationErrors">Ошибки валидации.</param>
		/// <param name="importErrors">Ошибки импорта.</param>
		public void SetData(List<Order> orders, List<ValidationError> validationErrors, List<ValidationError> importErrors)
		{
			Orders = orders;
			ValidationErrors = validationErrors;
			ImportErrors = importErrors;
		}

		#endregion Public Methods
	}
}