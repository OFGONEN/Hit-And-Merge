/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "tool_level_creator", menuName = "FFEditor/Tool/Level Creator" ) ]
public class LevelCreator : ScriptableObject
{
  [ Title( "Environment Setup" ) ]
    [ SerializeField ] public int ground_count;
    [ SerializeField ] public float environment_offset;

  [ Title( "Data Setup" ) ]
    [ SerializeField ] GroundData data_ground;
    [ SerializeField ] FinishLineData data_finishLine;

    [ Button() ]
    public void CreateEnvironment()
    {
		var environmentParent = GameObject.Find( "environment" ).transform;
   
		// EditorUtility.SetDirty( environmentParent );
		EditorSceneManager.MarkAllScenesDirty();
		environmentParent.DestoryAllChildren();
		environmentParent.position = Vector3.forward * environment_offset;

		int i;
		for( i = 0; i < ground_count; i++ )
        {
			var ground = PrefabUtility.InstantiatePrefab( data_ground.ground_object ) as GameObject;
			ground.name = "ground_" + ( i + 1 );
			ground.transform.SetParent( environmentParent );
			ground.transform.localPosition = Vector3.forward * i * data_ground.ground_length;
		}

		i -= 1;

		var finishLine = PrefabUtility.InstantiatePrefab( data_finishLine.finishLine_object ) as GameObject;
		finishLine.transform.SetParent( environmentParent );
        finishLine.transform.localPosition = Vector3.forward * ( i * data_ground.ground_length + data_finishLine.finishLine_offset );

		AssetDatabase.SaveAssets();
	}
}

[ Serializable ]
public struct GroundData
{
	public GameObject ground_object;
    public float ground_length;
}

[ Serializable ]
public struct FinishLineData
{
	public GameObject finishLine_object;
    public float finishLine_offset;
}