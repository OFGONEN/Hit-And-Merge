/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace FFStudio
{
	public class MovementTweenData_PhysicsRigidbody : TweenData
	{
#region Fields
	[ Title( "Movement Tween (Physics Rigidbody)" ) ]
		[ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ) ] public bool useDelta = true;
#if UNITY_EDITOR
		[ InfoBox( "End Value is RELATIVE.", "useDelta" ) ]
		[ InfoBox( "End Value is ABSOLUTE.", "EndValueIsAbsolute" ) ]
#endif
		[ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ) ] public Vector3 endValue;
#if UNITY_EDITOR
		[ InfoBox( "Duration is DURATION.", "useDelta" ) ]
		[ InfoBox( "Duration is VELOCITY.", "EndValueIsAbsolute" ) ]
#endif
		[ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ) ] public float duration;
#endregion

#region Properties
#if UNITY_EDITOR
		bool EndValueIsAbsolute => !useDelta;
#endif
#endregion

#region Unity API
#endregion

#region API
#endregion

#region Implementation
		protected override void CreateAndStartTween( UnityMessage onComplete, bool isReversed = false )
		{
            var theRigidbody = transform.GetComponent< Rigidbody >();
            
			var duration = useDelta ? Mathf.Abs( endValue.magnitude / this.duration ) : this.duration;

            recycledTween.Recycle( theRigidbody.DOMove( isReversed ? -endValue : endValue, duration ), onComplete );

			recycledTween.Tween
				.SetLoops( loop ? -1 : 0, loopType )
				.SetEase( easing )
				.SetUpdate( UpdateType.Fixed ); // Info: This is the main differing line of code between this & the MovementTweenData.

			if( useDelta )
				recycledTween.Tween.SetRelative();
#if UNITY_EDITOR
			recycledTween.Tween.SetId( "_ff_movement_tween_physics_rigidbody___" + description );
#endif

			base.CreateAndStartTween( onComplete, isReversed );
		}
#endregion

#region EditorOnly
#if UNITY_EDITOR
#endif
#endregion
	}
}