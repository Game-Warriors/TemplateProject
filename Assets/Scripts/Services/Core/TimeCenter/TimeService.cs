using Common.Extensions;
using GameWarriors.EventDomain.Abstraction;
using GameWarriors.StorageDomain.Abstraction;
using GameWarriors.TaskDomain.Abstraction;
using GameWarriors.TimeDomain.Abstraction;
using GameWarriors.TimeDomain.Data;
using Services.Abstraction;
using Services.Data.TimeCenter;
using System;
using System.Collections;
using UnityEngine;

namespace Services.Core.TimeCenter
{
    public class TimeService : ITimeService
    {
        private const int FETCHING_TIME_RETRY_COUNT = 5;
        private const float FETCHING_TIME_UPDATE_INTERVAL = 5;
        private const float TIME_IGNORE_THRESHOLD = 1;

        private readonly ITime _time;
        private readonly IAppService _appService;
        private TimeDataModel _dataModel;
        private float _startTime;
        private float _updateBuffer;
        private WaitForEndOfFrame _forEndOfFrame;
        public bool IsTimeValid
        {
            get
            {
                return _time.IsBaseTimeValid;
            }
        }

        public bool HasRemoteTime => _time.RemoteDateTime.HasValue;

        public DateTime DefaultIdleCoin => _dataModel.IdleCoinTime;

        public DateTime UtcNow
        {
            get
            {
                if (_time.RemoteDateTime.HasValue)
                {
                    return _time.RemoteDateTime - DateTime.UtcNow < TimeSpan.FromMinutes(TIME_IGNORE_THRESHOLD)
                        ? DateTime.UtcNow
                        : _time.RemoteDateTime.Value;
                }
                else
                {
                    return DateTime.UtcNow;
                }
            }
        }

        public DateTime FestivalIdleCoin => _dataModel.FestivalIdleDate;

        [UnityEngine.Scripting.Preserve]
        public TimeService(ITime time, IStorage storage, IEvent @event, ITaskRunner taskRunner, IAppService appService)
        {
            _time = time;
            _forEndOfFrame = new WaitForEndOfFrame();
            LoadData(storage);
            @event.ListenToEvent<bool>(EEventType.OnApplicationStateChange, AppStateChange);
            time.RegisterTimeUpdateApi(new TimeApiIO());
            time.RegisterTimeUpdateApi(new WorldClockApi());
            time.RegisterTimeUpdateApi(new UnixTimeApi());
            time.RegisterTimeUpdateApi(new WorldTimeApi());
            taskRunner.StartCoroutineTask(TimeUpdate());
            _appService = appService;
        }

        private void LoadData(IStorage storage)
        {
            storage.LoadingModelAsync<TimeDataModel>(TimeDataModel.FILE_NAME, false, OnDataLoad);
        }

        private void OnDataLoad(TimeDataModel input)
        {
            _dataModel = input;
            int length = _dataModel.DataCount;
            for (int i = 0; i < length; ++i)
            {
                TimeDataItem dataItem = _dataModel[i];
                if (!string.IsNullOrEmpty(dataItem.Id))
                    _time?.MentionTime(dataItem.Id, dataItem.Data);
            }

            if (DateTime.UtcNow > _dataModel.MinDate)
                _dataModel.MinDate = DateTime.UtcNow;
        }

        private void AppStateChange(bool focus)
        {
            if (focus)
            {
                _time.ClearRemoteDate();
                _time.UpdateBaseTime(true, FETCHING_TIME_RETRY_COUNT);
            }
        }

        public TimeSpan RemainTime(string key)
        {
            if (string.IsNullOrEmpty(key))
                return TimeSpan.Zero;

            return _time.TimeOffset(key);
        }

        public bool StartTime(string key, TimeSpan offset)
        {
            DefaultTimeData data = new DefaultTimeData(DateTime.UtcNow.Add(offset));
            bool isNew = _time.AddNewTime(key, data);
            if (isNew)
            {
                _dataModel.AddItem(key, data);
                return true;
            }

            return false;
        }

        public void UpdateTime(string key, TimeSpan offset)
        {
            if (string.IsNullOrEmpty(key))
                return;

            DefaultTimeData data = new DefaultTimeData(DateTime.UtcNow.Add(offset));
            bool isNew = _time.AddNewTime(key, data);
            if (isNew)
            {
                _dataModel.AddItem(key, data);
            }
            else
            {
                _time.UpdateTime(key, offset);
            }
        }


        public void RefreshTime(string key, TimeSpan offset)
        {
            if (string.IsNullOrEmpty(key))
                return;

            TimeSpan remainTime = RemainTime(key);
            if (remainTime.TotalSeconds < 0)
            {
                remainTime = TimeSpan.Zero;
            }

            bool isSuccess = _time.UpdateTime(key, remainTime + offset);
            if (!isSuccess)
            {
                DefaultTimeData data = new DefaultTimeData(DateTime.UtcNow.Add(offset));
                _time.AddNewTime(key, data);
                _dataModel.AddItem(key, data);
            }

            _dataModel.SetAsChange();
        }

        public bool HasKey(string key)
        {
            return _time.HasKey(key);
        }

        public bool EndTime(string key)
        {
            bool result = _time.UpdateTime(key, TimeSpan.Zero);
            if (result)
                _dataModel.SetAsChange();
            return result;
        }

        public void UpdateIdelCoin(DateTime value)
        {
            _dataModel.IdleCoinTime = value;
        }

        public void ForceUpdateRemoteTime()
        {
            _time.UpdateBaseTime(true, FETCHING_TIME_RETRY_COUNT);
        }


        private IEnumerator TimeUpdate()
        {
            while (Application.isPlaying)
            {
                if (HasRemoteTime)
                {
                    _startTime += Time.deltaTime;
                    if (_startTime > 0.5f)
                    {
                        //Debug.Log("remote date " + _time.RemoteDateTime.Value);
                        DateTime currentDateTime = _time.RemoteDateTime.Value.AddSeconds(_startTime);
                        //if ((currentDateTime - _time.RemoteDateTime.Value).TotalSeconds > 4)
                        //    EditorApplication.isPaused = true;
                        //Debug.Log(currentDateTime.ToLocalTime());
                        _startTime = 0;
                        _time.OverrideMinDate(currentDateTime);
                    }
                }
                yield return _forEndOfFrame;

                _updateBuffer += Time.deltaTime;
                if (_appService.IsInternetAvailable && _updateBuffer > FETCHING_TIME_UPDATE_INTERVAL)
                {
                    _updateBuffer = 0;
                    _time.UpdateBaseTime(false, FETCHING_TIME_RETRY_COUNT);
                }
            }
        }

        public void UpdateFestivalIdelCoin(DateTime newDate)
        {
            _dataModel.FestivalIdleDate = newDate;
        }
    }
}