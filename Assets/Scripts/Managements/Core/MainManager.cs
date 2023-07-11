using Common.Extensions;
using Common.ResourceKey;
using GameWarriors.AdDomain.Abstraction;
using GameWarriors.AdDomain.Core;
using GameWarriors.AnalyticDomain.Abstraction;
using GameWarriors.AnalyticDomain.Core;
using GameWarriors.AudioDomain.Abstraction;
using GameWarriors.AudioDomain.Core;
using GameWarriors.DependencyInjection.Core;
using GameWarriors.DependencyInjection.Extensions;
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
using Managements.Factory;
using Managements.Handlers.Audio;
using Managements.Handlers.Json;
using Services.Abstraction;
using Services.Core;
using Services.Core.Analytic;
using Services.Core.App;
using Services.Core.Level;
using Services.Core.Tutorial;
using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace Managements.Core
{
    public class MainManager : MonoBehaviour
    {
        [Preserve] private IEvent Event { get; set; }



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
