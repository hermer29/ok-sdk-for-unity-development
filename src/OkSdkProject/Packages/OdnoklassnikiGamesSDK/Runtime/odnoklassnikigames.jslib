const library = {
    
    $odnoklassnikiGames: {
        isInitialized: false,

        sdk: undefined,

        isInitializeCalled: false,
        
        callbackListener: undefined,

        odnoklassnikiGamesSdkInitialize: function (successCallbackPtr) {
            console.log("Initialization started odnoklassnikiGames.isInitializeCalled: " + odnoklassnikiGames.isInitializeCalled);
            if (odnoklassnikiGames.isInitializeCalled) {
                return;
            }
            odnoklassnikiGames.isInitializeCalled = true;

            const sdkScript = document.createElement('script');
            sdkScript.src = '//api.ok.ru/js/fapi5.js';
            document.head.appendChild(sdkScript);

            sdkScript.onload = function () {
                console.log("SDK script loaded started");
                odnoklassnikiGames.sdk = window['FAPI'];
                var rParams = odnoklassnikiGames.sdk.Util.getRequestParameters();
                odnoklassnikiGames.sdk.init(rParams["api_server"], rParams["apiconnection"], function () {
                    odnoklassnikiGames.isInitialized = true;
                    window.API_callback = function(method, result, data) {
                        odnoklassnikiGames.callbackListener(result, data);
                    }
                    dynCall('v', successCallbackPtr, []);
                }, function () {
                    console.log("While initializing sdk error occured");    
                });
            };
        },

        throwIfSdkNotInitialized: function () {
            if (!odnoklassnikiGames.isInitialized) {
                throw new Error('SDK is not initialized. Invoke OdnoklassnikiGamesSdk.Initialize() coroutine and wait for it to finish.');
            }
        },
        
        loadAd: function (successCallbackPtr, errorCallbackPtr) {
            odnoklassnikiGames.callbackListener = function (result, data) {
                if(result === "ok") {
                    dynCall('v', successCallbackPtr, []);
                } else if(result === "error") {
                    var errorStringPtr = odnoklassnikiGames.allocateUnmanagedString(data);
                    dynCall('vi', errorCallbackPtr, [errorStringPtr])
                }
            }
            odnoklassnikiGames.sdk.UI.loadAd();
        },
        
        showLoadedAd: function (successCallbackPtr, errorCallbackPtr) {
            odnoklassnikiGames.callbackListener = function (result, data) {
                if(result === "ok") {
                    dynCall('v', successCallbackPtr, []);
                } else if (result === "error") {
                    var errorStringPtr = odnoklassnikiGames.allocateUnmanagedString(data);
                    dynCall('vi', errorCallbackPtr, [errorStringPtr]);
                }
            }
            odnoklassnikiGames.sdk.UI.showLoadedAd();
        },
        
        showAd: function (foundCallbackPtr, endedCallbackPtr, errorCallbackPtr) {
            odnoklassnikiGames.callbackListener = function (result, data) {
                if(result === "ok") {
                    if(data === "ready") {
                        dynCall('v', readyCallbackPtr, []);
                    } else if (data === "ad_shown") {
                        dynCall('v', endedCallbackPtr, []);
                    }
                } else if (result === "error") {
                    var errorStringPtr = odnoklassnikiGames.allocateUnmanagedString(data);
                    dynCall('vi', errorCallbackPtr, [errorStringPtr]);
                }
            }
            odnoklassnikiGames.sdk.UI.showAd();
        },

        allocateUnmanagedString: function (string) {
            const stringBufferSize = lengthBytesUTF8(string) + 1;
            const stringBufferPtr = _malloc(stringBufferSize);
            stringToUTF8(string, stringBufferPtr, stringBufferSize);
            return stringBufferPtr;
        },
    },

    // External C# calls.

    OdnoklassnikiGamesSdkInitialize: function (successCallbackPtr) {
        console.log("OdnoklassnikiGamesSdkInitialize in index.html called");
        odnoklassnikiGames.odnoklassnikiGamesSdkInitialize(successCallbackPtr);
    },

    GetOdnoklassnikiGamesSdkIsInitialized: function () {
        return odnoklassnikiGames.isInitialized;
    },

    VideoAdLoad: function (successCallbackPtr, errorCallbackPtr) {
        odnoklassnikiGames.throwIfSdkNotInitialized();
        
        odnoklassnikiGames.loadAd(successCallbackPtr, errorCallbackPtr);
    },
    
    VideoAdShow: function (rewardedCallbackPtr, errorCallbackPtr) {
        odnoklassnikiGames.throwIfSdkNotInitialized();

        odnoklassnikiGames.showLoadedAd(rewardedCallbackPtr, errorCallbackPtr);
    },
    
    InterstitialAdShow: function (foundCallbackPtr, endedCallbackPtr, errorCallbackPtr) {
        odnoklassnikiGames.throwIfSdkNotInitialized();

        odnoklassnikiGames.showAd(foundCallbackPtr, endedCallbackPtr, errorCallbackPtr);
    }
}

autoAddDeps(library, '$odnoklassnikiGames');
mergeInto(LibraryManager.library, library);