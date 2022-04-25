using System.Collections.Generic;
using System.Threading.Tasks;
using GameWarriors.AnalyticDomain.Abstraction;

namespace Managements.Handlers.Analytics
{
#if APPS_FLYER
using AppsFlyerSDK;

    public class AppsFlyerAnalyticHandler : IAnalyticHandler, ICustomAnalytic, IShopAnalytic
    {
        private readonly Dictionary<string, string> CUSTOME_EVENT0;
        private readonly Dictionary<string, string> CUSTOME_EVENT1;
        private readonly Dictionary<string, string> CUSTOME_EVENT2;
        private readonly Dictionary<string, string> CUSTOME_EVENT3;

        public EAnalyticType AnalyticType => EAnalyticType.AppsFlyer;

        public AppsFlyerAnalyticHandler()
        {
            CUSTOME_EVENT0 = new Dictionary<string, string>(0);
            CUSTOME_EVENT1 = new Dictionary<string, string>(1);
            CUSTOME_EVENT2 = new Dictionary<string, string>(2);
            CUSTOME_EVENT3 = new Dictionary<string, string>(3);
        }

        public void SetABTestTag(string abTestTag)
        {
        }

        public Task Loading()
        {
            return Task.CompletedTask;
        }

        void ICustomAnalytic.CustomEvent(string eventName)
        {
            AppsFlyer.sendEvent(eventName, CUSTOME_EVENT0);
        }

        void ICustomAnalytic.CustomEvent<T1>(string eventName, string param1Name, T1 param1)
        {
            CUSTOME_EVENT1.Clear();
            CUSTOME_EVENT1.Add(param1Name, param1.ToString());
            AppsFlyer.sendEvent(eventName, CUSTOME_EVENT1);
        }

        void ICustomAnalytic.CustomEvent<T1,T2>(string eventName, string param1Name, T1 param1, string param2Name, T2 param2)
        {
            CUSTOME_EVENT2.Clear();
            CUSTOME_EVENT2.Add(param1Name, param1.ToString());
            CUSTOME_EVENT2.Add(param2Name, param2.ToString());
            AppsFlyer.sendEvent(eventName, CUSTOME_EVENT2);
        }


        void ICustomAnalytic.CustomEvent<T1,T2,T3>(string eventName, string param1Name, T1 param1, string param2Name, T2 param2, string param3Name, T3 param3)
        {
            CUSTOME_EVENT3.Clear();
            CUSTOME_EVENT3.Add(param1Name, param1.ToString());
            CUSTOME_EVENT3.Add(param2Name, param2.ToString());
            CUSTOME_EVENT3.Add(param3Name, param3.ToString());
            AppsFlyer.sendEvent(eventName, CUSTOME_EVENT3);
        }

        public void PurchaseShopPack(string packId)
        {
        }

        public void ShopPackClick(string itemName, string location)
        {
        }

        public void ShopOpen(string location)
        {
        }

        public void PurchaseShopPack(string itemName, string purchaseId, string currencyId, float price, string location, string transactionId)
        {
            Dictionary<string, string> eventValues = new Dictionary<string, string>();
            eventValues.Add(AFInAppEvents.CURRENCY, currencyId);
            eventValues.Add(AFInAppEvents.REVENUE, price.ToString());
            eventValues.Add(AFInAppEvents.QUANTITY, "1");
            eventValues.Add("SKU", purchaseId);
            eventValues.Add("TRANSACTION_ID", transactionId);
            AppsFlyer.sendEvent("af_purchase", eventValues);
        }
    }
#endif
}