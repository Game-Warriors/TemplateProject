using Services.Abstraction;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Services.Core.Analytic
{
    public class LogService : ILogService
    {

        private readonly Dictionary<string, bool> _tagTable;

        public LogService()
        {
            _tagTable = new Dictionary<string, bool>();
#if DEVELOPMENT || TEST
#endif
        }

        public void EnableTag(string tag)
        {
            if (_tagTable.TryGetValue(tag, out bool enable))
            {
                if (!enable)
                    _tagTable[tag] = true;
            }
            else
            {
                _tagTable.Add(tag, true);
            }
        }

        public void DisableLog(string tag)
        {
            if (_tagTable.TryGetValue(tag, out bool enable))
            {
                if (enable)
                    _tagTable[tag] = false;
            }
            else
            {
                _tagTable.Add(tag, false);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogInfo(object message, string tag = null)
        {
#if UNITY_EDITOR || DEVELOPMENT || TEST
            if (_tagTable.TryGetValue(tag, out bool enable))
            {
                if (enable)
                {
                    Debug.Log(message);
                }
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogWarning(object message, string tag = null)
        {
            if (_tagTable.TryGetValue(tag, out bool enable))
            {
                if (enable)
                {
                    Debug.LogWarning(message);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogError(object message, string tag = null)
        {
            if (_tagTable.TryGetValue(tag, out bool enable))
            {
                if (enable)
                {
                    Debug.LogError(message);
                }
            }
        }
    }
}
