using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ControlButton : SelectableButton
{
	public int playerNumber = -1;

	private int currentIndex;

	void Start()
	{
		if(playerNumber == -1)
		{
			playerNumber = int.TryParse(GetComponent<TextMesh>().text.Substring(1, 1), out playerNumber) ? playerNumber : -1;
			if(!(playerNumber > 0) || !(playerNumber < 5))
				Debug.LogError(name + ": does not have valid playerNumber");
		}
		UpdateDisplay();
	}

	public override void Activate()
	{
		Next ();
		UpdateDisplay();
		MainMenu myMenu = FindObjectOfType<MainMenu>();
		myMenu.currentMenu = areaToChangeTo;
	}

	public static ControlButton GetControlByIndex(int index)
	{
		try
		{
			ControlButton q = FindObjectsOfType<ControlButton>().First(x => x.playerNumber == index);
			Debug.Log("FINISHED");
			return q;
		}
		catch
		{
			Debug.LogError("could not find a character selector with index: " + index);
			return null;
		}
	}

	void UpdateControls()
	{
		string newProfile = MainMenu.controlProfileOptionsStatic[currentIndex];
		if(newProfile == "Keyboard")
			newProfile += playerNumber.ToString();
		foreach(MainMenuCharacterSelector m in new List<MainMenuCharacterSelector>(FindObjectsOfType<MainMenuCharacterSelector>()))
			m.UpdateDisplay();
	}

	public void Previous()
	{
		currentIndex = Mathf.RoundToInt(Mathf.Repeat(--currentIndex, MainMenu.controlProfileOptionsStatic.Count));
		UpdateControls();
		UpdateDisplay();
	}

	public void Next()
	{
		currentIndex = Mathf.RoundToInt(Mathf.Repeat(++currentIndex, MainMenu.controlProfileOptionsStatic.Count));
		UpdateControls();
		UpdateDisplay();
	}

	void UpdateDisplay()
	{

	}

	void OnMouseDown()
	{
		Activate();
	}
}
