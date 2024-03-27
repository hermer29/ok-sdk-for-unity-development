using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;
using OdnoklassnikiGamesSDK.Advertising;
using UnityEngine;
using UnityEngine.Scripting;

namespace OdnoklassnikiGamesSDK
{
    [Preserve]
    public static class OdnoklassnikiGamesSdk
    {
        private static Action s_onInitializeSuccessCallback;

        /// <summary>
        /// Enable it to log SDK callbacks in the console.
        /// </summary>
        public static bool CallbackLogging = false;

        /// <summary>
        /// SDK is initialized automatically on load.
        /// If either something fails or called way too early, this will return false.
        /// </summary>
        [Preserve]
        public static bool IsInitialized => GetOdnoklassnikiGamesSdkIsInitialized();

        [DllImport("__Internal")]        [Preserve]
        private static extern bool GetOdnoklassnikiGamesSdkIsInitialized();

        /// <summary>
        /// Invoke this and wait for coroutine to finish before using any SDK methods.<br/>
        /// Downloads Odnoklassniki SDK script and inserts it into the HTML page.
        /// </summary>
        /// <returns>Coroutine waiting for <see cref="IsInitialized"/> to return true.</returns>
        [Preserve]
        public static IEnumerator Initialize(Action onSuccessCallback = null)
        {
            s_onInitializeSuccessCallback = onSuccessCallback;
            Debug.Log("Initialization requested");
            OdnoklassnikiGamesSdkInitialize(OnInitializeSuccessCallback);

            while (!IsInitialized)
                yield return null;
                
            AdvertisingLoader.NotifyInitializationCompleted();
        }

        [DllImport("__Internal")]
        private static extern void OdnoklassnikiGamesSdkInitialize(Action successCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnInitializeSuccessCallback()
        {
            if (CallbackLogging)
                Debug.Log($"{nameof(OdnoklassnikiGamesSdk)}.{nameof(OnInitializeSuccessCallback)} invoked");

            s_onInitializeSuccessCallback?.Invoke();
        }
    }
}