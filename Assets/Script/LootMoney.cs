/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class LootMoney : MonoBehaviour
{
#region Fields
  [ Title( "Setup" ) ]
    [ SerializeField ] SharedReferenceNotifier notif_camera_reference;
    [ SerializeField ] Pool_Money pool_money;
    [ SerializeField ] UIParticlePool pool_ui_particle_money;
    [ SerializeField ] Rigidbody _rigidbody;

    Camera _camera;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    [ Button() ]
    public void Spawn( Vector3 position )
    {
		gameObject.SetActive( true );
		transform.position = position;
		transform.forward = Random.onUnitSphere;

		_rigidbody.velocity = Vector3.zero;


		_camera = ( notif_camera_reference.sharedValue as Transform ).GetComponent< Camera >();
	}

    public void OnSpawnParticleMoney()
    {
		var screenPosition = _camera.WorldToScreenPoint( transform.position );
		pool_ui_particle_money.Spawn( screenPosition );

		pool_money.ReturnEntity( this );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
