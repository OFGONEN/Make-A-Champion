using UnityEngine;
using NaughtyAttributes;

namespace FFStudio
{
	[CreateAssetMenu( fileName = "GameSettings", menuName = "FF/Data/GameSettings" )]
    public class GameSettings : ScriptableObject
    {
        public int maxLevelCount;
		[Foldout( "UI Settings" )] public float uiEntityScaleTweenDuration;
		[Foldout( "UI Settings" )] public float uiEntityMoveTweenDuration;
		[Foldout( "UI Settings" )] public float uiFloatingEntityMoveTweenDuration;
		[Foldout( "UI Settings" )] public Vector3 uiModelViewSpinAxis;
		[Foldout( "UI Settings" )] public float uiModelViewSpinSpeed;
        [Tooltip("Percentage of the screen to register a swipe")]
        public int swipeThreshold;

		[ Tooltip( "How much health will be depleted per second ? (Total starting health is always 100)" ) ]
		[ Foldout( "Cleaning GFX System" ) ] public float dirtDepletionRate = 20.0f;
	}
}
