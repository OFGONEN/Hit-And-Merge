/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "info_gun", menuName = "FF/Game/Info Gun" ) ]
public class GunInfo : ScriptableObject
{
#region Fields
  [ Title( "Setup" ) ]
    [ SerializeField ] Pool_Projectile pool_projectile;
    [ SerializeField ] Mesh gun_mesh;
    [ SerializeField ] float gun_damage;
    [ SerializeField ] float gun_fireRate;

    public Mesh GunMesh      => gun_mesh;
    public float GunDamage   => gun_damage;
    public float GunFireRate => gun_fireRate;
    public Pool_Projectile GunProjectilePool => pool_projectile;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void ChangeData( GunInfo data )
    {
		gun_mesh        = data.GunMesh;
		gun_damage      = data.GunDamage;
		gun_fireRate    = data.GunFireRate;
		pool_projectile = data.GunProjectilePool;
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
