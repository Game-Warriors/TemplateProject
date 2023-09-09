using Common.Extensions;
using GameWarriors.EventDomain.Abstraction;
using GameWarriors.ResourceDomain.Abstraction;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;

namespace Managements.Handlers.Analytics
{
#if FIREBASE
    using Firebase.RemoteConfig;
    public class FirebaseRemoteHandler : IRemoteDataHandler
    {
        private readonly IEvent _event;
        //private FirebaseDataBridge _dataBridge;
        private FirebaseRemoteConfig _remoteConfig;
        //private Dictionary<string, float> _floatDictionary;
        //private Dictionary<string, int> _intDictionary;
        private Task _fetchingTask;

        public FirebaseRemoteHandler(IEvent @event)
        {
            _fetchingTask = Task.CompletedTask;
            _event = @event;
#if UNITY_EDITOR
            return;
#endif
            _remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        }

        public void RegisterData(IReadOnlyDictionary<string, IConvertible> dataTable)
        {
            //_dataBridge = new FirebaseDataBridge(dataTable);
        }

        [UnityEngine.Scripting.Preserve]
        public Task WaitForLoading()
        {
            return SetupRemoteDataAsync();
        }

        public async Task SetupRemoteDataAsync()
        {
#if UNITY_EDITOR
            return;
#endif
            try
            {
                await _remoteConfig.EnsureInitializedAsync();
                FetchRemoteData(true);
                await _fetchingTask;
                //await _remoteConfig.SetDefaultsAsync(_dataBridge);
                if (_remoteConfig.Info.LastFetchStatus == LastFetchStatus.Success)
                {
                    await _remoteConfig.ActivateAsync();
                }

                _event.ListenToEvent<bool>(EEventType.OnApplicationStateChange, FetchRemoteData);
            }
            catch (Exception E)
            {
                Debug.LogError($"FirebaseRemoteConfig Error: {E.Message}");
            }
        }

        public bool TryGetValue(string key, TypeCode type, out IConvertible value)
        {
            value = null;
#if UNITY_EDITOR
            return false;
#endif
            ConfigValue config = _remoteConfig.GetValue(key);
            if (config.Source != ValueSource.RemoteValue)
                return false;

            //Debug.Log(key + " : " + config.StringValue);
            switch (type)
            {
                case TypeCode.Int32:
                    {
                        value = (int)config.LongValue;
                        return true;
                    }
                case TypeCode.Int64:
                    {
                        value = config.LongValue;
                        return true;
                    }
                case TypeCode.Double:
                    {
                        value = config.DoubleValue;
                        return true;
                    }
                case TypeCode.String:
                    {
                        value = config.StringValue;
                        return true;
                    }
                case TypeCode.Boolean:
                    {
                        value = config.BooleanValue;
                        return true;
                    }
                case TypeCode.Single:
                    {
                        string stringValue = config.StringValue;
                        if (!float.TryParse(stringValue, out var tmp))
                        {
                            tmp = Convert.ToSingle(stringValue, CultureInfo.InvariantCulture);
                        }
                        value = tmp;
                        return true;
                    }
            }
            return false;
        }

        private void FetchRemoteData(bool isFocus)
        {
            //lock (_fetchingTask)
            {
                //Debug.Log("FetchRemoteData");
                if (Application.internetReachability != NetworkReachability.NotReachable && isFocus && _fetchingTask.IsCompleted && _remoteConfig != null)
                {
                    //Debug.Log($"FirebaseRemoteConfig : FetchAsync Started ");
                    _fetchingTask = _remoteConfig.FetchAsync();
                }
            }
        }

        //private async void FetchAndApplyRemoteData(Action onDone)
        //{
        //    if (Application.internetReachability != NetworkReachability.NotReachable && _fetchingTask.IsCompleted)
        //    {
        //        //Debug.Log($"FirebaseRemoteConfig : FetchAsync Started " + FirebaseRemoteConfig.DefaultInstance);
        //        _fetchingTask = true;
        //        await _remoteConfig.FetchAsync();
        //        bool isSuccess = await _remoteConfig.ActivateAsync();
        //        if (isSuccess)
        //        {
        //            //_floatDictionary.Clear();
        //            //_intDictionary.Clear();
        //        }

        //        _fetchingTask = false;
        //    }

        //    onDone?.Invoke();
        //}
    }
#endif
}