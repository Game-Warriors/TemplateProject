using GameWarriors.DependencyInjection.Attributes;
using GameWarriors.DependencyInjection.Extensions;
using GameWarriors.EventDomain.Abstraction;
using GameWarriors.TaskDomain.Abstraction;
using GameWarriors.UIDomain.Abstraction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managements.Core
{
    public class UIManager : MonoBehaviour, IUIEventHandler
    {
        private IToastItem _lastToastObject;
        private IEnumerator _lastToast;
        private DateTime _screenShowData;
        private string _currentScreenName;

        [Inject] private IScreen ScreenHandler { get; set; }
        [Inject] private IEvent EventController { get; set; }
        [Inject] private ITaskRunner TaskRunner { get; set; }
        [Inject] private IEvent EventSystem { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }

        public void Initialization(IServiceProvider serviceProvider)
        {

        }

        public void OnCloseLastScreen()
        {
        }

        public void OnOpenScreen(IUIScreen screen)
        {
            if (screen.HasLogEvent)
            {
                _screenShowData = DateTime.UtcNow;
                _currentScreenName = screen.ScreenName;
            }
        }

        public void OnShowScreen(IUIScreen screen)
        {
            if (screen.HasLogEvent)
            {
                _screenShowData = DateTime.UtcNow;
                _currentScreenName = screen.ScreenName;
            }

            //play audio
        }

        public void OnCloseScreen(IUIScreen screen)
        {
            if (screen.HasLogEvent)
            {
                if (_currentScreenName == screen.ScreenName)
                {
                    double openTime = (DateTime.UtcNow - _screenShowData).TotalSeconds;
                }
            }
        }

        public void OnHideScreen(IUIScreen screen)
        {
            if (screen.HasLogEvent)
            {
                if (_currentScreenName == screen.ScreenName)
                {
                    double openTime = (DateTime.UtcNow - _screenShowData).TotalSeconds;
                }
            }
        }

        public void OnScreenForceClose(IUIScreen screen)
        {
            if (screen.HasLogEvent)
            {
                if (_currentScreenName == screen.ScreenName)
                {
                    double openTime = (DateTime.UtcNow - _screenShowData).TotalSeconds;
                }
            }
        }

        private IEnumerator DisableLastToast(float showTimeLength)
        {
            yield return new WaitForSeconds(showTimeLength);
            _lastToastObject.Activation =false;
        }

        public void SetUIUpdate(Action uiUpdate)
        {
            if (uiUpdate != null)
            {
                IUpdateTask updateTask = ServiceProvider.GetService<IUpdateTask>();
                updateTask.RegisterUpdateTask(uiUpdate);
            }
        }

        public void OnToastRises(float showTimeLength, IToastItem toast)
        {
            _lastToastObject = toast;
            if (_lastToast != null)
                StopCoroutine(_lastToast);
            _lastToast = DisableLastToast(showTimeLength);
            StartCoroutine(_lastToast);
        }
    }
}