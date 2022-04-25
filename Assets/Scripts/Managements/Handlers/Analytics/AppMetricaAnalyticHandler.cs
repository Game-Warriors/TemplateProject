using GameWarriors.AnalyticDomain.Abstraction;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Managements.Handlers.Analytics
{
#if APPMETRICA
    public class AppMetricaAnalyticHandler : IAnalyticHandler,ILevelAnalytic, IShopAnalytic, IResourceAnalytic, ICustomAnalytic
    {
        private readonly Dictionary<string, object> CUSTOME_EVENT1;
        private readonly Dictionary<string, object> CUSTOME_EVENT2;
        private readonly Dictionary<string, object> CUSTOME_EVENT3;

        public EAnalyticType AnalyticType => EAnalyticType.AppMetrica;

        public AppMetricaAnalyticHandler(string startAnalyticEvent)
        {

            CUSTOME_EVENT1 = new Dictionary<string, object>(1);
            CUSTOME_EVENT2 = new Dictionary<string, object>(2);
            CUSTOME_EVENT3 = new Dictionary<string, object>(3);

            if (!string.IsNullOrEmpty(startAnalyticEvent))
                AppMetrica.Instance.ReportEvent(startAnalyticEvent);
        }

        Task IAnalyticHandler.Loading()
        {
            return Task.CompletedTask;
        }


        public void CustomEvent(string eventName)
        {
            AppMetrica.Instance.ReportEvent(eventName);
        }

        void ICustomAnalytic.CustomEvent<T1>(string eventName, string param1Name, T1 param1)
        {
            CUSTOME_EVENT1.Clear();
            CUSTOME_EVENT1.Add(param1Name, param1);
            AppMetrica.Instance.ReportEvent(eventName, CUSTOME_EVENT1);
        }

        void ICustomAnalytic.CustomEvent<T1,T2>(string eventName, string param1Name, T1 param1, string param2Name, T2 param2)
        {
            CUSTOME_EVENT2.Clear();
            CUSTOME_EVENT2.Add(param1Name, param1);
            CUSTOME_EVENT2.Add(param2Name, param2);
            AppMetrica.Instance.ReportEvent(eventName, CUSTOME_EVENT2);
        }

        void ICustomAnalytic.CustomEvent<T1,T2,T3>(string eventName, string param1Name, T1 param1, string param2Name, T2 param2, string param3Name, T3 param3)
        {
            CUSTOME_EVENT3.Clear();
            CUSTOME_EVENT3.Add(param1Name, param1);
            CUSTOME_EVENT3.Add(param2Name, param2);
            CUSTOME_EVENT3.Add(param3Name, param3);
            AppMetrica.Instance.ReportEvent(eventName, CUSTOME_EVENT3);
        }

        void IAnalyticHandler.SetABTestTag(string abTestTag)
        {

        }

        void IShopAnalytic.ShopOpen(string location)
        {
            AppMetrica.Instance.ReportEvent("ShopOpen", new Dictionary<string, object>() { { "Location", location } });
        }

        void IShopAnalytic.ShopPackClick(string itemName, string location)
        {
            AppMetrica.Instance.ReportEvent("ShopPackClick", new Dictionary<string, object>() { { "PackName", itemName }, { "Location", location } });
        }

        void IShopAnalytic.PurchaseShopPack(string itemName, string purchaseId, string currencyId, float price, string location, string transactionId)
        {
            AppMetrica.Instance.ReportRevenue(new YandexAppMetricaRevenue((decimal)price, currencyId));
            AppMetrica.Instance.ReportEvent("Purchase", new Dictionary<string, object>() { { "PackName", itemName }, { "PurchaseId", purchaseId }, { "CurrencyId", currencyId }, { "Price", price }, { "Location", location }, { "TransactionId", transactionId } });
        }

        void IResourceAnalytic.EarnCurrency(string currencyType, int count, string earnType, string gainType)
        {
            AppMetrica.Instance.ReportEvent("EarnCurrency", new Dictionary<string, object>() { { "CurrencyId", currencyType }, { "Count", count }, { "EarnType", earnType }, { "GainType", gainType } });
        }

        void IResourceAnalytic.SpendCurrency(string currencyType, int count, string action, string type)
        {
            AppMetrica.Instance.ReportEvent("SpendCurrency", new Dictionary<string, object>() { { "CurrencyId", currencyType }, { "Count", count }, { "Action", action }, { "ItemType", type } });
        }

        void IShopAnalytic.PurchaseShopPack(string packId)
        {
            AppMetrica.Instance.ReportEvent("PurchaseShopPack", new Dictionary<string, object>() { { "PurchaseId", packId } });
        }

        void ILevelAnalytic.StartLevel(string levelId, int levelIndex)
        {
            AppMetrica.Instance.ReportEvent("level_start", new Dictionary<string, object>() { { "level_name", levelId }, { "level_number", levelIndex } });
            AppMetrica.Instance.SendEventsBuffer();
        }

        void ILevelAnalytic.LevelCompleted(string levelId, int levelIndex)
        {
            AppMetrica.Instance.ReportEvent("level_finish", new Dictionary<string, object>() { { "level_name", levelId }, { "level_number", levelIndex } });
            AppMetrica.Instance.SendEventsBuffer();
        }
    }
#endif
}
