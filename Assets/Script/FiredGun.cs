/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class FiredGun : MonoBehaviour
{
#region Fields
  [ Title( "Setup" ) ]
    [ SerializeField ] GunInfo gun_info_current;
    [ SerializeField ] MeshFilter _meshFilter;
#endregion

#region Properties
#endregion

#region Unity API
    private void Start()
    {
		SetGun();
	}
#endregion

#region API
    public void OnGunChanged()
    {
		SetGun();
	}
#endregion

#region Implementation
    void SetGun()
    {
		_meshFilter.mesh = gun_info_current.GunMesh;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
