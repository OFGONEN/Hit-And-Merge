/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;

public class SharedPositionSetter : MonoBehaviour
{
#region Fields
		[ SerializeField ] SharedVector3Notifier notif_vector3;
		[ SerializeField ] Transform _transform;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		notif_vector3.SetValue_NotifyAlways( _transform.position );
	}

    private void OnDisable()
    {
		notif_vector3.SetValue_NotifyAlways( Vector3.zero );
	}
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
