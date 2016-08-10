using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MainMenuGUI : MonoBehaviour 
{

	public Texture controlsImage;
	private MainMenu mainMenu;
	private Vector2 scroll;
	private bool customizeGametype = false;
	private bool showingControls = false;

	void Awake()
	{
		mainMenu = FindObjectOfType<MainMenu> ();
	}

	void OnGUI()
	{
		if (showingControls) 
		{
			GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));

			GUILayout.Box(controlsImage, GUILayout.MaxHeight(Screen.height - 100f));

			if(GUILayout.Button("Back", GUILayout.Height(50f)))
			{
				showingControls = false;
			}
			GUILayout.EndArea();
			return;
		}

		GUILayout.BeginArea (new Rect (Screen.width * 1/8f, 0f, Screen.width * 3f/4f, Screen.height));
		scroll = GUILayout.BeginScrollView (scroll);
		{
			// Gametype
			GUILayout.Label("Gametype");
//			LeftRightChanger (PreloadedAssets.Gametypes[MainMenu.gametypeIndex].name, mainMenu.PreviousGametype, mainMenu.NextGametype);
			customizeGametype = GUILayout.Toggle(customizeGametype, "Customize");
			GUI.enabled = customizeGametype;

			// Items (Within Gametype)
			GUILayout.Label("Items");
			LeftRightChanger(mainMenu.itemSelectionMethod.ToString(), mainMenu.PreviousItemSelectionMethod, mainMenu.NextItemSelectionMethod);
			if(mainMenu.itemSelectionMethod == MainMenu.ItemSelectionMethod.Custom)
			{
//				for (int index = 0; index < PreloadedAssets.Items.Length; index++) 
				{
//					bool oldSetting = MainMenu.customItems.Contains(PreloadedAssets.Items[index]);
//					bool newSetting = GUILayout.Toggle(oldSetting, PreloadedAssets.Items[index].name);
//					if(newSetting)
//						MainMenu.customItems.Add(PreloadedAssets.Items[index]);
//					else
//						MainMenu.customItems.Remove(PreloadedAssets.Items[index]);
				}
			}

			GUI.enabled = true;
			GUILayout.Space(20f);

			// World
			GUILayout.Label("World");
//			LeftRightChanger (PreloadedAssets.Worlds[MainMenu.worldIndex].properName, mainMenu.PreviousWorld, mainMenu.NextWorld);
//			GUILayout.Label(PreloadedAssets.Worlds[MainMenu.worldIndex].description);
			GUILayout.Space(10f);


			// Players
			GUILayout.BeginHorizontal();
			{
				PlayerField (Player.One, "XboxDefault", "Keyboard1");
				GUILayout.Space (15f);
				PlayerField (Player.Two, "XboxDefault", "Keyboard2");
			}
			GUILayout.EndHorizontal();
			GUILayout.Space (15f);
			GUILayout.BeginHorizontal();
			{
				PlayerField (Player.Three, "XboxDefault", "Keyboard3");
				GUILayout.Space (15f);
				PlayerField (Player.Four, "XboxDefault", "Keyboard4");
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(20f);

			// Play
			GUI.enabled = (mainMenu.GetEnabledPlayers().Length >= 2);
			if(GUILayout.Button("Play", GUILayout.Height(50f)))
			{
				mainMenu.BeginGame();
			}
			GUI.enabled = true;

			if(GUILayout.Button("Controls", GUILayout.Height(50f)))
			{
				showingControls = true;
			}

			if(GUILayout.Button("Quit", GUILayout.Height(50f)))
		   	{
				Application.Quit();
			}

			
			// Map
//			GUILayout.Label("Map");
//			LeftRightChanger (PreloadedAssets.Worlds[MainMenu.worldIndex].maps[MainMenu.mapIndex].properName, MainMenu.PreviousMap, MainMenu.NextMap);
//			GUILayout.BeginHorizontal();
//			GUILayout.FlexibleSpace();
//			GUILayout.Box(PreloadedAssets.Worlds[MainMenu.worldIndex].maps[MainMenu.mapIndex].image, GUILayout.Width(Screen.width/2f), GUILayout.Height(Screen.height / 2f));
//			GUILayout.FlexibleSpace();
//			GUILayout.EndHorizontal();
//			GUILayout.Label(PreloadedAssets.Worlds[MainMenu.worldIndex].maps[MainMenu.mapIndex].description);
//						GUILayout.Space(10f);
			
			// Random world/map
//						if(GUILayout.Button("Random"))
//							mainMenu.SetRandomWorldAndMap();
			GUILayout.Space(20f);


		}
		GUILayout.EndScrollView ();
		GUILayout.EndArea ();
	}

	void LeftRightChanger(string text, System.Action leftAction, System.Action rightAction)
	{
		GUILayout.BeginHorizontal ();
		if(GUILayout.Button("<", GUILayout.Width(20f)))
			leftAction();
		GUILayout.Label (text);
		if(GUILayout.Button(">", GUILayout.Width(20f)))
			rightAction();
		GUILayout.Space (50f);
		GUILayout.EndHorizontal ();
	}

	void PlayerField(Player player, params string[] controlProfileOptions)
	{
		GUILayout.BeginVertical ();
		{
//			GUILayout.Label ("Player " + player.Index);
//			player.SetEnabled(GUILayout.Toggle(player.Enabled, "Enabled"));
			
//			GUI.enabled = player.Enabled;
			
			// Team selection
//			GUI.color = player.Team.guiColor;
			System.Action previousTeamAction = delegate() { mainMenu.PreviousTeam(player); };
			System.Action nextTeamAction = delegate() {	mainMenu.NextTeam(player); };
//			LeftRightChanger(player.Team.name, previousTeamAction, nextTeamAction);
			GUI.color = Color.white;
			
			// Controls selection (it's a bit yucky)
			List<string> controlOptions = new List<string>(controlProfileOptions);
//			int currentIndex = controlOptions.IndexOf (player.Input.name);
			System.Action previousControlAction = delegate() 
			{ 
//				int newIndex = Mathf.RoundToInt(Mathf.Repeat(currentIndex - 1, controlProfileOptions.Length));
//				player.ChangeProfile(InputProfile.LoadName(controlOptions[newIndex])); 
			};
			System.Action nextControlAction = delegate() 
			{
//				int newIndex = Mathf.RoundToInt(Mathf.Repeat(currentIndex + 1, controlProfileOptions.Length));
//				player.ChangeProfile(InputProfile.LoadName(controlOptions[newIndex])); 
			};
//			LeftRightChanger(player.Input.name, previousControlAction, nextControlAction);
			
			GUI.enabled = true;
		}
		GUILayout.EndVertical ();
	}
}
