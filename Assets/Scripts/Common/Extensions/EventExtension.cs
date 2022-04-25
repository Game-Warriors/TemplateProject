using GameWarriors.EventDomain.Abstraction;
using System;

namespace Common.Extensions
{
    public enum EEventType
    {
        OnApplicationStateChange,
        OnApplicationQuit,
        OnAdAvailable
    }

    public static class EventExtension
    {
        public static void ListenToEvent(this IEvent eventController, EEventType eventType, Action action)
        {
            eventController.ListenToEvent((int) eventType, action);
        }

        public static void ListenToEvent<T>(this IEvent eventController, EEventType eventType, Action<T> action)
        {
            eventController.ListenToEvent((int) eventType, action);
        }

        public static void ListenToEvent<T, U>(this IEvent eventController, EEventType eventType, Action<T, U> action)
        {
            eventController.ListenToEvent((int) eventType, action);
        }

        public static void ListenToEvent<T, U, V>(this IEvent eventController, EEventType eventType, Action<T, U, V> action)
        {
            eventController.ListenToEvent((int) eventType, action);
        }

        public static void RemoveEventListener(this IEvent eventController, EEventType eventType, Action action)
        {
            eventController.RemoveEventListener((int) eventType, action);
        }

        public static void RemoveEventListener<T>(this IEvent eventController, EEventType eventType, Action<T> action)
        {
            eventController.RemoveEventListener((int) eventType, action);
        }

        public static void RemoveEventListener<T, U>(this IEvent eventController, EEventType eventType, Action<T, U> action)
        {
            eventController.RemoveEventListener((int) eventType, action);
        }

        public static void RemoveEventListener<T, U, V>(this IEvent eventController, EEventType eventType, Action<T, U, V> action)
        {
            eventController.RemoveEventListener((int) eventType, action);
        }

        public static void BroadcastEvent(this IEvent eventController, EEventType eventType)
        {
            eventController.BroadcastEvent((int) eventType);
        }

        public static void BroadcastEvent<T>(this IEvent eventController, EEventType eventType, T inputValue)
        {
            eventController.BroadcastEvent((int) eventType, inputValue);
        }

        public static void BroadcastEvent<T, U>(this IEvent eventController, EEventType eventType, T inputValue, U inputValue2)
        {
            eventController.BroadcastEvent((int) eventType, inputValue, inputValue2);
        }

        public static void BroadcastEvent<T, U, V>(this IEvent eventController, EEventType eventType, T inputValue, U inputValue2, V inputValue3)
        {
            eventController.BroadcastEvent((int) eventType, inputValue, inputValue2, inputValue3);
        }
    }
}