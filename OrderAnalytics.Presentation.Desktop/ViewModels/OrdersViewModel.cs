namespace OrderAnalytics.Presentation.Desktop.ViewModels
{
	using OrderAnalytics.Application.Services;
	using OrderAnalytics.Domain.Entities;
	using System.Collections.ObjectModel;

	/// <summary>
	/// Модель представления страницы заказов.
	/// </summary>
	public class OrdersViewModel : ViewModelBase
	{
		#region Private Fields

		/// <inheritdoc <see cref="ImportDataService" />
		private readonly ImportDataService _importDataService;

		#endregion Private Fields

		#region Public Properties

		/// <summary>
		/// Коллекция для хранения заказов.
		/// </summary>
		public ObservableCollection<Order> Orders { get; }

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="importDataService">Сервис для хранения импортированных данных.</param>
		public OrdersViewModel(ImportDataService importDataService)
		{
			_importDataService = importDataService;
			Orders = new ObservableCollection<Order>(_importDataService.Orders);
		}

		#endregion Public Constructors
	}
}