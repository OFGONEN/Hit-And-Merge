/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using FFStudio;
using TMPro;
using Sirenix.OdinInspector;

public class GateSpawn : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variable" ) ]
    [ SerializeField ] GunInfo shared_gun_current;
    [ SerializeField ] IntGameEvent event_ally_spawn;
    [ SerializeField ] IntGameEvent event_ally_kill;
    [ SerializeField ] Pool_UIPopUpText pool_ui_popUpText;

  [ Title( "Setup" ) ]
    [ SerializeField ] bool gate_spawn_isLocked;
    [ SerializeField ] float gate_spawn_count;
    [ SerializeField ] float gate_spawn_size;
    [ SerializeField ] UnityEvent gate_spawn_event_activate;

  [ Title( "Components" ) ]
    [ SerializeField ] ParticleSystem gate_spawn_color;
    [ SerializeField ] TextMeshProUGUI gate_spawn_text;
    [ SerializeField ] Image gate_spawn_locked_image;
    [ SerializeField ] Collider gate_spawn_collider_ally;
    [ SerializeField ] Collider gate_spawn_collider_projectile;
    [ SerializeField ] Transform gate_spawn_pivot_scale_left;
    [ SerializeField ] Transform gate_spawn_canvas;
    [ SerializeField ] Transform gate_spawn_pole_right;

    public UnityMessage onGateUpdate;
    public UnityMessage onGateActivate;
// Private
    char gate_spawn_sign;

    UnityMessage onTrigger_Ally;
    UnityMessage onAllySpawn;
    UnityMessage onSetGateColor;
    TriggerMessage onTrigger_Projectile;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		onTrigger_Projectile = Trigger_Projectile;
		onTrigger_Ally       = Trigger_Ally;
		onGateUpdate         = ExtensionMethods.EmptyMethod;
		onSetGateColor       = ExtensionMethods.EmptyMethod;
		onAllySpawn          = SpawnAlly;

		if( gate_spawn_isLocked )
        {
			onTrigger_Projectile = ExtensionMethods.EmptyMethod;
			onTrigger_Ally       = ExtensionMethods.EmptyMethod;
        }

        if( gate_spawn_count < 0 )
		{
			onSetGateColor = SetGateColor;
			onAllySpawn    = KillAlly;
		}
	}
#endregion

#region API
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

	public void Disable()
	{
		gate_spawn_collider_ally.enabled = false;
		gate_spawn_collider_projectile.enabled = false;
	}
#endregion

#region Implementation
    void Trigger_Projectile( Collider collider )
    {
		var damage = shared_gun_current.GunDamage;
		var gateHeight = GameSettings.Instance.gate_height;
		var uiSpawnPosition = collider.transform.position;

		uiSpawnPosition.y = Random.Range( gateHeight / 4f, gateHeight );
		uiSpawnPosition.z = transform.position.z + GameSettings.Instance.gate_ui_spawn_damage_offset;

		var stringBuilder = ExtensionMethods.stringBuilder;
		stringBuilder.Clear();
		stringBuilder.Append( '$' );
		stringBuilder.Append( damage );

		pool_ui_popUpText.GetEntity().Spawn(
			uiSpawnPosition,
			stringBuilder.ToString(),
			GameSettings.Instance.gate_ui_spawn_money_size,
			GameSettings.Instance.gate_ui_spawn_money_color
		);

		gate_spawn_count += damage;

		stringBuilder.Clear();
		stringBuilder.Append( '$' );
		stringBuilder.Append( gate_spawn_count.ToString( "0.0" ) );

		gate_spawn_text.text = stringBuilder.ToString();

		onSetGateColor();
		onGateUpdate();
	}

    void Trigger_Ally()
    {
		event_ally_spawn.Raise( Mathf.FloorToInt( gate_spawn_count ) );
		onGateActivate();
		onAllySpawn();

		gameObject.SetActive( false );
    }

    void SetGateColor()
    {
        if( gate_spawn_count > 0 )
        {
			var main            = gate_spawn_color.main;
			    main.startColor = GameSettings.Instance.gate_color_positive;

			onSetGateColor  = ExtensionMethods.EmptyMethod;
			gate_spawn_sign = '+';

			onAllySpawn = SpawnAlly;
		}
    }

	void SpawnAlly()
	{
		event_ally_spawn.Raise( Mathf.FloorToInt( gate_spawn_count ) );
	}

	void KillAlly()
	{
		event_ally_kill.Raise( Mathf.FloorToInt( gate_spawn_count ) );
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
			gate_spawn_sign = '-';
		}
        else
        {
			main.startColor = GameSettings.Instance.gate_color_positive;
			gate_spawn_sign = '+';
        }

		gate_spawn_locked_image.enabled        = false;
		gate_spawn_collider_ally.enabled       = true;
		gate_spawn_collider_projectile.enabled = true;
        
        if( gate_spawn_isLocked )
        {
			main.startColor = GameSettings.Instance.gate_color_locked;

			gate_spawn_locked_image.enabled        = true;
			gate_spawn_collider_ally.enabled       = false;
			gate_spawn_collider_projectile.enabled = false;
		}

		var stringBuiler = ExtensionMethods.stringBuilder;
		stringBuiler.Clear();
		stringBuiler.Append( gate_spawn_sign );
		stringBuiler.Append( gate_spawn_count.ToString( "0.0" ) );

		gate_spawn_text.text = stringBuiler.ToString();
	}
#endif
#endregion
}