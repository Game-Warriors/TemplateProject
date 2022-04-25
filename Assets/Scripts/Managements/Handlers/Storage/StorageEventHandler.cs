using Common.Extensions;
using GameWarriors.EventDomain.Abstraction;
using GameWarriors.StorageDomain.Abstraction;
using GameWarriors.TaskDomain.Abstraction;
using System;
using UnityEngine;

namespace Managements.Handlers.Storage
{
    public class StorageEventHandler : IStorageEventHandler
    {
        private Action<float> _update;
        private Action _forceSaveAction;

        [UnityEngine.Scripting.Preserve]
        public StorageEventHandler(IUpdateTask updateTask, IEvent eventController)
        {
            updateTask.RegisterUpdateTask(EventUpdate);
            eventController.ListenToEvent<bool>(EEventType.OnApplicationStateChange, ApplicationStateChange);
            eventController.ListenToEvent(EEventType.OnApplicationQuit, ApplicationQuit);
        }

        private void ApplicationQuit()
        {
            _forceSaveAction?.Invoke();
        }

        private void ApplicationStateChange(bool focus)
        {
            if (!focus)
                _forceSaveAction?.Invoke();
        }

        public void SetForceSaveEvent(Action forceSaveing)
        {
            _forceSaveAction = forceSaveing;
        }

        public void SetStorageUpdate(Action<float> update)
        {
            _update = update;
        }

        private void EventUpdate()
        {
            _update?.Invoke(Time.deltaTime);
        }

        public void LogErrorEvent(string message)
        {
            Debug.LogError(message);
        }
    }
}