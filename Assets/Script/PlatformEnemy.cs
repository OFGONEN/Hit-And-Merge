/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class PlatformEnemy : MonoBehaviour
{
#region Fields
  [ Title( "Shared" ) ]
    [ SerializeField ] Pool_PlatformEnemy pool_enemy_platform;
  [ Title( "Components" ) ]
    [ SerializeField ] Animator _animator;
    [ SerializeField ] ColorSetter renderer_color_setter;
    [ SerializeField ] BoxCollider collider_projectile_receiver;
    [ SerializeField ] BoxCollider collider_ally_receiver;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void Spawn( Vector3 position, Vector3 forward )
    {
		gameObject.SetActive( true );

		transform.position = position;
		transform.forward  = forward;

		collider_ally_receiver.enabled       = false;
		collider_projectile_receiver.enabled = false;

		_animator.Play( "Idle" );
	}

    public void StartRunning()
    {
		collider_ally_receiver.enabled       = true;
		collider_projectile_receiver.enabled = true;

		_animator.SetBool( "run", true );
	}

    public void OnTrigger_Projectile()
    {
		Die();
	}

    public void OnTrigger_Ally()
    {
		InstantlyDie();
	}
#endregion

#region Implementation
    void Die()
    {
		transform.parent = pool_enemy_platform.PoolParent;

		renderer_color_setter.SetColor( GameSettings.Instance.enemy_death_color );
		_animator.SetTrigger( "die" );
	}

    void InstantlyDie()
    {

    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}