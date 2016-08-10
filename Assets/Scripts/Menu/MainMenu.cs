using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MainMenu : MonoBehaviour 
{

	private static MainMenu instance;
	public static MainMenu Instance
	{
		get
		{
			if(instance == null)
				instance = GameObject.FindObjectOfType<MainMenu>();
			return instance;
		}
	}

	public enum ItemSelectionMethod
	{
		Default = 0,
		All = 1,
		None = 2,
		Custom = 3
	}

	public enum MenuArea
	{
		Main,
		Controls,
		Options,
		Credits,
		GameType,
		CharacterSelect,
		LevelSelect
	}
	
	public static int gametypeIndex, worldIndex, mapIndex;
	public ItemSelectionMethod itemSelectionMethod = ItemSelectionMethod.Custom;
	public HashSet<Item> customItems = new HashSet<Item>();
	public List<string> controlProfileOptions = new List<string>();
	public static List<string> controlProfileOptionsStatic = new List<string>();
	public bool testingOnKeyboard = false;
	public bool allowJoystickInput = true;
	public bool fourPlayersOnly = false;

	public MenuArea currentMenu = MenuArea.Main;
	
	void Awake()
	{
		itemSelectionMethod = ItemSelectionMethod.Custom;
		controlProfileOptionsStatic = controlProfileOptions;
		if(instance == null)
			instance = this;
		else if(this != instance)
			Destroy(gameObject);


		// FIXME: This will throw an error if ForcePreloadedAssets takes more than a frame to complete. If this happens, just start from the splash screen and tell Jeff
		SetGametype(0);
		SetWorld (0);
		SetMap(0);

		SetTeam (Player.One, 0);
		SetTeam (Player.Two, 0);
		SetTeam (Player.Three, 0);
		SetTeam (Player.Four, 0);
	}

	void Start()
	{
		Player.One.enabled = true;
		Player.Two.enabled = true;
		Player.Three.enabled = fourPlayersOnly;
		Player.Four.enabled = fourPlayersOnly;
		Player.Two.team = (Assets.Teams [1]);
		Player.Three.team = (Assets.Teams [2]);
		Player.Four.team = (Assets.Teams [3]);
		if(Input.GetJoystickNames().Length >= 3)
			Player.Three.enabled = true;
		if(Input.GetJoystickNames().Length >= 4)
			Player.Four.enabled = true;
	}

	#region Team
	void SetTeam(Player player, int index)
	{
		player.team = (Assets.Teams [Mathf.RoundToInt (Mathf.Repeat (index, Assets.Teams.Length))]);
	}

	public void NextTeam(Player player)
	{
		SetTeam (player, Assets.Teams.ToList().IndexOf (player.team) + 1);
	}

	public void PreviousTeam(Player player)
	{
		SetTeam (player, Assets.Teams.ToList().IndexOf (player.team) - 1);
	}
	#endregion
	
	#region Gametype
	static void SetGametype(int index)
	{
		gametypeIndex = Mathf.RoundToInt(Mathf.Repeat(index, Assets.Gametypes.Length));
	}
	
	public static void NextGametype()
	{
		SetGametype(gametypeIndex + 1);
	}
	
	public static void PreviousGametype()
	{
		SetGametype(gametypeIndex - 1);
	}
	#endregion

	#region Items
	void SetItemSelectionMethod(int index)
	{
		itemSelectionMethod = (ItemSelectionMethod)Mathf.RoundToInt(Mathf.Repeat(index, 4));
	}

	public void NextItemSelectionMethod()
	{
		SetItemSelectionMethod((int)itemSelectionMethod + 1);
	}

	public void PreviousItemSelectionMethod()
	{
		SetItemSelectionMethod((int)itemSelectionMethod - 1);
	}
	#endregion
	
	#region World
	static void SetWorld(int index)
	{
		worldIndex = Mathf.RoundToInt(Mathf.Repeat(index, Assets.Worlds.Length));
		SetMap(0);
	}
	
	static public void NextWorld()
	{
		SetWorld(worldIndex + 1);
	}
	
	static public void PreviousWorld()
	{
		SetWorld(worldIndex - 1);
	}
	#endregion
	
	#region Map
	static void SetMap(int index)
	{
		mapIndex = Mathf.RoundToInt(Mathf.Repeat(index, Assets.Worlds[worldIndex].maps.Length));
	}
	
	static public void NextMap()
	{
		SetMap(mapIndex + 1);
	}
	
	public static void PreviousMap()
	{
		SetMap(mapIndex - 1);
	}
	#endregion

	public void SetRandomWorldAndMap()
	{
		SetWorld(Random.Range(0, Assets.Worlds.Length));
		SetMap (Random.Range(0, Assets.Worlds[worldIndex].maps.Length));
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

	public void BeginGame()
	{
		Debug.Log (string.Format("Starting Game:\tGametype={0}!\tWorld={1}/{2}",
		                         Assets.Gametypes[gametypeIndex].name,
		                         Assets.Worlds[worldIndex].name,
		                         Assets.Worlds[worldIndex].maps[mapIndex].name));

		Game.gametype = Object.Instantiate (Assets.Gametypes[gametypeIndex]) as Gametype;
		Game.map = Assets.Worlds[worldIndex].maps[mapIndex];;
		Game.world = Assets.Worlds[worldIndex];
		Game.teams = GetTeams ();
		Game.players = GetEnabledPlayers();
		Game.allowedItems = customItems.ToArray();
		Game.itemSpawnDelay = 6f;
		Game.roundsRemaining = 1;
		Debug.Log("Starting map with: ");
		foreach(Item i in Game.allowedItems)
			Debug.Log('\t' + i.name);
		Game.StartNewGame ();
		Application.LoadLevel ("Loading Screen");
	}

	void AllowJoystickInput()
	{
		allowJoystickInput = true;
	}

}
