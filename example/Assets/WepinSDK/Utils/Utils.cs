using UnityEditor;
using UnityEngine;

public static class Utils {
    
    public static string getUrlFromAppkey(string appKey){
        
        string wepinUrl = "";

        if( appKey.StartsWith("ak_dev_") ){
            wepinUrl = "https://dev-widget.wepin.io";
        }else if( appKey.StartsWith("ak_test_") ){
            wepinUrl = "https://stage-widget.wepin.io";
        }else if( appKey.StartsWith("ak_live_") ){
            wepinUrl = "https://widget.wepin.io";
        }else{
            Debug.LogError("Invalid AppKey");
        }
        return wepinUrl;
    }

    public static string getApplicationUniqueId()
    {
        string appUniqueId = Application.identifier;
        Debug.Log("getApplicationUniqueId : " + appUniqueId);
        return appUniqueId;
    }

}
