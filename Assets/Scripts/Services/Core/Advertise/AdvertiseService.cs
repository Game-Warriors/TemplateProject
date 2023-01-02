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

namespace Services.Core
{
    public class AdvertiseService : IAdvertiseService
    {
        private const string NO_ADS_KEY = "No_Ads";

        private readonly IEvent _event;
        private readonly IAdvertise _advertise;
        private readonly IAdInterstitialHandler _adInterstitialHandler;

        private Action<bool> _onDone;
        private bool _hasReward;
        private bool _isShowDone;

        private readonly IAnalytic _analytic;
        private readonly IStorage _storage;

        public bool IsAdAvailable => _advertise.IsVideoAdExist;
        public bool IsNoAds => _storage.GetValue<bool>(NO_ADS_KEY, false);

        [UnityEngine.Scripting.Preserve]
        public AdvertiseService(IEvent eventSystem, IAdvertise advertise, IStorage storage, IAnalytic analytic, IAdInterstitialHandler adInterstitialHandler, IUpdateTask updateTask)
        {
            _analytic = analytic;
            _storage = storage;
            _event = eventSystem;
            _advertise = advertise;
            updateTask.RegisterUpdateTask(ServiceUpdate);
            _adInterstitialHandler = adInterstitialHandler;
        }


        [UnityEngine.Scripting.Preserve]
        public void Initialization()
        {
            _event.ListenToEvent<bool>(EEventType.OnApplicationStateChange, ApplicationStateChange);
        }

        private void OnAdmobVideoInited()
        {
        }

        public void RequestAd()
        {
            _advertise.LoadVideoAd();
        }

        public void ShowAd(Action<bool> onDone)
        {
            //dont comment this
            if (IsNoAds)
            {
                onDone?.Invoke(true);
                return;
            }
            _onDone = onDone;

            _advertise.ShowVideoAd(ShowVideoDone, null);
        }


        private void ShowVideoDone(bool isComplete, bool hasReward)
        {
            _isShowDone = true;
            _hasReward = hasReward;
        }

        private void ShowVideoFailed()
        {
            UnityEngine.Debug.Log("ShowVideoFailed");
            //IsAdvertiseSeenLastInSeconds = false;
        }

        private void VideoAvailable()
        {
            _event.BroadcastEvent(EEventType.OnAdAvailable);
        }

        private void ApplicationStateChange(bool state)
        {
            if (state)
                _advertise.LoadVideoAd();
        }

        public void SetNoAds()
        {
            _storage.SetValue<bool>(NO_ADS_KEY, true);
        }

        public void LoadInterstitialAd()
        {
            _adInterstitialHandler.LoadInterstitialAd();
        }

        public void ShowInterstitialAd()
        {
            _adInterstitialHandler.ShowInterstitialAd();
        }

        private void ServiceUpdate()
        {
            if (_isShowDone)
            {
                //_analytic.CustomEvent(AnalyticKeys.RV_FINISH);
                _onDone?.Invoke(_hasReward);
                _isShowDone = false;
                _hasReward = false;
            }
        }
    }
}