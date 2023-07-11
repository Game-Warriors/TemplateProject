using System;
using GameWarriors.TimeDomain.Abstraction;
using Services.Data.TimeCenter;
using UnityEngine;
using UnityEngine.Networking;

namespace Services.Core.TimeCenter
{
    public class TimeApiIO : IDateTimeApi
    {
        private const string API_URL = "https://www.timeapi.io/api/Time/current/zone?timeZone=Etc/UTC";

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
                        TimeApiIODto dto = JsonUtility.FromJson<TimeApiIODto>(webRequest.downloadHandler.text);
                        bool isParsed = DateTime.TryParse(dto.dateTime, out currentUtc);
                        currentDateTime(isParsed, currentUtc);
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