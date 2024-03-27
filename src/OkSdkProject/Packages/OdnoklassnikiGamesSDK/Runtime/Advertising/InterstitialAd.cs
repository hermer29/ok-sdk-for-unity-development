using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace OdnoklassnikiGamesSDK.Advertising
{
    public static class InterstitialAd
    {
        private static Action s_onReadyCallback;
        private static Action s_onEndedCallback;
        private static Action<string> s_onErrorCallback;

        /// <summary>
        /// Shows the fullscreen ad banner.
        /// </summary>
        public static void Show(Action onReadyCallback = null, Action onEndedCallback = null,
            Action<string> onErrorCallback = null)
        {
            s_onReadyCallback = onReadyCallback;
            s_onEndedCallback = onEndedCallback;
            s_onErrorCallback = onErrorCallback;

            InterstitialAdShow(OnReadyCallback, OnEndedCallback, OnErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern bool InterstitialAdShow(Action preparedCallback, Action onEnded, Action<string> errorCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnReadyCallback()
        {
            if (OdnoklassnikiGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(InterstitialAd)}.{nameof(OnReadyCallback)} invoked");

            s_onReadyCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnEndedCallback()
        {
            if (OdnoklassnikiGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(InterstitialAd)}.{nameof(OnEndedCallback)} invoked");

            s_onEndedCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnErrorCallback(string errorMessage)
        {
            if (OdnoklassnikiGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(InterstitialAd)}.{nameof(OnErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onErrorCallback?.Invoke(errorMessage);
        }
    }
}