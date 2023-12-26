using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;
using WepinSDK.Wepin;
using WepinSDK.Types;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;


public class WebviewRequestHandler
{
    public static void HandleRequest(Wepin wepinInstance, string requestStr, WebviewController webviewController)
    {
        string responseStr = "";
        string requestCmd = "";
        string requestParam = "";
        ResponseBody responseBody;

        JObject jsonObject = JObject.Parse(requestStr);
        JToken jTokenHeader = jsonObject["header"];
        JToken jTokenBody = jsonObject["body"];
        ResponseHeader responseHeader = new ResponseHeader(jTokenHeader["id"]?.ToString(), jTokenHeader["request_to"]?.ToString(), jTokenHeader["request_from"]?.ToString());
        requestCmd = JObject.Parse(jTokenBody.ToString())["command"].ToString();
        requestParam = JObject.Parse(jTokenBody.ToString())["parameter"]?.ToString();
        switch (requestCmd)
        {
            case "ready_to_widget":
                int platform = 1; //web
                string domain = Utils.getApplicationUniqueId();
                // android : 2 / ios : 3 
                if ( Application.platform == RuntimePlatform.Android ){
                    platform = 2;
                }else if( Application.platform == RuntimePlatform.IPhonePlayer ){
                    platform = 3;
                }
                string appVersion = "0.0.1";
                ResponseReadyToWidget responseReadyToWidget = new ResponseReadyToWidget(Wepin.Instance._appKey, Wepin.Instance._attribute, domain, platform, appVersion);
                responseBody = new ResponseBody(requestCmd, "SUCCESS", responseReadyToWidget);
                responseStr = new WebviewResponse(responseHeader, responseBody).ToJson();
                break;

            case "initialized_widget":
                responseStr = getGeneralResponse(responseHeader, requestCmd, true);
                break;

            case "set_accounts":
                AccountList accountList = JsonUtility.FromJson<AccountList>(requestParam);
                wepinInstance.SetAccounts(accountList);
                responseStr = getGeneralResponse(responseHeader, requestCmd, true);
                break;

            case "close_wepin_widget":
                responseStr = getGeneralResponse(responseHeader, requestCmd, true);
                webviewController.sendResponseToWebview(responseStr);
                webviewController.closeWepinWidget();
                return;

            case "dequeue_request":
                break;

            case "set_user_info":
                break;

            case "wepin_logout":
                break;

            case "set_user_email":
                break;

            case "window_open":
                string openUrl = requestParam;
                webviewController.OpenUrlInAppBrowser(openUrl);
                // No response to webview
                break;
            default:
                throw new Exception($"Command {requestCmd} is not supported.");
        }

        if( !string.IsNullOrEmpty(responseStr) ){
            webviewController.sendResponseToWebview(responseStr);
        }
    }

    private static string getGeneralResponse(ResponseHeader header, string cmd, bool success){
        string resultStr = "SUCCESS";
        if(!success){
            resultStr = "ERROR";
        }
        ResponseBody body = new ResponseBody(cmd, resultStr, null);
        string response = new WebviewResponse(header, body).ToJson();
        return response;
    }
}
