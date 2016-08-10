using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(CharacterSounds.AudioField))]
public class AudioFieldDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty (position, label, property);
		position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);
		
		float space = position.width / 8f;
		Rect clipRect = new Rect(
			position.x, 
			position.y, 
			space * 5f, 
			position.height);
		Rect volumeRect = new Rect(
			position.x + space * 5f, 
			position.y, 
			space * 3f, 
			position.height);
		
		EditorGUIHelper.LabeledPropertyField(clipRect, property.FindPropertyRelative ("clip"), "Clip");
		EditorGUIHelper.LabeledPropertyField(volumeRect, property.FindPropertyRelative ("volume"), "Vol %");
		
		EditorGUI.EndProperty ();
	}
}

