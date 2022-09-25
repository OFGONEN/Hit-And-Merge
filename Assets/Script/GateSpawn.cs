/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using FFStudio;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;

public class GateSpawn : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variable" ) ]
    [ SerializeField ] GunInfo shared_gun_current;
    [ SerializeField ] IntGameEvent event_ally_spawn;
    [ SerializeField ] IntGameEvent event_ally_kill;
    [ SerializeField ] Pool_UIPopUpText pool_ui_popUpText;
    [ SerializeField ] ParticleSpawnEvent event_particle_spawn;

  [ Title( "Setup" ) ]
    [ SerializeField ] bool gate_spawn_isLocked;
    [ SerializeField ] float gate_spawn_count;
    [ SerializeField ] float gate_spawn_size;
    [ SerializeField ] UnityEvent gate_spawn_event_activate;

  [ Title( "Components" ) ]
    [ SerializeField ] ParticleSystem gate_spawn_color;
    [ SerializeField ] TextMeshProUGUI gate_spawn_text;
    [ SerializeField ] Image gate_spawn_locked_plane_image;
    [ SerializeField ] Image gate_spawn_locked_image;
    [ SerializeField ] Collider gate_spawn_collider_ally;
    [ SerializeField ] Collider gate_spawn_collider_projectile;
    [ SerializeField ] Transform gate_spawn_pivot_scale_left;
    [ SerializeField ] Transform gate_spawn_canvas;
    [ SerializeField ] Transform gate_spawn_pole_right;

	public float GateCount => gate_spawn_count;
	public float GateSize => gate_spawn_size;

	public GateValueUpdate onGateUpdate;
    public UnityMessage onGateActivate;
// Private
    [ ReadOnly ] public int gate_spawn_index;
    string gate_spawn_sign;

    UnityMessage onTrigger_Ally;
    UnityMessage onAllySpawn;
    UnityMessage onSetGateColor;
    TriggerMessage onTrigger_Projectile;

	RecycledSequence recycledSequence        = new RecycledSequence();
	RecycledSequence recycledSequence_Locked = new RecycledSequence();
	RecycledTween    recycledTween_Text      = new RecycledTween();
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		onTrigger_Projectile = Trigger_Projectile;
		onTrigger_Ally       = Trigger_Ally;
		onGateActivate       = ExtensionMethods.EmptyMethod;
		onGateUpdate         = ExtensionMethods.EmptyMethod;
		onSetGateColor       = ExtensionMethods.EmptyMethod;
		onAllySpawn          = SpawnAlly;

		if( gate_spawn_isLocked )
        {
			onTrigger_Projectile = ExtensionMethods.EmptyMethod;
			onTrigger_Ally       = ExtensionMethods.EmptyMethod;
        }

		gate_spawn_locked_image.enabled       = false;
		gate_spawn_locked_plane_image.enabled = gate_spawn_isLocked;

		if( gate_spawn_count < 0 )
		{
			onSetGateColor = SetGateColor;
			onAllySpawn    = KillAlly;
		}
	}
#endregion

#region API
	public void Merge( float count, float size )
	{
		if( gate_spawn_isLocked )
		{
			gate_spawn_locked_plane_image.enabled = false;

			gate_spawn_locked_image.enabled = true;
			var lockedImageRectTransform = gate_spawn_locked_image.rectTransform;
			lockedImageRectTransform.SetParent( null );

			var sequence_locked = recycledSequence_Locked.Recycle();
			sequence_locked.Join( lockedImageRectTransform.DOMove(
				lockedImageRectTransform.position.SetY( GameSettings.Instance.gate_ui_canvas_float_position ),
				GameSettings.Instance.gate_ui_canvas_float_duration )
			);

			sequence_locked.AppendCallback( () =>
			{
				lockedImageRectTransform.gameObject.SetActive( false );
				// gameObject.SetActive( false );
			} );

			event_particle_spawn.Raise( "blast_lock",
				transform.position + Vector3.right * gate_spawn_size / 2f + Vector3.up * GameSettings.Instance.gate_height );
		}

		UnlockGate();
		DisableColliders();

		var sequence = recycledSequence.Recycle( OnMergeSequenceComplete );

		sequence.Append( transform.DOScale( gate_spawn_size * GameSettings.Instance.gate_merge_size_cofactor,
			GameSettings.Instance.gate_merge_duration ) );

		sequence.AppendCallback( () => 
			{
				transform.localScale = Vector3.one;

				gate_spawn_count += count;
				gate_spawn_size  += size;

				UpdateGateText();
				ChangeSize( gate_spawn_size );
				EnableColliders();
			} );

		sequence.Append( transform.DOPunchScale( Vector3.one * GameSettings.Instance.gate_merge_duration,
			GameSettings.Instance.gate_merge_punch_duration
		) );
	}

	public void OnMerged()
	{
		DisableColliders();

		var sequence = recycledSequence.Recycle();

		sequence.Append( transform.DOScale( gate_spawn_size * GameSettings.Instance.gate_merge_size_cofactor,
			GameSettings.Instance.gate_merge_duration ) );
		sequence.AppendCallback( () => { gameObject.SetActive( false ); });

		if( gate_spawn_isLocked )
		{
			gate_spawn_locked_plane_image.enabled = false;
			gate_spawn_locked_image.enabled       = true;
			gate_spawn_canvas.GetChild( 1 ).gameObject.SetActive( false );

			gate_spawn_canvas.SetParent( null );

			var sequence_locked = recycledSequence_Locked.Recycle();
			sequence_locked.Join( gate_spawn_canvas.DOMove(
				gate_spawn_canvas.position.SetY( GameSettings.Instance.gate_ui_canvas_float_position ),
				GameSettings.Instance.gate_ui_canvas_float_duration )
			);

			sequence_locked.AppendCallback( () =>
			{
				gate_spawn_canvas.gameObject.SetActive( false );
				// gameObject.SetActive( false );
			});

			event_particle_spawn.Raise( "blast_lock",
				transform.position + Vector3.right * gate_spawn_size / 2f + Vector3.up * GameSettings.Instance.gate_height );
		}
	}

    public void OnTrigger_Projectile( Collider collider )
    {
		onTrigger_Projectile( collider );
	}

    public void OnTrigger_Ally()
    {
		onTrigger_Ally();
	}

	[ Button() ]
    public void ChangeSize( float size )
    {
		gate_spawn_size = size;

		gate_spawn_pivot_scale_left.localScale = Vector3.one.SetX( gate_spawn_size );
		gate_spawn_pole_right.localPosition    = gate_spawn_pole_right.localPosition.SetX( gate_spawn_size );
		gate_spawn_canvas.localPosition        = gate_spawn_canvas.localPosition.SetX( gate_spawn_size / 2f );
	}

	public void ChangeSize()
	{
		gate_spawn_pivot_scale_left.localScale = Vector3.one.SetX( gate_spawn_size );
		gate_spawn_pole_right.localPosition    = gate_spawn_pole_right.localPosition.SetX( gate_spawn_size );
		gate_spawn_canvas.localPosition        = gate_spawn_canvas.localPosition.SetX( gate_spawn_size / 2f );
	}

	public void DisableColliders()
	{
		gate_spawn_collider_ally.enabled       = false;
		gate_spawn_collider_projectile.enabled = false;
	}

	public void EnableColliders()
	{
		gate_spawn_collider_ally.enabled       = true;
		gate_spawn_collider_projectile.enabled = true;
	}
