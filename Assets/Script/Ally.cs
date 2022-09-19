/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Ally : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variables" ) ]
    [ SerializeField ] Pool_Ally pool_ally;
    [ SerializeField ] ParticleSpawnEvent event_particle_spawn;
    [ SerializeField ] GunInfo shared_gun_info;
   
  [ Title( "Components" ) ]
    [ SerializeField ] Animator _animator;
    [ SerializeField ] Transform parent_projectile_shoot;
// Private
	int spawn_index;
    float gun_fire_cooldown;
	RecycledTween recycledTween = new RecycledTween();

	UnityMessage onUpdateMethod;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnDisable()
    {
		onUpdateMethod = ExtensionMethods.EmptyMethod;
	}
    private void Awake()
    {
		onUpdateMethod = ExtensionMethods.EmptyMethod;
	}
    private void Update()
    {
		onUpdateMethod();
	}
#endregion

#region API
    public void MoveToFinishLine( Vector3 position ) // Tween to world position
    {
		onUpdateMethod = ExtensionMethods.EmptyMethod;

		transform.parent = null;

		recycledTween.Recycle( transform.DOMove( position, GameSettings.Instance.ally_movement_speed )
			.SetSpeedBased(),
			OnFinishLineMovementComplete
		);
	}

    public void DoLocalMovePosition( Vector3 position )
    {
		recycledTween.Recycle( transform.DOLocalMove( position, GameSettings.Instance.ally_movement_speed )
		.SetSpeedBased()
		);
	}

    public void OnTrigger()
    {
		// Unregister from ally group
		InstantlyDie();
	}

    public void OnGunChanged()
    {
		ResetGunCooldown();
	}

    public void OnLevelStarted()
    {
		_animator.SetTrigger( "trigger" ); // Move to run and aim state
		StartShooting();
	}

    public void OnLevelComplete()
    {
		_animator.SetTrigger( "trigger" ); // Move to victory state
		onUpdateMethod = ExtensionMethods.EmptyMethod;
	}

    // Spawn
    public void SpawnIdle( Transform parent, Vector3 position, int spawnIndex )
    {
		spawn_index = spawnIndex;
		Spawn( parent, position );

		// group_enemy_platform = groupPlatformEnemy;

		_animator.Play( "idle" );
	}

	public void SpawnRunning( Transform parent, Vector3 position, int spawnIndex )
	{
		spawn_index = spawnIndex;
		Spawn( parent, position );
		// group_enemy_platform = groupPlatformEnemy;

		_animator.Play( "running_aiming" );

		StartShooting();
	}

    public void StartShooting()
    {
		ResetGunCooldown();
		onUpdateMethod = ExtensionMethods.EmptyMethod;
	}
#endregion

#region Implementation
    void Spawn( Transform parent, Vector3 position )
    {
		gameObject.SetActive( true );

		transform.parent = parent;
		transform.localPosition = position;
		transform.forward = parent.forward;
    }

    void OnFinishLineMovementComplete()
    {
		_animator.SetTrigger( "trigger" ); // Move to aiming state
	}

    void ShootingRoutine()
    {
        if( Time.time >= gun_fire_cooldown )
        {
			Shoot();
			gun_fire_cooldown = Time.time + shared_gun_info.GunFireRate;
		}
    }

    void Shoot()
    {
		shared_gun_info.GunProjectilePool.Spawn( parent_projectile_shoot.position, transform.forward );
	}

    void InstantlyDie()
    {
		event_particle_spawn.Raise( "death_blue", RandomSpawnPoint() );
		ReturnToPool();
	}

	void ReturnToPool()
	{
		pool_ally.ReturnEntity( this );
	}

	Vector3 RandomSpawnPoint()
	{
		return transform.position + Vector3.up / 2 + Random.onUnitSphere;
	}

    void ResetGunCooldown()
    {
		gun_fire_cooldown = Time.time + Random.Range( 0, shared_gun_info.GunFireRate );
    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}