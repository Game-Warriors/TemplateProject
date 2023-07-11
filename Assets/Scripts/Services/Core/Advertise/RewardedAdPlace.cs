using GameWarriors.AdDomain.Abstraction;
using Services.Abstraction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Core
{
    public class RewardedAdPlace : IRewardedAdPlace
    {
        private string _placeId;
        private bool _isAdAvailable;
        private IAdvertiseService _advertiseService;

        public RewardedAdPlace(IAdvertiseService advertiseService, string placeId)
        {
            _advertiseService = advertiseService;
            _placeId = placeId;
        }

        public string Id => _placeId;

        public bool IsAdAvailable => _isAdAvailable;

        public void OnVideoLoaded()
        {
            _isAdAvailable = true;
            _advertiseService.OnVideoLoaded();
        }

        public void OnVideoLoadFailed(EAdState adState, int code, string message)
        {
            _isAdAvailable = false;
            _advertiseService.OnVideoLoadFailed(adState, code, message);
        }

        public void OnVideoOpen(string madiationName, string response)
        {
            _isAdAvailable = false;
            _advertiseService.OnVideoOpen(madiationName, response);
        }

        public void OnVideoPaidData(string madiationName, string correncyCode, long amount, string precision)
        {
            _advertiseService.OnVideoPaidData(madiationName, correncyCode, amount, precision);
        }

        public void OnVideoReward(bool hasReward)
        {
            _advertiseService.OnVideoReward(hasReward);
        }

        public void OnVideoShowFailed(EAdState state, int statusCode, string message)
        {
            _isAdAvailable = false;
            _advertiseService.OnVideoShowFailed(state, statusCode, message);
        }
    }

}