#endregion

#region Implementation
	void OnMergeSequenceComplete()
	{
		onGateUpdate( gate_spawn_index );
	}

	void UnlockGate()
	{
		gate_spawn_isLocked  = false;
		onTrigger_Projectile = Trigger_Projectile;
		onTrigger_Ally       = Trigger_Ally;

		EnableColliders();
		SetGateColor();
	}

    void Trigger_Projectile( Collider collider )
    {
		var damage = shared_gun_current.GunDamage;
		var gateHeight = GameSettings.Instance.gate_height;
		var uiSpawnPosition = collider.transform.position;

		uiSpawnPosition.y = Random.Range( gateHeight * GameSettings.Instance.gate_ui_spawn_height_ratio_bottom,
			 gateHeight * GameSettings.Instance.gate_ui_spawn_height_ratio_upper );
		uiSpawnPosition.z = transform.position.z + GameSettings.Instance.gate_ui_spawn_offset;

		var stringBuilder = ExtensionMethods.stringBuilder;
		stringBuilder.Clear();
		stringBuilder.Append( '+' );
		stringBuilder.Append( damage );

		pool_ui_popUpText.GetEntity().Spawn(
			uiSpawnPosition,
			stringBuilder.ToString(),
			GameSettings.Instance.gate_ui_spawn_damage_size,
			GameSettings.Instance.gate_ui_spawn_damage_color
		);

		gate_spawn_count += damage;

		onSetGateColor();
		UpdateGateText();

		// gate text punch scale
		if( !recycledTween_Text.IsPlaying() )
			recycledTween_Text.Recycle( gate_spawn_text.rectTransform.DOPunchScale(
				GameSettings.Instance.gate_ui_text_punch_value * Vector3.one,
				GameSettings.Instance.gate_ui_text_punch_duration
			) );

		onGateUpdate( gate_spawn_index );
	}

    void Trigger_Ally()
    {
		onGateActivate();
		gate_spawn_event_activate.Invoke();
		onAllySpawn();

		gameObject.SetActive( false );
    }

	void UpdateGateText()
	{
		var stringBuilder = ExtensionMethods.stringBuilder;
		stringBuilder.Clear();
		stringBuilder.Append( gate_spawn_sign );
		stringBuilder.Append( gate_spawn_count.ToString( "0.0" ) );

		gate_spawn_text.text = stringBuilder.ToString();
	}

    void SetGateColor()
    {
        if( gate_spawn_count > 0 )
        {
			var main            = gate_spawn_color.main;
			    main.startColor = GameSettings.Instance.gate_color_positive;

			onSetGateColor  = ExtensionMethods.EmptyMethod;
			gate_spawn_sign = "+";

			onAllySpawn = SpawnAlly;
		}
    }

	void SpawnAlly()
	{
		event_ally_spawn.Raise( Mathf.FloorToInt( gate_spawn_count ) );
	}

	void KillAlly()
	{
		event_ally_kill.Raise( Mathf.Abs( Mathf.FloorToInt( gate_spawn_count ) ) );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
    private void OnValidate()
    {
		var main = gate_spawn_color.main;

		if( gate_spawn_count < 0 )
        {
			main.startColor = GameSettings.Instance.gate_color_negative;
			gate_spawn_sign = string.Empty;
		}
        else
        {
			main.startColor = GameSettings.Instance.gate_color_positive;
			gate_spawn_sign = "+";
        }

		gate_spawn_locked_plane_image.enabled  = gate_spawn_isLocked;
		gate_spawn_collider_ally.enabled       = !gate_spawn_isLocked;
		gate_spawn_collider_projectile.enabled = !gate_spawn_isLocked;
        
        if( gate_spawn_isLocked )
			main.startColor = GameSettings.Instance.gate_color_locked;

		UpdateGateText();
	}
#endif
#endregion
}