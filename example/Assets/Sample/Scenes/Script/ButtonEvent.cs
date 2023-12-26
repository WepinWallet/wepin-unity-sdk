
using UnityEngine;
using UnityEngine.UI;
using WepinSDK.Wepin;
using WepinSDK.Types;
using Newtonsoft.Json;
using System.Text;

public class ButtonEvent : MonoBehaviour
{
    public Button btn1, btn2, btn3, btn4, btn5, btn6;
        
    private Wepin _wepin;

    //Wepin _wepin;
    // Start is called before the first frame update
    void Start()
    {
        btn1.onClick.AddListener(Initialize);
        btn2.onClick.AddListener(IsInitialize);
        btn3.onClick.AddListener(OpenWidget);
        btn4.onClick.AddListener(CloseWidget);
        btn5.onClick.AddListener(GetAccounts);
        btn6.onClick.AddListener(Finalize);

        _wepin = Wepin.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Initialize() {
        Debug.Log("Clicked Initialize");
        if( _wepin == null ){
            Debug.LogError("Wepin is null");
            return;
        }

        _wepin.Initialize("test_appId", "test_AppKey", new Attributes()
        {
            defaultLanguage = "ko",
            defaultCurrency = "krw"
        });
    }

    void IsInitialize() {
        Debug.Log("Clicked IsInitialize");
        bool result;
        if( _wepin == null ){
            Debug.LogError("Wepin is null");
            return;
        }
        result = _wepin.IsInitialized();
        Debug.Log("_wepin.IsInitialized : " + result);
    }

    void OpenWidget() {
        Debug.Log("Clicked OpenWidget");
        if( _wepin == null ){
            Debug.LogError("Wepin is null");
            return;
        }
        if( !_wepin.IsInitialized() ){
            Debug.LogError("Wepin is not initialized");
            return;
        }
        _wepin.OpenWidget();
    }

    void CloseWidget() {
        Debug.Log("Clicked CloseWidget");
        if( _wepin == null ){
            Debug.LogError("Wepin is null");
            return;
        }
        if( !_wepin.IsInitialized() ){
            Debug.LogError("Wepin is not initialized");
            return;
        }
        _wepin.CloseWidget();
    }
    void GetAccounts() {
        Debug.Log("Clicked GetAccounts");
        if( _wepin == null ){
            Debug.LogError("Wepin is null");
            return;
        }
        if( !_wepin.IsInitialized() ){
            Debug.LogError("Wepin is not initialized");
            return;
        }
        AccountList accountList = _wepin.GetAccounts();
        Debug.Log("accountList : " + JsonConvert.SerializeObject(accountList));
    }
    void Finalize() {
        Debug.Log("Clicked Finalize");
        if( _wepin == null ){
            Debug.LogError("Wepin is null");
            return;
        }
        _wepin.Finalize();
    }
}
