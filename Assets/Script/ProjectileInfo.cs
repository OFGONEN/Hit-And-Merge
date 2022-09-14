/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "info_projectile", menuName = "FF/Game/Info Projectile" ) ]
public class ProjectileInfo : ScriptableObject
{
#region Fields
  [ Title("Setup") ]
    [ SerializeField ] string projectile_onHit_pfx_key;
    [ SerializeField ] float projectile_speed;

    public float ProjectileSpeed => projectile_speed;
    public string ProjectileOnHitPFXKey => projectile_onHit_pfx_key;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}