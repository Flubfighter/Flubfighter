//using UnityEngine;
//using UnityEditor;
//using UnityEditor.AnimatedValues;
//using System.Collections;
//using System.Collections.Generic;
//
//public class InputStateEditorWindow : EditorWindow
//{
//	Dictionary<string, AnimBool> opened = new Dictionary<string, AnimBool>();
//	Vector2 scroll;
//	InputState activeState;
//
//	[MenuItem ("Edit/Project Settings/Control Profiles")]
//	public static void  ShowWindow () 
//	{
//		EditorWindow.GetWindow<InputStateEditorWindow> (true, "Profile Editor");
//	}
//
//	void StateButton(InputState state)
//	{
//		GUI.enabled = (activeState != state);
//		GUI.color = (activeState == state ? Color.green : Color.white);
//		if(GUILayout.Button("Player " + state.Index))
//		{
//			activeState = state;
//		}
//	}
//	
//	void OnGUI () 
//	{
//		if(activeState == null)	activeState = InputState.Get(1);
//
//		GUILayout.BeginHorizontal();
//		StateButton(InputState.Get (1));
//		StateButton(InputState.Get (2));
//		StateButton(InputState.Get (3));
//		StateButton(InputState.Get (4));
//		GUI.enabled = true;
//		GUI.color = Color.white;
//		GUILayout.EndHorizontal();
//
//		GUILayout.Space (3f);
//
//		EditorGUIExtras.BeginBlock();
//		scroll = EditorGUILayout.BeginScrollView(scroll);
//		{
//			EditorGUI.BeginChangeCheck();
//			foreach(KeyValuePair<string, InputState.Button> pair in activeState.buttons)
//			{
//				HandleControl(pair.Key, pair.Value);
//				EditorGUIExtras.Separator();
//			}
//			foreach(KeyValuePair<string, InputState.Axis> pair in activeState.axes)
//			{
//				HandleControl(pair.Key, pair.Value);
//				EditorGUIExtras.Separator();
//			}
//			if(EditorGUI.EndChangeCheck())
//				InputState.Save(activeState);
//		}
//		EditorGUILayout.EndScrollView();
//		EditorGUIExtras.EndBlock();
//	}
//
//	void HandleControl(string name, InputState.Button button)
//	{
//		if(!opened.ContainsKey(name))
//			opened.Add(name, new AnimBool(false, Repaint));
//
//		opened[name].target = EditorGUILayout.Foldout(opened[name].target, name + " (Button)");
//		if(EditorGUILayout.BeginFadeGroup(opened[name].faded))
//		{
//			EditorGUI.indentLevel++;
//			button.xbox = (InputState.Button.XboxButton)EditorGUILayout.EnumPopup("Xbox", button.xbox);
//			button.keyboard = (KeyCode)EditorGUILayout.EnumPopup("Keyboard", button.keyboard);
//			EditorGUI.indentLevel--;
//		}
//		EditorGUILayout.EndFadeGroup();
//	}
//
//	void HandleControl(string name, InputState.Axis axis)
//	{
//		if(!opened.ContainsKey(name))
//			opened.Add(name, new AnimBool(false, Repaint));
//		
//		opened[name].target = EditorGUILayout.Foldout(opened[name].target, name + " (Axis)");
//		if(EditorGUILayout.BeginFadeGroup(opened[name].faded))
//		{
//			EditorGUI.indentLevel++;
//			axis.xbox = (InputState.Axis.XboxAxis)EditorGUILayout.EnumPopup("Xbox", axis.xbox);
//			axis.secondaryXbox = (InputState.Axis.XboxAxis)EditorGUILayout.EnumPopup("Secondary Xbox", axis.secondaryXbox);
//			axis.keyboardPositive = (KeyCode)EditorGUILayout.EnumPopup("Keyboard +", axis.keyboardPositive);
//			axis.keyboardNegative = (KeyCode)EditorGUILayout.EnumPopup("Keyboard -", axis.keyboardNegative);
//			axis.sensitivity = EditorGUILayout.FloatField("Sensitivity", axis.sensitivity);
//			axis.gravity = EditorGUILayout.FloatField("Gravity", axis.gravity);
//			EditorGUI.indentLevel--;
//		}
//		EditorGUILayout.EndFadeGroup();
//	}
//}
