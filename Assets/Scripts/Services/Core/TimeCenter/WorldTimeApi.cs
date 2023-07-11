using System;
using GameWarriors.TimeDomain.Abstraction;
using Services.Data.TimeCenter;
using UnityEngine;
using UnityEngine.Networking;

namespace Services.Core.TimeCenter
{
    public class WorldTimeApi : IDateTimeApi
    {
        private const string API_URL = "https://worldtimeapi.org/api/timezone/Etc/UTC";

        public void GetDateTime(Action<bool, DateTime> currentDateTime)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(API_URL);
            UnityWebRequestAsyncOperation operation = webRequest.SendWebRequest();
            operation.completed += asyncOperation =>
            {
                DateTime currentUtc = DateTime.UtcNow;

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        WorldTimeApiDto dto = JsonUtility.FromJson<WorldTimeApiDto>(webRequest.downloadHandler.text);
                        bool isParsed = DateTime.TryParse(dto.utc_datetime, out currentUtc);

                        if (isParsed)
                        {
                            currentUtc = currentUtc.ToUniversalTime();
                            currentDateTime(true, currentUtc);
                        }
                        else
                        {
                            currentDateTime(false, currentUtc);
                        }
                    }
                    catch (Exception e)
                    {
                        currentDateTime(false, currentUtc);
                    }
                }
                else
                {
                    currentDateTime(false, currentUtc);
                }
            };
        }
    }
}