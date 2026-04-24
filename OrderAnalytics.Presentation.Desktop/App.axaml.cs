namespace OrderAnalytics.Presentation.Desktop
{
	using Avalonia;
	using Avalonia.Controls.ApplicationLifetimes;
	using Avalonia.Markup.Xaml;
	using Microsoft.Extensions.DependencyInjection;
	using OrderAnalytics.Application.Abstractions;
	using OrderAnalytics.Domain.Services;
	using OrderAnalytics.Infrastructure.Services;
	using OrderAnalytics.Presentation.Desktop.Services;
	using OrderAnalytics.Presentation.Desktop.ViewModels;
	using OrderAnalytics.Presentation.Desktop.Views;
	using System;

	/// <summary>
	/// Класс приложения Avalonia.
	/// </summary>
	public partial class App : Application
	{
		#region Public Properties

		/// <summary>
		/// Контейнер зависимостей приложения.
		/// </summary>
		public IServiceProvider Services { get; private set; } = null!;

		#endregion Public Properties

		#region Public Methods

		/// <summary>
		/// Инициализирует XAML-разметку приложения.
		/// </summary>
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}

		/// <summary>
		/// Завершает инициализацию приложения и создает главное окно.
		/// </summary>
		public override void OnFrameworkInitializationCompleted()
		{
			ServiceCollection services = new ServiceCollection();
			ConfigureServices(services);

			Services = services.BuildServiceProvider();

			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				desktop.MainWindow = new MainWindow
				{
					DataContext = Services.GetRequiredService<MainWindowViewModel>()
				};
			}

			base.OnFrameworkInitializationCompleted();
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Регистрирует сервисы приложения в контейнере зависимостей.
		/// </summary>
		/// <param name="services">Коллекция сервисов.</param>
		private static void ConfigureServices(ServiceCollection services)
		{
			services.AddTransient<MainWindowViewModel>();

			services.AddSingleton<IOrderImportParser, OrderImportParser>();
			services.AddSingleton<ICsvImportService, CsvImportService>();
			services.AddSingleton<IValidationService, ValidationService>();
			services.AddSingleton<IFilePickerService, FilePickerService>();
		}

		#endregion Private Methods
	}
}