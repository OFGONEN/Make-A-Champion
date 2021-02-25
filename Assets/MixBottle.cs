/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;

public class MixBottle : MonoBehaviour
{
#region Fields
	[ Header( "Modified Shared Data" ) ]
	public SharedFloatProperty mixLiquidPercentage;

#endregion

#region Unity API
    private void Start()
    {
		mixLiquidPercentage.Value = 0.0f;
	}
#endregion

#region API
#endregion

#region Implementation
#endregion
}
