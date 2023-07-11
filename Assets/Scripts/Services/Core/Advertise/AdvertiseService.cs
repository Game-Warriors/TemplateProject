using GameWarriors.AdDomain.Abstraction;
using Common.Extensions;
using GameWarriors.EventDomain.Abstraction;
using Services.Abstraction;
using System;
using GameWarriors.StorageDomain.Abstraction;
using GameWarriors.AnalyticDomain.Abstraction;
using GameWarriors.AnalyticDomain.Extension;
using Common.ResourceKey;
using GameWarriors.TaskDomain.Abstraction;
using UnityEngine;
using System.Collections;

namespace Services.Core
{
    public class AdvertiseService : IAdvertiseService, IInterstitialAdPlace
    {
        private const string NO_ADS_KEY = "No_Ads";
        private const int REWARDED_AD_PLACES_LENGTH = 3;
        private const int NO_AD_USERS_COOLDOWN_TIME = 60;

        private readonly IEvent _event;
        private readonly IAdvertise _advertise;
        private readonly IAnalytic _analytic;
        private readonly IStorage _storage;
        private readonly ITimeService _timeService;

        private RewardedAdPlace[] _rewardedAdPlaces;
        private Action<bool> _onDone;
        private bool _hasReward;
        private bool _isShowDone;
        private string _lastRewardedPlacment;
        private string _lastInterstitialPlacement;

        public bool IsAdAvailable => IsNoAds ? true : _advertise.IsAnyVideoAdExist;
        public bool IsInterstitialAvailable => IsNoAds ? false : _advertise.IsAnyInterstitialExist;

        public string Id => throw new NotImplementedException();

        IInterstitialAdPlace IAdvertiseService.InterstitialAdPlace => this;
        IRewardedAdPlace IAdvertiseService.RewardedAdPlace => GetAvailableRewardedAdPlace();

        private IRewardedAdPlace GetAvailableRewardedAdPlace()
        {
            for (int i = 0; i < _rewardedAdPlaces.Length; i++)
            {
                if (_rewardedAdPlaces[i].IsAdAvailable)
                {
                    return _rewardedAdPlaces[i];
                }
            }
            return _rewardedAdPlaces[0];
        }

        public bool IsNoAds => _storage.GetValue<bool>(NO_ADS_KEY, false);

        [UnityEngine.Scripting.Preserve]
        public AdvertiseService(IEvent eventSystem, IAdvertise advertise, IStorage storage, IAnalytic analytic, IUpdateTask updateTask, ITimerTask timerTask, ITimeService timeService)
        {
            _analytic = analytic;
            _storage = storage;
            _event = eventSystem;
            _advertise = advertise;

            _timeService = timeService;
            updateTask.RegisterUpdateTask(ServiceUpdate);
            _rewardedAdPlaces = new RewardedAdPlace[REWARDED_AD_PLACES_LENGTH];
            for (int i = 0; i < REWARDED_AD_PLACES_LENGTH; i++)
            {
                _rewardedAdPlaces[i] = new RewardedAdPlace(this, "place" + i);
            }
        }

        [UnityEngine.Scripting.Preserve]
        public void Initialization()
        {
            _event.ListenToEvent<bool>(EEventType.OnApplicationStateChange, ApplicationStateChange);
        }

        public void RequestAd()
        {
            if (IsNoAds)
            {
                return;
            }
            for (int i = 0; i < REWARDED_AD_PLACES_LENGTH; i++)
            {
                if (_rewardedAdPlaces[i].IsAdAvailable)
                {
                    break;
                }
                _advertise.LoadVideoAd(_rewardedAdPlaces[i]);
            }            
        }

        public void SetNoAds()
        {
            _storage.SetValue<bool>(NO_ADS_KEY, true);
        }

        public void LoadInterstitialAd()
        {
            if (IsNoAds)
            {
                return;
            }

            _advertise.LoadInterstitialAd(this);
        }


