namespace Vanta.UI.TableView
{
	public interface ITableViewDataSource
	{
		/// <summary>
		/// Will be called if cell at index in table view is appearing to scroll view's viewport,
		/// before calling IUITableViewDelegate.CellAtIndexInTableViewWillAppear(UITableView, int).
		/// Use UITableView.ReuseOrCreateCell(T, UITableViewCellLifeCycle, bool) to obtain a cell.
		/// </summary>
		/// <param name="tableView"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		TableViewCell CellAtIndexInTableView(TableView tableView, int index);
		/// <summary>
		/// Will be called once after Reload(), Reload(int), RearrangeData(), AppendData() or PrependData() is called, for getting number of cells in table view.
		/// </summary>
		/// <param name="tableView"></param>
		/// <returns></returns>
		int NumberOfCellsInTableView(TableView tableView);
		/// <summary>
		/// Will be called once after Reload(), Reload(int), RearrangeData(), AppendData() or PrependData() is called, for getting height or width of cells in table view.
		/// </summary>
		/// <param name="tableView"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		float ScalarForCellInTableView(TableView tableView, int index);
	}
}
