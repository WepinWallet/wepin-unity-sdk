using UnityEngine;
using System;
using WepinSDK.Types;
using UnityEngine.SceneManagement;

namespace WepinSDK.Wepin
{

    public class Wepin: MonoBehaviour {
        static Wepin _instance;
        public string _appKey;
        public string _appId;
        public Attributes _attribute = null;
        public AccountList _accountList = null;
        private string _deepLinkUrl;

        private bool _isInitialized = false;

        private WebviewController _webviewController;

        void Awake() {
            if (_instance == null) {
                _instance = this;
            } else if (_instance != this) {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
            SetupSDK();
        }

        void Update()
        {

        }

        public static Wepin Instance {
            get {
                if (_instance == null) {
                    GameObject go = new GameObject("Wepin");
                    _instance = go.AddComponent<Wepin>();
                }
                return _instance;
            }
        }

        void SetupSDK() {
            try{
                _webviewController = (new GameObject("WebviewController")).AddComponent<WebviewController>();
                
                // Platform Check
                if ( Application.platform == RuntimePlatform.IPhonePlayer ){
                    Debug.Log("Platform Is iOS");
                }else if( Application.platform == RuntimePlatform.Android ){
                    Debug.Log("Platform Is Android");
                }else{
                    Debug.Log("Not Supported Platform : " + Application.platform);
                }
                // 딥링크 처리             
                Application.deepLinkActivated += this.onDeepLinkActivated;
                if (!string.IsNullOrEmpty(Application.absoluteURL)){
                    this.onDeepLinkActivated(Application.absoluteURL);
                }
            }catch(Exception e){
                throw new WepinException(e.Message);
            }

        }

        public void Initialize(string appId, string appKey, Attributes attributes) {
            if( this._isInitialized ){
                throw new WepinException("Wepin is already initialized");
            }
            try{
                Debug.Log("Wepin.Initialize appId : " + appId);
                Debug.Log("Wepin.Initialize appKey : " + appKey);
                Debug.Log("Wepin.Initialize language : " + attributes.defaultLanguage);        
                Debug.Log("Wepin.Initialize currency : " + attributes.defaultCurrency);        
                _appId = appId;
                _appKey = appKey;
                _attribute = attributes;
                this.startWepinWidget(true, null);
            }catch(Exception e){
                Debug.LogError("Exception : " + e.Message);
                throw new WepinException(e.Message);
            }
        }

        public bool IsInitialized(){
            return this._isInitialized;
        }
        public async void OpenWidget(){
            Debug.Log("Wepin.OpenWidget");
            if( !this._isInitialized ){
                throw new WepinException("need to initialize wepin");
            }
            try{
                await _webviewController.openWepinWidget();
            }catch(Exception e){
                throw new WepinException(e.Message);
            }
        }   
        public void CloseWidget(){
            Debug.Log("Wepin.CloseWidget");
            if( !this._isInitialized ){
                throw new WepinException("need to initialize wepin");
            }
            try{
                _webviewController.closeWepinWidget();
            }catch(Exception e){
                throw new WepinException(e.Message);
            }
        }   
        public AccountList GetAccounts(){
            Debug.Log("Wepin.GetAccounts");
            if( !this._isInitialized ){
                throw new WepinException("need to initialize wepin");
            }

            if( _accountList == null  || _accountList.accounts.Count == 0 ){   
                OpenWidget();
                return null;
            }

            try{
                foreach (var account in _accountList.accounts)
                {
                    Debug.Log($"Network: {account.network}, Address: {account.address}");
                }
            }catch(Exception e){
                throw new WepinException(e.Message);
            }

            return this._accountList;
        }   

        public void Finalize(){
            Debug.Log("Wepin.Finalize");
            try{
                this._accountList = null;
                this._attribute = null;
                this._webviewController.destroyWebviewObject();
                this._isInitialized = false;
            }catch(Exception e){
                throw new WepinException(e.Message);
            }
        } 

        // Use internal    
        private void startWepinWidget(bool isInit, string optionUrl){
            string baseUrl = Utils.getUrlFromAppkey(_appKey);
            if( !String.IsNullOrEmpty(optionUrl) ){
                baseUrl += optionUrl;
            }
            _webviewController.loadWebview(_instance, baseUrl);
            this._isInitialized = true;
        }
        public void SetAccounts(AccountList list){
            this._accountList = list;
        } 

        public void onDeepLinkActivated(string url)
        {
            this._deepLinkUrl = Utils.getApplicationUniqueId() + ".wepin://";
            if (url.Contains("?token="))
            {
                string idToken = url.Substring(_deepLinkUrl.Length + "?token=".Length);
                if (!string.IsNullOrEmpty(idToken))
                {
                // 구글로그인 후 웹뷰에서 idToken을 같이 넘겨주면 웹뷰에 setToken 이벤트를 요청해서 token을 넘겨준다
                #if UNITY_IOS
                    _webviewController.CloseInAppBrowser();
                #endif
                    _webviewController.sendNativeEvent(new NativeEventMessage.RequestData
                    {
                        header = new NativeEventMessage.RequestHeader
                        {
                            request_from = "unity",
                            request_to = "wepin_widget",
                            id = DateTime.Now.Ticks, // default
                        },
                        body = new NativeEventMessage.RequestBody
                        {
                            command = "set_token",
                            parameter = new NativeEventMessage.SetTokenParameter
                            {
                                token = idToken,
                            },
                        },
                    });
                }
            }else if(url.Equals(this._deepLinkUrl)){
                // 현재 이케이스는 위핀 지갑에서 위젯으로 돌아오는 경우만 있음
                Debug.Log("Return From WepinWallet");
            #if UNITY_IOS                
                _webviewController.CloseInAppBrowser();
            #endif
            }else{
                Debug.LogError("Invalid DeepLink Url");
                return;
            }
        }
    }
}
