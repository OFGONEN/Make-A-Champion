using UnityEngine;
using NaughtyAttributes;

namespace FFStudio
{
    public class GameSettings : ScriptableObject
    {
        public int maxLevelCount;
		[Foldout( "UI Settings" )] public float uiEntityScaleTweenDuration;
		[Foldout( "UI Settings" )] public float uiEntityMoveTweenDuration;
		[Foldout( "UI Settings" )] public float uiFloatingEntityMoveTweenDuration;
        [Tooltip("Percentage of the screen to register a swipe")]
        public int swipeThreshold;
    }
}
