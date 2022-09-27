/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Sirenix.OdinInspector;

namespace FFStudio
{
    public class UIManager : MonoBehaviour
    {
#region Fields
    [ Title( "Event Listeners" ) ]
        public EventListenerDelegateResponse levelLoadedResponse;
        public EventListenerDelegateResponse levelCompleteResponse;
        public EventListenerDelegateResponse levelFailResponse;
        public EventListenerDelegateResponse tapInputListener;

    [ Title( "UI Elements" ) ]
        public UI_Patrol_Scale level_loadingBar_Scale;
        public TextMeshProUGUI level_count_text;
        public TextMeshProUGUI level_information_text;
        public UI_Patrol_Scale level_information_text_Scale;
        public UIEntity image_level_completed;
        public UIEntity image_level_failed;
        public Image image_tapToStart;
        public Image loadingScreenImage;
        public Image foreGroundImage;
        public RectTransform tutorialObjects;
    
    [ Title( "UI Incremental Buttons" ) ]
        [ SerializeField ] UIIncrementalButtonManPower ui_incremental_manPower;
        [ SerializeField ] UIIncrementalButtonEmpty ui_incremental_money;
        [ SerializeField ] UIIncrementalButtonEmpty ui_incremental_gun_damage;
        [ SerializeField ] UIIncrementalButtonEmpty ui_incremental_gun_fireRate;
        [ SerializeField ] RectTransform ui_incremental_gun_background;

    [ Title( "Fired Events" ) ]
        public GameEvent levelRevealedEvent;
        public GameEvent loadNewLevelEvent;
        public GameEvent resetLevelEvent;
        public ElephantLevelEvent elephantLevelEvent;
#endregion

#region Unity API
        private void OnEnable()
        {
            levelLoadedResponse.OnEnable();
            levelFailResponse.OnEnable();
            levelCompleteResponse.OnEnable();
            tapInputListener.OnEnable();
        }

        private void OnDisable()
        {
            levelLoadedResponse.OnDisable();
            levelFailResponse.OnDisable();
            levelCompleteResponse.OnDisable();
            tapInputListener.OnDisable();
        }

        private void Awake()
        {
            levelLoadedResponse.response   = LevelLoadedResponse;
            levelFailResponse.response     = LevelFailResponse;
            levelCompleteResponse.response = LevelCompleteResponse;
            tapInputListener.response      = ExtensionMethods.EmptyMethod;

			level_information_text.text = string.Empty;
        }
#endregion

#region Implementation
        private void LevelLoadedResponse()
        {
			var sequence = DOTween.Sequence()
								.Append( level_loadingBar_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
								.Append( loadingScreenImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
								.AppendCallback( () => tapInputListener.response = StartLevel );

			level_count_text.text = "Level " + CurrentLevelData.Instance.currentLevel_Shown;
			EnableIncrementals_LevelStart();

            levelLoadedResponse.response = NewLevelLoaded;
        }

        private void NewLevelLoaded()
        {
			level_count_text.text       = "Level " + CurrentLevelData.Instance.currentLevel_Shown;
			level_information_text.text = "Tap to Start";
			EnableIncrementals_LevelStart();

			var sequence = DOTween.Sequence();

			// Tween tween = null;

			sequence.Append( foreGroundImage.DOFade( 0.0f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
					// .Append( tween ) // TODO: UIElements tween.
					.Append( level_information_text_Scale.DoScale_Start( GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
					.AppendCallback( () => tapInputListener.response = StartLevel );

            // elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
            // elephantLevelEvent.elephantEventType = ElephantEvent.LevelStarted;
            // elephantLevelEvent.Raise();
        }

        private void LevelCompleteResponse()
        {
            var sequence = DOTween.Sequence();

			// Tween tween = null;

			level_information_text.text = "Tap to Continue";
			EnableIncrementals_LevelEnd();

			sequence.Append( foreGroundImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
					// .Append( tween ) // TODO: UIElements tween.
					.Append( level_information_text_Scale.DoScale_Start( GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
					.Join( image_level_completed.GoToTargetPosition() )
					.AppendCallback( () => tapInputListener.response = LoadNewLevel );

            elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
            elephantLevelEvent.elephantEventType = ElephantEvent.LevelCompleted;
            elephantLevelEvent.Raise();
        }

        private void LevelFailResponse()
        {
            var sequence = DOTween.Sequence();

			// Tween tween = null;
			level_information_text.text = "Tap to Continue";
			EnableIncrementals_LevelEnd();

			sequence.Append( foreGroundImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
                    // .Append( tween ) // TODO: UIElements tween.
					.Append( level_information_text_Scale.DoScale_Start( GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
                    .Join( image_level_failed.GoToTargetPosition() )
					.AppendCallback( () => tapInputListener.response = Resetlevel );

            elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
            elephantLevelEvent.elephantEventType = ElephantEvent.LevelFailed;
            elephantLevelEvent.Raise();
        }



		private void StartLevel()
		{
			DisableIncrementals_LevelStart();
			image_tapToStart.enabled = false;

			foreGroundImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration );

			level_information_text_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration );
			level_information_text_Scale.Subscribe_OnComplete( levelRevealedEvent.Raise );

			tutorialObjects.gameObject.SetActive( false );

			tapInputListener.response = ExtensionMethods.EmptyMethod;

			elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
			elephantLevelEvent.elephantEventType = ElephantEvent.LevelStarted;
			elephantLevelEvent.Raise();
		}

		private void LoadNewLevel()
		{
			DisableIncrementals_LevelEnd();
			tapInputListener.response = ExtensionMethods.EmptyMethod;

			var sequence = DOTween.Sequence();

			sequence.Append( foreGroundImage.DOFade( 1f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
			        .Join( level_information_text_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
                    .Join( image_level_completed.GoToStartPosition() )
			        .AppendCallback( loadNewLevelEvent.Raise );
		}

		private void Resetlevel()
		{
			DisableIncrementals_LevelEnd();
			tapInputListener.response = ExtensionMethods.EmptyMethod;

			var sequence = DOTween.Sequence();

			sequence.Append( foreGroundImage.DOFade( 1f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
			        .Join( level_information_text_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
                    .Join( image_level_failed.GoToStartPosition() )
			        .AppendCallback( resetLevelEvent.Raise );
		}

        void EnableIncrementals_LevelStart()
        {
			ui_incremental_manPower.gameObject.SetActive( true );
			ui_incremental_money.gameObject.SetActive( true );

			ui_incremental_manPower.Configure();
			ui_incremental_money.Configure();
        }

		void EnableIncrementals_LevelEnd()
		{
			ui_incremental_gun_damage.gameObject.SetActive( true );
			ui_incremental_gun_fireRate.gameObject.SetActive( true );
			ui_incremental_gun_background.gameObject.SetActive( true );

			ui_incremental_gun_damage.Configure();
			ui_incremental_gun_fireRate.Configure();
		}

		void DisableIncrementals_LevelStart()
		{
			ui_incremental_manPower.gameObject.SetActive( false );
			ui_incremental_money.gameObject.SetActive( false );

			ui_incremental_manPower.Configure();
			ui_incremental_money.Configure();
		}

		void DisableIncrementals_LevelEnd()
		{
			ui_incremental_gun_damage.gameObject.SetActive( false );
			ui_incremental_gun_fireRate.gameObject.SetActive( false );

			ui_incremental_gun_background.gameObject.SetActive( false );
		}
#endregion
    }
}