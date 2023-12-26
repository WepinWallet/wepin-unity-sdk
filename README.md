# wepin-unity-sdk

<br />

Wepin Unity SDK for Android OS and iOS

## ⏩ Get App ID and Key

Contact to wepin.contact@iotrust.kr

## ⏩ Install

### wepin-unity-sdk

Download <wepinSDK_vX.X.X.unitypackage> file from our [github](https://github.com/WepinWallet/wepin-unity-sdk.git) and import the package file into your Unity3D project.

If you encounter errors below when importing this package, 

```xml
The type or namespace name 'Newtonsoft' could not be found (are you missing a using directive or an assembly reference?)
```

Please add dependency "com.unity.nuget.newtonsoft-json": "3.0.2" Packages/manifest.json file in your project's root

## ⏩ Config Deep Link

Deep link scheme format: Your app package name or bundle id + '.wepin'

### For Android

Add the below line in your app's `AndroidMainfest.xml` file

```xml
  <activity android:name="com.unity3d.player.UnityPlayerActivity"
    ....
  >
      <intent-filter>
        <action android:name="android.intent.action.VIEW" />
        <category android:name="android.intent.category.DEFAULT" />
        <category android:name="android.intent.category.BROWSABLE" />
        <data android:scheme="PACKAGE_NAME.wepin"/> <!-- package name of your android app + '.wepin' -->
      </intent-filter>            
    ....
  </activity>
```

### For iOS

Add the URL scheme as below:

1. Open your iOS project with the xcode
2. Click on Project Navigator
3. Select Target Project in Targets
4. Select Info Tab
5. Click the '+' buttons on URL Types
6. Enter Identifier and URL Schemes
   - Idenetifier: bundle id of your project
   - URL Schems: bundle id of your project + '.wepin'

![unity-ios-setup-image](https://github.com/IotrustGitHub/wepin-unity-sdk/assets/43332708/ba1ddc58-1b51-4253-b5e7-22741717fa9d)


## ⏩ Create Wepin Instance

Add name space of Wepin & Types

```c#
using WepinSDK.Wepin;
using WepinSDK.Types;
```

And create an Wepin instance within your Start() function

```c#
Wepin _wepin = Wepin.Instance;
```

## ⏩ Initialize

Methods about initializing Wepin SDK. If success, Wepin widget will show login page.

### init

```c#
_wepin.Initialize(appId, appSdkKey, Attributes);
```
#### Parameters

- `appId` \<string>
- `appKey` \<string>
- `Attributes` \<WepinSDK.Types.Attributes>
  - defaultLanguage: The language to be displayed on the widget (default: 'ko')
    - Currently, only 'ko' and 'en' are supported.
  - defaultCurrency: The currency to be displayed on the widget (default: 'KRW')

#### Example

```c#
_wepin.Initialize("test_appId", testAppKey, new Attributes()
{
    defaultLanguage = "ko",
    defaultCurrency = "krw"
});
```
## ⏩ Methods

Methods can be used after initialization of Wepin SDK.

### isInitialized

```c#
_wepin.isInitialized();
```

The `isInitialized()` method checks Wepin SDK is initialized.

#### Return value

- \<bool>
  - `true` if Wepin SDK is already initialized.


### openWidget

```c#
_wepin.OpenWidget();
```

The `openWidget()` method shows Wepin widget. 

#### Return value

- \<void>

### closeWidget

```c#
_wepin.CloseWidget();
```

The `closeWidget()` method closes Wepin widget.

#### Return value

- \<void>

### getAccounts

```c#
_wepin.GetAccounts();
```

The `getAccounts()` method returns user accounts. If user is not logged in, Wepin widget will be opened and show login page. It is recommended to use `getAccounts()` method without argument to get all user accounts.

#### Example

```c#
AccountList accountList = _wepin.GetAccounts();
```

#### Return value

- <WepinSDK.Types.AccountList>
  - If user is logged in, it returns list of `account`
    - Type of `account` is WepinTypes.Accounts
    - `account` \<WepinSDK.Types.Accounts>
      - `address` \<string>
      - `network` \<string>

  - Example
    ```c#
    [
      {
        address: '0x0000001111112222223333334444445555556666',
        network: 'Ethereum',
      },
    ]
    ```
- `null or []`
  - If user is not logged in, it returns null or []

### finalize

```c#
_wepin.Finalize();
```

The `Finalize()` method finalize Wepin widget.

#### Return value

- \<void>
