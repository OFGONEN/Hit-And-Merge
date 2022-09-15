/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;


[ CreateAssetMenu( fileName = "pool_enemy_platform", menuName = "FF/Data/Pool/Platform Enemy" ) ]
public class Pool_PlatformEnemy : ComponentPool< PlatformEnemy >
{
    public void Spawn( Vector3 position, Vector3 forward )
    {
		GetEntity().Spawn( position, forward );
	}
}