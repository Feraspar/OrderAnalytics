namespace OrderAnalytics.Presentation.Desktop.ViewModels
{
	using OrderAnalytics.Application.Services;
	using OrderAnalytics.Domain.Entities;
	using System.Collections.ObjectModel;
	using System.Linq;

	/// <summary>
	/// Модель представления страницы заказов.
	/// </summary>
	public class OrdersViewModel : ViewModelBase
	{
		#region Private Fields

		/// <inheritdoc <see cref="ImportDataService" />
		private readonly ImportDataService _importDataService;

		private string _visibleOrdersCount;

		private string _totalOrdersCount;

		#endregion Private Fields

		#region Public Properties

		/// <summary>
		/// Коллекция для хранения заказов.
		/// </summary>
		public ObservableCollection<OrderRowViewModel> Orders { get; }

		public string VisibleOrdersCount
		{
			get => _visibleOrdersCount;
			set => SetProperty(ref _visibleOrdersCount, value);
		}

		public string TotalOrdersCount
		{
			get => _totalOrdersCount;
			set => SetProperty(ref _totalOrdersCount, value);
		}

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="importDataService">Сервис для хранения импортированных данных.</param>
		public OrdersViewModel(ImportDataService importDataService)
		{
			_importDataService = importDataService;
			Orders = new ObservableCollection<OrderRowViewModel>(_importDataService.Orders.Select(order => new OrderRowViewModel(order)));


			VisibleOrdersCount = Orders.Count.ToString();
			TotalOrdersCount = Orders.Count.ToString();
		}

		#endregion Public Constructors
	}
}