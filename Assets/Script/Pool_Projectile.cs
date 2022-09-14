/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "pool_projectile_", menuName = "FF/Data/Pool/Projectile" ) ]
public class Pool_Projectile : ComponentPool< Projectile >
{
    public void Spawn( Vector3 position, Vector3 forward )
    {
		GetEntity().Spawn( position, forward );
	}
}