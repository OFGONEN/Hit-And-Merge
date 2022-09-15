/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [ SerializeField ] Vector3[] enemy_path;

    Transform ally_group_transform;
	RecycledTween recycledTween = new RecycledTween();
	Dictionary< int, PlatformEnemy > dictionary_platform_enemy;

    UnityMessage onUpdateMethod;
#endregion

#region Properties
#endregion

#region Unity API
    private void Start()
    {
		dictionary_platform_enemy = new Dictionary< int, PlatformEnemy >( enemy_spawn_count );
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

		if( distance <= enemy_spawn_distance )
		{
			StartMovementPath();
			onUpdateMethod = ExtensionMethods.EmptyMethod;
		}
    }

    void SpawnEnemies()
    {
        for( var i = 0; i < enemy_spawn_count; i++ )
        {
			var random = Random.insideUnitCircle.ConvertV3_Z();

			random *= enemy_spawn_radius - GameSettings.Instance.enemy_spawn_radius;

			var enemy = pool_enemy_platform.GetEntity();

			enemy.Spawn( transform, transform.TransformPoint( random ), transform.forward, this );
			dictionary_platform_enemy.Add( enemy.ReturnKey(), enemy );
		}
    }

    void StartMovementPath()
    {
		recycledTween.Recycle( transform.DOPath( enemy_path,
			GameSettings.Instance.enemy_movement_speed, PathType.Linear )
			.SetSpeedBased()
			.SetEase( Ease.Linear ),
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
#endif
#endregion
}
