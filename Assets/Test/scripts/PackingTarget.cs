using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class PackingTarget : MonoBehaviour
{
	#region Name
	public SharedReferance targetReferance;
	public PackingTargetSet targetSet;
	public MeshRenderer mesh;

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
		block = new MaterialPropertyBlock();
		colorID = Shader.PropertyToID( "_Color" );
		baseColor = mesh.material.color;


		targetReferance.sharedValue = this;
	}
	#endregion

	#region API
	public void Select( Color color )
	{
		block.SetColor( colorID, color );
		mesh.SetPropertyBlock( block );
	}
	[Button]
	public void DeSelect()
	{
		block.SetColor( colorID, baseColor );
		mesh.SetPropertyBlock( block );
	}

	#endregion
}
