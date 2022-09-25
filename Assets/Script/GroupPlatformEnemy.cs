/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class GroupPlatformEnemy : MonoBehaviour
{
#region Fields
  [ Title( "Shared" ) ]
    [ SerializeField ] SharedReferenceNotifier notif_ally_group_reference;
    [ SerializeField ] Pool_PlatformEnemy pool_enemy_platform;

  [ Title( "Setup" ) ]
    [ SerializeField ] int enemy_spawn_count;
    [ SerializeField ] float enemy_spawn_radius;
    [ SerializeField ] float enemy_spawn_distance;
    [ SerializeField ] float enemy_movement_distance;
    [ SerializeField ] float enemy_movement_speed;
    [ SerializeField ] Vector3[] enemy_spawn_points;
    [ SerializeField ] Transform enemy_path_parent;
    [ SerializeField ] Vector3[] enemy_path_points;

    Transform ally_group_transform;
	RecycledTween recycledTween = new RecycledTween();
	Dictionary< int, PlatformEnemy > dictionary_platform_enemy;

    UnityMessage onUpdateMethod;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		onUpdateMethod = ExtensionMethods.EmptyMethod;
		dictionary_platform_enemy = new Dictionary< int, PlatformEnemy >( enemy_spawn_count );
	}

	private void Update()
	{
		onUpdateMethod();
	}
#endregion

#region API
    public void OnLevelStart()
    {
		onUpdateMethod       = CheckIfEnemyCanSpawn;
		ally_group_transform = notif_ally_group_reference.sharedValue as Transform;
	}

    public void OnFinishLine()
    {
		onUpdateMethod = ExtensionMethods.EmptyMethod;

		recycledTween.Kill();
		DeSpawnAllEnemies();
	}

    public void UnRegisterEnemy( int key )
    {
		dictionary_platform_enemy.Remove( key );
	}
#endregion

#region Implementation
    void CheckIfEnemyCanSpawn()
    {
		var distance = Vector3.Distance( transform.position, ally_group_transform.position );

        if( distance <= enemy_spawn_distance )
        {
			SpawnEnemies();
			onUpdateMethod = CheckIfEnemyCanMove;
		}
	}

    void CheckIfEnemyCanMove()
    {
		var distance = Vector3.Distance( transform.position, ally_group_transform.position );

		if( distance <= enemy_movement_distance )
		{
			StartMovementPath();
			onUpdateMethod = ExtensionMethods.EmptyMethod;
		}
    }

    void SpawnEnemies()
    {
        for( var i = 0; i < enemy_spawn_count; i++ )
        {
			var enemy = pool_enemy_platform.GetEntity();

			enemy.Spawn( transform, enemy_spawn_points[ i ], transform.forward, this );
			dictionary_platform_enemy.Add( enemy.ReturnKey(), enemy );
		}
    }

    void StartMovementPath()
    {
		recycledTween.Recycle( transform.DOPath( enemy_path_points,
			enemy_movement_speed, PathType.Linear )
			.SetSpeedBased()
			.SetEase( Ease.Linear )
			.SetLookAt( GameSettings.Instance.enemy_movement_lookAt ),
            DeSpawnAllEnemies
        );

		foreach( var element in dictionary_platform_enemy )
			element.Value.StartRunning();
	}

    void DeSpawnAllEnemies()
    {
		foreach( var element in dictionary_platform_enemy )
			element.Value.DeSpawn();

		dictionary_platform_enemy.Clear();
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
	[ Button() ]
	void CachePathPoints()
	{
		enemy_path_points = new Vector3[ enemy_path_parent.childCount ];
		
		for( var i = 0; i < enemy_path_points.Length; i++ )
			enemy_path_points[ i ] = enemy_path_parent.GetChild( i ).position;
	}

	[ Button() ]
	private void CacheSpawnPoints()
	{
		enemy_spawn_points = new Vector3[ enemy_spawn_count ];

		for( var i = 0; i < enemy_spawn_count; i++ )
		{
			var random = Random.insideUnitCircle.ConvertV3_Z();

			random *= enemy_spawn_radius - GameSettings.Instance.enemy_spawn_radius;

			enemy_spawn_points[ i ] = random;
		}
	}

	private void OnDrawGizmos()
	{
		// Draw movement path
		Handles.color = Color.green;
		Vector3 currentPoint   = transform.position;
		Vector3 currentPointUp = transform.position + Vector3.up;

		for( var i = 0; i < enemy_path_points.Length; i++ )
		{
			var nextPoint   = enemy_path_points[ i ];
			var nextPointUp = enemy_path_points[ i ] + Vector3.up;

			Handles.DrawLine( currentPoint, currentPointUp );
			Handles.DrawLine( nextPoint, nextPointUp );
			Handles.DrawDottedLine( currentPointUp, nextPointUp, 1f );
			Handles.Label( ( currentPointUp + nextPointUp ) / 2f, gameObject.name + "_Path_" + i );

			currentPoint   = enemy_path_points[ i ];
			currentPointUp = enemy_path_points[ i ] + Vector3.up;
		}

		// Draw spawned enemies
		Handles.color = Color.red;
		Handles.DrawWireDisc( transform.position, Vector3.up, enemy_spawn_radius );

		for( var i = 0; i < enemy_spawn_points.Length; i++ )
		{
			var worldPosition = transform.TransformPoint( enemy_spawn_points[ i ] );
			Handles.DrawWireDisc( worldPosition, Vector3.up, GameSettings.Instance.enemy_spawn_radius );
			Handles.Label( worldPosition + Vector3.up / 2f,"Enemy_" + i );
		}

		// Draw Spawn and Movement Position
		var position         = transform.position;
		var spawnPosition    = transform.position + Vector3.back * enemy_spawn_distance;
		var movementPosition = transform.position + Vector3.back * enemy_movement_distance;

		Handles.color = Color.yellow;
		Handles.DrawDottedLine( position, spawnPosition, 0.1f );
		Handles.DrawWireDisc( spawnPosition, Vector3.up, 0.25f );
		Handles.Label( spawnPosition + Vector3.up / 2f, gameObject.name + " Spawn Position" );

		Handles.color = Color.red;
		Handles.DrawDottedLine( position + Vector3.up / 4f, movementPosition + Vector3.up / 4f, 0.1f );
		Handles.DrawWireDisc( movementPosition + Vector3.up / 4f, Vector3.up, 0.25f );
		Handles.Label( movementPosition + Vector3.up / 2f + Vector3.up / 4f, gameObject.name + " Movement Position" );
	}
#endif
#endregion
}