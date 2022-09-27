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
    [ SerializeField ] SharedVector3Notifier notif_position_cage;
    [ SerializeField ] ParticleSpawnEvent event_particle_spawn;
    [ SerializeField ] GameEvent event_enemy_finalStage_Register;
    [ SerializeField ] GameEvent event_enemy_finalStage_UnRegister;

  [ Title( "Components" ) ]
    [ SerializeField ] Animator _animator;
    [ SerializeField ] ColorSetter renderer_color_setter;
    [ SerializeField ] Collider collider_projectile_receiver;

// Private
	UnityMessage onUnregisterRaise;
    RecycledTween recycledTween = new RecycledTween();
#endregion

#region Properties
#endregion

#region Unity API
	private void OnDisable()
	{
		recycledTween.Kill();
	}

	private void Start()
	{
		onUnregisterRaise = ExtensionMethods.EmptyMethod;
	}
#endregion

#region API
    public void OnFinalStageEnemyStartRun()
    {
		event_enemy_finalStage_Register.Raise();
		onUnregisterRaise = event_enemy_finalStage_UnRegister.Raise;

		_animator.SetBool( "run", true );

		var targetPosition = transform.position + transform.forward * movement_distance;

		recycledTween.Recycle( transform.DOMove( targetPosition, GameSettings.Instance.enemy_finalStage_movement_speed )
			.SetEase( Ease.Linear )
			.SetSpeedBased()
			.OnUpdate( OnMovementUpdate ),
			OnMovementComplete );
	}

    public void OnTrigger()
    {
		Die();
	}
#endregion

#region Implementation
	void OnMovementUpdate()
	{
		if( transform.position.z >= notif_position_cage.sharedValue.z )
		{
			collider_projectile_receiver.enabled = false;
			onUnregisterRaise();
			onUnregisterRaise = ExtensionMethods.EmptyMethod;
		}
	}

    void Die()
    {
		event_enemy_finalStage_UnRegister.Raise();
		recycledTween.Kill();

		_animator.SetTrigger( "die" );

		event_particle_spawn.Raise( "death_red", RandomSpawnPoint() );
		pool_money.Spawn( RandomSpawnPoint()  );

		collider_projectile_receiver.enabled = false;
		renderer_color_setter.SetColor( GameSettings.Instance.enemy_death_color );

		recycledTween.Recycle( transform.DOScale( 0, GameSettings.Instance.enemy_death_duration ) );
	}

    void OnMovementComplete()
    {
		collider_projectile_receiver.enabled = false;
		_animator.SetBool( "run", false );

		onUnregisterRaise();
		onUnregisterRaise = ExtensionMethods.EmptyMethod;	
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