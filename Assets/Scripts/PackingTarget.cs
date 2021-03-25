using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class PackingTarget : MonoBehaviour
{
	#region Fields
	[Header( "Shared Variables" )]
	public SharedReferance targetReferance;
	public PackingTargetSet targetSet;

	public Vector3 targetRotation;


	// Private Fields
	private MeshRenderer meshRenderer;
	private MaterialPropertyBlock block;
	private int colorID;
	private Color baseColor;
	#endregion

	#region UnityAPI
	private void OnEnable()
	{
		targetSet.AddDictionary( GetInstanceID(), this );
	}
	private void OnDisable()
	{
		targetSet.RemoveDictionary( GetInstanceID() );
	}
	private void Awake()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		block = new MaterialPropertyBlock();
		colorID = Shader.PropertyToID( "_Color" );
		baseColor = meshRenderer.material.color;

		targetReferance.sharedValue = this;
	}
	#endregion

	#region API
	public void Select( Color color )
	{
		block.SetColor( colorID, color );
		meshRenderer.SetPropertyBlock( block );
	}
	[Button]
	public void DeSelect()
	{
		block.SetColor( colorID, baseColor );
		meshRenderer.SetPropertyBlock( block );
	}

	#endregion
}
