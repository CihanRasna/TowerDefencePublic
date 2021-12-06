//#define ENABLE_ADS

using Vanta.Core;

#if ENABLE_ADS
using System;
using UnityEngine;
using ElephantSDK;
using RollicGames.Advertisements;
#endif



namespace Vanta.Ads
{

    public class AdManager : Singleton<AdManager>
    {
        
#if ENABLE_ADS
        
#if UNITY_IOS
        private readonly string[] _bannerAdUnits = { "866ae404d7e941bb9704b8ce537364f7" };
        private readonly string[] _interstitialAdUnits = { "6d5dd8dba30a45b397409c1cb50f8c71" };
        private readonly string[] _rewardedVideoAdUnits = { "d17129ff4b6448aeac252bff2016f152" };
        private readonly string[] _rewardedRichMediaAdUnits = { };
#elif UNITY_ANDROID || UNITY_EDITOR
        private readonly string[] _bannerAdUnits = { "b195f8dd8ded45fe847ad89ed1d016da" };
        private readonly string[] _interstitialAdUnits = { "24534e1901884e398f1253216226017e" };
        private readonly string[] _rewardedVideoAdUnits = { "920b6145fb1546cf8b5cf2ac34638bb7" };
        private readonly string[] _rewardedRichMediaAdUnits = { "a96ae2ef41d44822af45c6328c4e1eb1" };
#endif
        
        
        
        private bool _didInitialize = false;
        [HideInInspector] public bool didInitialize => _didInitialize;
    
        private int adDisplayPeriod = 30;
        private float lastAdDisplayTime = 0;
        private Action<bool> adCompletionCallback = null;



    #region Life Cycle

        private void Start()
        {
            RLAdvertisementManager.OnRollicAdsSdkInitializedEvent += OnAdSdkInitialized;
            RLAdvertisementManager.OnRollicAdsInterstitialDismissedEvent += OnInterstitialDismissed;
            RLAdvertisementManager.OnRollicAdsRewardedVideoFailedToPlayEvent += OnRewardedVideoFailedToPlay;
            RLAdvertisementManager.OnRollicAdsRewardedVideoClosedEvent += OnRewardedVideoClosed;
            RLAdvertisementManager.OnRollicAdsAdLoadedEvent += OnBannerLoaded;
            
            RLAdvertisementManager.Instance.init(_rewardedVideoAdUnits, _bannerAdUnits, _interstitialAdUnits);
        }
        
        private void OnAdSdkInitialized(string arg1)
        {
            RLAdvertisementManager.Instance.rewardedAdResultCallback = result =>
            {
                if (result == RLRewardedAdResult.Finished)
                {
                    if (adCompletionCallback != null)
                    {
                        adCompletionCallback(true);
                        adCompletionCallback = null;
                    }
                }
                else
                {
                    if (adCompletionCallback != null)
                    {
                        adCompletionCallback(false);
                        adCompletionCallback = null;
                    }
                }
            };
            
            LoadBannerAd();
            
            adDisplayPeriod = RemoteConfig.GetInstance().GetInt("v2_ad_display_period", 30);
            
            _didInitialize = true;
        }

    #endregion



    #region Banner Ads

        private void LoadBannerAd() {
            RLAdvertisementManager.Instance.loadBanner();
        }
        
        private void OnBannerLoaded(string adId, float height)
        {

        }

    #endregion

        
    
    #region Interstitial Ads
        
        public void DisplayInterstitialAd(Action<bool> action)
        {
            if (!_didInitialize)
            {
                action(false);
                return;
            }
            
            if (adCompletionCallback != null)
            {
                adCompletionCallback(false);
                adCompletionCallback = null;
            }

            if (RLAdvertisementManager.Instance.isInterstitialReady() && Time.time - lastAdDisplayTime > adDisplayPeriod)
            {
                adCompletionCallback = action;
                RLAdvertisementManager.Instance.showInterstitial();
            }
            else
            {
                action(false);
            }
        }

        private void OnInterstitialDismissed(string arg1)
        {
            lastAdDisplayTime = Time.time;
            if (adCompletionCallback != null)
            {
                adCompletionCallback(true);
                adCompletionCallback = null;
            }
        }

    #endregion
        
        
        
    #region Rewarded Ads
        
        public void DisplayRewardedAd(Action<bool> action)
        {
            if (!_didInitialize)
            {
                action(false);
                return;
            }
            
            if (adCompletionCallback != null)
            {
                adCompletionCallback(false);
                adCompletionCallback = null;
            }
            
            if (RLAdvertisementManager.Instance.isRewardedVideoAvailable())
            {
                adCompletionCallback = action;
                RLAdvertisementManager.Instance.showRewardedVideo();
            }
            else
            {
                action(false);
            }
        }
        
        private void OnRewardedVideoFailedToPlay(string arg1, string arg2)
        {
            if (adCompletionCallback != null)
            {
                adCompletionCallback(false);
            }
        }

        private void OnRewardedVideoClosed(string arg1)
        {
            lastAdDisplayTime = Time.time;
        }

    #endregion
        
#endif
        
    }

}

