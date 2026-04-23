namespace OrderAnalytics.Domain.Enums
{
	/// <summary>
	/// Статус заказа.
	/// </summary>
	public enum OrderStatus
	{
		Processing = 0,
		Completed = 1,
		Cancelled = 2,
		Pending = 3,
		Failed = 4
	}
}