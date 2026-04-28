namespace OrderAnalytics.Presentation.Desktop.ViewModels
{
	using CommunityToolkit.Mvvm.ComponentModel;
	using OrderAnalytics.Application.Services;
	using OrderAnalytics.Domain.Entities;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;

	/// <summary>
	/// Модель представления страницы заказов.
	/// </summary>
	public partial class OrdersViewModel : ViewModelBase
	{
		#region Private Fields

		/// <summary>
		/// Все заказы.
		/// </summary>
		private readonly List<OrderRowViewModel> _allOrders;

		/// <inheritdoc <see cref="ImportDataService" />
		private readonly ImportDataService _importDataService;

		private ObservableCollection<OrderRowViewModel> _orders;

		/// <summary>
		/// Поле для хранения общего числа заказов.
		/// </summary>
		private string _totalOrdersCount;

		/// <summary>
		/// Поле для хранения текущего числа заказов.
		/// </summary>
		private string _visibleOrdersCount;

		/// <summary>
		/// Поле для хранения текста поиска.
		/// </summary>
		private string? _searchOrderId;

		/// <summary>
		/// Поле для хранения выбранного менеджера.
		/// </summary>
		private string? _selectedManager;

		/// <summary>
		/// Поле для хранения выбранного региона.
		/// </summary>
		private string? _selectedRegion;

		/// <summary>
		/// Поле для хранения выбранного статуса.
		/// </summary>
		private string? _selectedStatus;

		private bool _isOnlyProblematic;

		#endregion Private Fields

		#region Public Properties

		/// <summary>
		/// Коллекция менеджеров.
		/// </summary>
		public ObservableCollection<string> Managers { get; }

		/// <summary>
		/// Коллекция отображаемых заказов.
		/// </summary>
		public ObservableCollection<OrderRowViewModel> Orders
		{
			get => _orders;
			set => SetProperty(ref  _orders, value);
		}

		/// <summary>
		/// Коллекция регионов.
		/// </summary>
		public ObservableCollection<string> Regions { get; }

		/// <summary>
		/// Коллекция статусов заказа.
		/// </summary>
		public ObservableCollection<string> Statuses { get; }

		public bool IsOnlyProblematic
		{
			get => _isOnlyProblematic;
			set
			{
				if (SetProperty(ref _isOnlyProblematic, value))
				{
					ApplyFilters();
				}
			}
		}

		/// <summary>
		/// Общее число заказов.
		/// </summary>
		public string TotalOrdersCount
		{
			get => _totalOrdersCount;
			set => SetProperty(ref _totalOrdersCount, value);
		}

		/// <summary>
		/// Текущее число заказов.
		/// </summary>
		public string VisibleOrdersCount
		{
			get => _visibleOrdersCount;
			set => SetProperty(ref _visibleOrdersCount, value);
		}

		/// <summary>
		/// Текст поиска заказа по Id.
		/// </summary>
		public string? SearchOrderId
		{
			get => _searchOrderId;
			set
			{
				if (SetProperty(ref _searchOrderId, value))
				{
					ApplyFilters();
				}
			}
		}

		/// <summary>
		/// Выбранный статус заказа.
		/// </summary>
		public string? SelectedStatus
		{
			get => _selectedStatus;
			set
			{
				if (SetProperty(ref _selectedStatus, value))
				{
					ApplyFilters();
				}
			}
		}

		/// <summary>
		/// Выбранный регион заказа.
		/// </summary>
		public string? SelectedRegion
		{
			get => _selectedRegion;
			set
			{
				if (SetProperty(ref _selectedRegion, value))
				{
					ApplyFilters();
				}
			}
		}

		/// <summary>
		/// Выбранный менеджер.
		/// </summary>
		public string? SelectedManager
		{
			get => _selectedManager;
			set
			{
				if (SetProperty(ref _selectedManager, value))
				{
					ApplyFilters();
				}
			}
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

			_allOrders = importDataService.Orders.Select(order => new OrderRowViewModel(order)).ToList();
			Orders = new ObservableCollection<OrderRowViewModel>(_allOrders);

			Statuses = new ObservableCollection<string>();
			Regions = new ObservableCollection<string>();
			Managers = new ObservableCollection<string>();

			FillFilterCollections();

			VisibleOrdersCount = Orders.Count.ToString();
			TotalOrdersCount = _allOrders.Count.ToString();
		}

		#endregion Public Constructors

		#region Private Methods

		/// <summary>
		/// Заполняет коллекции фильтров данными.
		/// </summary>
		private void FillFilterCollections()
		{
			Statuses.Add("All");
			foreach (string status in _allOrders.Select(x => x.Status).Distinct().OrderBy(x => x))
			{
				Statuses.Add(status);
			}

			Regions.Add("All");
			foreach (string status in _allOrders.Select(x => x.Region).Distinct().OrderBy(x => x))
			{
				Regions.Add(status);
			}

			Managers.Add("All");
			foreach (string status in _allOrders.Select(x => x.Manager).Distinct().OrderBy(x => x))
			{
				Managers.Add(status);
			}
		}

		/// <summary>
		/// Применяет фильтры.
		/// </summary>
		private void ApplyFilters()
		{
			IEnumerable<OrderRowViewModel> query = _allOrders;

			if (!string.IsNullOrWhiteSpace(SearchOrderId))
			{
				query = query.Where(x => x.OrderId.Contains(SearchOrderId, StringComparison.OrdinalIgnoreCase));
			}

			if (!string.IsNullOrWhiteSpace(SelectedStatus) && SelectedStatus != "All")
			{
				query = query.Where(x => x.Status == SelectedStatus);
			}

			if (!string.IsNullOrWhiteSpace(SelectedRegion) && SelectedRegion != "All")
			{
				query = query.Where(x => x.Region == SelectedRegion);
			}

			if (!string.IsNullOrWhiteSpace(SelectedManager) && SelectedManager != "All")
			{
				query = query.Where(x => x.Manager == SelectedManager);
			}

			if (IsOnlyProblematic)
			{
				query = query.Where(x => x.IsProblematic);
			}

			Orders = new ObservableCollection<OrderRowViewModel>(query);

			VisibleOrdersCount = Orders.Count.ToString();
		}

		#endregion Private Methods
	}
}