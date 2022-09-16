/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class Projectile : MonoBehaviour
{
#region Fields
  [ Title( "Setup" ) ]
    [ SerializeField ] ProjectileInfo projectile_info;
    [ SerializeField ] Pool_Projectile pool_projectile;
    [ SerializeField ] ParticleSpawnEvent event_particle_spawn;

// Private
    UnityMessage onUpdateMethod;

    float projectile_distance;
#endregion

#region Properties
    private void OnDisable()
    {
		onUpdateMethod = ExtensionMethods.EmptyMethod;
	}
#endregion

#region Unity API
	private void Awake()
	{
		onUpdateMethod = ExtensionMethods.EmptyMethod;
	}

    private void Update()
    {
		onUpdateMethod();
	}
#endregion

#region API
    [ Button() ]
    public void Spawn( Vector3 position, Vector3 forward )
    {
		gameObject.SetActive( true );

		transform.position = position;
		transform.forward  = forward;

		projectile_distance = 0;

		onUpdateMethod = OnUpdate_Move;
	}

    public void OnTrigger()
    {
		ProjectileReturnToPool();
		event_particle_spawn.Raise( projectile_info.ProjectileOnHitPFXKey, transform.position );
	}

    public void OnProjectilesDisappear()
    {
		ProjectileReturnToPool();
	}
#endregion

#region Implementation
    void OnUpdate_Move()
    {
		var delta = Time.deltaTime * projectile_info.ProjectileSpeed;

        projectile_distance += delta;

        if( projectile_distance >= GameSettings.Instance.projectile_travel_distance )
			ProjectileReturnToPool();

		var position    = transform.position;
		    position.z += delta;

		transform.position = position;
	}

    void ProjectileReturnToPool()
    {
		onUpdateMethod = ExtensionMethods.EmptyMethod;
		pool_projectile.ReturnEntity( this );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
