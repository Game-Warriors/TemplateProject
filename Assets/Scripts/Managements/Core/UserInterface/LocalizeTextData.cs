using TMPro;
using UnityEngine;

namespace Managements.Core.UserInterface
{
    [System.Serializable]
    public struct LocalizeTextData 
    {        
        [SerializeField]
        private string _key;
        [SerializeField]
        private TMP_Text _label;

        public TMP_Text Label => _label;
        public string Key => _key;

        public LocalizeTextData(TMP_Text label, string key)
        {
            _label = label;
            _key = key;
        }
    }
}
