using GameWarriors.StorageDomain.Abstraction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Data.Tutorial
{
    [Serializable]
    public struct TutorialSaveItem : IStorageDataItem
    {
        [SerializeField]
        private string _key;

        public string Key => _key;

        public TutorialSaveItem(string key)
        {
            _key = key;
        }

        public bool Equals(IStorageDataItem other)
        {
            if (other is TutorialSaveItem)
            {
                TutorialSaveItem item = (TutorialSaveItem)other;
                return item._key == _key;
            }
            return false;
        }
    }

}