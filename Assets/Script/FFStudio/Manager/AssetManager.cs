/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace FFStudio
{
	/* This class holds references to ScriptableObject assets. These ScriptableObjects are singletons, so they need to load before a Scene does.
	 * Using this class ensures at least one script from a scene holds a reference to these important ScriptableObjects. */
	public class AssetManager : MonoBehaviour
	{
#region Fields
	[ Title( "UnityEvent" ) ]
	[ SerializeField ] UnityEvent onAwakeEvent;
	[ SerializeField ] UnityEvent onEnableEvent;
	[ SerializeField ] UnityEvent onStartEvent;

	[ Title( "Setup" ) ]
		[ SerializeField ] GameSettings gameSettings;
		[ SerializeField ] CurrentLevelData currentLevelData;
		[ SerializeField ] RectTransform parent_pool_ui;

	[ Title( "Pool" ) ]
		[ SerializeField ] Pool_Money pool_money;
		[ SerializeField ] Pool_Projectile pool_projectile_pistol;
		[ SerializeField ] Pool_Projectile pool_projectile_desertEagle;
		[ SerializeField ] Pool_Projectile pool_projectile_uzi;
		[ SerializeField ] Pool_PlatformEnemy pool_enemy_platform;
		[ SerializeField ] Pool_UIPopUpText pool_UIPopUpText;
		[ SerializeField ] UIParticlePool pool_UI_particle;
#endregion

#region UnityAPI
		void OnEnable()
		{
			onEnableEvent.Invoke();
		}

		void Awake()
		{
			Vibration.Init();

			pool_money.InitPool( transform, false );
			pool_projectile_pistol.InitPool( transform, false );
			pool_projectile_desertEagle.InitPool( transform, false );
			pool_projectile_uzi.InitPool( transform, false );
			pool_enemy_platform.InitPool( transform, false );

			// UI Pools
			pool_UIPopUpText.InitPool( transform, false );
			pool_UI_particle.InitPool( parent_pool_ui, false );

			onAwakeEvent.Invoke();
		}

		void Start()
		{
			onStartEvent.Invoke();
		}
#endregion

#region API
		public void VibrateAPI( IntGameEvent vibrateEvent )
		{
			switch ( vibrateEvent.eventValue )
			{
				case 0:
					Vibration.VibratePeek();
					break;
				case 1:
					Vibration.VibratePop();
					break;
				case 2:
					Vibration.VibrateNope();
					break;
				default:
					Vibration.Vibrate();
					break;
			}
		}
#endregion

#region Implementation
#endregion
	}
}