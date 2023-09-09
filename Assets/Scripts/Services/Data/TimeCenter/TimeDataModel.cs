using GameWarriors.StorageDomain.Abstraction;
using GameWarriors.TimeDomain.Data;
using System;
using UnityEngine;

namespace Services.Data.TimeCenter
{
    [Serializable]
    public class TimeDataModel : IStorageItem
    {
        public const string FILE_NAME = "TDoM.bin";

        [SerializeField]
        private TimeDataItem[] _timeItems;
        [SerializeField]
        private int _counter;
        [SerializeField]
        private long _minDate;
        [SerializeField]
        private long _idleCoinTime;
        [SerializeField]
        private long _fIdleCoinDate;

        public bool IsEncrypt => false;

        public string ModelName => FILE_NAME;

        public Type DataType => typeof(TimeDataModel);

        public string GetDataString => JsonUtility.ToJson(this);

        public bool IsChanged { get; private set; }

        public int DataCount => _timeItems?.Length ?? 0;

        public TimeDataItem this[int index] => _timeItems[index];

        public DateTime MinDate { get => DateTime.FromBinary(_minDate); set => _minDate = value.ToBinary(); }
        public DateTime FestivalIdleDate { get => DateTime.FromBinary(_fIdleCoinDate); set => _fIdleCoinDate = value.ToBinary(); }
        

        public bool IsInvalid => false;

        public DateTime IdleCoinTime
        {
            get => DateTime.FromBinary(_idleCoinTime);
            set
            {
                _idleCoinTime = value.ToBinary();
                SetAsChange();
            }
        }

        public void SetAsSaved()
        {
            IsChanged = false;
        }

        public void SetAsChange()
        {
            IsChanged = true;
        }

        public void Initialization()
        {
            _timeItems = new TimeDataItem[20];
            _counter = 0;
            MinDate = DateTime.MaxValue;
            FestivalIdleDate = DateTime.MaxValue;
            SetAsChange();
        }

        public void AddItem(string key, DefaultTimeData newData)
        {
            if (_counter >= _timeItems.Length)
            {
                Array.Resize(ref _timeItems, _counter + 10);
            }

            int index = FindItem(key);
            if (index == -1)
            {
                _timeItems[_counter] = new TimeDataItem(key, newData);
                ++_counter;
            }
            else
            {
                _timeItems[index] = new TimeDataItem(key, newData);
            }
            SetAsChange();
        }

        private int FindItem(string key)
        {
            int length = _counter;
            for (int i = 0; i < length; ++i)
            {
                if (string.Compare(key, _timeItems[i].Id) == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        public void ClearData()
        {

        }
    }
}