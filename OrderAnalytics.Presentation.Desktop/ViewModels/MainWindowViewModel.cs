namespace OrderAnalytics.Presentation.Desktop.ViewModels
{
	/// <summary>
	/// Модель представления главного окна.
	/// </summary>
	public class MainWindowViewModel : ViewModelBase
	{
		#region Private Fields

		/// <summary>
		/// Поле для хранения текущей модели представления в контроле контента.
		/// </summary>
		private ViewModelBase _currentViewModel;

		#endregion Private Fields

		#region Public Properties

		/// <summary>
		/// Текущая модель представления в контроле контента.
		/// </summary>
		public ViewModelBase CurrentViewModel
		{
			get => _currentViewModel;
			set => SetProperty(ref _currentViewModel, value);
		}

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="importViewModel">Модель представления экрана импорта.</param>
		public MainWindowViewModel(ImportViewModel importViewModel)
		{
			CurrentViewModel = importViewModel;
		}

		#endregion Public Constructors
	}
}