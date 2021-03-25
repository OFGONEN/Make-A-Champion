using UnityEngine;
using NaughtyAttributes;

namespace FFStudio
{
	[ CreateAssetMenu( fileName = "Dirt-Set", menuName = "FF/Data/Sets/Dirt Set" ) ]
    public class DirtSet : RuntimeSet< int /* GUID maybe ? Don't care */, Dirt >
    {
		[ Header( "Fired Events" ) ]
		public GameEvent dirtRemovedFromSet;

		public new /* Hiding is intentional */ void RemoveList( Dirt value )
        {
			base.RemoveList( value );
			dirtRemovedFromSet.Raise();
		}
    }    
}