/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.Events;
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
        public GameEvent event_level_started;
        public GameEvent event_finalStage_complete;
        public GameEvent event_projectile_disappear;
        public GameEvent event_spawn_particle_money;
        public GameEvent event_enemy_finalStage_run;
        public GameEvent event_ally_finalStage_shoot;
        public IntGameEvent event_ally_spawn;

    [ Title( "Level Releated" ) ]
        public SharedFloatNotifier levelProgress;
        public GunInfo gun_info_current;
        public GunInfo gun_info_default;
        public SharedFloatNotifier notif_money_currency;
        public SharedIntNotifier notif_ally_count;

// Private
        int enemy_finalStage_count;
        int ally_finalStage_count;

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
			var levelData = CurrentLevelData.Instance.levelData;

			levelProgress.SetValue_NotifyAlways( 0 );

			enemy_finalStage_count = 0;
			ally_finalStage_count  = 0;

            // Spawn allies according to level data

            // Set Active Scene.
			if( levelData.scene_overrideAsActiveScene )
				SceneManager.SetActiveScene( SceneManager.GetSceneAt( 1 ) );
            else
				SceneManager.SetActiveScene( SceneManager.GetSceneAt( 0 ) );
		}

        // Info: Called from Editor.
        public void LevelRevealedResponse()
        {
			event_level_started.Raise();
		}

        // Info: Called from Editor.
        public void LevelStartedResponse()
        {
            enemy_finalStage_count = 0;
        }

        public void OnLevelLoadStart()
        {
			gun_info_current.ChangeData( gun_info_default );
		}

        public void OnEnemyFinalStageRegister()
        {
			enemy_finalStage_count++;
		}

		public void OnEnemyFinalStageUnRegister()
		{
			enemy_finalStage_count--;

            if( enemy_finalStage_count <= 0 )
				StartLevelCompleteSequence();
		}

		public void OnAllyFinalStageRegister()
		{
			ally_finalStage_count++;
		}

		public void OnAllyFinalStageUnRegister()
		{
			ally_finalStage_count--;

			if( ally_finalStage_count <= 0 )
				FinalStageSequence();
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

        void FinalStageSequence()
        {
			var sequence = recycledSequence_FinalStage.Recycle();
			sequence.AppendCallback( event_enemy_finalStage_run.Raise );
			sequence.AppendInterval( GameSettings.Instance.game_finalStage_shoot_delay );
			sequence.AppendCallback( event_ally_finalStage_shoot.Raise );
		}
#endregion
    }
}