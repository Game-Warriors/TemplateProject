using GameWarriors.DependencyInjection.Extensions;
using GameWarriors.EventDomain.Abstraction;
using GameWarriors.TaskDomain.Abstraction;
using GameWarriors.UIDomain.Abstraction;
using GameWarriors.UIDomain.Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

namespace Managements.Core.UserInterface
{
    public class UIManager : MonoBehaviour, IUIEventHandler
    {

        private IToastItem _lastToastObject;
        private IEnumerator _lastToast;
        private DateTime _screenShowData;
        private string _currentScreenName;


        [Preserve] private IScreenStack ScreenHandler { get; set; }
        [Preserve] private IEvent EventController { get; set; }
        [Preserve] private ITaskRunner TaskRunner { get; set; }
        [Preserve] private IEvent EventSystem { get; set; }
        [Preserve] private IServiceProvider ServiceProvider { get; set; }

        public void Initialization(IUpdateTask updateTask, IScreenStack screenStack, IServiceProvider serviceProvider, IToast toast,IUIOperation uiOperation)
        {
            BaseScreenItem.Initialization(screenStack, serviceProvider, toast);
            updateTask.RegisterUpdateTask(uiOperation.SystemUpdate);
        }

        public void OnCloseLastScreen()
        {
        }

        public void OnOpenScreen(IScreenItem screen)
        {
            if (screen.HasLogEvent)
            {
                _screenShowData = DateTime.UtcNow;
                _currentScreenName = screen.ScreenName;
            }
        }

        public void OnShowScreen(IScreenItem screen)
        {
            if (screen.HasLogEvent)
            {
                _screenShowData = DateTime.UtcNow;
                _currentScreenName = screen.ScreenName;
            }

            //play audio
        }

        public void OnCloseScreen(IScreenItem screen)
        {
            if (screen.HasLogEvent)
            {
                if (_currentScreenName == screen.ScreenName)
                {
                    double openTime = (DateTime.UtcNow - _screenShowData).TotalSeconds;
                }
            }
        }

        public void OnHideScreen(IScreenItem screen)
        {
            if (screen.HasLogEvent)
            {
                if (_currentScreenName == screen.ScreenName)
                {
                    double openTime = (DateTime.UtcNow - _screenShowData).TotalSeconds;
                }
            }
        }

        public void OnScreenForceClose(IScreenItem screen)
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
            _lastToastObject.Activation = false;
        }


        public void OnToastRises(float showTimeLength, IToastItem toast)
        {
            _lastToastObject = toast;
            if (_lastToast != null)
                StopCoroutine(_lastToast);
            _lastToast = DisableLastToast(showTimeLength);
            StartCoroutine(_lastToast);
        }

        public void OnCanvasCameraChange(Camera newCamera)
        {

        }
    }
}