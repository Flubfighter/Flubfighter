using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using UnityEngine.EventSystems;

public class GameGUI : MonoBehaviour
{
	public static GameGUI instance;

	[SerializeField] private GameObject pauseMenuFirstSelected;
	[SerializeField] private GameObject roundResultsMenuFirstSelected;
	[SerializeField] private GameObject gameResultsMenuFirstSelected;

	public Text timerText;
	public Text[] teamInfo;
	public GameObject roundResultsWindow;
	public GameObject eventSystems;
	public Text roundResultsText;
	public GameObject gameResultsWindow;
	public Text gameResultsText;
	public GameObject pauseWindow;
	[SerializeField] private float identifierFadeOutDelay = 1f;
	[SerializeField] private float identifierFadeOutDuration = 1f;
	[SerializeField] private Vector3 identifierOffset = Vector3.up;
	[SerializeField] private PlayerIdentifier[] playerIdentifiers;
	[Header("Button Stuff")] // This is beautiful, whoever found this! -Jeff
	public Button gameResultsButton;
	public Button roundResultsButton;
	public Button resumeButton;
	public Button exitButton;
	public Color buttonHoverColor;

	[HideInInspector]
	public Player playerPaused;
	public Button currentButton;

	private Color startColor;
	private bool canControl = true;

	void Awake()
	{
		instance = this;
		Game.RoundEnded += ShowRoundResults;
		Game.GameEnded += ShowGameResults;
		pauseWindow.SetActive (false);

		// Set player colors/indexes
		for (int index = 0; index < Player.All.Length; index++) 
		{
			// Players
			if(Player.All[index].enabled)
			{
				playerIdentifiers[Player.All[index].index - 1].SetColor(
					Player.All[index].team.guiColor, // Base color
					Color.Lerp(Player.All[index].team.guiColor, Color.white, 0.5f)); // Slightly lighter for the highlight // TODO: Add highlight gui color to team?
				playerIdentifiers[Player.All[index].index - 1].SetIndex(Player.All[index].index); // FIXME: index + 1?
			}
		}
	}

	void Start()
	{
		startColor = resumeButton.colors.normalColor;
	}

	void OnDestroy()
	{
		Game.RoundEnded -= ShowRoundResults;
		Game.GameEnded -= ShowGameResults;
	}

	void Update()
	{
		if (Game.TimeRemaining < 0f)
		{
			timerText.text = "0:00";
		}
		else 
		{
			int minutes = Mathf.FloorToInt (Game.TimeRemaining / 60f);
			int seconds = Mathf.FloorToInt (Game.TimeRemaining - minutes * 60);
			timerText.text = string.Format ("{0:0}:{1:00}", minutes, seconds);
		}
		if(currentButton != null)
		{
			if(playerPaused != null)
			{
				if(playerPaused.input.GetAxis("Vertical") != 0f)
				{
					canControl = false;
					EventSystem.current.SetSelectedGameObject(currentButton == resumeButton ? exitButton.gameObject : resumeButton.gameObject); // switch buttons

				}
				else if (playerPaused.input.GetButtonDown("Submit"))
				{
					currentButton.onClick.Invoke();
					currentButton = null;
					playerPaused = null;
				}
			}
			else if(Player.One.input.GetButtonDown("Pause") || Player.One.input.GetButtonDown("Jump") || Player.One.input.GetButtonDown("Submit"))
			{
				currentButton.onClick.Invoke();
				currentButton = null;
				playerPaused = null;
		}
		}

		for (int index = 0; index < 4; index++) 
		{
			// Players
			if(Player.All[index].enabled && Player.All[index].CharacterIsAlive)
			{
				playerIdentifiers[Player.All[index].index - 1].transform.position = Player.All[index].playableCharacter.transform.position + identifierOffset;
			}
			else
			{
				playerIdentifiers[Player.All[index].index - 1].gameObject.SetActive(false); // FIXME: Is this what we want? A player dying with a non-faded-out identifier will have their identifier blink out
			}

			// Teams
			if(index < Game.teams.Length)
			{
				teamInfo[index].transform.parent.gameObject.SetActive(true);
				teamInfo[index].text = Game.teams[index].name + "\n" + 
					Game.Scores[Game.teams[index]].ToString() + " Points" +
					(Game.gametype.lives > 0 ? ", " + Game.Lives[Game.teams[index]].ToString() + " Lives Remaining" : "");
				teamInfo[index].color = Game.teams[index].guiColor;
			}
			else
			{
				teamInfo[index].transform.parent.gameObject.SetActive(false);
			}
		}
	}

