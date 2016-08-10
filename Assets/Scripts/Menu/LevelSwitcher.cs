using UnityEngine;
using System.Collections;

public class LevelSwitcher : SelectableButton
{
	public TextMesh textMesh;
	public SpriteRenderer sprite;
	public static LevelSwitcher instance;

	void Start()
	{
		instance = this;
		UpdateDisplay();
	}
	
	public override void Activate()
	{
//		Debug.Log("activate from LevelSwitcher");
		Next ();
		FindObjectOfType<MainMenu>().currentMenu = areaToChangeTo;
	}
	
	public void Previous()
	{
		MainMenu.PreviousMap();
		UpdateDisplay();
	}
	
	public void Next()
	{
		MainMenu.NextMap();
		UpdateDisplay();
	}
	
	void UpdateDisplay()
	{
		if(textMesh == null)
			Debug.LogError(name + " does not have a text mesh reference!");
		else
		{
			textMesh.text = Assets.Worlds[MainMenu.worldIndex].maps[MainMenu.mapIndex].properName.Replace(' ', '\n');
//			SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
			Texture2D t = Assets.Worlds[MainMenu.worldIndex].maps[MainMenu.mapIndex].image as Texture2D;
			sprite.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero);
		}
	}
	
	void OnMouseDown()
	{
		FindObjectOfType<Selector>().SendActivate();
	}
}
