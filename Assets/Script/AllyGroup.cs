/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using UnityEditor;
using Sirenix.OdinInspector;

public class AllyGroup : MonoBehaviour
{
#region Fields
  [ Title( "Setup" ) ]
    [ SerializeField ] Vector3[] ally_spawn_points;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
    List< Vector3 > spawnList;
    [ Button() ]
    void CacheSpawnPoints( int rowCount )
    {
        spawnList = new List<Vector3>();
		spawnList.Add( Vector3.zero );

        for( var i = 1; i <= rowCount; i++ )
			CreateSpawnPoints( i );

		ally_spawn_points = spawnList.ToArray();
	}

    void CreateSpawnPoints( int rowIndex )
    {
		var pointCount = 6 * rowIndex;
		var angleStep = 360f / pointCount;
		var radius = 2f * GameSettings.Instance.ally_spawn_radius * rowIndex;

        for( var i = 0; i < pointCount; i++ )
        {
			var angle = Mathf.Deg2Rad * angleStep * i;
			spawnList.Add( new Vector3( Mathf.Cos( angle ) * radius, 0, Mathf.Sin( angle ) * radius ) );
		}
	}

    private void OnDrawGizmos()
    {
		Handles.color = Color.blue;

		for( var i = 0; i < ally_spawn_points.Length; i++ )
			Handles.DrawWireDisc( transform.TransformPoint( ally_spawn_points[ i ] ), Vector3.up, GameSettings.Instance.ally_spawn_radius );
    }
#endif
#endregion
}