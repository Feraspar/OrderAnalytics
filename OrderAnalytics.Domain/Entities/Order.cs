namespace OrderAnalytics.Domain.Entities
{
	using System;

	/// <summary>
	/// Модель заказа.
	/// </summary>
	public class Order
	{
		#region Public Properties

		/// <summary>
		/// Id заказа.
		/// </summary>
		public string OrderId { get; set; } = string.Empty;

		/// <summary>
		/// Номер строки в файле.
		/// </summary>
		public int LineNumber { get; set; }

		/// <summary>
		/// Дата создания заказа.
		/// </summary>
		public DateTime CreatedAt { get; set; }

		/// <summary>
		/// Имя менеджера.
		/// </summary>
		public string Manager { get; set; } = string.Empty;

		/// <summary>
		/// Регион.
		/// </summary>
		public string Region { get; set; } = string.Empty;

		/// <summary>
		/// Продукт.
		/// </summary>
		public string Product { get; set; } = string.Empty;

		/// <summary>
		/// Количество продукта.
		/// </summary>
		public int Quantity { get; set; }

		/// <summary>
		/// Цена единицы товара.
		/// </summary>
		public decimal UnitPrice { get; set; }

		/// <summary>
		/// Скидка.
		/// </summary>
		public decimal Discount { get; set; }

		/// <summary>
		/// Статус заказа.
		/// </summary>
		public string Status { get; set; } = string.Empty;

		/// <summary>
		/// Есть ли ошибки в заказе.
		/// </summary>
		public bool IsProblematic { get; set; }

		/// <summary>
		/// Итоговая стоимость заказа.
		/// </summary>
		public decimal NetAmount => Quantity * UnitPrice * (1 - Discount);

		#endregion Public Properties
	}
}