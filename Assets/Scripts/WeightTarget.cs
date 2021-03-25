/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using FFStudio;
using NaughtyAttributes;
using UnityEngine;

public class WeightTarget : MonoBehaviour
{
	private MeshRenderer meshRenderer;
	private MaterialPropertyBlock block;
	private int colorID;
	private Color baseColor;

	private void Awake()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		block = new MaterialPropertyBlock();
		colorID = Shader.PropertyToID( "_Color" );
		baseColor = meshRenderer.material.color;

		gameObject.SetActive( false );
	}
	public void Select( Color color )
	{
		gameObject.SetActive( true );
		block.SetColor( colorID, color );
		meshRenderer.SetPropertyBlock( block );
	}
	[Button]
	public void DeSelect()
	{
		gameObject.SetActive( false );
		block.SetColor( colorID, baseColor );
		meshRenderer.SetPropertyBlock( block );
	}
}