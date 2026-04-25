using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using OrderAnalytics.Presentation.Desktop.ViewModels;
using System;
using System.Linq;

namespace OrderAnalytics.Presentation.Desktop;

/// <summary>
/// Контрол с экраном импорта.
/// </summary>
public partial class ImportView : UserControl
{
	#region Public Constructors

	/// <summary>
	/// Конструктор класса.
	/// </summary>
	public ImportView()
	{
		InitializeComponent();

		DropZone.AddHandler(DragDrop.DragOverEvent, OnDragOver);
		DropZone.AddHandler(DragDrop.DropEvent, OnDrop);
	}

	/// <summary>
	/// Перемещение в зону drag & drop.
	/// </summary>
	/// <param name="sender">Объект, вызвавший событие.</param>
	/// <param name="e">Аргументы события.</param>
	private void OnDragOver(object? sender, DragEventArgs e)
	{
		if (!e.DataTransfer.Contains(DataFormat.File))
		{
			e.DragEffects = DragDropEffects.None;
			e.Handled = true;
			return;
		}

		var file = e.DataTransfer.TryGetFiles()?.FirstOrDefault();
		string? path = file?.TryGetLocalPath();

		e.DragEffects =
			!string.IsNullOrWhiteSpace(path) &&
			path.EndsWith(".csv", StringComparison.OrdinalIgnoreCase)
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
		if (DataContext is not ImportViewModel vm)
			return;

		var file = e.DataTransfer.TryGetFiles()?.FirstOrDefault();
		string? path = file?.TryGetLocalPath();

		if (string.IsNullOrWhiteSpace(path))
			return;

		if (!path.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
			return;

		vm.SetSelectedFile(path);
	}

	#endregion Public Constructors
}