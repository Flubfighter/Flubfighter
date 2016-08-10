using UnityEngine;
using System.Collections;

public class Team : ScriptableObject
{
	public string shortName;
	public int menuOrder;
	public Color guiColor = Color.white;
	public Color particleColor = Color.white;
	public Material characterMaterial;
	public Color fistTrailColor;
}
