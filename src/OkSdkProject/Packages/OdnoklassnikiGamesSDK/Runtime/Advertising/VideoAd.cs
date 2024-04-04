using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using UnityEngine.Scripting;

namespace OdnoklassnikiGamesSDK.Advertising
{
    public static class VideoAd
    {
        private static Action s_onLoadAdSuccessCallback;
        private static Action<string> s_onLoadAdErrorCallback;
        private static Action s_onShowAdSuccessCallback;
        private static Action<string> s_onShowAdErrorCallback;

        /// <summary>
        /// Loads the rewarded video ad.
        /// </summary>
        [Preserve]
        public static void Load(Action successCallback = null, Action<string> errorCallback = null)
        {   
            s_onLoadAdSuccessCallback = successCallback;
            s_onLoadAdErrorCallback = errorCallback;

            VideoAdLoad(OnLoadAdSuccessCallback, OnLoadAdErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern bool VideoAdLoad(Action successCallback, Action<string> errorCallback);
        
        [DllImport("__Internal")]
        private static extern bool VideoAdShow(Action successCallback, Action<string> errorCallback);
        
        [MonoPInvokeCallback(typeof(Action))]
        private static void OnLoadAdSuccessCallback()
        {
            if (OdnoklassnikiGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(VideoAd)}.{nameof(OnLoadAdSuccessCallback)} invoked");

            s_onLoadAdSuccessCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnLoadAdErrorCallback(string errorMessage)
        {
            if (OdnoklassnikiGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(VideoAd)}.{nameof(OnLoadAdErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onLoadAdErrorCallback?.Invoke(errorMessage);
        }

        /// <summary>
        /// Shows the rewarded video ad.
        /// </summary>
        [Preserve]
        public static void Show(Action rewardedCallback, Action<string> errorCallback = null)
        {
            s_onShowAdSuccessCallback = rewardedCallback;
            s_onShowAdErrorCallback = errorCallback;

            VideoAdShow(OnShowAdSuccessCallback, OnShowAdErrorCallback);
        }
        
        [MonoPInvokeCallback(typeof(Action))]
        private static void OnShowAdSuccessCallback()
        {
            if (OdnoklassnikiGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(VideoAd)}.{nameof(OnShowAdSuccessCallback)} invoked");

            s_onShowAdSuccessCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnShowAdErrorCallback(string errorMessage)
        {
            if (OdnoklassnikiGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(VideoAd)}.{nameof(OnShowAdErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onShowAdErrorCallback?.Invoke(errorMessage);
        }
    }
}