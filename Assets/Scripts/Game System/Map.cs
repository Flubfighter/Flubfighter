using UnityEngine;
using System.Collections;

public class Map : ScriptableObject
{
	public string properName;
	public string sceneName;
	public Texture image;
	public string description;
	public Item[] defaultDisabledItems;
}
