using GameWarriors.AnalyticDomain.Abstraction;

using GameWarriors.EventDomain.Abstraction;
using GameWarriors.ResourceDomain.Abstraction;
using Services.Abstraction;
using System;
using GameWarriors.TaskDomain.Abstraction;
using UnityEngine;
using GameWarriors.AnalyticDomain.Extension;
using Common.ResourceKey;

#if APPS_FLYER
using AppsFlyerSDK;
#endif

namespace Services.Core.App
{

#if APPS_FLYER
    public class AppService : IAppService , IAppsFlyerConversionData
#else
    public class AppService : IAppService
#endif
    {
        private readonly IAnalytic _analytic;
        private float _timer;
        private int _playedMinuets;

        [UnityEngine.Scripting.Preserve]
        public AppService(IEvent eventController, IUpdateTask updateTask, IAnalytic analytic)
        {
            _analytic = analytic;
            updateTask.RegisterFixedUpdateTask(TimerUpdate);
        }

        public void Initialization(IServiceProvider serviceProvider)
        {
#if APPS_FLYER
            IVariableDatabase variableDatabase = serviceProvider.GetService(typeof(IVariableDatabase)) as IVariableDatabase;
            string appsFlyerDevKey = variableDatabase.GetVariable<string>(VariableKey.AppsFlyerDevKey);
            string appsFlyerAppId = string.Empty;
#if UNITY_iOS
            appsFlyerAppId = variableDatabase.GetVariable<string>(VariableKey.AppsFlyerAppIdKey);
#endif
            if (!string.IsNullOrEmpty(appsFlyerDevKey))
            {
                AppsFlyer.initSDK(appsFlyerDevKey, appsFlyerAppId);
                AppsFlyer.startSDK();
            }
#endif

            //FB.Init(this.OnInitComplete, this.OnHideUnity);
        }

        public void onAppOpenAttribution(string attributionData)
        {
            // AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
            // System.Collections.Generic.Dictionary<string, object> attributionDataDictionary = AppsFlyer.CallbackStringToDictionary(attributionData);
            // add direct deeplink logic here
        }

#if APPS_FLYER
        public void onAppOpenAttributionFailure(string error)
        {
            AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
        }

        public void onConversionDataFail(string error)
        {
            AppsFlyer.AFLog("onConversionDataFail", error);
        }

        public void onConversionDataSuccess(string conversionData)
        {
            AppsFlyer.AFLog("onConversionDataSuccess", conversionData);
            System.Collections.Generic.Dictionary<string, object> conversionDataDictionary = AppsFlyer.CallbackStringToDictionary(conversionData);
            // add deferred deeplink logic here
        }
#endif
        private void OnInitComplete()
        {
            //string logMessage = string.Format(
            //    "OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'",
            //    FB.IsLoggedIn,
            //    FB.IsInitialized);
            //Debug.Log(logMessage);
            //if (AccessToken.CurrentAccessToken != null)
            //{
            //    // Debug.Log(AccessToken.CurrentAccessToken.ToString());
            //}
        }

        private void OnHideUnity(bool isGameShown)
        {
            //Debug.Log("Is game shown: " + isGameShown);
        }

        private void TimerUpdate()
        {
            _timer += Time.deltaTime;
            if (_timer >= 60)
            {
                _timer = 0;
                ++_playedMinuets;
                _analytic.CustomEvent("timer", "minuets", _playedMinuets, EAnalyticType.AppsFlyer);
            }
        }
    }

}