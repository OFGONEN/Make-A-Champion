using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;
using DG.Tweening;

public class FallBackFillBarTest : MonoBehaviour
{
	public SharedFloatPropertyFallBackTweener fillingAmount;
	public float setValue;

	private void Awake()
	{
		fillingAmount.Value = 0;
	}

	[Button]
	public void SetValue()
	{
		fillingAmount.Value += setValue;
	}
}
