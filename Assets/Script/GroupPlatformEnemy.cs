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

  [ Title( "Setup" ) ]
    [ SerializeField ] int enemy_spawn_count;
    [ SerializeField ] float enemy_spawn_radius;
    [ SerializeField ] float enemy_spawn_distance;
    [ SerializeField ] float enemy_movement_distance;
    [ SerializeField ] Vector3[] enemy_path;

    Transform ally_group_transform;
	RecycledTween recycledTween = new RecycledTween();
	List< PlatformEnemy > list_platform_enemy;

    UnityMessage onUpdateMethod;
#endregion

#region Properties
#endregion

#region Unity API
    private void Start()
    {
		list_platform_enemy = new List< PlatformEnemy >( enemy_spawn_count );
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

    }

    void StartMovementPath()
    {
		recycledTween.Recycle( transform.DOPath( enemy_path,
			GameSettings.Instance.enemy_movement_speed, PathType.Linear )
			.SetSpeedBased()
			.SetEase( Ease.Linear ),
            DeSpawnAllEnemies
        );

        for( var i = 0; i < list_platform_enemy.Count; i++ )
			list_platform_enemy[ i ].StartRunning();
	}

    void DeSpawnAllEnemies()
    {
        for( var i = 0; i < list_platform_enemy.Count; i++ )
			list_platform_enemy[ i ].DeSpawn();

		list_platform_enemy.Clear();
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
