using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class NewMenu : MonoBehaviour
{
	[Header("GameType")]
	public Text gameTypeTitle;
	public Text gameTypeDiscription;
	[Header("MapSelection")]
	[Tooltip("Should contain all level name text objects.")]
	public Text[] mapTitles;
	public Text worldTitle;
	public Text mapTitle;
	public Image mapPreview;
	[Header("Controls")]
	public Image controlsImage;
	public Sprite[] controlsImages;

	private int selectedMapIndex = 0;
	private int controlsImageIndex = 0;
	private Button backButton;

	public void SetBackButton(Button newBackButton) { backButton = newBackButton; }

	// Current Menu bools // // Because I can have enum dropdowns for componenets but not OnClick events apparently... (Thanks Unity 4.6) //
	private bool onCharacterSelect = false;
	private bool onControlScreen = false;
	private bool onMapMenu = false;

	// Set bools through GUI //
	public void SetOnCharacterSelect(bool newValue)	{ onCharacterSelect	= newValue; }
	public void SetOnControlScreen(bool newValue)	{ onControlScreen	= newValue; }
	public void SetOnMapMenu(bool newValue)			{ onMapMenu			= newValue; }

	// MainMenu wrapers for GUI //
	public void NextGameType()		{ MainMenu.NextGametype();		UpdateGameTypeDisplay(); }
	public void PreviousGameType()	{ MainMenu.PreviousGametype();	UpdateGameTypeDisplay(); }
	public void NextWorld()			{ mapTitles[MainMenu.mapIndex].color = Color.white; MainMenu.NextWorld();		UpdateLevelNamesDisplay(); }
	public void PreviousWorld()		{ mapTitles[MainMenu.mapIndex].color = Color.white; MainMenu.PreviousWorld();	UpdateLevelNamesDisplay(); }
	public void NextMap()			{ mapTitles[MainMenu.mapIndex].color = Color.white;	MainMenu.NextMap();			UpdateLevelSelectionDisplay(); }
	public void PreviouwMap()		{ mapTitles[MainMenu.mapIndex].color = Color.white;	MainMenu.PreviousMap();		UpdateLevelSelectionDisplay(); }
	public void StartGame()			{ MainMenu.Instance.BeginGame(); }

	void Start()
	{
		controlsImage.sprite = controlsImages[controlsImageIndex];
		UpdateLevelNamesDisplay();
		UpdateGameTypeDisplay();
	}

	void Update()
	{
		// Handle input in ways Unity GUI can't
		Player p = Player.All.FirstOrDefault(item => item.input.GetAxis("Horizontal") != 0);
		if(p != null)
			HandlePlayerInputHorizontal(p);
		else
		{
			p = Player.All.FirstOrDefault(item => item.input.GetAxis("Vertical") != 0);
			if(p != null)
				HandlePlayerInputVertical(p);
		}
		if (Player.One.input.GetButtonDown ("Back"))
		{
			if(backButton)
				backButton.onClick.Invoke();
			else
				Debug.LogWarning("Back button request but no button set.");
		}
	}

	// Display Updates //
	private void UpdateGameTypeDisplay()
	{
		gameTypeTitle.text = Assets.Gametypes[MainMenu.gametypeIndex].name;
		gameTypeDiscription.text = "Durration: " + Assets.Gametypes[MainMenu.gametypeIndex].duration;
		gameTypeDiscription.text += "\nFriendly Fire: " + Assets.Gametypes[MainMenu.gametypeIndex].friendlyFire;
		gameTypeDiscription.text += "\nLives: " + Assets.Gametypes[MainMenu.gametypeIndex].lives;
		gameTypeDiscription.text += "\nScore To Win Round: " + Assets.Gametypes[MainMenu.gametypeIndex].scoreToWinRound;
		gameTypeDiscription.text += "\nSuicide Rewpan Penalty: " + Assets.Gametypes[MainMenu.gametypeIndex].suicideRespawnPenalty;
		gameTypeDiscription.text += "\nSuicide Score Value: " + Assets.Gametypes[MainMenu.gametypeIndex].suicideScoreValue;
		gameTypeDiscription.text += "\nTeam Kill Score Value: " + Assets.Gametypes[MainMenu.gametypeIndex].teamkillScoreValue;
		gameTypeDiscription.text += "\nTeam Mode: " + Assets.Gametypes[MainMenu.gametypeIndex].teamMode;
	}
	public void ToggleControlsImage()
	{
		controlsImageIndex++;
		if (controlsImageIndex >= controlsImages.Length)
			controlsImageIndex = 0;
		controlsImage.sprite = controlsImages[controlsImageIndex];
	}
	private void UpdateLevelNamesDisplay()
	{
		worldTitle.text = Assets.Worlds[MainMenu.worldIndex].properName;
		foreach(Text t in mapTitles)
			t.enabled = false; // Disable all text

		for(int i=0; i < Assets.Worlds[MainMenu.worldIndex].maps.Length; i++)
		{
			mapTitles[i].enabled = true; // Enable the ones we need
			mapTitles[i].text = Assets.Worlds[MainMenu.worldIndex].maps[i].properName;
		}
		UpdateLevelSelectionDisplay();
	}
	private void UpdateLevelSelectionDisplay()
	{
		// TODO: When completely switched to new menu, make the textures sprites so we don't have to convert.
		mapTitle.text = Assets.Worlds[MainMenu.worldIndex].maps[MainMenu.mapIndex].properName;
		mapTitles[MainMenu.mapIndex].color = Color.blue;
		Texture2D t = Assets.Worlds[MainMenu.worldIndex].maps[MainMenu.mapIndex].image as Texture2D;
		mapPreview.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero);
	}

	// Input //
	void HandlePlayerInputHorizontal(Player player)
	{
		if(player.menuControlsEnabled)
		{
			player.menuControlsEnabled = false;
			StartCoroutine("RestoreControls", player);

			if(onMapMenu)
			{
				mapTitles[MainMenu.mapIndex].color = Color.white;
				if(player.input.GetAxis("Horizontal") > 0)
					MainMenu.NextWorld();
				else
					MainMenu.PreviousWorld();
				UpdateLevelNamesDisplay();
			}
			else if(onControlScreen)
				ToggleControlsImage();
		}
	}
	
	void HandlePlayerInputVertical(Player player)
	{

		if(player.menuControlsEnabled)
		{
			player.menuControlsEnabled = false;
			StartCoroutine("RestoreControls", player);

			if(onCharacterSelect)
			{
				if(player.input.GetAxis("Vertical") > 0)
					MainMenuCharacterSelector.GetCharacterSelectorByIndex(player.index).Next();
				else
					MainMenuCharacterSelector.GetCharacterSelectorByIndex(player.index).Previous();
			}
			else if(onMapMenu)
			{
				mapTitles[MainMenu.mapIndex].color = Color.white;
				if(player.input.GetAxis("Vertical") < 0)
					MainMenu.NextMap();
				else
					MainMenu.PreviousMap();
				UpdateLevelSelectionDisplay();
			}
		}
	}

	// Utility //
	IEnumerator RestoreControls(Player player)
	{
		yield return new WaitForSeconds(0.18f);
		player.menuControlsEnabled = true;
	}
}