using GameWarriors.StorageDomain.Abstraction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Data.Tutorial
{
    public class TutorialDataModel : IStorageItem
    {
        public const string FILE_NAME = "TutoD.bin";

        [SerializeField]
        private string[] _doneTutorials;
        [SerializeField]
        private int _doneCounter;

        [SerializeField]
        private string[] _currnetTutorials;
        [SerializeField]
        private int _currentCounter;

        public bool IsEncrypt => false;

        public string FileName => FILE_NAME;

        public Type DataType => typeof(TutorialDataModel);

        public string GetDataString => JsonUtility.ToJson(this);

        public bool IsChanged { get; private set; }

        public bool IsInvalid => _doneTutorials == null || _currnetTutorials == null;

        public IEnumerable<string> DoneTutorialsKey
        {
            get
            {
                int length = _doneCounter;
                for (int i = 0; i < length; ++i)
                {
                    yield return _doneTutorials[i];
                }
            }
        }

        public IEnumerable<string> CurrnetTutorialsKey
        {
            get
            {
                int length = _currentCounter;
                for (int i = 0; i < length; ++i)
                {
                    yield return _currnetTutorials[i];
                }
            }
        }

        public void Initialization()
        {
            _doneTutorials = new string[20];
            _doneCounter = 0;
            _currnetTutorials = new string[20];
            _currentCounter = 0;
        }

        public void AddDoneTutorial(string id)
        {
            if (_doneCounter >= _doneTutorials.Length)
                Array.Resize<string>(ref _doneTutorials, _doneCounter + 9);

            _doneTutorials[_doneCounter] = id;
            _doneCounter++;
            SetAsChange();
        }

        public void RemoveCurrentTutorial(string tutorialKey)
        {
            if (_currentCounter == 0)
                return;

            int index = FindCurrentTutorial(tutorialKey);
            if (index > -1)
            {
                var targetItem = _currnetTutorials[index];
                --_currentCounter;
                if (_currentCounter != index)
                {
                    _currnetTutorials[index] = _currnetTutorials[_currentCounter];
                }
                else
                {
                    _currnetTutorials[index] = default;
                }
                SetAsChange();
                return;
            }
        }

        private int FindCurrentTutorial(string tutorialKey)
        {
            int length = _currentCounter;
            for (int i = 0; i < length; ++i)
            {
                if (_currnetTutorials[i] == tutorialKey)
                    return i;
            }
            return -1;
        }

        public void AddCurrentTutorial(string id)
        {
            if (_currentCounter >= _currnetTutorials.Length)
                Array.Resize<string>(ref _currnetTutorials, _currentCounter + 9);

            _currnetTutorials[_currentCounter] = id;
            _currentCounter++;
            SetAsChange();
        }

        private void SetAsChange()
        {
            IsChanged = true;
        }

        public void SetAsSaved()
        {
            IsChanged = false;
        }

        public void ClearData()
        {
            _doneTutorials = new string[20];
            _doneCounter = 0;
            _currnetTutorials = new string[20];
            _currentCounter = 0;
        }
    }
}