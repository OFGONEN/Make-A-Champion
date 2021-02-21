using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;
using DG.Tweening;

public class FillBarTest : MonoBehaviour
{
	public SharedFloatProperty fillingAmount;

	public float setValue;

	private void Awake()
	{
		fillingAmount.Value = 0;

		DOTween.To( () => fillingAmount.Value, x => fillingAmount.Value = x, 1, 2f );
	}

	[Button]
	public void SetValue()
	{
		fillingAmount.Value = setValue;
	}
}
