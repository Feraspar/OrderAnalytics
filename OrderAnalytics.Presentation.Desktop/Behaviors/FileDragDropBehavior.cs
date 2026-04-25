namespace OrderAnalytics.Presentation.Desktop.Behaviors
{
	using Avalonia;
	using Avalonia.Controls;
	using Avalonia.Input;
	using Avalonia.Platform.Storage;
	using Avalonia.Xaml.Interactivity;
	using System;
	using System.Linq;
	using System.Windows.Input;

	/// <summary>
	/// Поведение для drag & drop.
	/// </summary>
	public class FileDragDropBehavior : Behavior<Control>
	{
		#region Public Fields

		/// <summary>
		/// Свойство зависимости для привязки команды назначения пути к файлу.
		/// </summary>
		public static readonly StyledProperty<ICommand?> CommandProperty = AvaloniaProperty.Register<FileDragDropBehavior, ICommand?>(nameof(Command));

		#endregion Public Fields

		#region Public Properties

		/// <summary>
		/// Команда для назначения пути к файлу.
		/// </summary>
		public ICommand? Command
		{
			get => GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		#endregion Public Properties

		#region Protected Methods

		/// <summary>
		/// Присоединяет поведение к контролу.
		/// </summary>
		protected override void OnAttached()
		{
			base.OnAttached();

			if (AssociatedObject == null)
			{
				return;
			}

			DragDrop.SetAllowDrop(AssociatedObject, true);
			AssociatedObject.AddHandler(DragDrop.DragOverEvent, OnDragOver);
			AssociatedObject.AddHandler(DragDrop.DropEvent, OnDrop);
		}

		/// <summary>
		/// Отсоединяет поведение от контрола.
		/// </summary>
		protected override void OnDetaching()
		{
			if (AssociatedObject != null)
			{
				AssociatedObject.RemoveHandler(DragDrop.DragOverEvent, OnDragOver);
				AssociatedObject.RemoveHandler(DragDrop.DropEvent, OnDrop);
			}

			base.OnDetaching();
		}

		#endregion Protected Methods

		#region Private Methods

		/// <summary>
		/// Перемещение в зону drag & drop.
		/// </summary>
		/// <param name="sender">Объект, вызвавший событие.</param>
		/// <param name="e">Аргументы события.</param>
		private void OnDragOver(object? sender, DragEventArgs e)
		{
			if (!e.DataTransfer.Formats.Contains(DataFormat.File))
			{
				e.DragEffects = DragDropEffects.None;
				e.Handled = true;
				return;
			}

			var file = e.DataTransfer.TryGetFiles()?.FirstOrDefault();
			string? path = file?.TryGetLocalPath();

			e.DragEffects = !string.IsNullOrWhiteSpace(path) && path.EndsWith(".csv", StringComparison.OrdinalIgnoreCase)
					? DragDropEffects.Copy
					: DragDropEffects.None;

			e.Handled = true;
		}

		/// <summary>
		/// Поместили файл в зону drag & drop.
		/// </summary>
		/// <param name="sender">Объект, вызвавший событие.</param>
		/// <param name="e">Аргументы события.</param>
		private void OnDrop(object? sender, DragEventArgs e)
		{
			var file = e.DataTransfer.TryGetFiles()?.FirstOrDefault();
			string? path = file?.TryGetLocalPath();

			if (string.IsNullOrWhiteSpace(path))
				return;

			if (!path.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
				return;

			if (Command?.CanExecute(path) == true)
			{
				Command.Execute(path);
			}
		}

		#endregion Private Methods
	}
}