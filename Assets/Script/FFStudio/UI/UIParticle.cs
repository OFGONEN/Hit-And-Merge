/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;
using DG.Tweening;
using TMPro;
using Sirenix.OdinInspector;

namespace FFStudio
{
	public class UIParticle : MonoBehaviour
	{
#region Fields
	[ Title( "SharedVariable" ) ]
		[ SerializeField ] RectTransform _rectTransform;
		[ SerializeField ] UIParticlePool pool_ui_particle;

		RecycledSequence recycledSequence = new RecycledSequence();
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
		// [ Button() ]
		// public void Spawn( Vector3 screenPositionStart, Vector3 screenPositionEnd )
		// {
		// 	gameObject.SetActive( true );

		// 	transform.position = screenPositionStart;
		// 	var spawnTargetPosition = screenPositionStart + Random.insideUnitCircle.ConvertV3() * GameSettings.Instance.ui_particle_spawn_width * Screen.width / 100f;

		// 	var sequence = recycledSequence.Recycle( OnSequenceComplete )
		// 						.Append( transform
		// 									.DOMove( spawnTargetPosition, GameSettings.Instance.ui_particle_spawn_duration )
		// 									.SetEase( GameSettings.Instance.ui_particle_spawn_ease ) )
		// 						.AppendInterval( GameSettings.Instance.ui_particle_target_waitTime )
		// 						.Append( transform
		// 									.DOMove( screenPositionEnd, GameSettings.Instance.ui_particle_target_duration )
		// 									.SetEase( GameSettings.Instance.ui_particle_target_ease ) );
		// }

		[ Button() ]
		public void Spawn( Vector3 screenPosition, SharedReferenceNotifier targetReference )
		{
			gameObject.SetActive( true );

			transform.position    = screenPosition;
			transform.localScale  = Vector3.zero;
			transform.eulerAngles = Vector3.forward * Random.Range(
				-GameSettings.Instance.ui_particle_spawn_rotationRange,
				GameSettings.Instance.ui_particle_spawn_rotationRange
			);

			var targetPosition = ( targetReference.sharedValue as RectTransform ).position;

			var sequence = recycledSequence.Recycle( OnSequenceComplete );

			sequence.Append( transform.DOScale(
				GameSettings.Instance.ui_particle_spawn_size,
				GameSettings.Instance.ui_particle_spawn_duration )
				.SetEase( GameSettings.Instance.ui_particle_spawn_ease )
			);

			sequence.Append( transform.DOScale(
				1,
				GameSettings.Instance.ui_particle_return_duration )
				.SetEase( GameSettings.Instance.ui_particle_return_ease )
			);

			sequence.Append( transform.DOMove(
				targetPosition,
				GameSettings.Instance.ui_particle_movement_duration )
				.SetEase( GameSettings.Instance.ui_particle_movement_ease )
			);

			sequence.Join( transform.DOScale(
				GameSettings.Instance.ui_particle_movement_size_end,
				GameSettings.Instance.ui_particle_movement_size_end_duration )
			);
		}
#endregion

#region Implementation
		void OnSequenceComplete()
		{
			pool_ui_particle.ReturnEntity( this );
		}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
	}
}