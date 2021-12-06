namespace Vanta.UI.TableView
{
	/// <summary>
	/// Implement this if you want to add margins to cell.
	/// </summary>
	public interface ITableViewMargin
	{
		/// <summary>
		/// Top or right margin for cell at index in tableview.
		/// </summary>
		/// <returns>Scalar of margin</returns>
		float ScalarForUpperMarginInTableView(TableView tableView, int index);
		/// <summary>
		/// Bottom or left margin for cell at index in tableview.
		/// </summary>
		/// <returns>Scalar of margin</returns>
		float ScalarForLowerMarginInTableView(TableView tableView, int index);
	}
}
