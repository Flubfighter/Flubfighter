using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// TODO: Add a "Quit" button to player settings panel
public class NewMainMenu : MonoBehaviour 
{
	public enum WindowMode
	{
		Fullscreen=0,
		Windowed=1
	}

	public class Resolution
	{
		public readonly int width;
		public readonly int height;
		
		public Resolution(int width, int height)
		{
			this.width = width;
			this.height = height;
		}

		public override string ToString ()
		{
			return width + "x" + height;
		}
	}
	
	class Settings
	{
		public List<Resolution> resolutions = new List<Resolution>()
		{
			new Resolution(1024, 576),
			new Resolution(1152, 648),
			new Resolution(1280, 720),
			new Resolution(1360, 768),
			new Resolution(1366, 768),
			new Resolution(1600, 900),
			new Resolution(1920, 1080)
		};

		private WindowMode windowMode;
		private Resolution resolution;
		private int qualityLevel;
		private float volume;

		public WindowMode WindowMode { get { return windowMode; } }
		public Resolution Resolution { get { return resolution; } }
		public int ResolutionIndex { get { return resolutions.IndexOf(resolution); } }
		public int QualityLevel { get { return qualityLevel; } }
		public float Volume { get { return volume; } }
		public bool IsDirty { get; private set; }

		public Settings()
		{
			Discard();
		}

		public void SetWindowSettings(WindowMode windowSettings)
		{
			if(this.windowMode != windowSettings)
				IsDirty = true;
			this.windowMode = windowSettings;
		}

		public void SetVolume(float volume)
		{
			if (this.volume != volume)
				IsDirty = true;
			this.volume = Mathf.Clamp01(volume);
		}

		public void SetQualityLevel(int qualityLevel)
		{
			if(this.qualityLevel != qualityLevel)
				IsDirty = true;
			this.qualityLevel = qualityLevel;
		}

		public void SetResolutionIndex(int index)
		{
			if(resolution != resolutions[index])
				IsDirty = true;
			resolution = resolutions[index];
		}

		public void Discard()
		{
			windowMode = Screen.fullScreen ? WindowMode.Fullscreen : WindowMode.Windowed;
			resolution = resolutions.FirstOrDefault(obj => obj.width == Screen.width && obj.height == Screen.height) ?? new Resolution(Screen.width, Screen.height);
			if(!resolutions.Contains(resolution))
			{
				resolutions.Add (resolution);
			}
			resolutions = resolutions.OrderBy(obj => obj.width).ThenBy(obj => obj.height).ToList();
			qualityLevel = QualitySettings.GetQualityLevel();
			volume = PlayerPrefs.GetFloat("Volume", 1f);
			AudioListener.volume = volume;
//			Debug.Log (string.Format ("Volume {0} = setting of {1}", volume, AudioListener.volume));
			IsDirty = false;
		}

		public void Apply()
		{
			PlayerPrefs.SetFloat("Volume", volume);
			AudioListener.volume = volume;
//			Debug.Log (string.Format ("Volume {0} = setting of {1}", volume, AudioListener.volume));
			Screen.SetResolution(resolution.width, resolution.height, windowMode == WindowMode.Fullscreen);
			QualitySettings.SetQualityLevel(qualityLevel, true);
			IsDirty = false;
		}
	}

	[Header("Controls")]
	[SerializeField] private Text controlsLabel;
	[SerializeField] private Image controlsDisplay;
	[SerializeField] private string controlsKeyboardLabel;
	[SerializeField] private Sprite controlsKeyboardSprite;
	[SerializeField] private string controlsGamepadLabel;
	[SerializeField] private Sprite controlsGamepadSprite;
	[Header("Options")]
	[SerializeField] private Button applyButton;
	[SerializeField] private Text windowLabel;
	[SerializeField] private Text resolutionLabel;
	[SerializeField] private Text graphicsLabel;
	[SerializeField] private Slider volumeSlider;
	[Header("Game Settings")]
	[SerializeField] private Text gametypeLabel;
	[SerializeField] private Text durationLabel;
	[SerializeField] private int[] durationMinutes = new int[] { 1, 3, 5, 10 };
	[SerializeField] private Gametype defaultGametype;
	[SerializeField] private int defaultDurationIndex;
	[Header("Player Settings")]
	[SerializeField] private SkinnedMeshRenderer[] playerFlubs; // TODO: Replace me with something to automate color changing + particle shower/etc
	[SerializeField] private Text[] playerTeamTexts;
	[SerializeField] private LeftRightButtonSelector[] playerTeamSelecters;
	[Tooltip("The text shown when the player is ready.")]
	[SerializeField] private string readyText = "Ready";
	[SerializeField] private Button playersAllReadyButton;
	[Header("Level Select Settings")]
	[SerializeField] private Button startButton;
	[SerializeField] private RectTransform selectionBox;
	[SerializeField] private Vector2 selectionBoxBorder;

