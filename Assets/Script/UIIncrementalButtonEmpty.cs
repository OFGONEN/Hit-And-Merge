/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FFStudio;
using Sirenix.OdinInspector;

public class UIIncrementalButtonEmpty : MonoBehaviour
{
#region Fields
  [ Title( "Shared" ) ]
    [ SerializeField ] Currency notif_currency;
    [ SerializeField ] IncrementalEmpty incremental_empty;

  [ Title( "Setup" ) ]
    [ SerializeField ] Color color_positive;
    [ SerializeField ] Color color_inactive;
    [ SerializeField ] Color color_negative;

  [ Title( "Components" ) ]
    [ SerializeField ] Button _button;
    [ SerializeField ] TextMeshProUGUI text_value;
    [ SerializeField ] TextMeshProUGUI text_cost;

	IncrementalEmptyData incremental;
	int incremental_index;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void Configure()
    {
		incremental_index = PlayerPrefsUtility.Instance.GetInt( incremental_empty.IncrementalKey, 0 );
		incremental       = incremental_empty.ReturnIncrementalAtIndex( incremental_index );

		text_value.text = "Lvl " + ( incremental_index + 1 );
		text_cost.text  = incremental.incremental_cost.ToString();

		if( incremental_index >= incremental_empty.IncrementalCount - 1 )
			InactiveButton();
		else if( incremental.incremental_cost > notif_currency.sharedValue )
			DisableButton();
		else
			EnableButton();
	}

	public void OnButtonPress()
	{
		incremental_index++;

		PlayerPrefsUtility.Instance.SetInt( incremental_empty.IncrementalKey, incremental_index );

		notif_currency.SharedValue -= incremental.incremental_cost;
		notif_currency.SaveCurrency();
	}
#endregion

#region Implementation
    void InactiveButton()
	{
		_button.interactable = false;
		text_cost.color      = color_inactive;
	}
    void DisableButton()
    {
		_button.interactable = false;
		text_cost.color      = color_negative;
	}

    void EnableButton()
    {
		_button.interactable = true;
		text_cost.color      = color_positive;
    }
#endregion
}
