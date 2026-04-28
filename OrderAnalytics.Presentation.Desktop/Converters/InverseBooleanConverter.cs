namespace OrderAnalytics.Presentation.Desktop.Converters
{
	using Avalonia.Data.Converters;
	using System;
	using System.Globalization;

	/// <summary>
	/// Конвертер для инверсии булевого значения.
	/// </summary>
	public class InverseBooleanConverter : IValueConverter
	{
		#region Public Methods

		/// <summary>
		/// Конвертирует выбранную страницу в икноку.
		/// </summary>
		/// <param name="value">Значение на вход.</param>
		/// <param name="targetType">Тип свойства, для которого выполняется конвертация.</param>
		/// <param name="parameter">Дополнительный параметр конвертера.</param>
		/// <param name="culture">Информация о языковой культуре.</param>
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return value is bool boolValue && !boolValue;
		}

		/// <summary>
		/// Уонвертирует обратно.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion Public Methods
	}
}