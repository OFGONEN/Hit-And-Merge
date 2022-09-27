/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "incremental_", menuName = "FF/Data/Incremental/Empty" ) ]
public class IncrementalEmpty : ScriptableObject
{
    [ SerializeField ] IncrementalEmptyData[] incremental_empty_array;

    public IncrementalEmptyData ReturnIncrementalAtIndex( int index )
    {
		return incremental_empty_array[ index ];
	}
}