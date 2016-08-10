using UnityEngine;
using System.Collections;
using System.Linq;

public class MainMenuCharacterSelector : MonoBehaviour
{

	public int playerNumber;

	private Player player;

	void Start()
	{
		if(playerNumber > 4 || playerNumber < 1)
			Debug.LogError(name + " does not have a valid player number!");
		else
		{
			player = Player.All[playerNumber - 1];
			UpdateDisplay();
		}
	}

	public static MainMenuCharacterSelector GetCharacterSelectorByIndex(int index)
	{
		try
		{
			return FindObjectsOfType<MainMenuCharacterSelector>().First(x => x.playerNumber == index);
		}
		catch
		{
			Debug.LogError("could not find a character selector with index: " + index);
			return null;
		}
	}
	
	public void Activate()
	{
		Next ();
		UpdateDisplay();
	}
	
	public void Previous()
	{
		FindObjectOfType<MainMenu>().NextTeam(player);
		UpdateDisplay();
	}
	
	public void Next()
	{
		FindObjectOfType<MainMenu>().PreviousTeam(player);
		UpdateDisplay();
	}
	
	public void UpdateDisplay()
	{
		SkinnedMeshRenderer body = GetComponentInChildren<SkinnedMeshRenderer>();
		body.material = player.team.characterMaterial;
		body.renderer.enabled = player.enabled;
	}
	
	void OnMouseDown()
	{
		Activate();
	}
}
