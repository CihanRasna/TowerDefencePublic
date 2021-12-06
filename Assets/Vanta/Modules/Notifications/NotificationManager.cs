using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vanta.Core;
using Vanta.Persist;

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif



namespace Vanta.Notifications
{

    public class NotificationManager : Singleton<NotificationManager>
    {
        
        public delegate void NotificationEvent(Notification notification);
        public delegate void NotificationAuthorizationEvent(bool authorized);
        public event NotificationEvent notificationReceived;
        public event NotificationAuthorizationEvent notificationsAuthorized;

        private Action<bool, bool> authorizationCallback = null;
        
        

    #region Life Cycle

        private void Start()
        {
#if UNITY_IOS
            StartCoroutine(CheckLastRespondedNotification());
#endif
        }

    #endregion



    #region iOS
        
#if UNITY_IOS
        
        public void RequestAuthorization(Action<bool, bool> completion)
        {
            if (authorizationCallback != null)
            {
                Debug.LogWarning("Notification authorization is already requested!");
                return;
            }

            var status = iOSNotificationCenter.GetNotificationSettings().AuthorizationStatus;
            bool granted = status == AuthorizationStatus.Authorized;
            bool requestedBefore = status != AuthorizationStatus.NotDetermined;
            
            if (requestedBefore)
            {
                completion(granted, requestedBefore);
                return;
            }

            authorizationCallback = completion;
            StartCoroutine(_RequestAuthorization());
        }

        private IEnumerator _RequestAuthorization()
        {
            using (var request = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge, false))
            {
                while (!request.IsFinished)
                {
                    yield return null;
                }
                
                authorizationCallback?.Invoke(request.Granted, false);
                authorizationCallback = null;

                notificationsAuthorized?.Invoke(request.Granted);
            }
        }
        
        public void SetupRecurringLocalNotifications(List<string> messages)
        {
            var status = iOSNotificationCenter.GetNotificationSettings().AuthorizationStatus;
            if (status != AuthorizationStatus.Authorized)
            {
                Debug.LogWarning("Tried to setup recurring notifications but notifications not authorized!");
                return;
            }

            if (PersistManager.Instance.recurringNotificationsSet)
            {
                Debug.LogWarning("Recurring notifications are already set!");
                return;
            }

            for (int i = 0; i < 31; i++)
            {
                var calendarTrigger = new iOSNotificationCalendarTrigger()
                {
                    Day = i + 1,
                    Hour = 18,
                    Minute = 0,
                    Repeats = true
                };
                
                var notification = new iOSNotification()
                {
                    Identifier = "recurring_notification_" + i,
                    Title = "Wheel Smash",
                    Body = messages[(i % messages.Count)],
                    ShowInForeground = false,
                    Trigger = calendarTrigger
                };
                
                iOSNotificationCenter.ScheduleNotification(notification);
            }

            PersistManager.Instance.recurringNotificationsSet = true;
        }
        
        private IEnumerator CheckLastRespondedNotification()
        {
            // Wait one frame before checking the app-open notifications to give enough time to
            // other managers which will subscribe to the receive event on Start() event.
            yield return null;
            
            HandleReceivedNotification(iOSNotificationCenter.GetLastRespondedNotification());
            iOSNotificationCenter.OnNotificationReceived += HandleReceivedNotification;
        }

        private void HandleReceivedNotification(iOSNotification notification)
        {
            if (notification != null)
            {
                notificationReceived?.Invoke(new Notification(notification));
            }
        }
        
#endif

    #endregion
        
    }

}