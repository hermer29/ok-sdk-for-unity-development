using System;
using System.Collections;
using OdnoklassnikiGamesSDK;
using OdnoklassnikiGamesSDK.Advertising;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Testing
{
    public class PlaytestingCanvas : MonoBehaviour
    {
        [SerializeField] private Image _initializedIndicator;
        
        public void Initialize()
        {
            Debug.Log("Initialization requested from playtesting canvas");
            OdnoklassnikiGamesSdk.CallbackLogging = true;
            StartCoroutine(InitializeExecution());
        }

        private IEnumerator InitializeExecution()
        {
            yield return OdnoklassnikiGamesSdk.Initialize();
        }

        private void Update()
        {
            _initializedIndicator.color = OdnoklassnikiGamesSdk.IsInitialized ? Color.green : Color.red;
        }

        public void ShowRewarded()
        {
            VideoAd.Show(() => {});
        }

        public void ShowInterstitial()
        {
            InterstitialAd.Show();
        }
    }
}