using Common.Extensions;
using GameWarriors.DependencyInjection.Extensions;
using GameWarriors.EventDomain.Abstraction;
using GameWarriors.TutorialDomain.Abstraction;
using GameWarriors.TutorialDomain.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Data.Tutorial
{
    [CreateAssetMenu(fileName = "FirstTestTutorial", menuName = "Tutorial/Create FirstTestTutorial")]
    public class FirstTestTutorial : TutorialSessionData
    {
        [SerializeField]
        private EEventType _eventType;
        public override string TutorialKey => name;

        private IEvent _event;

        public override void Initialization(IServiceProvider serviceProvider)
        {
            _event = serviceProvider.GetService<IEvent>();
            //_event.ListenToEvent();
        }

        //[CreateAssetMenu(fileName = "TutorialItemData", menuName = "Tutorial/Create Tutorial Item Data")]

    }
}