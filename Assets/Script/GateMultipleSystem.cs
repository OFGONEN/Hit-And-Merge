/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;
using UnityEditor;

public class GateMultipleSystem : MonoBehaviour
{
#region Fields
    [ SerializeField ] List< GateSpawn > gate_list = new List< GateSpawn >( 4 );
#endregion

#region Properties
#endregion

#region Unity API
    private void Start()
    {
        for( var i = 0; i < gate_list.Count; i++ )
        {
			var gate = gate_list[ i ];
			gate.onGateUpdate   = OnGateUpdate;
			gate.onGateActivate = OnGateActivate;
		}
    }
#endregion

#region API
	public void DisableAllGates()
	{
        for( var i = 0; i < gate_list.Count; i++ )
			gate_list[ i ].DisableColliders();
	}
#endregion

#region Implementation
    void OnGateUpdate( int index )
    {
		var gate = gate_list[ index ];
		if( index - 1 >= 0 )
			TryMerge( gate_list[ index - 1 ], gate_list[ index ] );
		else if( index + 1 < gate_list.Count )
			TryMerge( gate_list[ index ], gate_list[ index + 1 ] );
	}

    void TryMerge( GateSpawn leftGate, GateSpawn rightGate )
    {
		var canMerge = Mathf.Approximately( Mathf.Abs( rightGate.GateCount - leftGate.GateCount ), GameSettings.Instance.gate_merge_buffer );

		if( canMerge && leftGate.GateCount > 0 && rightGate.GateCount > 0 )
		{
			leftGate.Merge( rightGate.GateCount, rightGate.GateSize );
			rightGate.OnMerged();
		}        
    }

    void OnGateActivate()
    {
        for( var i = 0; i < gate_list.Count; i++ )
			gate_list[ i ].DisableColliders();
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
    [ Button() ]
    void SpawnHalfSizedGate()
    {
		//Assets/Prefab/Gate/gate_spawn.prefab
		var gateObject = AssetDatabase.LoadAssetAtPath( "Assets/Prefab/Gate/gate_spawn.prefab", typeof( GameObject ) );
		var instance   = PrefabUtility.InstantiatePrefab( gateObject ) as GameObject;

		instance.transform.SetParent( transform );
		instance.transform.localPosition = Vector3.right * TotalGateSize();

        var gate = instance.GetComponent< GateSpawn >();
		gate.ChangeSize( gate.GateSize / 2f );

		gate_list.Add( gate );
	}

    [ Button() ]
    void SpawnQuarterSizedGate()
    {
		//Assets/Prefab/Gate/gate_spawn.prefab
		var gateObject = AssetDatabase.LoadAssetAtPath( "Assets/Prefab/Gate/gate_spawn.prefab", typeof( GameObject ) );
		var instance = PrefabUtility.InstantiatePrefab( gateObject ) as GameObject;

		instance.transform.SetParent( transform );
		instance.transform.localPosition = Vector3.right * TotalGateSize();

		var gate = instance.GetComponent< GateSpawn >();
		gate.ChangeSize( gate.GateSize / 4f );

		gate_list.Add( gate );

	}

    [ Button() ]
    void SpawnGate()
    {
		var gateObject = AssetDatabase.LoadAssetAtPath( "Assets/Prefab/Gate/gate_spawn.prefab", typeof( GameObject ) );
		var instance = PrefabUtility.InstantiatePrefab( gateObject ) as GameObject;

		instance.transform.SetParent( transform );
		instance.transform.localPosition = Vector3.right * TotalGateSize();

		var gate = instance.GetComponent< GateSpawn >();

		gate_list.Add( gate );
	}

    float TotalGateSize()
    {
		float size = 0;

        for( var i = 0; i < gate_list.Count; i++ )
        {
			size += gate_list[ i ].GateSize;
		}

		return size;
	}
#endif
#endregion
}