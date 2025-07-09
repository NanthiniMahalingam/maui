using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Primitives;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Microsoft.Maui.UnitTests.Layouts.LayoutTestHelpers;

namespace Microsoft.Maui.UnitTests.Layouts
{
    [Category(TestCategory.Core, TestCategory.Layout)]
    public class GridInVerticalStackLayoutTests
    {
        [Fact]
        public void GridWithStarColumnsInCenteredVerticalStackLayout()
        {
            // This test reproduces the issue: Grid with star columns inside VerticalStackLayout with HorizontalOptions=Center
            
            // Create a Grid with star columns
            var grid = CreateGridLayout(columns: "*,*");
            
            var view0 = CreateTestView(new Size(20, 20));
            var view1 = CreateTestView(new Size(30, 20));
            
            SubstituteChildren(grid, view0, view1);
            SetLocation(grid, view0, col: 0);
            SetLocation(grid, view1, col: 1);
            
            // Create a VerticalStackLayout with HorizontalOptions=Center
            var stack = CreateVerticalStackLayout();
            stack.HorizontalLayoutAlignment.Returns(LayoutAlignment.Center);
            
            // Add the grid to the stack
            SubstituteChildren(stack, grid);
            
            // Simulate the parent container giving the stack a width constraint
            var stackManager = new VerticalStackLayoutManager(stack);
            var stackSize = stackManager.Measure(200, double.PositiveInfinity);
            
            // Now arrange the stack 
            stackManager.ArrangeChildren(new Rect(0, 0, stackSize.Width, stackSize.Height));
            
            // Check what arrangement the grid received
            var gridArrangeCalls = grid.ReceivedCalls().Where(c => c.GetMethodInfo().Name == "Arrange").ToList();
            
            if (gridArrangeCalls.Any())
            {
                var gridBounds = (Rect)gridArrangeCalls.Last().GetArguments()[0];
                
                // Now measure and arrange the grid with the bounds it received
                var gridManager = new GridLayoutManager(grid);
                var gridMeasuredSize = gridManager.Measure(gridBounds.Width, gridBounds.Height);
                gridManager.ArrangeChildren(gridBounds);
                
                // For star columns, when the grid is in a centered container, 
                // we expect the columns to size to their content:
                // - view0 should be 20 pixels wide
                // - view1 should be 30 pixels wide  
                // - They should be positioned at (0, 0) and (20, 0)
                
                // Get the arrangement calls for the views
                var view0ArrangeCalls = view0.ReceivedCalls().Where(c => c.GetMethodInfo().Name == "Arrange").ToList();
                var view1ArrangeCalls = view1.ReceivedCalls().Where(c => c.GetMethodInfo().Name == "Arrange").ToList();
                
                if (view0ArrangeCalls.Any() && view1ArrangeCalls.Any())
                {
                    var view0Bounds = (Rect)view0ArrangeCalls.Last().GetArguments()[0];
                    var view1Bounds = (Rect)view1ArrangeCalls.Last().GetArguments()[0];
                    
                    // Expected behavior: star columns should size to content width
                    // view0: 20 pixels wide, positioned at (0, 0)
                    // view1: 30 pixels wide, positioned at (20, 0) 
                    
                    // Check if columns are sized to content (not equally divided)
                    Assert.Equal(20, view0Bounds.Width);
                    Assert.Equal(30, view1Bounds.Width);
                    
                    // Check if columns are positioned correctly (not overlapping)
                    Assert.Equal(0, view0Bounds.X);
                    Assert.Equal(20, view1Bounds.X);
                    
                    // Check if columns are not hidden (height should be > 0)
                    Assert.True(view0Bounds.Height > 0);
                    Assert.True(view1Bounds.Height > 0);
                }
            }
        }
        
        [Fact]
        public void GridWithStarColumnsDirectlyCentered()
        {
            // This test verifies the existing behavior: Grid with star columns and HorizontalLayoutAlignment=Center
            
            // Create a Grid with star columns
            var grid = CreateGridLayout(columns: "*,*");
            grid.HorizontalLayoutAlignment.Returns(LayoutAlignment.Center);
            
            var view0 = CreateTestView(new Size(20, 20));
            var view1 = CreateTestView(new Size(30, 20));
            
            SubstituteChildren(grid, view0, view1);
            SetLocation(grid, view0, col: 0);
            SetLocation(grid, view1, col: 1);
            
            // Measure and arrange the grid directly
            var gridManager = new GridLayoutManager(grid);
            var gridSize = gridManager.Measure(200, 200);
            gridManager.ArrangeChildren(new Rect(0, 0, gridSize.Width, gridSize.Height));
            
            // Expected behavior: star columns should size to content width
            AssertArranged(view0, new Rect(0, 0, 20, 20));
            AssertArranged(view1, new Rect(20, 0, 30, 20)); 
        }
        
