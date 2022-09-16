/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace FFStudio
{
    public class LevelManager : MonoBehaviour
    {
#region Fields
    [ Title( "Fired Events" ) ]
        public GameEvent levelFailedEvent;
        public GameEvent levelCompleted;
        public GameEvent event_finalStage_complete;
        public GameEvent event_projectile_disappear;
        public GameEvent event_spawn_particle_money;

    [ Title( "Level Releated" ) ]
        public SharedFloatNotifier levelProgress;
        public GunInfo gun_info_current;
        public GunInfo gun_info_default;
        public SharedFloatNotifier notif_money_currency;

// Private
        int enemy_finalStage_count;

		RecycledSequence recycledSequence_FinalStage = new RecycledSequence();
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
            enemy_finalStage_count = 0;
        }

        public void OnLevelLoadStart()
        {
			gun_info_default.ChangeData( gun_info_current );
		}

        public void OnEnemyFinalStageRegister()
        {
			enemy_finalStage_count++;
		}

		public void OnEnemyFinalStageUnRegister()
		{
			enemy_finalStage_count--;

            if( enemy_finalStage_count == 0 )
				StartLevelCompleteSequence();
		}

        public void OnMoneyGained()
        {
			notif_money_currency.SharedValue += CurrentLevelData.Instance.levelData.money_value;
		}
#endregion

#region Implementation
        void StartLevelCompleteSequence()
        {
			var sequence = recycledSequence_FinalStage.Recycle( levelCompleted.Raise );
			sequence.AppendCallback( event_finalStage_complete.Raise );
			sequence.AppendCallback( event_projectile_disappear.Raise );
			sequence.AppendCallback( event_spawn_particle_money.Raise );
			sequence.AppendInterval( GameSettings.Instance.game_event_level_complete_delay );
		}
#endregion
    }
}