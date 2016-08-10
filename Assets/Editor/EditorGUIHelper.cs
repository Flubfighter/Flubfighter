using UnityEngine;
using UnityEditor;
using System.Collections;

public static class EditorGUIHelper
{
	public static void LabeledPropertyField (Rect rect, SerializedProperty property, string label)
	{
		Rect fieldRect = rect;
		float offset = GUI.skin.label.CalcSize (new GUIContent (label)).x + 3f;
		fieldRect.x += offset;
		fieldRect.width -= offset + 3f;
		EditorGUI.LabelField (rect, label);
		EditorGUI.PropertyField (fieldRect, property, GUIContent.none);
	}
}