        void IAdvertiseService.ShowAd(Action<bool> onDone, string placement)
        {
            _lastRewardedPlacment = placement;
            //dont comment this
            if (IsNoAds)
            {
                onDone?.Invoke(true);
                _timeService.UpdateTime(TimeKeys.AD_COOLDOWN, new TimeSpan(0, 0, NO_AD_USERS_COOLDOWN_TIME));
                return;
            }

            _onDone = onDone;
            for (int i = 0; i < REWARDED_AD_PLACES_LENGTH; i++)
            {
                if (_rewardedAdPlaces[i].IsAdAvailable)
                {
                    _advertise.ShowVideoAd(_rewardedAdPlaces[i]);
                    break;
                }
            }
        }

        void IAdvertiseService.ShowInterstitialAd(string placement)
        {
            _lastInterstitialPlacement = placement;
            //if (_remoteService.GetIntValue(RemoteConfigKey.IsShowInterstitialAdEnable) < 1)
            //{
            //    return;
            //}

            if (IsNoAds)
            {
                return;
            }

            _advertise.ShowInterstitialAd(this);
        }

        public void OnVideoLoaded()
        {
            _event.BroadcastEvent(EEventType.OnAdAvailable);
            _analytic.LoadAdSucessEvent(AnalyticKeys.AD_REWARDED);
        }

        public void OnVideoLoadFailed(EAdState adState, int code, string message)
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
                RequestAd();

            _analytic.LoadAdFailEvent(AnalyticKeys.AD_REWARDED, code, message);
        }

        public void OnVideoReward(bool hasReward)
        {
            _isShowDone = true;
            _hasReward = hasReward;
        }

        public void OnVideoOpen(string madiationName, string response)
        {
            _analytic.ShowAdSuccessEvent(AnalyticKeys.AD_REWARDED, _lastRewardedPlacment, madiationName, response);
        }

        public void OnVideoShowFailed(EAdState state, int statusCode, string message)
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
                RequestAd();

            _analytic.ShowAdFailEvent(AnalyticKeys.AD_REWARDED, _lastRewardedPlacment, statusCode, message);
        }

        public void OnVideoPaidData(string madiationName, string correncyCode, long amount, string precision)
        {
            _analytic.AdRevenueEvent(AnalyticKeys.AD_REWARDED, madiationName, correncyCode, amount, precision);
        }

        void IInterstitialAdPlace.OnInterstitialLoaded()
        {
            _analytic.LoadAdSucessEvent(AnalyticKeys.AD_INTERSTITIAL);
        }

        void IInterstitialAdPlace.OnInterstitialOnAdLoadFailed(int statusCode, string message)
        {
            _analytic.LoadAdFailEvent(AnalyticKeys.AD_INTERSTITIAL, statusCode, message);
        }

        void IInterstitialAdPlace.OnInterstitialOpen(string madiationName, string response)
        {
            _analytic.ShowAdSuccessEvent(AnalyticKeys.AD_INTERSTITIAL, _lastInterstitialPlacement, madiationName, response);
        }

        void IInterstitialAdPlace.OnInterstitialShowFailed(EAdState state, int statusCode, string message)
        {

            _analytic.ShowAdFailEvent(AnalyticKeys.AD_INTERSTITIAL, _lastInterstitialPlacement, statusCode, message);
        }

        void IInterstitialAdPlace.OnInterstitialPaidData(string madiationName, string correncyCode, long amount, string precision)
        {
            _analytic.AdRevenueEvent(AnalyticKeys.AD_INTERSTITIAL, madiationName, correncyCode, amount, precision);
        }

        void IInterstitialAdPlace.OnInterstitialAdClosed()
        {
            //throw new NotImplementedException();
        }

        private void OnPurchaseSuccessful(string sku)
        {
            if (sku == VendorKey.NoAdsKey)
                SetNoAds();
        }

        private void ApplicationStateChange(bool state)
        {
            if (state)
            {
                for (int i = 0; i < REWARDED_AD_PLACES_LENGTH; i++)
                {
                    if (_rewardedAdPlaces[i].IsAdAvailable)
                    {
                        break;
                    }
                    _advertise.LoadVideoAd(_rewardedAdPlaces[i]);
                }
            }
        }

        private void ServiceUpdate()
        {
            if (_isShowDone)
            {
                _onDone?.Invoke(_hasReward);
                _isShowDone = false;
                _hasReward = false;

                if (Application.internetReachability != NetworkReachability.NotReachable)
                    RequestAd();
            }
        }
    }
}