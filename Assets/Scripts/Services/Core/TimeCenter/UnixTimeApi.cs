using System;
using GameWarriors.TimeDomain.Abstraction;
using Services.Data.TimeCenter;
using UnityEngine;
using UnityEngine.Networking;

namespace Services.Core.TimeCenter
{
    public class UnixTimeApi : IDateTimeApi
    {
        private const string API_URL = "https://showcase.api.linx.twenty57.net/UnixTime/tounixtimestamp?datetime=now";

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
                        UnixTimeApiDto dto = JsonUtility.FromJson<UnixTimeApiDto>(webRequest.downloadHandler.text);
                        bool isParsed = long.TryParse(dto.UnixTimeStamp, out var timeStamp);

                        if (isParsed)
                        {
                            currentUtc = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                            currentUtc = currentUtc.AddSeconds(timeStamp);

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