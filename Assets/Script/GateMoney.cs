/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using TMPro;
using Sirenix.OdinInspector;

public class GateMoney : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variables" ) ]
    [ SerializeField ] Pool_UIPopUpText pool_ui_popUpText;
    [ SerializeField ] GunInfo shared_gun_current;
    [ SerializeField ] Currency notif_currency;

  [ Title( "Setup" ) ]
    [ LabelText( "Money Gate Start Value" ), SerializeField ] float gate_money_value;
    [ LabelText( "Money Gate Value Gain Cofactor" ), SerializeField ] float gate_money_value_cofactor;

  [ Title( "Components" ) ]
    [ SerializeField ] TextMeshProUGUI gate_text; 
    [ SerializeField ] ParticleSpawner gate_particleSpawner; 
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void OnTrigger_Projectile( Collider collider )
    {
		var damage          = shared_gun_current.GunDamage * gate_money_value_cofactor;
		var gateHeight      = GameSettings.Instance.gate_height;
		var uiSpawnPosition = collider.transform.position;

		uiSpawnPosition.y = Random.Range( gateHeight / 4f, gateHeight);
		uiSpawnPosition.z = transform.position.z + GameSettings.Instance.gate_ui_spawn_damage_offset;

		pool_ui_popUpText.GetEntity().Spawn( 
			uiSpawnPosition, 
			"$" + damage, 
			GameSettings.Instance.gate_ui_spawn_money_size, 
			GameSettings.Instance.gate_ui_spawn_money_color 
		);

		gate_money_value += damage;
		gate_text.text = "$" + gate_money_value;
	}

    public void OnTrigger_Ally()
    {
		notif_currency.SharedValue += gate_money_value;
		gate_particleSpawner.Spawn( 0 );

		gameObject.SetActive( false );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
    private void OnValidate()
    {
		gate_text.text = "$" + gate_money_value;
	}
#endif
#endregion
}
