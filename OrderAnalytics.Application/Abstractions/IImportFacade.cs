namespace OrderAnalytics.Application.Abstractions
{
	using OrderAnalytics.Application.DTO;
	using System.Threading.Tasks;

	/// <summary>
	/// Интерфейс для сервиса-фасада.
	/// </summary>
	public interface IImportFacade
	{
		#region Public Methods

		/// <summary>
		/// Импортирует CSV файл и валидирует данные.
		/// </summary>
		/// <param name="filePath">Путь к файлу.</param>
		/// <param name="cancellationToken">Токен для отмены операции.</param>
		/// <returns>Результат импорта.</returns>
		Task<ImportWorkflowResult> ImportAsync(string filePath, CancellationToken cancellationToken = default);

		#endregion Public Methods
	}
}