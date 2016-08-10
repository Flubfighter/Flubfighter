using UnityEngine;
using System.Collections;
using System;

public class CharacterDebugController : MonoBehaviour 
{
	private bool deathTypeOpen = false;
	private Vector2 scrollArea = Vector2.zero;
	private Character character;

	void Awake()
	{
		character = GetComponent<Character>();
	}

	void OnGUI()
	{
		Rect panelArea = new Rect(Screen.width - 400f, 0f, 400f, Screen.height);
//		GUI.Box(panelArea, GUIContent.none);
		GUILayout.BeginArea(panelArea);
		scrollArea = GUILayout.BeginScrollView(scrollArea);
		{
			#region Death
			deathTypeOpen = GUILayout.Toggle(deathTypeOpen, "Death Types");
			if(deathTypeOpen)
			{
				foreach (var deathType in Enum.GetValues(typeof(DeathType)))
				{
					if(GUILayout.Button (deathType.ToString()))
						character.Die((DeathType)deathType);
				}
			}
			#endregion
		}
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}
}
