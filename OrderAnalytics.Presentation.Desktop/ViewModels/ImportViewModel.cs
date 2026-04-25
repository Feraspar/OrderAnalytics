namespace OrderAnalytics.Presentation.Desktop.ViewModels
{
	using CommunityToolkit.Mvvm.Input;
	using OrderAnalytics.Application.Abstractions;
	using OrderAnalytics.Application.DTO;
	using OrderAnalytics.Presentation.Desktop.Services;
	using System.IO;
	using System.Threading.Tasks;

	/// <summary>
	/// Модель представления для экрана импорта.
	/// </summary>
	public class ImportViewModel : ViewModelBase
	{
		#region Private Fields

		/// <inheritdoc <see cref="ICsvImportService" />
		private readonly ICsvImportService _csvImportService;

		/// <inheritdoc <see cref="IFilePickerService" />
		private readonly IFilePickerService _filePickerService;

		/// <summary>
		/// Поле для хранения булевого значения доступности кнопки импорта файла.
		/// </summary>
		private bool _isBusy = true;

		/// <summary>
		/// Поле для хранения пути к файлу.
		/// </summary>
		private string? _selectedFilePath;

		/// <summary>
		/// Поле для хранения текста текущего состояния.
		/// </summary>
		private string _summaryText = "Select file";

		#endregion Private Fields

		#region Public Properties

		/// <summary>
		/// Команда выбора файла.
		/// </summary>
		public IAsyncRelayCommand ChooseFileCommand { get; }

		/// <summary>
		/// Команда импорта файла.
		/// </summary>
		public IAsyncRelayCommand ImportFileCommand { get; }

		/// <summary>
		/// Команда для назначения пути к файлу.
		/// </summary>
		public IRelayCommand<string> SetSelectedFileCommand { get; }

		/// <summary>
		/// Доступна ли кнопка импорта файла.
		/// </summary>
		public bool IsBusy
		{
			get => _isBusy;
			set
			{
				if (SetProperty(ref _isBusy, value))
				{
					ImportFileCommand.NotifyCanExecuteChanged();
				}
			}
		}

		/// <summary>
		/// Путь к выбранному файлу.
		/// </summary>
		public string? SelectedFilePath
		{
			get => _selectedFilePath;
			set => SetProperty(ref _selectedFilePath, value);
		}

		/// <summary>
		/// Текст о текущем состоянии.
		/// </summary>
		public string SummaryText
		{
			get => _summaryText;
			set => SetProperty(ref _summaryText, value);
		}

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="csvImportService">Сервис импорта CSV файла.</param>
		/// <param name="filePickerService">Сервис выбора файла.</param>
		public ImportViewModel(ICsvImportService csvImportService, IFilePickerService filePickerService)
		{
			_csvImportService = csvImportService;
			_filePickerService = filePickerService;

			ChooseFileCommand = new AsyncRelayCommand(ChooseFileAsync);
			ImportFileCommand = new AsyncRelayCommand(ImportFileAsync, CanImport);
			SetSelectedFileCommand = new RelayCommand<string>(SetSelectedFile);
		}

		#endregion Public Constructors

		#region Private Methods

		/// <summary>
		/// Доступна ли кнопка импорта.
		/// </summary>
		/// <returns>Булевое значение.</returns>
		private bool CanImport()
		{
			return !IsBusy;
		}

		/// <summary>
		/// Выбирает файл из файловой системы.
		/// </summary>
		private async Task ChooseFileAsync()
		{
			IsBusy = true;

			string? filePath = await _filePickerService.PickCsvFileAsync();

			if (string.IsNullOrWhiteSpace(filePath))
			{
				return;
			}

			SetSelectedFile(filePath);
		}

		/// <summary>
		/// Импортирует файл.
		/// </summary>
		private async Task ImportFileAsync()
		{
			if (string.IsNullOrWhiteSpace(SelectedFilePath))
			{
				return;
			}

			SummaryText = "Import is in progress...";

			CsvImportResult result = await _csvImportService.ImportAsync(SelectedFilePath);

			if (result.Errors.Count > 0)
			{
				SummaryText = $"Successfully created {result.Orders.Count} orders. Failed rows: {result.Errors.Count}.";
				return;
			}

			SummaryText = $"Successfully created {result.Orders.Count} orders.";
		}

		/// <summary>
		/// Помечает выбранный файл.
		/// </summary>
		/// <param name="filePath">Путь к файлу.</param>
		private void SetSelectedFile(string? filePath)
		{
			SelectedFilePath = filePath;
			SummaryText = $"File selected: {Path.GetFileName(filePath)}";
			IsBusy = false;
		}

		#endregion Private Methods
	}
}