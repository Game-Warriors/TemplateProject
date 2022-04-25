using GameWarriors.AnalyticDomain.Abstraction;

namespace AnalyticDomain.Core
{
#if GAME_ANALYTICS
    using GameAnalyticsSDK;
    using System.Threading.Tasks;

    public class GameAnalyticHandler : IAnalyticHandler, ICustomAnalytic, IEngagementAnalytic, IShopAnalytic, ITutorialAnalytic, ILevelAnalytic, IResourceAnalytic
    {
        public EAnalyticType AnalyticType => EAnalyticType.GameAnalytic;


        public GameAnalyticHandler(string startAnalyticEvent)
        {
            GameAnalytics.Initialize();
            if (!string.IsNullOrEmpty(startAnalyticEvent))
                GameAnalytics.NewDesignEvent(startAnalyticEvent);
        }

        Task IAnalyticHandler.Loading()
        {
            return Task.CompletedTask;
        }

        void IAnalyticHandler.SetABTestTag(string abTestTag)
        {
            GameAnalytics.SetCustomDimension01(abTestTag);
        }

        void ICustomAnalytic.CustomEvent(string eventName)
        {
            GameAnalytics.NewDesignEvent(eventName);
        }

        void ICustomAnalytic.CustomEvent(string eventName, string param1Name, object param1)
        {
            if (param1 is float)
            {
                GameAnalytics.NewDesignEvent($"{eventName}");
                return;
            }
            if (param1 is double)
            {
                GameAnalytics.NewDesignEvent($"{eventName}");
                return;
            }

            GameAnalytics.NewDesignEvent($"{eventName}:{param1}");
        }

        void ICustomAnalytic.CustomEvent(string eventName, string param1Name, object param1, string param2Name, object param2)
        {
            GameAnalytics.NewDesignEvent($"{eventName}:{param1}:{param2}");
        }

        void ILevelAnalytic.StartLevel(string levelId, int levelIndex)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, levelId, levelIndex.ToString());
        }

        void ILevelAnalytic.LevelCompleted(string levelId, int levelIndex)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, levelId, levelIndex.ToString());
        }

        void ITutorialAnalytic.StartTutorial(string level)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, $"Tutorial_{level}_Start");
        }

        void ITutorialAnalytic.CompleteTutorial(string level)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, $"Tutorial_{level}_Complete");
        }

        public void OnMusicClicked(bool value, string location)
        {
            string val = value ? "On" : "Off";
            GameAnalytics.NewDesignEvent($"Music:{location}:{val}");
        }

        public void OnEffectClicked(bool value, string location)
        {
            string v = value ? "On" : "Off";
            GameAnalytics.NewDesignEvent($"Sound:{location}:{v}");
        }

        void IEngagementAnalytic.FriendHelpClicked(string location)
        {
            GameAnalytics.NewDesignEvent($"Help:{location}:Touched");
        }

        void IEngagementAnalytic.RateUsClicked(string location)
        {
            string eventName = $"Rate:{location}:Touched";
            GameAnalytics.NewDesignEvent(eventName);
        }

        void IEngagementAnalytic.InviteFriendClicked(string location)
        {
            string eventName = $"Share:{location}:Touched";
            GameAnalytics.NewDesignEvent(eventName);
        }

        void IEngagementAnalytic.ReferralUser()
        {
            GameAnalytics.NewDesignEvent("ReferralUser");
        }

        void IEngagementAnalytic.FreeCoinsClick(string type)
        {
            string eventName = $"FreeCoin:{type}:Touched";
            GameAnalytics.NewDesignEvent(eventName);
        }

        void IEngagementAnalytic.VideoAdClick(string location)
        {
            GameAnalytics.NewDesignEvent($"VideoAd:{location}:Touched");
        }

        void IEngagementAnalytic.WatchVideoAd(string location)
        {
            GameAnalytics.NewDesignEvent($"VideoAd:{location}:Watched");
        }

        void IEngagementAnalytic.InviteClick(string location)
        {
            GameAnalytics.NewDesignEvent($"InviteButton:{location}:Touched");
        }

        void IShopAnalytic.PurchaseShopPack(string packId)
        {
            GameAnalytics.NewDesignEvent($"Purchase:{packId}");
        }

        void IShopAnalytic.PurchaseShopPack(string itemName, string purchaseId, string currencyId, float price, string location, string transactionId)
        {
            GameAnalytics.NewBusinessEvent(currencyId, (int)(price * 100), itemName, purchaseId, location);
        }

        void IEngagementAnalytic.DailyReward(string type)
        {
            string eventName = $"DailyReward:{type}";
            GameAnalytics.NewDesignEvent(eventName);
        }

        void IShopAnalytic.ShopOpen(string location)
        {
            GameAnalytics.NewDesignEvent($"ShopOpen:{location}");
        }

        void IShopAnalytic.ShopPackClick(string itemName, string location)
        {
            GameAnalytics.NewDesignEvent($"PackClick:{itemName}:{location}");
        }

        void IResourceAnalytic.EarnCurrency(string currencyType, int count, string earnType, string gainType)
        {
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, currencyType, count, earnType, gainType);
        }

        void IResourceAnalytic.SpendCurrency(string currencyType, int count, string spendType, string type)
        {
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, currencyType, count, spendType, type);
        }

    }
#endif
}
