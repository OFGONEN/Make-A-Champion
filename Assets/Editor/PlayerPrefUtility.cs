using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace FFEditor
{
    [CreateAssetMenu(fileName = "PlayerPrefUtility", menuName = "FF/Editor/PlayerPrefUtil")]
    public class PlayerPrefUtility : ScriptableObject
    {
        public string valueName;
        public int intValue;

        [Button("Set Int Value")]
        public void SetIntValue()
        {
            PlayerPrefs.SetInt(valueName, intValue);
        }
        [MenuItem("FFStudios/Delete PlayerPrefs"), Button("Reset Player Prefs")]
        public static void ResetPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}