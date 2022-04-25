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
            _key = new byte[] { 4, 5, 3, 18, 65, 10, 15, 55, 63, 12, 25, 94, 116, 83, 17, 57, 50, 36, 45, 75, 14, 28, 13, 119 };
            _iv = Encoding.ASCII.GetBytes(SystemInfo.deviceUniqueIdentifier.Substring(0, 16));
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
                    return "ca-app-pub-1155937478045283/5108600685";
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
                    return "ca-app-pub-1155937478045283/3798592283";
#endif
                }
                else if (unitType == EUnitAdType.NativeBannerId)
                {
#if DEVELOPMENT || UNITY_EDITOR
                    return "ca-app-pub-3940256099942544/2247696110";
#else
                    return "ca-app-pub-1155937478045283/7758814750";
#endif
                }

            }
            return null;
        }
    }
}
