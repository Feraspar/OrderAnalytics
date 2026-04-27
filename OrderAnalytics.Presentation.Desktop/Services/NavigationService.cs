namespace OrderAnalytics.Presentation.Desktop.Services
{
	using Microsoft.Extensions.DependencyInjection;
	using OrderAnalytics.Presentation.Desktop.Enums;
	using OrderAnalytics.Presentation.Desktop.ViewModels;
	using System;

	public class NavigationService : INavigationService
	{
		#region Private Fields

		/// <summary>
		/// Контейнер зависимостей приложения.
		/// </summary>
		private readonly IServiceProvider _serviceProvider;

		#endregion Private Fields

		#region Public Properties

		/// <inheritdoc />
		public AppPage CurrentPage { get; private set; }

		/// <inheritdoc />
		public ViewModelBase CurrentViewModel { get; private set; }

		#endregion Public Properties

		#region Public Events

		/// <inheritdoc />
		public event Action? Navigated;

		#endregion Public Events

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="serviceProvider">Контейнер зависимостей приложения.</param>
		public NavigationService(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;

			CurrentPage = AppPage.ImportPage;
			CurrentViewModel = CreateViewModel(CurrentPage);
		}

		#endregion Public Constructors

		#region Public Methods

		/// <inheritdoc />
		public void NavigateTo(AppPage page)
		{
			if (CurrentPage == page)
			{
				return;
			}

			CurrentPage = page;
			CurrentViewModel = CreateViewModel(page);

			Navigated?.Invoke();
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Определяет модель представления для выбранной страницы.
		/// </summary>
		/// <param name="page">Тип страницы</param>
		/// <returns>Модель представления.</returns>
		private ViewModelBase CreateViewModel(AppPage page)
		{
			return page switch
			{
				AppPage.ImportPage => _serviceProvider.GetRequiredService<ImportViewModel>(),
				AppPage.OrdersPage => _serviceProvider.GetRequiredService<OrdersViewModel>(),
				AppPage.ErrorsPage => _serviceProvider.GetRequiredService<ErrorsViewModel>(),
				_ => throw new ArgumentOutOfRangeException(nameof(page), page, null)
			};
		}

		#endregion Private Methods
	}
}