namespace OrderAnalytics.Presentation.Desktop.ViewModels
{
	using CommunityToolkit.Mvvm.ComponentModel;
	using OrderAnalytics.Domain.Entities;
	using System;

	/// <summary>
	/// Модель представления для строки.
	/// </summary>
	public class OrderRowViewModel : ObservableObject
	{
		#region Public Properties

		/// <summary>
		/// Модель заказа.
		/// </summary>
		public Order Model { get; }

		/// <summary>
		/// Дата заказа.
		/// </summary>
		public DateTime CreatedAt => Model.CreatedAt;

		/// <summary>
		/// Имеет ли заказ статус "Completed".
		/// </summary>
		public bool IsCompleted => Status == "Completed";

		/// <summary>
		/// Имеет ли заказ статус "Failed".
		/// </summary>
		public bool IsFailed => Status == "Failed";

		/// <summary>
		/// Имеет ли заказ статус "Pending".
		/// </summary>
		public bool IsPending => Status == "Pending";

		/// <summary>
		///Проблемный ли заказ.
		/// </summary>
		public bool IsProblematic => Model.IsProblematic;

		/// <summary>
		/// Имеет ли заказ статус "Processing".
		/// </summary>
		public bool IsProcessing => Status == "Processing";

		/// <summary>
		/// Менеджер заказа.
		/// </summary>
		public string Manager => Model.Manager;

		/// <summary>
		/// Стоимость заказа.
		/// </summary>
		public decimal NetAmount => Model.NetAmount;

		/// <summary>
		/// Id заказа.
		/// </summary>
		public string OrderId => Model.OrderId;

		/// <summary>
		/// Количество предметов в заказе.
		/// </summary>
		public int Quantity => Model.Quantity;

		/// <summary>
		/// Регион заказа.
		/// </summary>
		public string Region => Model.Region;

		/// <summary>
		/// Статус заказа.
		/// </summary>
		public string Status => Model.Status;

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="model">Модель заказа.</param>
		public OrderRowViewModel(Order model)
		{
			Model = model;
		}

		#endregion Public Constructors
	}
}