namespace OrderAnalytics.Presentation.Desktop.ViewModels
{
	using CommunityToolkit.Mvvm.ComponentModel;
	using OrderAnalytics.Presentation.Desktop.Enums;

	/// <summary>
	/// Модель представления для элемента навигационного меню.
	/// </summary>
	public class NavigationItemViewModel : ObservableObject
	{
		#region Private Fields

		/// <summary>
		/// Поле для хранения булевого свойства выбранной страницы.
		/// </summary>
		private bool _isSelected;

		#endregion Private Fields

		#region Public Properties

		/// <summary>
		/// Выбрана ли текущая страница.
		/// </summary>
		public bool IsSelected
		{
			get => _isSelected;
			set => SetProperty(ref _isSelected, value);
		}

		/// <summary>
		/// Тип страницы.
		/// </summary>
		public AppPage Page { get; }

		/// <summary>
		/// Заголовок.
		/// </summary>
		public string Title { get; }

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="title">Заголовок.</param>
		/// <param name="page">Тип страницы</param>
		public NavigationItemViewModel(string title, AppPage page)
		{
			Title = title;
			Page = page;
		}

		#endregion Public Constructors
	}
}