/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class LiquidFiller : MonoBehaviour
{
#region Fields
	[ Header( "Event Listeners" ) ]
    public EventListenerDelegateResponse liquidFillEventListener;

	[ Header( "Observed Shared Data" ) ]
    public SharedFloatProperty liquidFillPercentage;
    
    [ Header( "Liquid Fill Parameters" ) ]
    [ MinMaxSlider( -3, +3 ) ] public Vector2 limits;
    
    private MeshRenderer meshRenderer;
	private MaterialPropertyBlock materialPropertyBlock;
    private int fillAmountShaderID = Shader.PropertyToID( "_FillAmount" );
    
    private float CurrentFillAmount => limits.x + ( limits.y - limits.x ) * ( liquidFillPercentage.Value / 100.0f );
#endregion

#region Unity API
	private void OnEnable()
    {
		liquidFillEventListener.OnEnable();
	}
    
    private void OnDisable()
    {
		liquidFillEventListener.OnDisable();
	}
    
    private void Awake()
    {
		liquidFillEventListener.response = OnLiquidFilled;
	}

	private void Start()
    {
		meshRenderer               = GetComponent< MeshRenderer >();
		materialPropertyBlock      = new MaterialPropertyBlock();
		liquidFillPercentage.Value = 0.0f;
		OnLiquidFilled();
	}
#endregion

#region API
#endregion

#region Implementation
	private void OnLiquidFilled()
	{
		// FFLogger.Log( name + ": liquidFillPercentage: " + liquidFillPercentage.Value + ", CurrentFillAmount: " + CurrentFillAmount );
		meshRenderer.GetPropertyBlock( materialPropertyBlock );
		materialPropertyBlock.SetFloat( fillAmountShaderID, CurrentFillAmount );
		meshRenderer.SetPropertyBlock( materialPropertyBlock );
	}
#endregion
}
