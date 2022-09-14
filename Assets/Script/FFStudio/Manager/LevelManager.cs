/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace FFStudio
{
    public class LevelManager : MonoBehaviour
    {
#region Fields
    [ Title( "Fired Events" ) ]
        public GameEvent levelFailedEvent;
        public GameEvent levelCompleted;

    [ Title( "Level Releated" ) ]
        public SharedFloatNotifier levelProgress;
        public GunInfo gun_info_current;
        public GunInfo gun_info_default;
        public SharedFloatNotifier notif_money_currency;
#endregion

#region UnityAPI
        private void Awake()
        {
			gun_info_current.ChangeData( gun_info_default );
		}
#endregion

#region API
        // Info: Called from Editor.
        public void LevelLoadedResponse()
        {
			levelProgress.SetValue_NotifyAlways( 0 );

			var levelData = CurrentLevelData.Instance.levelData;
            // Set Active Scene.
			if( levelData.scene_overrideAsActiveScene )
				SceneManager.SetActiveScene( SceneManager.GetSceneAt( 1 ) );
            else
				SceneManager.SetActiveScene( SceneManager.GetSceneAt( 0 ) );
		}

        // Info: Called from Editor.
        public void LevelRevealedResponse()
        {

        }

        // Info: Called from Editor.
        public void LevelStartedResponse()
        {

        }

        public void OnLevelLoadStart()
        {
			gun_info_default.ChangeData( gun_info_current );
		}

        public void OnMoneyGained()
        {
			notif_money_currency.SharedValue += CurrentLevelData.Instance.levelData.money_value;
		}
#endregion

#region Implementation
#endregion
    }
}