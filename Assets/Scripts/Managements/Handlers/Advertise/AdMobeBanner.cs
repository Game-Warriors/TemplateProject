using GameWarriors.AdDomain.Abstraction;
using System;

namespace Managements.Handlers.Advertise
{
#if ADMOB
    using GoogleMobileAds.Api;
    using UnityEngine;

    public class AdMobeBanner : IAdBannerHandler
    {
        private readonly string BANNER_ID;
        private BannerView _bannerView;

        public event Action<bool> OnLoadDone;
        public event Action OnAdOpen;

        bool _isInShow = false;
        private int _adHeight;


        public bool IsLoad { get; private set; }
        public int AdHeight => _adHeight;

        public AdMobeBanner(IAdvertiseConfig advertiseConfig)
        {
            BANNER_ID = advertiseConfig.GetAdUnitId(EAdHandlerType.Admobe, EUnitAdType.BannerId);
        }

        public void HideBanner()
        {
            _bannerView?.Hide();
            _isInShow = false;
        }

        public void DestroyBanner()
        {
            _bannerView?.Destroy();
            _bannerView = null;
        }

        public void LoadBanner(EBannerType bannerType)
        {
            if (!string.IsNullOrEmpty(BANNER_ID))
            {
                BannerView newBannerView = new BannerView(BANNER_ID, AdSize.Banner, AdPosition.Bottom);
                AdRequest request = new AdRequest.Builder().Build();
                newBannerView.OnAdLoaded += (sender, e) => AdLoadSuccess(sender, e, newBannerView);
                newBannerView.OnAdFailedToLoad += AdLoadFailed;
                newBannerView.OnAdOpening += AdOpen;
                newBannerView.LoadAd(request);
            }
        }

        private void AdLoadSuccess(object sender, EventArgs e, BannerView newBannerView)
        {
            IsLoad = true;
            OnLoadDone?.Invoke(true);
            DestroyBanner();
            _bannerView = newBannerView;

        }

        private void AdLoadFailed(object sender, EventArgs e)
        {
            IsLoad = false;
            OnLoadDone?.Invoke(false);
        }

        private void AdOpen(object sender, EventArgs e)
        {
            OnAdOpen?.Invoke();
        }

        public void ShowBanner()
        {
            _isInShow = true;
            _bannerView?.Show();
        }

    }
#endif
}