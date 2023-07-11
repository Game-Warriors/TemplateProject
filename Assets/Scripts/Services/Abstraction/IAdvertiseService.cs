using GameWarriors.AdDomain.Abstraction;
using System;

namespace Services.Abstraction
{
    public interface IAdvertiseService
    {
        IInterstitialAdPlace InterstitialAdPlace { get; }
        IRewardedAdPlace RewardedAdPlace { get; }
        bool IsNoAds { get; }
        bool IsAdAvailable { get; }
        bool IsInterstitialAvailable { get; }
        void ShowAd(Action<bool> onDone, string placement);
        void RequestAd();
        void SetNoAds();
        void LoadInterstitialAd();
        void ShowInterstitialAd(string placement);
        void OnVideoLoaded();
        void OnVideoLoadFailed(EAdState adState, int code, string message);
        void OnVideoOpen(string madiationName, string response);
        void OnVideoPaidData(string madiationName, string correncyCode, long amount, string precision);
        void OnVideoReward(bool hasReward);
        void OnVideoShowFailed(EAdState state, int statusCode, string message);
    }
}