namespace OrderAnalytics.Application.Abstractions
{
	using OrderAnalytics.Application.DTO;

	/// <summary>
	/// Интерфейс для сервиса парсинга строки в заказ.
	/// </summary>
	public interface IOrderImportParser
	{
		#region Public Methods

		/// <summary>
		/// Пытается распарсить строку CSV в заказ.
		/// </summary>
		/// <param name="line">Строка CSV.</param>
		/// <param name="lineNumber">Номер строки.</param>
		/// <returns>Результат парсинга строки.</returns>
		OrderImportParseResult TryParseLine(string line, int lineNumber);

		#endregion Public Methods
	}
}