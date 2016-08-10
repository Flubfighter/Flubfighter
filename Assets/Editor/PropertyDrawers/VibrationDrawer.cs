using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(CharacterHaptics.Vibration))]
public class VibrationDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty (position, label, property);
		position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);

		float space = position.width / 4f;
		Rect leftRect = new Rect(position.x, position.y, space, position.height);
		Rect rightRect = new Rect(position.x+space, position.y, space, position.height);
		Rect durationRect = new Rect(position.x+space*2, position.y, space, position.height);
		Rect silencedRect = new Rect (position.x + space * 3, position.y, space, position.height);

		EditorGUIHelper.LabeledPropertyField(leftRect, property.FindPropertyRelative ("left"), "L");
		EditorGUIHelper.LabeledPropertyField(rightRect, property.FindPropertyRelative ("right"), "R");
		EditorGUIHelper.LabeledPropertyField(durationRect, property.FindPropertyRelative ("duration"), "T");
		EditorGUIHelper.LabeledPropertyField(silencedRect, property.FindPropertyRelative ("silenced"), "Silent");

		EditorGUI.EndProperty ();
	}
}

