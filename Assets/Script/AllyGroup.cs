/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using UnityEditor;
using Sirenix.OdinInspector;

public class AllyGroup : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variable" ) ]
    [ SerializeField ] SharedIntNotifier notif_ally_count;
    [ SerializeField ] SharedVector3Notifier notif_fireRange_position;
    [ SerializeField ] Pool_Ally pool_ally;
    [ SerializeField ] SharedVector2 shared_input_delta; // updates every frame
    [ SerializeField ] GameEvent event_ally_finalStage_Register;
    [ SerializeField ] GameEvent event_ally_finalStage_UnRegister;

  [ Title( "Setup" ) ]
    [ SerializeField ] Vector3[] ally_spawn_points;

// Private	
    List< Ally > ally_list = new List< Ally >( 331 );
	int spawn_index     = 0;
	int spawn_row_ceil  = 0;
	int spawn_row_floor = 0;
	int spawn_row_index = 0;
	float movement_clamp = 0;

	RecycledTween recycledTween = new RecycledTween();

	UnityMessage onAllySpawn;
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
		onAllySpawn    = SpawnAlly_Idle;
		onUpdateMethod = ExtensionMethods.EmptyMethod;
	}

    private void Update()
    {
		onUpdateMethod();
	}
#endregion

#region API
    public void OnLevelStarted()
    {
		onAllySpawn = SpawnAlly_Running;
	}

	public void OnLevelFailed()
	{
		onUpdateMethod = ExtensionMethods.EmptyMethod;
	}

	public void OnFinishLine()
	{
		onUpdateMethod = ExtensionMethods.EmptyMethod;
		AlignAlliesOnFinalStage();

		event_ally_finalStage_Register.Raise();

		recycledTween.Recycle( transform.DOMove( notif_fireRange_position.sharedValue, GameSettings.Instance.ally_group_movement_speed_forward ),
			event_ally_finalStage_UnRegister.Raise 
		);
	}

    public void OnSpawnAlly( IntGameEvent gameEvent )
    {
		var eventValue = gameEvent.eventValue;
		var spawnCount = Mathf.Min( GameSettings.Instance.ally_group_count_max - ally_list.Count, eventValue );

		for( var i = 0; i < spawnCount; i++ )
			onAllySpawn();
	}

    public void OnAllyDied( IntGameEvent gameEvent )
    {
		ally_list.RemoveAt( gameEvent.eventValue );
	}
#endregion

#region Implementation
	void AlignAlliesOnFinalStage()
	{
		var position      = notif_fireRange_position.sharedValue;
		    position.z   -= GameSettings.Instance.game_finalStage_offset;
		var lateralCount  = GameSettings.Instance.AllyLateralCountOnFinalStage;
		var rowCount      = ally_list.Count / lateralCount;

		int allyCount = 0;

		for( var y = 0; y < rowCount; y++ )
		{
			for( var x = 0; x < lateralCount; x++ )
			{
				var movePosition = new Vector3(
					-GameSettings.Instance.ally_group_movement_clamp + x * GameSettings.Instance.ally_spawn_radius,
					0,
					position.z - y * GameSettings.Instance.ally_spawn_radius
				);

				ally_list[ allyCount ].MoveToFinishLine( movePosition );
				allyCount++;
			}
		}
	}

    [ Button() ]
    void SpawnAlly_Idle()
    {
		var ally = pool_ally.GetEntity();

		var targetPosition = ally_spawn_points[ spawn_index ];
		var spawnPosition = targetPosition - targetPosition.normalized * GameSettings.Instance.ally_spawn_radius;

		ally.SpawnIdle( transform, spawnPosition, spawn_index );
		ally.DoLocalMovePosition( targetPosition + ReturnBufferedPosition() );

		ally_list.Add( ally );

		IncreaseSpawnIndex();
	}

    [ Button() ]
	void SpawnAlly_Running()
	{
		var ally = pool_ally.GetEntity();

		var targetPosition = ally_spawn_points[ spawn_index ];
		var spawnPosition = targetPosition - targetPosition.normalized * GameSettings.Instance.ally_spawn_radius;

		ally.SpawnRunning( transform, spawnPosition, spawn_index );
		ally.DoLocalMovePosition( targetPosition + ReturnBufferedPosition() );

		ally_list.Add( ally );

		IncreaseSpawnIndex();
	}

    void IncreaseSpawnIndex()
    {
		spawn_index++;
		notif_ally_count.SharedValue++;

		if( spawn_index >= spawn_row_ceil )
		{
			spawn_row_index++;

			spawn_row_floor = spawn_row_ceil;
			spawn_row_ceil += spawn_row_index * 6;

			movement_clamp = ( spawn_row_index + 0.5f ) * GameSettings.Instance.ally_spawn_radius;
		}
    }

	void DecreaseSpawnIndex()
	{
		spawn_index--;
		notif_ally_count.SharedValue--;

		bool arrangeAllies = notif_ally_count.sharedValue <= spawn_row_floor;

		if( spawn_index < spawn_row_ceil )
		{
			spawn_row_index--;

			spawn_row_ceil = spawn_row_floor;
			spawn_row_floor -= spawn_row_index * 6;

			movement_clamp = ( spawn_row_index + 0.5f ) * GameSettings.Instance.ally_spawn_radius;
		}

        if( arrangeAllies )
			ArrangeAllies();
	}

    void ArrangeAllies()
    {
        for( var i = 0; i < ally_list.Count; i++ )
			ally_list[ i ].Rearrange( i, ally_spawn_points[ i ] + ReturnBufferedPosition() );
	}

    Vector3 ReturnBufferedPosition()
    {
        return Random.insideUnitCircle.ConvertV3_Z() * GameSettings.Instance.ally_spawn_radius_buffer;
	}

	void OnUpdate_Movement()
	{
		var position = transform.position;
		position.z += Time.deltaTime * GameSettings.Instance.ally_group_movement_speed_forward;
		position.x += shared_input_delta.sharedValue.x * GameSettings.Instance.ally_group_movement_speed_lateral * GameSettings.Instance.game_input_resolution;

		var clamValue = GameSettings.Instance.ally_group_movement_clamp - movement_clamp;
		position.x = Mathf.Clamp( position.x, -clamValue, clamValue );

		transform.position = position;

		shared_input_delta.sharedValue = Vector2.zero;
	}
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