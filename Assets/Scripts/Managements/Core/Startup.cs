using Common.Extensions;
using GameWarriors.EventDomain.Abstraction;
using GameWarriors.EventDomain.Core;
using Managements.Core.UI;
using Services.Abstraction;
using System;
using UnityEngine;
using GameWarriors.PoolDomain.Abstraction;
using Services.Core.App;
using GameWarriors.UIDomain.Abstraction;
using GameWarriors.UIDomain.Core;
using GameWarriors.DependencyInjection.Core;
using Services.Core.Analytic;
using GameWarriors.TaskDomain.Abstraction;
using GameWarriors.TaskDomain.Core;
using GameWarriors.PoolDomain.Core;
using Common.ResourceKey;
using GameWarriors.AdDomain.Abstraction;
using GameWarriors.AdDomain.Core;
using GameWarriors.AnalyticDomain.Abstraction;
using GameWarriors.AnalyticDomain.Core;
using GameWarriors.AudioDomain.Abstraction;
using GameWarriors.AudioDomain.Core;
using GameWarriors.DependencyInjection.Extensions;
using GameWarriors.ResourceDomain.Abstraction;
using GameWarriors.ResourceDomain.Core;
using GameWarriors.StorageDomain.Abstraction;
using GameWarriors.StorageDomain.Core;
using GameWarriors.TutorialDomain.Abstraction;
using GameWarriors.TutorialDomain.Core;
using Managements.Core;
using Managements.Factory;
using Managements.Handlers.Audio;
using Managements.Handlers.Json;
using Services.Core.Level;
using Services.Core.Tutorial;
using Services.Core;
using GameWarriors.LocalizeDomain.Abstraction;
using GameWarriors.LocalizeDomain.Core;

namespace Management.Core
{
    /// <summary>
    /// Main and top manager class, start and end application lifecycle and handler application Type (3D and 2D).
    /// </summary>
    public class Startup : MonoBehaviour
    {
        public const string INIT_METHOD_NAME = "Initialization";

        [SerializeField] private GameObject _splash;

        public bool IsStarted { get; private set; }

        private async void Awake()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<MainManager>(GetComponent<MainManager>());
            serviceCollection.AddSingleton<UIManager>(GetComponent<UIManager>());
            serviceCollection.AddSingleton<IEvent, EventSystem>();

            serviceCollection.AddSingleton<IUpdateTask, TaskSystem>();
            serviceCollection.AddSingleton<ITaskRunner, TaskSystem>();
            serviceCollection.AddSingleton<ITimerTask, TaskSystem>();

            serviceCollection.AddSingleton<IAnalytic, AnalyticSystem>(input => input.WaitForLoading());

            serviceCollection.AddSingleton<IPool, PoolSystem>(input => input.WaitForLoading());
            serviceCollection.AddSingleton<IBehaviorInitializer<string>, CompondInitializer>();

            serviceCollection.AddSingleton<ITutorial, TutorialSystem>();
            serviceCollection.AddSingleton<IAdvertise, AdSystem>(input => input.WaitForLoading());

            serviceCollection.AddSingleton<IVariableDatabase, ResourceSystem>(input => input.WaitForLoading());
            serviceCollection.AddSingleton<IContentDatabase, ResourceSystem>();
            serviceCollection.AddSingleton<ISpriteDatabase, ResourceSystem>();

            serviceCollection.AddSingleton<IStorageJsonHandler, DefaultJsonHandler>();
            serviceCollection.AddSingleton<IStorage, StorageSystem>(input => input.WaitForLoading());
            serviceCollection.AddSingleton<IStorageConfig, MainConfiguration>();
            serviceCollection.AddSingleton<IStorageOperations, StorageSystem>();
            serviceCollection.AddSingleton<IFileHandler, FileHandler>();

            serviceCollection.AddSingleton<IAudioLoop, AudioSystem>(input => input.WaitForLoading());
            serviceCollection.AddSingleton<IAudioEffect, AudioSystem>();
            serviceCollection.AddSingleton<IAudioEventHandler, AudioEventHandler>();

            serviceCollection.AddSingleton<ILocalize, LocalizationSystem>(input => input.WaitForLoading());

            serviceCollection.AddSingleton<IUIEventHandler, UIManager>(GetComponent<UIManager>());
            serviceCollection.AddSingleton<IScreen, UISystem>(input => input.WaitForLoading());
            serviceCollection.AddSingleton<IToast, UISystem>();
            serviceCollection.AddSingleton<IAspectRatio, UISystem>();

            serviceCollection.AddSingleton<IAnalyticConfig, MainConfiguration>();
            serviceCollection.AddSingleton<IResourceConfig, MainConfiguration>();
            serviceCollection.AddSingleton<IAdvertiseConfig, MainConfiguration>();

            serviceCollection.AddSingleton<ITutorialService, TutorialService>();
            serviceCollection.AddSingleton<IAppService, AppService>();
            serviceCollection.AddSingleton<IAdvertiseService, AdvertiseService>();

            serviceCollection.AddSingleton<ILevelService, LevelService>();
            serviceCollection.AddSingleton<ILevelFactory, LevelFactory>();

            serviceCollection.AddSingleton<ILogService, LogService>();
            AddAdHandler(serviceCollection);
            Screen.sleepTimeout = -1;
            await serviceCollection.Build(StartupDone);
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

        private void StartupDone(IServiceProvider serviceProvider)
        {
            QualitySettings.vSyncCount = 1;
            Application.runInBackground = true;
            Application.targetFrameRate = 50;
            QualitySettings.vSyncCount = 2;
            Debug.Log(Environment.UserName);
            ILogService logService = serviceProvider.GetService<ILogService>();
            logService?.EnableTag(Environment.UserName);
            logService?.LogInfo("hello mahdi", LogKey.MAHDI_TAG_KEY);


            GC.Collect();
            serviceProvider.GetService<IEvent>().BroadcastStartupEvent(serviceProvider);
            serviceProvider.GetService<IUpdateTask>()?.EnableUpdate();
            IsStarted = true;
            if (_splash)
                Destroy(_splash);
            Destroy(this);
        }
    }
}