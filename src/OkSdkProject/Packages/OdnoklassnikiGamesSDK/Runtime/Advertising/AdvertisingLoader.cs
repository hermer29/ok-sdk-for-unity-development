using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AOT;
using UnityEngine;
using UnityEngine.Scripting;

namespace OdnoklassnikiGamesSDK.Advertising
{
   internal static class AdvertisingLoader
   {
      internal static bool s_advertisingLoaded = false;
      internal static bool s_advertisingLoadRequested = false;
      
      internal static async void NotifyInitializationCompleted()
      {
         while (true)
         {
            await Task.Delay(2000);
            if (s_advertisingLoaded == false && s_advertisingLoadRequested == false)
            {
               s_advertisingLoadRequested = true;
               VideoAd.Load(() =>
               {
                  s_advertisingLoaded = true;
                  s_advertisingLoadRequested = false;
               }, error =>
               {
                  s_advertisingLoadRequested = false;
               });
            }
         }
      }
   }
}