using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Services.Abstraction
{
    public interface ILogService
    {
        void EnableTag(string tag);
        void DisableLog(string tag);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void LogInfo(object message, string tag = null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void LogWarning(object message, string tag = null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void LogError(object message, string tag = null);
    }
}