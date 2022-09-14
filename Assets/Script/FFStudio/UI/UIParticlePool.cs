/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "pool_ui_particle", menuName = "FF/Data/Pool/UI Particle Pool" ) ]
public class UIParticlePool : ComponentPool< UIParticle > 
{
	[ SerializeField ] SharedReferenceNotifier ui_target_reference;

    [ Button() ]
    public void Spawn( Vector3 screenStartPosition )
    {
		GetEntity().Spawn( screenStartPosition, ui_target_reference );
	}
}