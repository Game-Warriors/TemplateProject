using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Abstraction
{
    public interface IAdvertiseService
    {
        bool IsAdAvailable { get; }

        void ShowAd(Action<bool> onDone);
        void RequestAd();
    }
}