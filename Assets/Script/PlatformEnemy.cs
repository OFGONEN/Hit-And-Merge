/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;
using DG.Tweening;

public class PlatformEnemy : MonoBehaviour
{
#region Fields
  [ Title( "Shared" ) ]
    [ SerializeField ] Pool_PlatformEnemy pool_enemy_platform;
    [ SerializeField ] ParticleSpawnEvent event_particle_spawn;

  [ Title( "Components" ) ]
    [ SerializeField ] Animator _animator;
    [ SerializeField ] ColorSetter renderer_color_setter;
    [ SerializeField ] Collider collider_projectile_receiver;
    [ SerializeField ] Collider collider_ally_receiver;

// Private Fields
	RecycledTween recycledTween = new RecycledTween();
	GroupPlatformEnemy group_enemy_platform;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void Spawn( Transform parent, Vector3 position, Vector3 forward, GroupPlatformEnemy groupPlatformEnemy )
    {
		gameObject.SetActive( true );

		transform.parent     = parent;
		transform.localPosition   = position;
		transform.forward    = forward;
		transform.localScale = Vector3.one;

		group_enemy_platform = groupPlatformEnemy;

		collider_ally_receiver.enabled       = false;
		collider_projectile_receiver.enabled = false;

		_animator.Play( "idle" );
	}

	public void DeSpawn()
	{
		recycledTween.Kill();
		ReturnToPool();
	}

    public void StartRunning()
    {
		collider_ally_receiver.enabled       = true;
		collider_projectile_receiver.enabled = true;

		_animator.SetBool( "run", true );
	}

    public void OnTrigger_Projectile()
    {
		group_enemy_platform.UnRegisterEnemy( ReturnKey() );
		Die();
	}

    public void OnTrigger_Ally()
    {
		group_enemy_platform.UnRegisterEnemy( ReturnKey() );
		InstantlyDie();
	}

	public int ReturnKey()
	{
		return GetInstanceID();
	}
#endregion

#region Implementation
    void Die()
    {
		transform.parent = pool_enemy_platform.PoolParent;

		renderer_color_setter.SetColor( GameSettings.Instance.enemy_death_color );

		_animator.SetTrigger( "die" );
		event_particle_spawn.Raise( "death_red", RandomSpawnPoint() );

		recycledTween.Recycle( transform.DOScale( 0, GameSettings.Instance.enemy_death_duration ), ReturnToPool );
	}

    void InstantlyDie()
    {
		event_particle_spawn.Raise( "death_red", RandomSpawnPoint() );
		ReturnToPool();
	}

	Vector3 RandomSpawnPoint()
	{
		return transform.position + Vector3.up / 2 + Random.onUnitSphere;
	}

	void ReturnToPool()
	{
		pool_enemy_platform.ReturnEntity( this );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}