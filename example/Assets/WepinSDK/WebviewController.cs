using UnityEngine;
using WepinSDK.Wepin;
using System.Runtime.InteropServices;
using System;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;


public class WebviewController : MonoBehaviour
{
    public WebViewObject _webViewObject = null;
    private Wepin _wepinInstance = null;
   
    private string _lastLoadedUrl = "";

    void Start()
    {
        initWebview();
    }

    private void initWebview(){
        _webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        _webViewObject.Init(
                    cb: requestFromWebview,
                    err: (msg) => { Debug.Log($"WebView Error : {msg}"); },
                    httpErr: (msg) => { Debug.Log($"WebView HttpError : {msg}"); },
                    started: (msg) => { Debug.Log($"WebView started"); },
                    hooked: (msg) => { Debug.Log($"WebView Hooked : {msg}"); },
                    ld: (msg) => { Debug.Log($"WebView loaded"); },
                    transparent:true
                );
        //웹뷰 크기 및 위치 설정
        _webViewObject.SetMargins(0, 0, 0, 0);
    }

    public void loadWebview(Wepin instance, string loadUrl)
    {
        this._wepinInstance = instance;
        if (_webViewObject == null)
        {
            initWebview();
        }

        loadUrl += "?from=unitySDK";
        _lastLoadedUrl = loadUrl;
        _webViewObject.LoadURL(loadUrl);
        _webViewObject.SetVisibility(true);
    }


    // 웹뷰 요청 처리
    void requestFromWebview(string request)
    {
        WebviewRequestHandler.HandleRequest(this._wepinInstance, request, this);
    }

    public void sendResponseToWebview(string response)
    {
        if (_webViewObject == null)
        {
            Debug.LogError("webviewObject is null");
            return;
        }
        // 웹뷰에 결과값 전달
        _webViewObject.EvaluateJS($"onUnityResponse('{response}')");
    }

    public void destroyWebviewObject(){
        if( _webViewObject != null ){
            Destroy(_webViewObject.gameObject);
        }
    }

    public async Task openWepinWidget(){
        if(_webViewObject != null ){
            _webViewObject.LoadURL(_lastLoadedUrl);
            await Task.Delay(1000); // add delay 
            _webViewObject.SetVisibility(true);
        }
    }

    public void closeWepinWidget(){
        if(_webViewObject != null ){
            _webViewObject.SetVisibility(false);
        }   
    }

    public void sendNativeEvent(NativeEventMessage.RequestData request)
    {
        string reuqestMsg = JsonConvert.SerializeObject(request);
        //Debug.Log("sendNativeEvent Message : " + reuqestMsg);
        _webViewObject.EvaluateJS($"onUnityEvent('{reuqestMsg}')");
    }

    // Not Used
    void returnToApp()
    {
        // 현재 씬의 빌드 인덱스 가져오기
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // 이전 씬의 빌드 인덱스 계산
        int previousSceneIndex = Mathf.Max(0, currentSceneIndex - 1);
        // 이전 씬으로 전환
        SceneManager.LoadScene(previousSceneIndex);
    }

#if UNITY_IOS
        [DllImport("__Internal")]
        extern static void launch_inapp_browser(string url);

       [DllImport("__Internal")]
        extern static void close_inapp_browser();

#endif
    public void OpenUrlInAppBrowser(string url)
    {
        //Debug.Log("OpenUrlInAppBrowser_url : " + url);
#if UNITY_EDITOR || UNITY_STANDALONE
        Application.OpenURL(url);
#elif UNITY_ANDROID
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        using (var browserView = new AndroidJavaObject("com.unity3d.player.InAppBrowserView"))
        {
            browserView.CallStatic("launchUrl", activity, url);
        }

#elif UNITY_IOS
        var uri = new Uri(url);
        launch_inapp_browser(url);
#endif
    }

#if UNITY_IOS
    public void CloseInAppBrowser()
    {
        close_inapp_browser();
    }
#endif
}
