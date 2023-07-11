using GameWarriors.TimeDomain.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Data.TimeCenter
{
    [Serializable]
    public struct TimeDataItem
    {
        [SerializeField]
        private string _id;
        [SerializeField]
        private DefaultTimeData _data;

        public string Id => _id;
        public DateTime EndDate => _data.EndDate;

        public DefaultTimeData Data => _data;

        public TimeDataItem(string id, DefaultTimeData data)
        {
            _id = id;
            _data = data;
        }
    }
}