	private int duration;
	private int gametype;
	private List<Item> items;
	private bool controlsShowKeyboard = true;
	private Settings changedSettings;
	private Map selectedMap;
	private bool player3Joined = false;
	private bool player4Joined = false;
	private bool[] readyState = new bool[] { false, false, false, false };

	void Awake()
	{
		// Settings
		changedSettings = new Settings();

		// Game Settings
		gametype = Assets.Gametypes.ToList().IndexOf(defaultGametype);
		duration = defaultDurationIndex;
		items = Assets.Items.ToList();

		// Player Settings
		ButtonJoinPlayer(0);
		ButtonJoinPlayer(1);
		for (int index = 0; index < 4; index++)
		{
			Player.All[index].team = Assets.Teams[index];
		}

		// Map Select
		ButtonSetSelection(null);

		ApplyGraphics();
	}

	void Update()
	{
		applyButton.interactable = changedSettings.IsDirty;
		startButton.interactable = selectedMap != null;

		bool playersAllReady = true;
		for (int index = 0; index < 4; index++) 
		{
			if(Player.All[index].enabled && !readyState[index])
				playersAllReady = false;
		}
		playersAllReadyButton.interactable = playersAllReady;
	}

	void ApplyGraphics()
	{
		// Settings
		windowLabel.text = changedSettings.WindowMode.ToString();
		graphicsLabel.text = QualitySettings.names[changedSettings.QualityLevel];
		resolutionLabel.text = changedSettings.Resolution.ToString();
		volumeSlider.value = changedSettings.Volume * 10f;

		// Game Settings
		gametypeLabel.text = Assets.Gametypes[gametype].name;
		durationLabel.text = durationMinutes[duration] + " Minute" + (durationMinutes[duration] > 1 ? "s" : "");

		// Player Settings
		for (int index = 0; index < 4; index++) 
		{
			playerFlubs[index].gameObject.SetActive(Player.All[index].enabled);
			if(Player.All[index].team != null)
			{
				playerFlubs[index].material = Player.All[index].team.characterMaterial;
				playerTeamTexts[index].text = readyState[index] ? readyText : Player.All[index].team.shortName;
				ColorBlock cb = playerTeamSelecters[index].colors;
				cb.normalColor = Player.All[index].team.guiColor;
				cb.highlightedColor = Player.All[index].team.guiColor;
				playerTeamSelecters[index].colors = cb;
				playerTeamSelecters[index].LeftRightInteractable = !readyState[index];
			}
		}
	}

	public Player[] GetEnabledPlayers()
	{
		List<Player> activePlayers = new List<Player> ();
		foreach(Player player in Player.All)
		{
			if(player.enabled)
				activePlayers.Add(player);
		}
		return activePlayers.ToArray ();
	}
	
	public Team[] GetTeams()
	{
		HashSet<Team> teams = new HashSet<Team> ();
		foreach(Player player in GetEnabledPlayers())
			teams.Add(player.team);
		return teams.ToArray ();
	}

	public void ButtonExit()
	{
		Application.Quit();
	}

	#region Controls
	public void ButtonControlsPrevious()
	{
		controlsShowKeyboard = !controlsShowKeyboard;
		controlsLabel.text = controlsShowKeyboard ? controlsKeyboardLabel : controlsGamepadLabel;
		controlsDisplay.sprite = controlsShowKeyboard ? controlsKeyboardSprite : controlsGamepadSprite;
	}

	public void ButtonControlsNext()
	{
		ButtonControlsPrevious(); // If you want to have multiple selections for controls, you can have different code here. As it is, both buttons do the same thing (toggle to other control type)
	}
	#endregion

	#region Gametype
	public void ButtonGametypeNext()
	{
		gametype = Mathf.RoundToInt(Mathf.Repeat(gametype + 1, Assets.Gametypes.Length));
		ApplyGraphics ();
	}

	public void ButtonGametypePrevious()
	{
		gametype = Mathf.RoundToInt(Mathf.Repeat(gametype - 1, Assets.Gametypes.Length));
		ApplyGraphics ();
	}
	
	public void ButtonDurationNext()
	{
		duration = Mathf.RoundToInt(Mathf.Repeat(duration + 1, durationMinutes.Length));
		ApplyGraphics ();
	}

	public void ButtonDurationPrevious()
	{
		duration = Mathf.RoundToInt(Mathf.Repeat(duration - 1, durationMinutes.Length));
		ApplyGraphics ();
	}
	
	public void ButtonToggleItem(Item item)
	{
		if(items.Contains(item))
		{
			items.Remove(item);
		}
		else
		{
			items.Add (item);
		}
	}
	#endregion

	#region Player Settings
	public void ButtonToggleReadyPlayer(int playerIndex)
	{
		readyState[playerIndex] = !readyState[playerIndex];
		ApplyGraphics();
	}

