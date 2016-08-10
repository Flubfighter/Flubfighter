using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameTypeSwitcher : SelectableButton 
{
	public Text description;

	void Start()
	{
		UpdateDisplay();
	}

	public override void Activate()
	{
//		Debug.Log("activate from GameTypeSwitcher");
		Next ();
		UpdateDisplay();
		FindObjectOfType<MainMenu>().currentMenu = areaToChangeTo;
	}
	
	public void Previous()
	{
		MainMenu.PreviousGametype();
		UpdateDisplay();
	}
	
	public void Next()
	{
		MainMenu.NextGametype();
		UpdateDisplay();
	}
	
	void UpdateDisplay()
	{
		// TODO: Remove TextMesh part once the new Game Type menu is fully implemented
		TextMesh textMesh = GetComponentInChildren<TextMesh>();
		Text text = GetComponentInChildren<Text>();
		if(textMesh)
			textMesh.text = Assets.Gametypes[MainMenu.gametypeIndex].name;
		if(text)
			text.text = Assets.Gametypes[MainMenu.gametypeIndex].name;

        if(description)
        {
            description.text = "Durration: " + Assets.Gametypes[MainMenu.gametypeIndex].duration;
		    description.text += "\nFriendly Fire: " + Assets.Gametypes[MainMenu.gametypeIndex].friendlyFire;
		    description.text += "\nLives: " + Assets.Gametypes[MainMenu.gametypeIndex].lives;
		    description.text += "\nScore To Win Round: " + Assets.Gametypes[MainMenu.gametypeIndex].scoreToWinRound;
		    description.text += "\nSuicide Rewpan Penalty: " + Assets.Gametypes[MainMenu.gametypeIndex].suicideRespawnPenalty;
		    description.text += "\nSuicide Score Value: " + Assets.Gametypes[MainMenu.gametypeIndex].suicideScoreValue;
		    description.text += "\nTeam Kill Score Value: " + Assets.Gametypes[MainMenu.gametypeIndex].teamkillScoreValue;
		    description.text += "\nTeam Mode: " + Assets.Gametypes[MainMenu.gametypeIndex].teamMode;
        }
	}
	
	void OnMouseDown()
	{
		FindObjectOfType<Selector>().SendActivate();
	}
}
