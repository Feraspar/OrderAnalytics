namespace OrderAnalytics.Presentation.Desktop.Converters
{
	using Avalonia.Data.Converters;
	using OrderAnalytics.Presentation.Desktop.Enums;
	using System;
	using System.Collections.Generic;
	using System.Globalization;

	/// <summary>
	/// Конвертер для конвертации выбранной страницы в соответствующую иконку в меню навигации.
	/// </summary>
	public class NavigationIconConverter : IMultiValueConverter
	{
		#region Public Methods

		/// <summary>
		/// Конвертирует выбранную страницу в икноку.
		/// </summary>
		/// <param name="values">Значения на вход.</param>
		/// <param name="targetType">Тип свойства, для которого выполняется конвертация.</param>
		/// <param name="parameter">Дополнительный параметр конвертера.</param>
		/// <param name="culture">Информация о языковой культуре.</param>
		/// <returns>Иконка, соответствующая пункту навигации.</returns>
		public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
		{
			if (values[0] is not AppPage page || values[1] is not bool isSelected)
			{
				return null;
			}

			string resourceKey = page switch
			{
				AppPage.ImportPage => isSelected ? "SelectedImportIcon" : "ImportIcon",
				AppPage.OrdersPage => isSelected ? "SelectedOrdersIcon" : "OrdersIcon",
				AppPage.ErrorsPage => isSelected ? "SelectedErrorIcon" : "ErrorIcon",
				_ => string.Empty
			};

			if (string.IsNullOrWhiteSpace(resourceKey))
			{
				return null;
			}

			if (Avalonia.Application.Current?.Resources.TryGetResource(resourceKey, null, out object? resource) == true)
			{
				return resource;
			}

			return null;
		}

		#endregion Public Methods
	}
}