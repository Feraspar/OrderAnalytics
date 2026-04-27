namespace OrderAnalytics.Presentation.Desktop.Services
{
	using OrderAnalytics.Presentation.Desktop.Enums;
	using OrderAnalytics.Presentation.Desktop.ViewModels;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// Интерфейс для сервиса навигации.
	/// </summary>
	public interface INavigationService
	{
		/// <summary>
		/// Текущая страница.
		/// </summary>
		AppPage CurrentPage { get; }

		/// <summary>
		/// Текущая модель представления.
		/// </summary>
		ViewModelBase CurrentViewModel { get; }

		/// <summary>
		/// Событие для уведомления о навигации.
		/// </summary>
		event Action? Navigated;

		/// <summary>
		/// Навигация на выбранную страницу.
		/// </summary>
		/// <param name="page">Тип страницы.</param>
		void NavigateTo(AppPage page);
	}
}
