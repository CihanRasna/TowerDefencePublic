namespace Vanta.Notifications
{

    public class Notification
    {

        private string _title = "";
        public string title => _title;

        private string _body = "";
        public string body => _body;

        private string _id = "";
        public string id => _id;

        private string _data = "";
        public string data => _data;


    #region Life Cycle

        public Notification()
        {
            
        }
        
#if UNITY_IOS
        
        public Notification(Unity.Notifications.iOS.iOSNotification notification)
        {
            _title = notification.Title;
            _body = notification.Body;
            _id = notification.Identifier;
            _data = notification.Data;
        }
        
#endif

    #endregion

    }

}