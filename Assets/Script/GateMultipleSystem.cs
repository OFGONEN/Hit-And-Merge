/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FFStudio;
using Sirenix.OdinInspector;
using UnityEditor;

public class GateMultipleSystem : MonoBehaviour
{
#region Fields
    [ SerializeField ] List< GateSpawn > gate_list = new List< GateSpawn >( 4 );
    [ SerializeField ] UnityEvent gate_spawn_event_activate;
#endregion

#region Properties
#endregion

#region Unity API
    private void Start()
    {
        for( var i = 0; i < gate_list.Count; i++ )
        {
			var gate = gate_list[ i ];
			gate.onGateUpdate     = OnGateUpdate;
			gate.onGateActivate   = OnGateActivate;
			gate.gate_spawn_index = i;
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
		var tryToMerge = true;

		if( tryToMerge && index - 1 >= 0 )
			tryToMerge = !TryMerge( gate_list[ index - 1 ], gate_list[ index ] );

		if( tryToMerge && index + 1 < gate_list.Count )
			TryMerge( gate_list[ index ], gate_list[ index + 1 ] );
	}

    bool TryMerge( GateSpawn leftGate, GateSpawn rightGate )
    {
		var canMerge = Mathf.Abs( rightGate.GateCount - leftGate.GateCount ) <= GameSettings.Instance.gate_merge_buffer;
		bool merged = false;

		if( canMerge && leftGate.GateCount > 0 && rightGate.GateCount > 0 )
		{
			leftGate.Merge( rightGate.GateCount, rightGate.GateSize );
			rightGate.OnMerged();
			merged = true;

			RemoveAndRearrangeGates( rightGate.gate_spawn_index );
		}

		return merged;
	}

    void OnGateActivate()
    {
        for( var i = 0; i < gate_list.Count; i++ )
			gate_list[ i ].DisableColliders();

		gate_spawn_event_activate.Invoke();
	}

	void RemoveAndRearrangeGates( int index )
	{
		gate_list.RemoveAt( index );

		for( var i = 0; i < gate_list.Count; i++ )
			gate_list[ i ].gate_spawn_index = i;
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
		gate.ChangeSize( GameSettings.Instance.gate_size / 2f );

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
		gate.ChangeSize( GameSettings.Instance.gate_size / 4f );

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
		gate.ChangeSize( GameSettings.Instance.gate_size );

		gate_list.Add( gate );
	}

	[ Button() ]
	void SpawnGateWithSize( float size )
	{
		var gateObject = AssetDatabase.LoadAssetAtPath( "Assets/Prefab/Gate/gate_spawn.prefab", typeof( GameObject ) );
		var instance = PrefabUtility.InstantiatePrefab( gateObject ) as GameObject;

		instance.transform.SetParent( transform );
		instance.transform.localPosition = Vector3.right * TotalGateSize();

		var gate = instance.GetComponent< GateSpawn >();
		gate.ChangeSize( size );

		gate_list.Add( gate );
	}

	[ Button() ]
	void DeleteGateAtIndex( int index )
	{
		if( index < 0 && index >= gate_list.Count ) return;

		DestroyImmediate( gate_list[ index ] );
		gate_list.RemoveAt( index );

		float totalGateSize = 0;

		for( var i = 0; i < gate_list.Count; i++ )
		{
			var gate = gate_list[ i ];
			gate.transform.localPosition = Vector3.right * totalGateSize;
			totalGateSize += gate.GateSize;
		}
	}

	[ Button() ]
	void DeleteAllGates()
	{
		for( var i = 0; i < gate_list.Count; i++ )
			DestroyImmediate( gate_list[ i ].gameObject );

		gate_list.Clear();
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