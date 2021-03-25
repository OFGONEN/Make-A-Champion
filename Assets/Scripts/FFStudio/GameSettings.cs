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
		[ Foldout( "Cleaning GFX System" ), MinMaxSlider( 0.1f, 10.0f ) ] 
        public Vector2 bubbleSpreadForceMultiplier = new Vector2( 3.0f, 3.0f );
		[ Foldout( "Cleaning GFX System" ), Range( 0.1f, 3.0f ) ] public float bubbleDestroyTweenDuration = 1.0f;

		public float cameraTravelDuration;

		public float packingItemSearchDistance;
		public float packingWeightSearchDistance;
		[Foldout( "Weight Loop" )] public float weightLoopFailTime;
		[Foldout( "Weight Loop" )] public float weightLoopGreenValue = 0.3f;
		[Foldout( "Weight Loop" )] public float weightLoopYellowValue = 0.2f;
		[Foldout( "Weight Loop" )] public float weightLoopRedValue = 0.2f;
		[Foldout( "Weight Loop" )] public int weightLoopComplete;
		public float shakerSpeed;
	}
}
