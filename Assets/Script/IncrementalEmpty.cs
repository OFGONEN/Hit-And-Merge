/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "incremental_", menuName = "FF/Data/Incremental/Empty" ) ]
public class IncrementalEmpty : ScriptableObject
{
    [ SerializeField ] string incremental_key;
    [ SerializeField ] IncrementalEmptyData[] incremental_empty_array;

	public string IncrementalKey => incremental_key;
	public int IncrementalCount => incremental_empty_array.Length;

    public IncrementalEmptyData ReturnIncrementalAtIndex( int index )
    {
		return incremental_empty_array[ index ];
	}
}