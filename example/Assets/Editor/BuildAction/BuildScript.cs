using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEditor.iOS.Xcode;
using System.Diagnostics;

namespace BuildAction
{
    public static class BuildScript
    {
        private const string APP_NAME = "wepinUnitySample";
        private const string KEYSTORE_PASSWORD = "123456";
        private const string KEYALIAS_PASSWORD = "unity_debug";
        private const string BUILD_BASIC_PATH = "./build/"; // root 폴더 기준으로 상대경로
        private const string BUILD_ANDROID_PATH = BUILD_BASIC_PATH + "Android/";
        private const string BUILD_IOS_PATH = BUILD_BASIC_PATH + "Ios/";


        [MenuItem("Builder/Android/Build")] // Unity Menu Bar Item 
        public static void BuildAndroid()
        {
            DoBuildAndRunAndroid(true);
        }

        [MenuItem("Builder/Android/BuildAndRun")] // Unity Menu Bar Item 

        public static void BuildAndRunAndroid()
        {
            DoBuildAndRunAndroid(false);
        }

        private static string[] GetBuildSceneList()
        {
            EditorBuildSettingsScene[] scenes = UnityEditor.EditorBuildSettings.scenes;

            List<string> listScenePath = new List<string>();

            for (int i = 0; i< scenes.Length; i++)
            {
                if(scenes[i].enabled)
                listScenePath.Add(scenes[i].path);
            }

            return listScenePath.ToArray();
        }

        private static string SetPlayerSettingForAndroid()
        {
            PlayerSettings.Android.keystorePass = KEYSTORE_PASSWORD;
            PlayerSettings.Android.keyaliasPass = KEYALIAS_PASSWORD;
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;

            string fileName = string.Format("{0}_{1}.apk", APP_NAME, PlayerSettings.bundleVersion);
            return fileName;

        }

        
        public static void DoBuildAndRunAndroid(bool onlyBuild)
        {
            string apkName = SetPlayerSettingForAndroid();

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.locationPathName = BUILD_ANDROID_PATH + apkName;
            buildPlayerOptions.scenes = GetBuildSceneList();
            buildPlayerOptions.target = BuildTarget.Android;
            if(onlyBuild)
            {
                buildPlayerOptions.options = BuildOptions.None; // Only build
            }else{
                buildPlayerOptions.options = BuildOptions.AutoRunPlayer; // Build And Run
            }
            // Android 빌드 시작
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions); 
            // 빌드 결과 확인
            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Succeeded)
            {
                UnityEngine.Debug.Log("Android Build succeeded");
            }
            else
            {
                UnityEngine.Debug.LogError("Android Build failed");
            }
        }   
    }
}