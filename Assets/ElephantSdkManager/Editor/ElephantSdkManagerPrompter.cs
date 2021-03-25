using System.Reflection;
using ElephantSdkManager.Model;
using ElephantSdkManager.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace ElephantSdkManager
{
    [InitializeOnLoad]
    public class ElephantSdkManagerPrompter
    {
        private const string KeyVersion = "key_version";

        static ElephantSdkManagerPrompter()
        {
            FetchManifestStatus();
        }

        private static void FetchManifestStatus()
        {
            var request = UnityWebRequest.Get(ManifestSource.ManifestURL);
            request.SendWebRequest();
            while (!request.isDone && !request.isHttpError && !request.isNetworkError)
            {
                // no-op
            }

            if (request.isHttpError || request.isNetworkError || !string.IsNullOrWhiteSpace(request.error))
            {
                Debug.LogError("Couldn't finish opening request!");
                return;
            }

            var responseJson = request.downloadHandler.text;
            request.Dispose();
            HandleResponse(responseJson);
        }

        private static void HandleResponse(string responseJson)
        {
            var manifest = JsonUtility.FromJson<Manifest>(responseJson);
            Sdk elephantSdk = manifest.sdkList.Find(sdk => sdk.sdkName.Equals("Elephant"));
            var adsSDK = manifest.sdkList.Find(sdk => sdk.sdkName.Equals("RollicGames"));

            Assembly myAssembly = Assembly.GetExecutingAssembly();
            foreach (var type in myAssembly.GetTypes())
            {
                if (type.FullName == null) return;

                if (type.FullName.Equals("ElephantSDK.ElephantVersion"))
                {
                    FieldInfo fieldInfo = type.GetField("SDK_VERSION",
                        BindingFlags.NonPublic | BindingFlags.Static);
                    if (!(fieldInfo is null)) elephantSdk.currentVersion = fieldInfo.GetValue(null).ToString();
                }

                if (type.FullName.Equals("RollicGames.Advertisements.AdsSdkVersion"))
                {
                    var fieldInfo = type.GetField("SDK_VERSION",
                        BindingFlags.NonPublic | BindingFlags.Static);

                    if (!(fieldInfo is null)) adsSDK.currentVersion = fieldInfo.GetValue(null).ToString();
                }
            }

            if (IsReadyToShow(elephantSdk) || IsReadyToShow(adsSDK))
            {
                if (elephantSdk != null) PlayerPrefs.SetString(KeyVersion + elephantSdk.sdkName, elephantSdk.version);
                if (adsSDK != null) PlayerPrefs.SetString(KeyVersion + adsSDK.sdkName, adsSDK.version);
                ElephantSdkManager.ShowSdkManager();
            }
        }

        private static bool IsReadyToShow(Sdk sdk)
        {
            if (sdk == null)
            {
                return false;
            }
            
            var currentVersion = sdk.currentVersion;
            var latestVersion = sdk.version;

            return !string.IsNullOrEmpty(latestVersion) &&
                   (!string.IsNullOrEmpty(currentVersion) ||
                    VersionUtils.CompareVersions(currentVersion, latestVersion) < 0) &&
                   !HasShown(sdk);
        }

        private static bool HasShown(Sdk sdk)
        {
            return sdk.version.Equals(PlayerPrefs.GetString(KeyVersion + sdk.sdkName, ""));
        }
    }
}