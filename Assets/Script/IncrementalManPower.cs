/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "incremental_manPower", menuName = "FF/Data/Incremental/Man Power" ) ]
public class IncrementalManPower : ScriptableObject
{
    [ SerializeField ] IncrementalManPower[] incremental_man_array;

    public IncrementalManPower ReturnIncrementalAtIndex( int index )
    {
		return incremental_man_array[ index ];
	}
}
