namespace OrderAnalytics.Presentation.Desktop.ViewModels
{
	using CommunityToolkit.Mvvm.Input;
	using OrderAnalytics.Application.Services;
	using OrderAnalytics.Presentation.Desktop.Enums;
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

		/// <summary>
		/// Поле для хранения значения для сортировки.
		/// </summary>
		private OrdersSortField? _currentSortField;

		/// <summary>
		/// Поле для хранения булевого значения проблемных заказов.
		/// </summary>
		private bool _isOnlyProblematic;

		/// <summary>
		/// Поле для хранения булевого значения обратной сортировки.
		/// </summary>
		private bool _isSortDescending;

		/// <summary>
		/// Поле для хранения коллекции отображаемых заказов.
		/// </summary>
		private ObservableCollection<OrderRowViewModel> _orders;

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

		/// <summary>
		/// Поле для хранения общего числа заказов.
		/// </summary>
		private string _totalOrdersCount;

		/// <summary>
		/// Поле для хранения текущего числа заказов.
		/// </summary>
		private string _visibleOrdersCount;

		#endregion Private Fields

		#region Public Properties

		/// <summary>
		/// Поле таблицы заказов для сортировки.
		/// </summary>
		public OrdersSortField? CurrentSortField
		{
			get => _currentSortField;
			set => SetProperty(ref _currentSortField, value);
		}

		/// <summary>
		/// Выбран ли фильтр только проблемных заказов.
		/// </summary>
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
		/// Идет ли сортировка по убыванию.
		/// </summary>
		public bool IsSortDescending
		{
			get => _isSortDescending;
			set => SetProperty(ref _isSortDescending, value);
		}

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
			set => SetProperty(ref _orders, value);
		}

		/// <summary>
		/// Коллекция регионов.
		/// </summary>
		public ObservableCollection<string> Regions { get; }

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
		/// Команда сортировки.
		/// </summary>
		public IRelayCommand<OrdersSortField> SortCommand { get; }

		/// <summary>
		/// Коллекция статусов заказа.
		/// </summary>
		public ObservableCollection<string> Statuses { get; }

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

			SortCommand = new RelayCommand<OrdersSortField>(Sort);

			FillFilterCollections();

			VisibleOrdersCount = Orders.Count.ToString();
			TotalOrdersCount = _allOrders.Count.ToString();
		}

		#endregion Public Constructors

		#region Private Methods

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

			query = ApplySorting(query);

			Orders = new ObservableCollection<OrderRowViewModel>(query);

			VisibleOrdersCount = Orders.Count.ToString();
		}

		/// <summary>
		/// Сортирует заказы.
		/// </summary>
		/// <param name="query">Коллекция заказов.</param>
		/// <returns>Коллекция заказов.</returns>
		private IEnumerable<OrderRowViewModel> ApplySorting(IEnumerable<OrderRowViewModel> query)
		{
			if (CurrentSortField is null)
			{
				return query;
			}

			switch (CurrentSortField)
			{
				case OrdersSortField.OrderId:
					return IsSortDescending ? query.OrderByDescending(x => x.OrderId) : query.OrderBy(x => x.OrderId);

				case OrdersSortField.CreatedAt:
					return IsSortDescending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt);

				case OrdersSortField.Manager:
					return IsSortDescending ? query.OrderByDescending(x => x.Manager) : query.OrderBy(x => x.Manager);

				case OrdersSortField.Region:
					return IsSortDescending ? query.OrderByDescending(x => x.Region) : query.OrderBy(x => x.Region);

				case OrdersSortField.Status:
					return IsSortDescending ? query.OrderByDescending(x => x.Status) : query.OrderBy(x => x.Status);

				case OrdersSortField.Quantity:
					return IsSortDescending ? query.OrderByDescending(x => x.Quantity) : query.OrderBy(x => x.Quantity);

				case OrdersSortField.NetAmount:
					return IsSortDescending ? query.OrderByDescending(x => x.NetAmount) : query.OrderBy(x => x.NetAmount);

				default:
					return query;
			}
		}

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
		/// Вызывает сортировку.
		/// </summary>
		/// <param name="field">Поле таблицы для сортировки.</param>
		private void Sort(OrdersSortField field)
		{
			if (CurrentSortField == field)
			{
				IsSortDescending = !IsSortDescending;
			}
			else
			{
				CurrentSortField = field;
				IsSortDescending = false;
			}

			ApplyFilters();
		}

		#endregion Private Methods
	}
}