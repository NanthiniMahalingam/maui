#nullable disable
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
		int? _selectedPosition = null;
		HashSet<int> _selectedPositions = new HashSet<int>();

		protected internal SelectableItemsViewAdapter(TItemsView selectableItemsView,
			Func<View, Context, ItemContentView> createView = null) : base(selectableItemsView, createView)
		{
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

		void SelectableClicked(object sender, int adapterPosition)
		{
			if (adapterPosition >= 0 && adapterPosition < ItemsSource?.Count)
			{
				var mode = ItemsView.SelectionMode;
				switch (mode)
				{
					case SelectionMode.Single:
						_selectedPosition = adapterPosition;
						_selectedPositions.Clear();
						_selectedPositions.Add(adapterPosition);
						UpdateMauiSelection(adapterPosition);
						NotifySelectionChanged();
						break;
					case SelectionMode.Multiple:
						if (_selectedPositions.Contains(adapterPosition))
							_selectedPositions.Remove(adapterPosition);
						else
							_selectedPositions.Add(adapterPosition);
						UpdateMauiSelection(adapterPosition);
						NotifySelectionChanged();
						break;
				}
			}
		}

		internal void MarkPlatformSelection(object selectedItem)
		{
			var mode = ItemsView.SelectionMode;


			_selectedPosition = GetPositionForItem(selectedItem);
			if (mode == SelectionMode.Multiple)
			{
				_selectedPositions.Add(_selectedPosition.Value);
			}
			else if (_selectedPosition.HasValue)
			{
				_selectedPositions.Clear();
				_selectedPositions.Add(_selectedPosition.Value);
			}

			for (int i = 0; i < _currentViewHolders.Count; i++)
			{
				var holder = _currentViewHolders[i];
				if (mode == SelectionMode.Single)
				{
					holder.IsSelected = _selectedPosition.HasValue && holder.BindingAdapterPosition == _selectedPosition.Value;
				}
				else if (mode == SelectionMode.Multiple)
				{
					holder.IsSelected = _selectedPositions.Contains(holder.BindingAdapterPosition);
				}
				else
				{
					holder.IsSelected = false;
				}
			}
		}

		void NotifySelectionChanged()
		{
			var mode = ItemsView.SelectionMode;
			for (int i = 0; i < _currentViewHolders.Count; i++)
			{
				var holder = _currentViewHolders[i];
				if (mode == SelectionMode.Single)
				{
					holder.IsSelected = _selectedPosition.HasValue && holder.BindingAdapterPosition == _selectedPosition.Value;
				}
				else if (mode == SelectionMode.Multiple)
				{
					holder.IsSelected = _selectedPositions.Contains(holder.BindingAdapterPosition);
				}
				else
				{
					holder.IsSelected = false;
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
			_selectedPosition = selectedPositions.Length > 0 ? selectedPositions[0] : null;
			_selectedPositions = new HashSet<int>(selectedPositions);

			return ItemsView.SelectionMode switch
			{
				SelectionMode.Single => _selectedPosition == position,
				SelectionMode.Multiple => _selectedPositions.Contains(position),
				_ => false
			};
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