	public void ButtonToggleInteractable(Button button)
	{
		button.interactable = !button.interactable;
	}

	public void ButtonJoinPlayer(int playerIndex)
	{
		Player.All[playerIndex].enabled = true;
		ApplyGraphics();
	}

	public void ButtonPlayerTeamNext(int playerIndex)
	{
		// TODO: Player team limiting based on gametype
		Player.All[playerIndex].team = Assets.Teams[Mathf.RoundToInt(Mathf.Repeat(Assets.Teams.ToList().IndexOf(Player.All[playerIndex].team) + 1, Assets.Teams.Length))];
		ApplyGraphics();
	}

	public void ButtonPlayerTeamPrevious(int playerIndex)
	{
		// TODO: Player team limiting based on gametype
		Player.All[playerIndex].team = Assets.Teams[Mathf.RoundToInt(Mathf.Repeat(Assets.Teams.ToList().IndexOf(Player.All[playerIndex].team) - 1, Assets.Teams.Length))];
		ApplyGraphics();
	}

	#endregion

	#region Level Select
	public void ButtonLevelSelectBack()
	{
		selectedMap = null;
		ButtonSetSelection(null);
		for (int index = 0; index < readyState.Length; index++) 
		{
			readyState[index] = false;
		}
		ApplyGraphics();
	}

	public void ButtonSetSelection(RectTransform target)
	{
		selectionBox.gameObject.SetActive(target != null);
		if(target)
		{
			selectionBox.anchoredPosition = target.anchoredPosition;
			selectionBox.sizeDelta = target.sizeDelta + selectionBoxBorder;
		}
	}

	public void ButtonSelectLevel(Map level)
	{
		selectedMap = level;
		// TODO: Selection graphic
	}

	public void ButtonRandomLevel()
	{
		selectedMap = Assets.Maps[Random.Range(0, Assets.Maps.Length)];
		// TODO: Selection graphic
	}
	
	public void ButtonStartGame()
	{		
		Game.gametype = Object.Instantiate (Assets.Gametypes[gametype]) as Gametype;
		Game.map = selectedMap;
		//		Game.world = Assets.Worlds[worldIndex];
		Game.teams = GetTeams ();
		Game.players = GetEnabledPlayers();
		Game.allowedItems = items.ToArray();
		Game.gametype.duration = durationMinutes[duration] * 60f;
		Game.itemSpawnDelay = 6f;
		Game.roundsRemaining = 1;
		Debug.Log("Starting map with: ");
		foreach(Item i in Game.allowedItems)
			Debug.Log('\t' + i.name);
		Game.StartNewGame ();
		Application.LoadLevel ("Loading Screen");
	}
	#endregion

	#region Options
	public void SetVolumeSlider(Slider slider)
	{
		slider.value = PlayerPrefs.GetFloat("Volume", 1f) * 10f;
	}

	public void SliderVolume(float volume)
	{
		Debug.Log ("Slider value = " + volume);
		changedSettings.SetVolume(volume / 10f);
		AudioListener.volume = volume / 10f;
	}

	public void ButtonWindowNext()
	{
		changedSettings.SetWindowSettings((WindowMode)Mathf.RoundToInt(Mathf.Repeat((int)changedSettings.WindowMode + 1, System.Enum.GetNames(typeof(WindowMode)).Length)));
		ApplyGraphics();
	}

	public void ButtonWindowPrevious()
	{
		changedSettings.SetWindowSettings((WindowMode)Mathf.RoundToInt(Mathf.Repeat((int)changedSettings.WindowMode - 1, System.Enum.GetNames(typeof(WindowMode)).Length)));
		ApplyGraphics();
	}

	public void ButtonResolutionNext()
	{
		changedSettings.SetResolutionIndex(Mathf.RoundToInt(Mathf.Repeat(changedSettings.ResolutionIndex + 1, changedSettings.resolutions.Count)));
		ApplyGraphics();
	}

	public void ButtonResolutionPrevious()
	{
		changedSettings.SetResolutionIndex(Mathf.RoundToInt(Mathf.Repeat(changedSettings.ResolutionIndex - 1, changedSettings.resolutions.Count)));
		ApplyGraphics();
	}

	public void ButtonGraphicsNext()
	{
		changedSettings.SetQualityLevel(Mathf.RoundToInt(Mathf.Repeat(changedSettings.QualityLevel + 1, QualitySettings.names.Length)));
		ApplyGraphics();
	}

	public void ButtonGraphicsPrevious()
	{
		changedSettings.SetQualityLevel(Mathf.RoundToInt(Mathf.Repeat(changedSettings.QualityLevel - 1, QualitySettings.names.Length)));
		ApplyGraphics();
	}

	public void ButtonDiscardOptions()
	{
		changedSettings.Discard();
		ApplyGraphics();
	}

	public void ButtonApplyOptions()
	{
		changedSettings.Apply();
	}
	#endregion
}
