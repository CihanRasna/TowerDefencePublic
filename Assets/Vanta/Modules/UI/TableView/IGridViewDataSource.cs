namespace Vanta.UI.TableView
{
	public interface IGridViewDataSource : ITableViewDataSource
	{
		/// <summary>
		/// Number of cells at row or column in grid (table view).
		/// The return value should be greater than 0.
		/// </summary>
		int NumberOfCellsAtRowOrColumnInGrid(TableView grid);

		/// <summary>
		/// Alignment of cells at last row or column in grid view (table view).
		/// </summary>
		GridViewAlignment AlignmentOfCellsAtRowOrColumnInGrid(TableView grid);
	}
}
