using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(CharacterParticles.ParticleBlock))]
public class ParticleBlockDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		
		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		
		Rect leftRect = new Rect(position.x, position.y, position.width - 100f, position.height);
		Rect labelRect = new Rect (position.x + leftRect.width, position.y, 80f, position.height);
		Rect rightRect = new Rect(position.x + leftRect.width + labelRect.width, position.y, 10f, position.height);

		EditorGUI.PropertyField(leftRect, property.FindPropertyRelative ("particle"), GUIContent.none);
		EditorGUI.LabelField (labelRect, "Team Color");
		EditorGUI.PropertyField(rightRect, property.FindPropertyRelative ("matchTeamColor"), GUIContent.none);

		EditorGUI.indentLevel = indent;		
		EditorGUI.EndProperty();
	}
}

