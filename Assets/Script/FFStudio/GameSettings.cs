﻿/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace FFStudio
{
	public class GameSettings : ScriptableObject
    {
#region Fields (Settings)
    // Info: You can use Title() attribute ONCE for every game-specific group of settings.
    [ Title( "Ally" ) ]
        [ LabelText( "Ally Movement Speed In Group" ) ] public float ally_movement_speed_group;
        [ LabelText( "Ally Movement Speed In FinishLine" ) ] public float ally_movement_speed_finishLine;
        [ LabelText( "Ally spawn radius" ) ] public float ally_spawn_radius;
        [ LabelText( "Ally finish line radius" ) ] public float ally_finishLine_radius;
        [ LabelText( "Ally buffer spawn radius" ) ] public float ally_spawn_radius_buffer;
        
    [ Title( "Ally Group" ) ]
        [ LabelText( "Ally Group Max Count" ) ] public int ally_group_count_max;
        [ LabelText( "Ally Group Movement Speed Forward" ) ] public float ally_group_movement_speed_forward;
        [ LabelText( "Ally Group Movement Speed Lateral" ) ] public float ally_group_movement_speed_lateral;
        [ LabelText( "Ally Group Movement Clamp" ) ] public float ally_group_movement_clamp;
        [ LabelText( "Ally Group ReArrange Delay" ) ] public float ally_group_reArrange_delay = 0.75f;

    [ Title( "Enemy" ) ]
        [ LabelText( "Final Stage Enemy Movement Speed" ) ] public float enemy_finalStage_movement_speed;
        [ LabelText( "Platform Enemy Spawn Radius" ) ] public float enemy_spawn_radius;
        [ LabelText( "Platform Enemy Path Look At" ) ] public float enemy_movement_lookAt = 1f;
        [ LabelText( "Enemy Death Duration" ) ] public float enemy_death_duration;
        [ LabelText( "Enemy Death Color" ) ] public Color enemy_death_color;
        [ LabelText( "Enemy Color" ) ] public Color enemy_color;

    [ Title( "Projectile" ) ]
        [ LabelText( "Projectile Travel Distance" ) ] public float projectile_travel_distance;

    [ Title( "Gate" ) ]
        [ LabelText( "Gate Height" ) ] public float gate_height;
        [ LabelText( "Gate Size" ) ] public float gate_size = 5;
        [ LabelText( "Gate Merge Buffer" ) ] public float gate_merge_buffer = 0.1f;
        [ LabelText( "Gate Color Positive" ) ] public Color gate_color_positive;
        [ LabelText( "Gate Color Negative" ) ] public Color gate_color_negative;
        [ LabelText( "Gate Color Locked" ) ] public Color gate_color_locked;
	    [ LabelText( "Gate UI Spawn Height Ratio Bottom" ) ] public float gate_ui_spawn_height_ratio_bottom; 
	    [ LabelText( "Gate UI Spawn Height Ratio Upper" ) ] public float gate_ui_spawn_height_ratio_upper; 
	    [ LabelText( "Gate UI Spawn Offset" ) ] public float gate_ui_spawn_offset; 
	    [ LabelText( "Gate UI Spawn Damage Size" ) ] public float gate_ui_spawn_damage_size; 
	    [ LabelText( "Gate UI Spawn Damage Color" ) ] public Color gate_ui_spawn_damage_color; 
	    [ LabelText( "Gate UI Spawn Money Size" ) ] public float gate_ui_spawn_money_size; 
	    [ LabelText( "Gate UI Spawn Money Color" ) ] public Color gate_ui_spawn_money_color; 
	    [ LabelText( "Gate UI Canvas Float Position" ) ] public float gate_ui_canvas_float_position; // Absolute position 
	    [ LabelText( "Gate UI Canvas Float Duration" ) ] public float gate_ui_canvas_float_duration; 
        [ LabelText( "Gate UI Text Punch Value" ) ] public float gate_ui_text_punch_value;  
	    [ LabelText( "Gate UI Text Punch Duration" ) ] public float gate_ui_text_punch_duration;  
	    [ LabelText( "Gate Merge Size Cofactor" ) ] public float gate_merge_size_cofactor;  
	    [ LabelText( "Gate Merge Duration" ) ] public float gate_merge_duration;  
	    [ LabelText( "Gate Merge Punch Value" ) ] public float gate_merge_punch_value;  
	    [ LabelText( "Gate Merge Punch Duration" ) ] public float gate_merge_punch_duration;  
    
    [ Title( "Camera" ) ]
        [ LabelText( "Follow Speed (Z)" ), SuffixLabel( "units/seconds" ), Min( 0 ) ] public float camera_follow_speed_depth = 2.8f;
        [ LabelText( "End Level Offset" ) ] public Vector3 camera_finalStage_offset;
        [ LabelText( "End Level Offset Tween Duration" ) ] public float camera_finalStage_duration;
    
    [ Title( "Game" ) ]
        [ LabelText( "Level Complete Event Raise Delay" ) ] public float game_event_level_complete_delay;
        [ LabelText( "Input Resolution" ) ] public float game_input_resolution;
        [ LabelText( "Final Stage Shooting Delay" ) ] public float game_finalStage_shoot_delay = 0.35f;
        [ LabelText( "Final Stage Offset" ) ] public float game_finalStage_offset;
        [ LabelText( "DoTween Tween Count" ) ] public int game_tween_count = 500;
        [ LabelText( "DoTween Sequence Count" ) ] public int game_sequence_count = 50;
    
    [ Title( "Project Setup", "These settings should not be edited by Level Designer(s).", TitleAlignments.Centered ) ]
        public int maxLevelCount;

    [ Title( "UI Particle" ) ]
        [ LabelText( "Random Spawn Rotation Range") ] public float ui_particle_spawn_rotationRange;
        [ LabelText( "Spawn Size") ] public float ui_particle_spawn_size;
        [ LabelText( "Spawn Delay Max") ] public float ui_particle_spawn_delay;
        [ LabelText( "Spawn Duration") ] public float ui_particle_spawn_duration;
        [ LabelText( "Spawn Ease") ] public Ease ui_particle_spawn_ease;
        [ LabelText( "Return Default Duration") ] public float ui_particle_return_duration;
        [ LabelText( "Return Default Ease") ] public Ease ui_particle_return_ease;
        [ LabelText( "Target Movement Duration") ] public float ui_particle_movement_duration;
        [ LabelText( "Target Movement Ease") ] public Ease ui_particle_movement_ease;
        [ LabelText( "Target Movement End Size") ] public float ui_particle_movement_size_end;
        [ LabelText( "Target Movement End Size Duration") ] public float ui_particle_movement_size_end_duration;
        
        // Info: 3 groups below (coming from template project) are foldout by design: They should remain hidden.
		[ FoldoutGroup( "Remote Config" ) ] public bool useRemoteConfig_GameSettings;
        [ FoldoutGroup( "Remote Config" ) ] public bool useRemoteConfig_Components;

        [ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the movement for ui element"          ) ] public float ui_Entity_Move_TweenDuration;
        [ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the fading for ui element"            ) ] public float ui_Entity_Fade_TweenDuration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the scaling for ui element"           ) ] public float ui_Entity_Scale_TweenDuration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the movement for floating ui element" ) ] public float ui_Entity_FloatingMove_TweenDuration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Joy Stick"                                        ) ] public float ui_Entity_JoyStick_Gap;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Pop Up Text relative float height"                ) ] public float ui_PopUp_height;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Pop Up Text float duration"                       ) ] public float ui_PopUp_duration;
		// [ FoldoutGroup( "UI Settings" ), Tooltip( "UI Particle Random Spawn Area in Screen" ), SuffixLabel( "percentage" ) ] public float ui_particle_spawn_width; 
		// [ FoldoutGroup( "UI Settings" ), Tooltip( "UI Particle Spawn Duration" ), SuffixLabel( "seconds" ) ] public float ui_particle_spawn_duration; 
		// [ FoldoutGroup( "UI Settings" ), Tooltip( "UI Particle Spawn Ease" ) ] public Ease ui_particle_spawn_ease;
		// [ FoldoutGroup( "UI Settings" ), Tooltip( "UI Particle Wait Time Before Target" ) ] public float ui_particle_target_waitTime;
		// [ FoldoutGroup( "UI Settings" ), Tooltip( "UI Particle Target Travel Time" ) ] public float ui_particle_target_duration;
		// [ FoldoutGroup( "UI Settings" ), Tooltip( "UI Particle Target Travel Ease" ) ] public Ease ui_particle_target_ease;
        [ FoldoutGroup( "UI Settings" ), Tooltip( "Percentage of the screen to register a swipe"     ) ] public int swipeThreshold;
        [ FoldoutGroup( "UI Settings" ), Tooltip( "Safe Area Base Top Offset" ) ] public int ui_safeArea_offset_top = 88;

        [ FoldoutGroup( "Debug" ) ] public float debug_ui_text_float_height;
        [ FoldoutGroup( "Debug" ) ] public float debug_ui_text_float_duration;
#endregion

#region Property
        public float UIParticleDuration => ui_particle_spawn_duration + ui_particle_return_duration + ui_particle_movement_duration;
        public float AllyLateralCountOnFinalStage => Mathf.FloorToInt( ally_group_movement_clamp * 2f / ally_finishLine_radius );
#endregion

#region Fields (Singleton Related)
        static GameSettings instance;

        delegate GameSettings ReturnGameSettings();
        static ReturnGameSettings returnInstance = LoadInstance;

		public static GameSettings Instance => returnInstance();
#endregion

#region Implementation
        static GameSettings LoadInstance()
		{
			if( instance == null )
				instance = Resources.Load< GameSettings >( "game_settings" );

			returnInstance = ReturnInstance;

			return instance;
		}

		static GameSettings ReturnInstance()
        {
            return instance;
        }
#endregion
    }
}