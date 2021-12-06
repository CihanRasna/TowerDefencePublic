using GameAnalyticsSDK;
using ElephantSDK;
using Vanta.Core;
using Vanta.Levels;
using Vanta.Notifications;
using Vanta.Persist;



namespace Vanta.Analytics
{
    using Notification = Notifications.Notification;

    public class AnalyticsManager : Singleton<AnalyticsManager>
    {

    #region Life Cycle

        private void Start()
        {
            LevelManager.Instance.levelDidLoad += LevelDidLoad;
            LevelManager.Instance.levelDidSuccees += LevelDidSuccees;
            LevelManager.Instance.levelDidFail += LevelDidFail;
            NotificationManager.Instance.notificationsAuthorized += NotificationsAuthorized;
            NotificationManager.Instance.notificationReceived += NotificationReceived;
        }

    #endregion



    #region Level Events

        private void LevelDidLoad(BaseLevel baseLevel)
        {
            var levelIdx = PersistManager.Instance.displayingLevelIdx;
            Elephant.LevelStarted(levelIdx);
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, levelIdx.ToString());
        }

        private void LevelDidSuccees(BaseLevel baseLevel)
        {
            var levelIdx = PersistManager.Instance.displayingLevelIdx;
            Elephant.LevelCompleted(levelIdx);
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, levelIdx.ToString());
        }

        private void LevelDidFail(BaseLevel baseLevel)
        {
            var levelIdx = PersistManager.Instance.displayingLevelIdx;
            Elephant.LevelFailed(levelIdx);
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, levelIdx.ToString());
        }

    #endregion



    #region Notification Events

        private void NotificationsAuthorized(bool authorized)
        {
            var parameters = Params.New();
            parameters.Set("status", authorized ? "authorized" : "denied");
            Elephant.Event("notification_authorization", PersistManager.Instance.displayingLevelIdx, parameters);
        }

        private void NotificationReceived(Notification notification)
        {
            var parameters = Params.New();
            parameters.Set("message", notification.body);
            Elephant.Event("notification_received", PersistManager.Instance.displayingLevelIdx, parameters);
        }

    #endregion

    }

}