using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Collections;
using System;

public class IosPostBuildProcess
{
    // Runs all the post process build steps. Called from Unity during build
    [PostProcessBuildAttribute(0)] // 릴리즈시는 주석처리할것!!
    public static void OnPostBuildProcess(BuildTarget target, string pathToBuiltProject)
    {
#if UNITY_IOS
        var infoPlist = new UnityEditor.iOS.Xcode.PlistDocument();
        var infoPlistPath = pathToBuiltProject + "/Info.plist";
        infoPlist.ReadFromFile(infoPlistPath);

        // Bundle Identifier 설정
        infoPlist.root.SetString("CFBundleIdentifier", "io.wepin.rn.sample");
        var urlTypeDict = infoPlist.root.CreateArray("CFBundleURLTypes").AddDict();
        var urlSchemes = urlTypeDict.CreateArray("CFBundleURLSchemes");
        urlSchemes.AddString("io.wepin.rn.sample.wepin");
        // Info.plist 파일 저장
        infoPlist.WriteToFile(infoPlistPath);
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}