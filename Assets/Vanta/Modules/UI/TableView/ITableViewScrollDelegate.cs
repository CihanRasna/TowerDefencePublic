namespace Vanta.UI.TableView
{

    public interface ITableViewScrollDelegate
    {
        void TableView_DidBeginDragging(TableView tableView);
        void TableView_DidDrag(TableView tableView);
        void TableView_DidEndDragging(TableView tableView);
    }
    
}