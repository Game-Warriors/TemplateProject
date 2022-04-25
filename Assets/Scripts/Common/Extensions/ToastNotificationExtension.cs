using Common.ResourceKey;
using GameWarriors.DependencyInjection;
using System.Net;
using UnityEngine;

namespace Common.Extensions
{
    public static class ToastNotificationExtension
    {
        /*
        private static ILocalizeHandler _localizeHandler;

        static ToastNotificationExtension()
        {
            _localizeHandler = ServiceLocator.Resolve<ILocalizeHandler>();
        }

        public static void ShowToastByContext(this IToast ToastNotification, string localizeKey)
        {
            if (_localizeHandler == null)
                _localizeHandler = ServiceLocator.Resolve<ILocalizeHandler>();
            ToastNotification.ShowStaticToast(localizeKey);
        }

        public static void ShowToastByContext(this IToast ToastNotification, string localizeKey, int intValue)
        {
            if (_localizeHandler == null)
                _localizeHandler = ServiceLocator.Resolve<ILocalizeHandler>();
            ToastNotification.ShowStaticToast($"{_localizeHandler.GetTermTranslation(localizeKey)} {intValue}");
        }

        public static void ShowSyncingServerPopup(this IToast ToastNotification)
        {
            if (_localizeHandler == null)
                _localizeHandler = ServiceLocator.Resolve<ILocalizeHandler>();
            ToastNotification.ShowStaticToast(LocalizeKey.SyncServerKey);
        }

        public static void ShowNoServerToast(this IToast ToastNotification)
        {
            if (_localizeHandler == null)
                _localizeHandler = ServiceLocator.Resolve<ILocalizeHandler>();
            ToastNotification.ShowStaticToast(LocalizeKey.NoServerKey);
        }

        public static void ShowNotEnoughCoinToast(this IToast ToastNotification)
        {
            if (_localizeHandler == null)
                _localizeHandler = ServiceLocator.Resolve<ILocalizeHandler>();
            ToastNotification.ShowStaticToast(_localizeHandler.GetTermTranslation(LocalizeKey.NotEnoughCoinKey));
        }


        public static void ShowNoGemToast(this IToast ToastNotification)
        {
            if (_localizeHandler == null)
                _localizeHandler = ServiceLocator.Resolve<ILocalizeHandler>();
            ToastNotification.ShowStaticToast(_localizeHandler.GetTermTranslation(LocalizeKey.NotEnoughGemKey));
        }
        
        public static void ShowNoXpToast(this IToast ToastNotification)
        {
            if (_localizeHandler == null)
                _localizeHandler = ServiceLocator.Resolve<ILocalizeHandler>();
            ToastNotification.ShowStaticToast(_localizeHandler.GetTermTranslation(LocalizeKey.NotEnoughXpKey));
        }

        public static void ShowNoInternetToast(this IToast ToastNotification)
        {
            if (_localizeHandler == null)
                _localizeHandler = ServiceLocator.Resolve<ILocalizeHandler>();
            ToastNotification.ShowStaticToast(LocalizeKey.NoInternetKey);
        }

        public static void ShowTryAgianToast(this IToast ToastNotification)
        {
            if (_localizeHandler == null)
                _localizeHandler = ServiceLocator.Resolve<ILocalizeHandler>();
            ToastNotification.ShowStaticToast(LocalizeKey.TryAgainKey);
        }

        public static void HandleStatusCodeNotification(this IToast ToastNotification, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.NotFound)
                ShowNoServerToast(ToastNotification);
            else
                if (statusCode == HttpStatusCode.ServiceUnavailable)
                    ShowNoInternetToast(ToastNotification);
                else
                    ShowTryAgianToast(ToastNotification);
        }
        */
    }
}