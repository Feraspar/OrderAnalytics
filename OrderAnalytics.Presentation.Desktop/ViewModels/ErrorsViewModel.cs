using OrderAnalytics.Application.Services;
using OrderAnalytics.Domain.Entities;
using OrderAnalytics.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OrderAnalytics.Presentation.Desktop.ViewModels
{
	/// <summary>
	/// Модель представления страницы заказов.
	/// </summary>
	public class ErrorsViewModel : ViewModelBase
	{
		#region Private Fields

		/// <summary>
		/// Все ошибки.
		/// </summary>
		private readonly List<ValidationError> _allErrors;

		/// <inheritdoc <see cref="ImportDataService" />
		private readonly ImportDataService _importDataService;

		/// <summary>
		/// Поле для хранения коллекции отображаемых ошибок.
		/// </summary>
		private ObservableCollection<ValidationError> _errors;

		/// <summary>
		/// Поле для хранения текста поиска.
		/// </summary>
		private string? _searchErrorMessage;

		/// <summary>
		/// Поле для хранения выбранного типа ошибки.
		/// </summary>
		private string? _selectedSeverity;

		#endregion Private Fields

		#region Public Properties

		/// <summary>
		/// Коллекция отображаемых ошибок.
		/// </summary>
		public ObservableCollection<ValidationError> Errors
		{
			get => _errors;
			set => SetProperty(ref _errors, value);
		}

		/// <summary>
		/// Есть ли ошибки в коллекции.
		/// </summary>
		public bool HasErrors => Errors.Count > 0;

		/// <summary>
		/// Текст поиска описания ошибки.
		/// </summary>
		public string? SearchErrorMessage
		{
			get => _searchErrorMessage;
			set
			{
				if (SetProperty(ref _searchErrorMessage, value))
				{
					ApplyFilters();
				}
			}
		}

		/// <summary>
		/// Выбранный тип ошибки.
		/// </summary>
		public string? SelectedSeverity
		{
			get => _selectedSeverity;
			set
			{
				if (SetProperty(ref _selectedSeverity, value))
				{
					ApplyFilters();
				}
			}
		}

		/// <summary>
		/// Коллекция типов ошибок.
		/// </summary>
		public ObservableCollection<string> Severities { get; }

		/// <summary>
		/// Количество ошибок.
		/// </summary>
		public string ErrorsCount => Errors.Count(x => x.ErrorSeverity == ErrorSeverity.Error).ToString();

		/// <summary>
		/// Количество предупреждений.
		/// </summary>
		public string WarningsCount => Errors.Count(x => x.ErrorSeverity == ErrorSeverity.Warning).ToString();

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="importDataService">Сервис для хранения импортированных данных.</param>
		public ErrorsViewModel(ImportDataService importDataService)
		{
			_importDataService = importDataService;
			_allErrors = importDataService.ValidationErrors;

			Errors = new ObservableCollection<ValidationError>(_allErrors);

			Severities = new ObservableCollection<string>();
			FillFilterCollections();
		}

		#endregion Public Constructors

		#region Private Methods

		/// <summary>
		/// Применяет фильтры.
		/// </summary>
		private void ApplyFilters()
		{
			IEnumerable<ValidationError> query = _allErrors;

			if (!string.IsNullOrWhiteSpace(SearchErrorMessage))
			{
				query = query.Where(x => x.Message.Contains(SearchErrorMessage, StringComparison.OrdinalIgnoreCase));
			}

			if (!string.IsNullOrWhiteSpace(SelectedSeverity) && SelectedSeverity != "All")
			{
				query = query.Where(x => x.ErrorSeverity.ToString() == SelectedSeverity);
			}

			Errors = new ObservableCollection<ValidationError>(query);

			OnPropertyChanged(nameof(HasErrors));
			OnPropertyChanged(nameof(ErrorsCount));
			OnPropertyChanged(nameof(WarningsCount));
		}

		/// <summary>
		/// Заполняет коллекцию фильтров данными.
		/// </summary>
		private void FillFilterCollections()
		{
			Severities.Add("All");
			foreach (string severity in _allErrors.Select(x => x.ErrorSeverity.ToString()).Distinct().OrderBy(x => x))
			{
				Severities.Add(severity);
			}
		}

		#endregion Private Methods
	}
}