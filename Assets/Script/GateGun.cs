/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class GateGun : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variables" ) ]
    [ SerializeField ] GunInfo shared_gun_current;
    [ LabelText( "Gun Gate Health" ), SerializeField ] float gate_gun_health;
    [ SerializeField ] Image gate_gun_image; 
    [ SerializeField ] GameObject gate_gun_loot; 

    float gate_gun_health_current = 0;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    [ Button() ]
    public void OnTrigger_Projectile()
    {
		gate_gun_health_current += shared_gun_current.GunDamage;
		gate_gun_image.fillAmount = Mathf.InverseLerp( 0, 1, gate_gun_health_current / gate_gun_health );

        if( gate_gun_health_current >= gate_gun_health )
        {
			gate_gun_loot.transform.SetParent( null );
			gate_gun_loot.SetActive( true );

			gameObject.SetActive( false );
        }
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
