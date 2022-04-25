using GameWarriors.AdDomain.Abstraction;
using GameWarriors.AnalyticDomain.Abstraction;
using GameWarriors.DependencyInjection.Attributes;
using GameWarriors.ResourceDomain.Abstraction;
using GameWarriors.StorageDomain.Abstraction;
using Managements.Handlers.Analytics;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Managements.Core
{
    public class GameConfiguration : IAnalyticConfig, IResourceConfig, IAdvertiseConfig, IStorageConfig
    {
        private readonly IAnalyticHandler[] Analytics_Handlers = {
#if GAME_ANALYTICS
                new GameAnalyticHandler(START_ANALYTIC_EVENT) , 
#endif
#if METRIX
                new MetrixAnalyticHandler(),
#endif
#if FIREBASE
                new FirebaseAnalyticHandler(START_ANALYTIC_EVENT),
#endif
#if APPMETRICA
                new AppMetricaAnalyticHandler(START_ANALYTIC_EVENT),
#endif
#if APPS_FLYER
                new AppsFlyerAnalyticHandler(),
#endif
        };
        private readonly byte[] _iv;
        private readonly byte[] _key;

        public IAnalyticHandler[] AnalyticHandlers => Analytics_Handlers;

        public int ShiftCount => 3;

        public bool IsPreloadBundles => false;

        public bool IsUseDefaultDownloadContent => false;

        public MonoBehaviour CoroutineHandler => Manager;

        public string StorageDataPath => Application.persistentDataPath;

        byte[] IStorageConfig.Key => _key;

        byte[] IStorageConfig.IV => _iv;

        string IStorageConfig.DirectoryPrefix => "0";
        int IStorageConfig.SaveingInterval => 15;

        [Inject]
        private GameManager Manager { get; set; }



        [UnityEngine.Scripting.Preserve]
        public GameConfiguration()
        {            
            _key = Encoding.ASCII.GetBytes("123456789123456789123456");
            _iv = Encoding.ASCII.GetBytes("1234567891234567");
        }


        public GameObject AdUnitNativeBanner(EAdHandlerType type)
        {
            return null;
        }

        public string GetAdUnitId(EAdHandlerType handlerType, EUnitAdType unitType)
        {
            if (handlerType == EAdHandlerType.Admobe)
            {
                if (unitType == EUnitAdType.RewardAdId)
                {
#if DEVELOPMENT || UNITY_EDITOR
                    return "ca-app-pub-3940256099942544/5224354917";
#else
                    return "ca-app-pub-3940256099942544/5224354917";
#endif
                }
                else if (unitType == EUnitAdType.BannerId)
                {
#if DEVELOPMENT || UNITY_EDITOR
                    return "ca-app-pub-3940256099942544/6300978111";
#else
                    return "ca-app-pub-3940256099942544/6300978111";
#endif
                }
                else if (unitType == EUnitAdType.InterstitalId)
                {
#if DEVELOPMENT || UNITY_EDITOR
                    return "ca-app-pub-3940256099942544/1033173712";
#else
                    return "ca-app-pub-3940256099942544/1033173712";
#endif
                }
                else if (unitType == EUnitAdType.NativeBannerId)
                {
#if DEVELOPMENT || UNITY_EDITOR
                    return "ca-app-pub-3940256099942544/2247696110";
#else
                    return "ca-app-pub-3940256099942544/2247696110";
#endif
                }

            }
            return null;
        }
    }
}
