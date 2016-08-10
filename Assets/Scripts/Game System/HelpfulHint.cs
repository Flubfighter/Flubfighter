using UnityEngine;
using System.Collections;

public class HelpfulHint : ScriptableObject
{
	[TextArea]
	public string message;
	public Sprite image;
}