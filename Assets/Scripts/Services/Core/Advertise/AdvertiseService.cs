using GameWarriors.AdDomain.Abstraction;
using Common.Extensions;
using GameWarriors.EventDomain.Abstraction;
using Services.Abstraction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Core
{
    public class AdvertiseService : IAdvertiseService
    {
        private readonly IEvent _event;
        private readonly IAdvertise _advertise;
        private Action<bool> _onDone;
        private bool _hasReward;

        public bool IsAdAvailable => _advertise.IsVideoAdExist;
        public bool IsNoAds { get; private set; }

        public AdvertiseService(IEvent eventSystem, IAdvertise advertise)
        {
            eventSystem.ListenToInitializeEvent(Initialization);
            _event = eventSystem;
            _advertise = advertise;
            _advertise.OnVideoAvailable += VideoAvailable;
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
#if DEVELOPMENT
            FakeAdvertisePanel screen = _screenHandler.ShowScreen<FakeAdvertisePanel>(FakeAdvertisePanel.SCREEN_NAME, ECanvasType.ScreenCanvas, EPreviosScreenAct.Stay);
            screen.SetData(onDone);
#elif DEVELOPMENT_RELEASE //&& !UNITY_EDITOR
            UnityEngine.Debug.Log("ShowAd");
            IsAdvertiseSeenLastInSeconds = true;
            _advertise.ShowVideoAd(ShowVideoDone, ShowVideoFailed);
#else
            //IsAdvertiseSeenLastInSeconds = true;
            _advertise.ShowVideoAd(ShowVideoDone, ShowVideoFailed);
#endif
        }

        private void Initialization(IServiceProvider serviceProvider)
        {

            _event.ListenToEvent<bool>(EEventType.OnApplicationStateChange, ApplicationStateChange);

            //GoogleMobileAds.Api.Mediation.AppLovin.AppLovin.Initialize();
            //GoogleMobileAds.Api.Mediation.AppLovin.AppLovin.SetHasUserConsent(true);
            //GoogleMobileAds.Api.Mediation.UnityAds.UnityAds.SetGDPRConsentMetaData(true);
            //GoogleMobileAds.Api.Mediation.IronSource.IronSource.SetConsent(true);
            //GoogleMobileAds.Api.Mediation.Chartboost.Chartboost.AddDataUseConsent(GoogleMobileAds.Api.Mediation.Chartboost.CBGDPRDataUseConsent.Behavioral);

        }

        private void ShowVideoDone(bool isComplete, bool hasReward)
        {
            UnityEngine.Debug.Log("ShowVideoDone");
            _onDone?.Invoke(true);
            _hasReward = true;
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
    }
}