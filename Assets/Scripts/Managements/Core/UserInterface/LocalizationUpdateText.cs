using GameWarriors.DependencyInjection.Extensions;
using GameWarriors.LocalizeDomain.Abstraction;
using UnityEngine;

namespace Managements.Core.UserInterface
{
    public class LocalizationUpdateText : MonoBehaviour
    {
        [SerializeField]
        private LocalizeTextData[] _localizationTextTable;

        private void Start()
        {
            ILocalize localizationSystem = MainManager.ServiceProvider.GetService<ILocalize>();
            if (localizationSystem != null)
            {
                localizationSystem.OnLanguageChanged += UpdateTextsLanguage;
                UpdateTextsLanguage();
            }
        }

        private void OnDestroy()
        {
            ILocalize localizationSystem = MainManager.ServiceProvider.GetService<ILocalize>();
            if (localizationSystem != null)
            {
                localizationSystem.OnLanguageChanged -= UpdateTextsLanguage;
            }
        }

        private void UpdateTextsLanguage()
        {
            ILocalize localizationSystem = MainManager.ServiceProvider.GetService<ILocalize>();
            for (int i = 0; i < _localizationTextTable.Length; ++i)
            {
                _localizationTextTable[i].Label.text = localizationSystem.GetTermTranslation(_localizationTextTable[i].Key);
            }
        }
    }
}