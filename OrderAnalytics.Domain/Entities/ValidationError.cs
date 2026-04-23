namespace OrderAnalytics.Domain.Entities
{
	using OrderAnalytics.Domain.Enums;

	/// <summary>
	/// Модель для ошибки валидации.
	/// </summary>
	public class ValidationError
	{
		#region Public Properties

		/// <summary>
		/// Тип ошибки.
		/// </summary>
		public ErrorSeverity ErrorSeverity { get; }

		/// <summary>
		/// Номер строки.
		/// </summary>
		public int LineNumber { get; }

		/// <summary>
		/// Поле с ошибкой.
		/// </summary>
		public string Field {  get; }

		/// <summary>
		/// Описание ошибки.
		/// </summary>
		public string Message { get; }

		/// <summary>
		/// Id заказа.
		/// </summary>
		public string OrderId { get; }

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="lineNumber">Номер строки.</param>
		/// <param name="orderId">Id заказа.</param>
		/// <param name="field">Поле с ошибкой.</param>
		/// <param name="message">Описание ошибки.</param>
		/// <param name="errorSeverity">Тип ошибки.</param>
		public ValidationError(int lineNumber, string orderId, string field, string message, ErrorSeverity errorSeverity)
		{
			LineNumber = lineNumber;
			OrderId = orderId;
			Field = field;
			Message = message;
			ErrorSeverity = errorSeverity;
		}

		#endregion Public Constructors
	}
}