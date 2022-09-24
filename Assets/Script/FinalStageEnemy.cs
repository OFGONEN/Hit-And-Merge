/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEditor;

public class FinalStageEnemy : MonoBehaviour
{
#region Fields
  [ Title( "Setup")]
    [ SerializeField ] float movement_distance;
    [ SerializeField ] Pool_Money pool_money;
    [ SerializeField ] ParticleSpawnEvent event_particle_spawn;
    [ SerializeField ] GameEvent event_enemy_finalStage_Register;
    [ SerializeField ] GameEvent event_enemy_finalStage_UnRegister;

  [ Title( "Components" ) ]
    [ SerializeField ] Animator _animator;
    [ SerializeField ] ColorSetter renderer_color_setter;
    [ SerializeField ] Collider collider_projectile_receiver;

// Private
    RecycledTween recycledTween = new RecycledTween();
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
	public void OnLevelFinished()
	{
		recycledTween.Kill();
		collider_projectile_receiver.enabled = false;
	}

    public void OnFinalStageEnemyStartRun()
    {
		event_enemy_finalStage_Register.Raise();

		_animator.SetBool( "run", true );

		var targetPosition = transform.position + transform.forward * movement_distance;

		recycledTween.Recycle( transform.DOMove( targetPosition, GameSettings.Instance.enemy_finalStage_movement_speed )
			.SetEase( Ease.Linear )
			.SetSpeedBased(),
			OnMovementComplete );
	}

    public void OnTrigger()
    {
		Die();
	}
#endregion

#region Implementation
    void Die()
    {
		event_enemy_finalStage_UnRegister.Raise();
		recycledTween.Kill();

		_animator.SetTrigger( "die" );

		event_particle_spawn.Raise( "death_red", RandomSpawnPoint() );
		pool_money.Spawn( RandomSpawnPoint()  );

		renderer_color_setter.SetColor( GameSettings.Instance.enemy_death_color );

		recycledTween.Recycle( transform.DOScale( 0, GameSettings.Instance.enemy_death_duration ) );
	}

    void OnMovementComplete()
    {
		_animator.SetBool( "run", false );
		event_enemy_finalStage_UnRegister.Raise();
    }

    Vector3 RandomSpawnPoint()
    {
		return transform.position + Vector3.up / 2 + Random.onUnitSphere;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
	Vector3 startPosition;

	private void Awake()
	{
		startPosition = transform.position;
	}

    private void OnDrawGizmos()
    {
		if( !Application.isPlaying )
			startPosition = transform.position;

		Handles.DrawWireDisc( startPosition, Vector3.up, 0.1f );
		Handles.DrawWireDisc( startPosition + transform.forward * movement_distance, Vector3.up, 0.1f );
		Handles.DrawDottedLine( startPosition, startPosition + transform.forward * movement_distance, 0.1f );
	}
#endif
#endregion
}