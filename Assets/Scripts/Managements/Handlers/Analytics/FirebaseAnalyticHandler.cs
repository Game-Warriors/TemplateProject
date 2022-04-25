using GameWarriors.AnalyticDomain.Abstraction;

namespace Managements.Handlers.Analytics
{
#if FIREBASE
    using Firebase.Analytics;
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    public class FirebaseAnalyticHandler : IAnalyticHandler, ILevelAnalytic, IShopAnalytic, IResourceAnalytic, ICustomAnalytic
    {
        private readonly string Start_Analytic_Event;

        public EAnalyticType AnalyticType => EAnalyticType.Firebase;

        public FirebaseAnalyticHandler(string startAnalyticEvent)
        {
            Start_Analytic_Event = startAnalyticEvent;
        }

        Task IAnalyticHandler.Loading()
        {
#if !DEVELOPMENT
            return Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    //   app = Firebase.FirebaseApp.DefaultInstance;
                    //Debug.Log("Firebase Initialized");

                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                    if (!string.IsNullOrEmpty(Start_Analytic_Event))
                        FirebaseAnalytics.LogEvent(Start_Analytic_Event);
                }
                else
                {
                    UnityEngine.Debug.LogError(string.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });
#else
            return Task.CompletedTask;
#endif
        }

        void ICustomAnalytic.CustomEvent(string eventName)
        {
            FirebaseAnalytics.LogEvent(eventName);
        }

        void ICustomAnalytic.CustomEvent<T1>(string eventName, string param1Name, T1 param1)
        {
            FirebaseAnalytics.LogEvent(eventName, CreateParameter<T1>(param1Name, param1));
        }

        void ICustomAnalytic.CustomEvent<T1, T2>(string eventName, string param1Name, T1 param1, string param2Name, T2 param2)
        {
            FirebaseAnalytics.LogEvent(eventName, CreateParameter<T1>(param1Name, param1), CreateParameter<T2>(param2Name, param2));
        }

        void ICustomAnalytic.CustomEvent<T1, T2, T3>(string eventName, string param1Name, T1 param1, string param2Name, T2 param2, string param3Name, T3 param3)
        {
            FirebaseAnalytics.LogEvent(eventName, CreateParameter<T1>(param1Name, param1), CreateParameter<T2>(param2Name, param2), CreateParameter<T3>(param3Name, param3));
        }

        void IAnalyticHandler.SetABTestTag(string abTestTag)
        {
        }

        void IShopAnalytic.ShopOpen(string location)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventScreenView, new Parameter(FirebaseAnalytics.ParameterScreenName, "Shop"), new Parameter(FirebaseAnalytics.ParameterShipping, location));
        }

        void IShopAnalytic.ShopPackClick(string itemName, string location)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAddToCart, new Parameter(FirebaseAnalytics.ParameterItemName, itemName), new Parameter(FirebaseAnalytics.ParameterShipping, location));
        }

        void IShopAnalytic.PurchaseShopPack(string itemName, string purchaseId, string currencyId, float price, string location, string transactionId)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventPurchase, new Parameter(FirebaseAnalytics.ParameterItemName, itemName), new Parameter(FirebaseAnalytics.ParameterItemId, purchaseId), new Parameter(FirebaseAnalytics.ParameterCurrency, currencyId), new Parameter(FirebaseAnalytics.ParameterValue, price), new Parameter(FirebaseAnalytics.ParameterShipping, location));
        }

        void IResourceAnalytic.EarnCurrency(string currencyType, int count, string earnType, string gainType)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventEarnVirtualCurrency, new Parameter(FirebaseAnalytics.ParameterVirtualCurrencyName, currencyType), new Parameter(FirebaseAnalytics.ParameterValue, count), new Parameter(FirebaseAnalytics.ParameterMethod, earnType), new Parameter("GainType", gainType));
        }

        void IResourceAnalytic.SpendCurrency(string currencyType, int count, string action, string type)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSpendVirtualCurrency, new Parameter(FirebaseAnalytics.ParameterVirtualCurrencyName, currencyType), new Parameter(FirebaseAnalytics.ParameterValue, count), new Parameter(FirebaseAnalytics.ParameterMethod, action), new Parameter("Type", type));
        }

        void IShopAnalytic.PurchaseShopPack(string packId)
        {
            FirebaseAnalytics.LogEvent("Purchase", new Parameter(FirebaseAnalytics.ParameterItemId, packId));
        }

        void ILevelAnalytic.StartLevel(string levelId, int levelIndex)
        {
            FirebaseAnalytics.LogEvent("level_start", new Parameter("level_name", levelId), new Parameter("level_number", levelIndex));
        }

        void ILevelAnalytic.LevelCompleted(string levelId, int levelIndex)
        {
            FirebaseAnalytics.LogEvent("level_finish", new Parameter("level_name", levelId), new Parameter("level_number", levelIndex));
        }

        private Parameter CreateParameter<T>(string paramName, T param) where T : IConvertible
        {
            if (param is float)
            {
                return new Parameter(paramName, param.ToSingle(CultureInfo.CurrentCulture.NumberFormat));
            }
            else if (param is double)
            {
                return new Parameter(paramName, param.ToDouble(CultureInfo.CurrentCulture.NumberFormat));
            }
            else if (param is int)
            {
                return new Parameter(paramName, param.ToInt32(CultureInfo.CurrentCulture.NumberFormat));
            }
            else if (param is long)
            {
                return new Parameter(paramName, param.ToInt64(CultureInfo.CurrentCulture.NumberFormat));
            }
            else
                return new Parameter(paramName, param.ToString());
        }
    }
#endif
}