        private IGridLayout CreateGridLayout(string columns = null, string rows = null)
        {
            var grid = Substitute.For<IGridLayout>();
            
            grid.Height.Returns(Dimension.Unset);
            grid.Width.Returns(Dimension.Unset);
            grid.MinimumHeight.Returns(Dimension.Minimum);
            grid.MinimumWidth.Returns(Dimension.Minimum);
            grid.MaximumHeight.Returns(Dimension.Maximum);
            grid.MaximumWidth.Returns(Dimension.Maximum);
            
            grid.RowSpacing.Returns(0);
            grid.ColumnSpacing.Returns(0);
            
            // Default alignment is Fill
            grid.HorizontalLayoutAlignment.Returns(LayoutAlignment.Fill);
            grid.VerticalLayoutAlignment.Returns(LayoutAlignment.Fill);
            
            if (columns != null)
            {
                var colDefs = CreateTestColumns(columns.Split(","));
                grid.ColumnDefinitions.Returns(colDefs);
            }
            else
            {
                grid.ColumnDefinitions.Returns(new List<IGridColumnDefinition>());
            }
            
            if (rows != null)
            {
                var rowDefs = CreateTestRows(rows.Split(","));
                grid.RowDefinitions.Returns(rowDefs);
            }
            else
            {
                grid.RowDefinitions.Returns(new List<IGridRowDefinition>());
            }
            
            return grid;
        }
        
        private IStackLayout CreateVerticalStackLayout()
        {
            var stack = Substitute.For<IStackLayout>();
            stack.Height.Returns(Dimension.Unset);
            stack.Width.Returns(Dimension.Unset);
            stack.MinimumHeight.Returns(Dimension.Minimum);
            stack.MinimumWidth.Returns(Dimension.Minimum);
            stack.MaximumHeight.Returns(Dimension.Maximum);
            stack.MaximumWidth.Returns(Dimension.Maximum);
            stack.Spacing.Returns(0);
            stack.Padding.Returns(Thickness.Zero);
            
            // Default alignment is Fill
            stack.HorizontalLayoutAlignment.Returns(LayoutAlignment.Fill);
            stack.VerticalLayoutAlignment.Returns(LayoutAlignment.Fill);
            
            return stack;
        }
        
        private void SetLocation(IGridLayout grid, IView view, int row = 0, int col = 0, int rowSpan = 1, int colSpan = 1)
        {
            grid.GetRow(view).Returns(row);
            grid.GetColumn(view).Returns(col);
            grid.GetRowSpan(view).Returns(rowSpan);
            grid.GetColumnSpan(view).Returns(colSpan);
        }
        
        private IEnumerable<IGridColumnDefinition> CreateTestColumns(string[] columns)
        {
            var colDefs = new List<IGridColumnDefinition>();
            
            foreach (var column in columns)
            {
                var colDef = Substitute.For<IGridColumnDefinition>();
                var columnStr = column.Trim();
                
                if (columnStr == "*")
                {
                    colDef.Width.Returns(GridLength.Star);
                }
                else if (columnStr.EndsWith("*"))
                {
                    var value = double.Parse(columnStr.Substring(0, columnStr.Length - 1));
                    colDef.Width.Returns(new GridLength(value, GridUnitType.Star));
                }
                else if (columnStr.ToLower() == "auto")
                {
                    colDef.Width.Returns(GridLength.Auto);
                }
                else
                {
                    var value = double.Parse(columnStr);
                    colDef.Width.Returns(new GridLength(value, GridUnitType.Absolute));
                }
                
                colDefs.Add(colDef);
            }
            
            return colDefs;
        }
        
        private IEnumerable<IGridRowDefinition> CreateTestRows(string[] rows)
        {
            var rowDefs = new List<IGridRowDefinition>();
            
            foreach (var row in rows)
            {
                var rowDef = Substitute.For<IGridRowDefinition>();
                var rowStr = row.Trim();
                
                if (rowStr == "*")
                {
                    rowDef.Height.Returns(GridLength.Star);
                }
                else if (rowStr.EndsWith("*"))
                {
                    var value = double.Parse(rowStr.Substring(0, rowStr.Length - 1));
                    rowDef.Height.Returns(new GridLength(value, GridUnitType.Star));
                }
                else if (rowStr.ToLower() == "auto")
                {
                    rowDef.Height.Returns(GridLength.Auto);
                }
                else
                {
                    var value = double.Parse(rowStr);
                    rowDef.Height.Returns(new GridLength(value, GridUnitType.Absolute));
                }
                
                rowDefs.Add(rowDef);
            }
            
            return rowDefs;
        }
    }
}