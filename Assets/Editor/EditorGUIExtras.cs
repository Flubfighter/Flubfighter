using UnityEngine;
using UnityEditor;
using System.Collections;

public static class EditorGUIExtras
{
	public static void TitleBlock(string title)
	{
		//		GUI.color = color;
		GUILayout.Label(title, EditorStyles.toolbarButton);
		//		GUI.color = Color.white;
		//		Separator();
	}
	
	public static void Separator()
	{
		Color start = GUI.color;
		Rect reserved = GUILayoutUtility.GetLastRect();
		reserved = GUILayoutUtility.GetRect(reserved.width, 2f);
		GUI.color = new Color(0.6f, 0.6f, 0.6f, 1f);
		GUI.Box(reserved, "");
		//GUILayout.Space(3);
		GUI.color = start;
	}
	
	public static void BeginBlock()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Space(4f);
		EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.ExpandHeight(true));
		GUILayout.BeginVertical();
		GUILayout.Space(2f);
	}
	
	public static void EndBlock()
	{
		GUILayout.Space(3f);
		GUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(3f);
		GUILayout.EndHorizontal();
		GUILayout.Space(3f);
	}
}
