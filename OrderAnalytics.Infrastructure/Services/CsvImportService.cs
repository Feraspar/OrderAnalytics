namespace OrderAnalytics.Infrastructure.Services
{
	using OrderAnalytics.Application.Abstractions;
	using OrderAnalytics.Application.DTO;
	using OrderAnalytics.Domain.Entities;
	using OrderAnalytics.Domain.Enums;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Сервис импорта CSV-файла.
	/// </summary>
	public class CsvImportService : ICsvImportService
	{
		#region Private Fields

		/// <inheritdoc <see cref="IOrderImportParser" />
		private readonly IOrderImportParser _orderImportParser;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="orderImportParser">Сервис парсинга строки в заказ.</param>
		public CsvImportService(IOrderImportParser orderImportParser)
		{
			_orderImportParser = orderImportParser;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <inheritdoc />
		public async Task<CsvImportResult> ImportAsync(string filePath, CancellationToken cancellationToken = default)
		{
			List<Order> orders = new List<Order>();
			List<ValidationError> errors = new List<ValidationError>();

			if (!File.Exists(filePath))
			{
				errors.Add(new ValidationError(0, string.Empty, "File", "File does not exist.", ErrorSeverity.Error));

				return new CsvImportResult(orders, errors);
			}

			try
			{
				using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

				using StreamReader reader = new StreamReader(fileStream);

				int lineNumber = 0;

				while (!reader.EndOfStream)
				{
					cancellationToken.ThrowIfCancellationRequested();

					string? line = await reader.ReadLineAsync();
					lineNumber++;

					if (string.IsNullOrWhiteSpace(line))
					{
						errors.Add(new ValidationError(lineNumber, string.Empty, "Line", "Line is empty", ErrorSeverity.Error));
						continue;
					}

					OrderImportParseResult parseResult = _orderImportParser.TryParseLine(line, lineNumber);

					if (parseResult.Order is not null)
					{
						orders.Add(parseResult.Order);
					}

					if (parseResult.Errors.Count > 0)
					{
						errors.AddRange(parseResult.Errors);
					}
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex)
			{
				errors.Add(new ValidationError(0, string.Empty, "File", $"Failed to read file: {ex.Message}", ErrorSeverity.Error));
			}

			return new CsvImportResult(orders, errors);
		}

		#endregion Public Methods
	}
}