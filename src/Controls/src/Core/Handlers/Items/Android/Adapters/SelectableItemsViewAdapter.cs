﻿#nullable disable
using System;
using System.Collections.Generic;
using Android.Content;
using AndroidX.RecyclerView.Widget;
using Object = Java.Lang.Object;

namespace Microsoft.Maui.Controls.Handlers.Items
{
	public class SelectableItemsViewAdapter<TItemsView, TItemsSource> : StructuredItemsViewAdapter<TItemsView, TItemsSource>
		where TItemsView : SelectableItemsView
		where TItemsSource : IItemsViewSource
	{
		List<SelectableViewHolder> _currentViewHolders = new List<SelectableViewHolder>();

		int _adapterPosition;
		object _previousSelectedItem = null;
		SelectableItemsView _selectableItemsView;

		protected internal SelectableItemsViewAdapter(TItemsView selectableItemsView,
			Func<View, Context, ItemContentView> createView = null) : base(selectableItemsView, createView)
		{
			_selectableItemsView = selectableItemsView;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			base.OnBindViewHolder(holder, position);

			if (!(holder is SelectableViewHolder selectable))
			{
				return;
			}

			// Watch for clicks so the user can select the item held by this ViewHolder
			selectable.Clicked += SelectableClicked;

			// Keep track of the view holders here so we can clear the native selection
			_currentViewHolders.Add(selectable);

			// Make sure that if this item is one of the selected items, it's marked as selected
			selectable.IsSelected = PositionIsSelected(position);
		}

		public override void OnViewRecycled(Object holder)
		{
			if (holder is SelectableViewHolder selectable)
			{
				_currentViewHolders.Remove(selectable);
				selectable.Clicked -= SelectableClicked;
				selectable.IsSelected = false;
			}

			base.OnViewRecycled(holder);
		}

		internal void ClearPlatformSelection()
		{
			for (int i = 0; i < _currentViewHolders.Count; i++)
			{
				_currentViewHolders[i].IsSelected = false;
			}
		}

		internal void MarkPlatformSelection(object selectedItem)
		{
			if (selectedItem == null)
			{
				return;
			}

			int position = ItemsView.SelectionMode == SelectionMode.Multiple ? GetPositionForItem(selectedItem) : _adapterPosition;

			for (int i = 0; i < _currentViewHolders.Count; i++)
			{
				if (_currentViewHolders[i].BindingAdapterPosition == position)
				{
					_currentViewHolders[i].IsSelected = true;
					return;
				}
			}
		}

		int[] GetSelectedPositions()
		{
			switch (ItemsView.SelectionMode)
			{
				case SelectionMode.None:
					return Array.Empty<int>();

				case SelectionMode.Single:
					var selectedItem = ItemsView.SelectedItem;
					if (selectedItem == null)
					{
						return Array.Empty<int>();
					}

					return new int[1] { GetPositionForItem(selectedItem) };

				case SelectionMode.Multiple:
					var selectedItems = ItemsView.SelectedItems;
					var result = new int[selectedItems.Count];

					for (int n = 0; n < result.Length; n++)
					{
						result[n] = GetPositionForItem(selectedItems[n]);
					}

					return result;
			}

			return Array.Empty<int>();
		}

		bool PositionIsSelected(int position)
		{
			var selectedPositions = GetSelectedPositions();
			foreach (var selectedPosition in selectedPositions)
			{
				if (selectedPosition == position)
				{
					return true;
				}
			}

			return false;
		}

		void SelectableClicked(object sender, int adapterPosition)
		{
			if (adapterPosition >= 0 && adapterPosition < ItemsSource?.Count)
			{
				_adapterPosition = adapterPosition;
				UpdateMauiSelection(adapterPosition);
			}
		}

		void UpdateMauiSelection(int adapterPosition)
		{
			var mode = ItemsView.SelectionMode;

			switch (mode)
			{
				case SelectionMode.None:
					// Selection's not even on, so there's nothing to do here
					return;
				case SelectionMode.Single:
					ItemsView.SelectedItem = ItemsSource.GetItem(adapterPosition);
					_previousSelectedItem = ItemsView.SelectedItem;
					// if adapterPosition is valid and the item is not already selected, we need to update the selection manually
					if (adapterPosition < _currentViewHolders.Count && adapterPosition >= 0 && !_currentViewHolders[adapterPosition].IsSelected)
					{
						_selectableItemsView.SelectedItemPropertyChanged(_previousSelectedItem, ItemsView.SelectedItem);
					}

					return;
				case SelectionMode.Multiple:
					var item = ItemsSource.GetItem(adapterPosition);
					var selectedItems = ItemsView.SelectedItems;

					if (selectedItems.Contains(item))
					{
						selectedItems.Remove(item);
					}
					else
					{
						selectedItems.Add(item);
					}
					return;
			}
		}
	}
}
