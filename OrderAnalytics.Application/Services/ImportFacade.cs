namespace OrderAnalytics.Application.Services
{
	using OrderAnalytics.Application.Abstractions;
	using OrderAnalytics.Application.DTO;
	using OrderAnalytics.Domain.Services;
	using System.Threading.Tasks;

	/// <summary>
	/// Сервис для обработки данных заказов.
	/// </summary>
	public class ImportFacade : IImportFacade
	{
		#region Private Fields

		/// <inheritdoc <see cref="ICsvImportService" />
		private readonly ICsvImportService _csvImportService;

		/// <inheritdoc <see cref="ImportDataService" />
		private readonly ImportDataService _importDataService;

		/// <inheritdoc <see cref="IValidationService" />
		private readonly IValidationService _validationService;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="csvImportService">Сервис импорта CSV-файла.</param>
		/// <param name="validationService">Сервис валидации.</param>
		/// <param name="importDataService">Сервис для хранения импортированных данных.</param>
		public ImportFacade(ICsvImportService csvImportService, IValidationService validationService, ImportDataService importDataService)
		{
			_csvImportService = csvImportService;
			_validationService = validationService;
			_importDataService = importDataService;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <inheritdoc />
		public async Task<ImportWorkflowResult> ImportAsync(string filePath, CancellationToken cancellationToken = default)
		{
			var importResult = await _csvImportService.ImportAsync(filePath, cancellationToken);
			var validationResult = _validationService.Validate(importResult.Orders);

			_importDataService.SetData(validationResult.Orders, validationResult.Errors, importResult.Errors);

			return new ImportWorkflowResult(validationResult.Orders, importResult.Errors, validationResult.Errors);
		}

		#endregion Public Methods
	}
}