	public void PlayerSpawned(Player player)
	{
		// FIXME: This could cause a bug where the identifier doesn't match the player index, if player index is out of order in Player.All
		// The fixme above ought to be fixed
		StartCoroutine(playerIdentifiers[player.index - 1].Show(identifierFadeOutDelay, identifierFadeOutDuration));
	}

	public void ShowRoundResults()
	{
		foreach(StandaloneInputModule sim in FindObjectsOfType<StandaloneInputModule>())
		{
			sim.enabled = true;
		}
		roundResultsWindow.SetActive (true);
		if(currentButton != gameResultsButton)
			currentButton = roundResultsButton;
		EventSystem.current.SetSelectedGameObject(roundResultsMenuFirstSelected);
		roundResultsButton.interactable = true;
		string gameText = "";
		Team[] orderedTeams = Game.teams.OrderByDescending (team => Game.Scores [team]).ToArray ();
		for (int index = 0; index < Game.teams.Length; index++) 
		{
			gameText += string.Format ("{0}:\t{1}\t\t{2}\n", index + 1, orderedTeams[index].name, Game.Scores[orderedTeams[index]]);
		}
		roundResultsText.text = gameText;
	}

	public void RoundResultsContinue()
	{
		foreach(StandaloneInputModule sim in FindObjectsOfType<StandaloneInputModule>())
		{
			sim.enabled = false;
		}
		Game.RoundResultsContinue ();
	}

	public void ShowGameResults()
	{
		foreach(StandaloneInputModule sim in FindObjectsOfType<StandaloneInputModule>())
		{
			sim.enabled = true;
		}
		roundResultsWindow.SetActive (false);
		gameResultsWindow.SetActive (true);
		EventSystem.current.SetSelectedGameObject(gameResultsMenuFirstSelected);
		gameResultsButton.interactable = true;
		string gameText = "";
		Team[] orderedTeams = Game.teams.OrderByDescending (team => Game.Wins [team]).ToArray ();
		for (int index = 0; index < Game.teams.Length; index++) 
		{
			gameText += string.Format ("{0}:\t{1}\t\t{2}\n", index + 1, orderedTeams[index].name, Game.Wins[orderedTeams[index]]);
		}
		gameResultsText.text = gameText;
		Invoke("SetCurrentToGameResultsButton", 0.5f);
	}

	void SetCurrentToGameResultsButton()
	{
		currentButton = gameResultsButton;
	}

	public void GameResultsContinue() // Need to do this almost after game results finishes.  Don't know why.  Couln't figure it out after half an hour so here is the fix.
	{
		foreach(StandaloneInputModule sim in FindObjectsOfType<StandaloneInputModule>())
		{
			sim.enabled = false;
		}
		Game.GameResultsContinue ();
	}

	public void GoToMainMenu()
	{
		Time.timeScale = 1f;
		Application.LoadLevel ("Main Menu");
	}

	public void Pause(Player pausingPlayer)
	{
		playerPaused = pausingPlayer;
		// FIXME: The below code isn't reliable or maybe doesn't work?
		Debug.Log (4 - pausingPlayer.index);
		FindObjectsOfType<StandaloneInputModule>()[4 - pausingPlayer.index].enabled = true;
		pauseWindow.SetActive (true);
		EventSystem.current.SetSelectedGameObject(pauseMenuFirstSelected);
		Game.PauseGame();
	}

	public void ResumeGame()
	{
		FindObjectsOfType<StandaloneInputModule>()[4 - playerPaused.index].enabled = false;
		playerPaused = null;
		pauseWindow.SetActive (false);
		Game.ResumeGame ();
	}
}