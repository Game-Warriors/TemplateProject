using Common.Extensions;
using Common.ResourceKey;
using GameWarriors.AdDomain.Abstraction;
using GameWarriors.AdDomain.Core;
using GameWarriors.AnalyticDomain.Abstraction;
using GameWarriors.AnalyticDomain.Core;
using GameWarriors.AudioDomain.Abstraction;
using GameWarriors.AudioDomain.Core;
using GameWarriors.DependencyInjection.Attributes;
using GameWarriors.DependencyInjection.Core;
using GameWarriors.EventDomain.Abstraction;
using GameWarriors.EventDomain.Core;
using GameWarriors.PoolDomain.Abstraction;
using GameWarriors.PoolDomain.Core;
using GameWarriors.ResourceDomain.Abstraction;
using GameWarriors.ResourceDomain.Core;
using GameWarriors.StorageDomain.Abstraction;
using GameWarriors.StorageDomain.Core;
using GameWarriors.TaskDomain.Abstraction;
using GameWarriors.TaskDomain.Core;
using GameWarriors.TutorialDomain.Abstraction;
using GameWarriors.TutorialDomain.Core;
using GameWarriors.UIDomain.Abstraction;
using GameWarriors.UIDomain.Core;
using Management.Factory;
using Management.Handlers.Json;
using Managements.Handlers.Storage;
using Services.Abstraction;
using Services.Core;
using Services.Core.Analytic;
using Services.Core.App;
using Services.Core.Level;
using Services.Core.Tutorial;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Managements.Core
{
    public class GameManager : MonoBehaviour
    {
        private Task _buildTask;

        [Inject] private ILogService LogService { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private IEvent Event { get; set; }
        [Inject] private IUpdateTask UpdateTask { get; set; }

        private void Awake()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<GameManager>(this);
            serviceCollection.AddSingleton<UIManager>(GetComponent<UIManager>());
            serviceCollection.AddSingleton<IEvent, EventSystem>();
            serviceCollection.AddSingleton<IUpdateTask, TaskSystem>();
            serviceCollection.AddSingleton<ITaskRunner, TaskSystem>();
            serviceCollection.AddSingleton<IAnalytic, AnalyticSystem>();
            serviceCollection.AddSingleton<IPool, PoolSystem>();
            serviceCollection.AddSingleton<ITutorial, TutorialSystem>();
            serviceCollection.AddSingleton<IAdvertise, AdSystem>();

            serviceCollection.AddSingleton<IVariableDatabase, ResourceSystem>();
            serviceCollection.AddSingleton<IContentDatabase, ResourceSystem>();
            serviceCollection.AddSingleton<ISpriteDatabase, ResourceSystem>();

            serviceCollection.AddSingleton<IStorageJsonHandler, DefaultJsonHandler>();
            serviceCollection.AddSingleton<IStorage, StorageSystem>();
            serviceCollection.AddSingleton<IStorageConfig, GameConfiguration>();
            serviceCollection.AddSingleton<IStorageEventHandler, StorageEventHandler>();
            serviceCollection.AddSingleton<IFileHandler, FileHandler>();

            serviceCollection.AddSingleton<IAudioLoop, AudioSystem>();
            serviceCollection.AddSingleton<IAudioEffect, AudioSystem>();

            serviceCollection.AddSingleton<IUIEventHandler, UIManager>(GetComponent<UIManager>());
            serviceCollection.AddSingleton<IScreen, UISystem>();
            serviceCollection.AddSingleton<IToast, UISystem>();
            serviceCollection.AddSingleton<IAspectRatio, UISystem>();

            serviceCollection.AddSingleton<IAnalyticConfig, GameConfiguration>();
            serviceCollection.AddSingleton<IResourceConfig, GameConfiguration>();
            serviceCollection.AddSingleton<IAdvertiseConfig, GameConfiguration>();

            serviceCollection.AddSingleton<ITutorialService, TutorialService>();
            serviceCollection.AddSingleton<IAppService, AppService>();
            serviceCollection.AddSingleton<IAdvertiseService, AdvertiseService>();

            serviceCollection.AddSingleton<ILevelService, LevelService>();
            serviceCollection.AddSingleton<ILevelFactory, LevelFactory>();
            AddAdHandler(serviceCollection);
            _buildTask = serviceCollection.Build();
        }

        private void AddAdHandler(ServiceCollection serviceCollection)
        {
#if ADMOB
            serviceCollection.AddSingleton<IAdVideoHandler, AdMobeHandler>();
            serviceCollection.AddSingleton<IAdBannerHandler, AdMobeBanner>();
            serviceCollection.AddSingleton<IAdInterstitialHandler, AdMobeHandler>();
#elif BAZAAR && ADMOB
            serviceCollection.AddSigleton<IAdVideoHnadler, AdMobeHandler>();
            serviceCollection.AddSigleton<IAdInterstitialHandler, AdMobeHandler>();
#elif BAZAAR && TAPSELL
            var tapsell = new TapsellHandler(unitIdHandler,tapsellNativeBanner);
            _videoHnadler = tapsell as IAdVideoHnadler;
            _nativeBannerHandler = tapsell as IAdNativeBannerHandler;
            _interstitialHandler = tapsell as IAdInterstitialHandler;
#endif
        }

        private async void Start()
        {
            Debug.Log(Environment.UserName);
            LogService.EnableTag(Environment.UserName);
            LogService.LogInfo("hello mahdi", LogKey.MAHDI_TAG_KEY);

            QualitySettings.vSyncCount = 1;
            Screen.sleepTimeout = -1;
            await _buildTask;
            _buildTask = null;
            GC.Collect();
            Event.BroadcastInitializeEvent(ServiceProvider);
            UpdateTask.EnableUpdate();
        }

        private void OnApplicationFocus(bool focus)
        {
            Event?.BroadcastEvent(EEventType.OnApplicationStateChange, focus);
        }

        private void OnApplicationQuit()
        {
            Event.BroadcastEvent(EEventType.OnApplicationQuit);
        }
    }
}
