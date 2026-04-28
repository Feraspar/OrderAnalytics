namespace OrderAnalytics.Presentation.Desktop.ViewModels
{
	using CommunityToolkit.Mvvm.Input;
	using OrderAnalytics.Presentation.Desktop.Enums;
	using OrderAnalytics.Presentation.Desktop.Services;
	using System.Collections.ObjectModel;

	/// <summary>
	/// Модель представления главного окна.
	/// </summary>
	public class MainWindowViewModel : ViewModelBase
	{
		#region Private Fields

		/// <inheritdoc <see cref="INavigationService" />
		private readonly INavigationService _navigationService;

		/// <summary>
		/// Поле для хранения типа текущей страницы.
		/// </summary>
		private AppPage _currentPage;

		/// <summary>
		/// Поле для хранения текущей модели представления в контроле контента.
		/// </summary>
		private ViewModelBase? _currentViewModel;

		#endregion Private Fields

		#region Public Properties

		/// <summary>
		/// Тип текущей страницы.
		/// </summary>
		public AppPage CurrentPage
		{
			get => _currentPage;
			set => SetProperty(ref _currentPage, value);
		}

		/// <summary>
		/// Заголовок выбранной страницы.
		/// </summary>
		public string CurrentPageTitle => CurrentPage switch
		{
			AppPage.ImportPage => "Import",
			AppPage.OrdersPage => "Orders",
			AppPage.ErrorsPage => "Errors",
			_ => string.Empty
		};

		/// <summary>
		/// Текущая модель представления в контроле контента.
		/// </summary>
		public ViewModelBase? CurrentViewModel
		{
			get => _currentViewModel;
			set => SetProperty(ref _currentViewModel, value);
		}

		/// <summary>
		/// Команды для навигации.
		/// </summary>
		public IRelayCommand<AppPage> NavigateCommand { get; }

		/// <summary>
		/// Коллекция элементов меню навигации.
		/// </summary>
		public ObservableCollection<NavigationItemViewModel> NavigationItems { get; }

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="navigationService">Сервис навигации.</param>
		public MainWindowViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
			CurrentPage = _navigationService.CurrentPage;
			CurrentViewModel = _navigationService.CurrentViewModel;

			NavigationItems =
			[
				new NavigationItemViewModel("Import", AppPage.ImportPage),
				new NavigationItemViewModel("Orders", AppPage.OrdersPage),
				new NavigationItemViewModel("Errors", AppPage.ErrorsPage)
			];

			NavigateCommand = new RelayCommand<AppPage>(Navigate);

			UpdateSelection();

			_navigationService.Navigated += OnNavigated;
		}

		#endregion Public Constructors

		#region Private Methods

		/// <summary>
		/// Навигирует на выбранную страницу.
		/// </summary>
		/// <param name="page">Тип выбранной страницы.</param>
		private void Navigate(AppPage page)
		{
			_navigationService.NavigateTo(page);
		}

		/// <summary>
		/// Изменяет текущую страницу.
		/// </summary>
		private void OnNavigated()
		{
			CurrentPage = _navigationService.CurrentPage;
			CurrentViewModel = _navigationService.CurrentViewModel;
			OnPropertyChanged(nameof(CurrentPageTitle));

			UpdateSelection();
		}

		/// <summary>
		/// Помечает выбранную страницу.
		/// </summary>
		private void UpdateSelection()
		{
			foreach (NavigationItemViewModel item in NavigationItems)
			{
				item.IsSelected = item.Page == CurrentPage;
			}
		}

		#endregion Private Methods
	}
}