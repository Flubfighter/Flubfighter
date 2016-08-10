using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class Game
{
	public static event System.Action RoundStarted;
	public static event System.Action RoundCountdownStarted;
	public static event System.Action RoundEnded;
	public static event System.Action GameEnded;
	public static event System.Action<Team, int> ScoreChanged;
	
	public static int roundsRemaining;
	public static Gametype gametype;
	public static World world;
	public static Map map;
	public static Team[] teams;
	public static Player[] players;
	public static Item[] allowedItems = new Item[] { };
	public static float itemSpawnDelay;
	public static bool paused = false;
	
	private static GameDummy dummy;
	private static Coroutine timerCoroutine;
	private static Tuple<Vector2, float>[] rigidbodyTempData = new Tuple<Vector2, float>[4];
	
	public static bool TimerIsRunning { get; private set; }
	private static bool _roundIsPlaying = true; // Allow play if there is no Game running
	public static bool RoundIsPlaying { get { return _roundIsPlaying; } private set { _roundIsPlaying = value; } }
	public static float TimeRemaining { get; private set; }
	public static Dictionary<Team, int> Lives { get; private set; }
	public static Dictionary<Team, int> Scores { get; private set; }
	public static Dictionary<Team, int> Wins { get; private set; }

	public static void StartNewGame()
	{
		//Debug.Log ("Starting a new game");
		Scores = new Dictionary<Team, int> ();
		Wins = new Dictionary<Team, int> ();
		Lives = new Dictionary<Team, int> ();
		paused = false;
		foreach(Team team in teams)
		{
			Wins.Add(team, 0);
			Scores.Add(team, 0);
			Lives.Add(team, gametype.lives);
		}
		// TODO: Items?
	}
	
	public static void StartRound()
	{
		//Debug.Log ("Starting a round");
		RoundIsPlaying = false;
		dummy = new GameObject ("Game Dummy [DO NOT DELETE]").AddComponent<GameDummy>();
		Scores = new Dictionary<Team, int> ();
		Lives = new Dictionary<Team, int> ();
		foreach(Team team in teams)
		{
			Scores.Add(team, 0);
			Lives.Add(team, gametype.lives);
		}
		foreach(Player player in players)
		{
			SpawnPoint.FindSafeSpawnPoint(player.team).Spawn(player).Died += HandleDeath;
			GameGUI.instance.PlayerSpawned(player);
		}
		if(RoundCountdownStarted != null)
			RoundCountdownStarted();
		else
			CountdownEnded();
	}
	
	public static void CountdownEnded()
	{
		//Debug.Log ("Round countdown ended");
		RoundIsPlaying = true;
		if(gametype.duration > 0f)
		{
			TimerIsRunning = true;
			timerCoroutine = dummy.StartCoroutine (TimerCoroutine ());
		}
		foreach (Character character in GameObject.FindObjectsOfType<Character>())
			character.RoundStart ();
		if(gametype.allowItems && allowedItems.Length > 0)
			dummy.StartCoroutine(ItemSpawnCoroutine());
		if(RoundStarted != null)
			RoundStarted();
	}

	public static IEnumerator TimerCoroutine()
	{
		TimeRemaining = gametype.duration;
		while(TimeRemaining >= 0f)
		{
			while(paused) yield return null;
			TimeRemaining -= Time.deltaTime;
			yield return null;
		}
		//Debug.Log ("Timer ended");
		EndRound ();
	}

	public static IEnumerator ItemSpawnCoroutine()
	{
		while(true)
		{
			yield return new WaitForSeconds(itemSpawnDelay);
			try
			{
				ItemSpawnPoint selectedSpawnPoint = ItemSpawnPoint.FindOpenSpawnPoint();
				Item item = allowedItems[Random.Range(0, allowedItems.Length)];
				selectedSpawnPoint.Spawn (item);
			}
			catch(System.Exception e)
			{
				Debug.LogWarning (e.Message);
			}
		}
	}

	public static void EndRound()
	{
		//Debug.Log ("Round ended");
		RoundIsPlaying = false;
		foreach (Character character in GameObject.FindObjectsOfType<Character>())
			character.RoundEnd ();
		Team[] orderedTeams = Game.teams.OrderByDescending (team => Game.Scores [team]).ToArray ();
		Wins [orderedTeams [0]]++;
		//Debug.Log ("Giving Win to " + orderedTeams [0]);
		TimerIsRunning = false;
		if(timerCoroutine != null)
			dummy.StopCoroutine (timerCoroutine);
		timerCoroutine = null;
		roundsRemaining--;
		if(RoundEnded != null)
			RoundEnded ();
		else
			RoundResultsContinue();
	}
	
	public static void GiveScore(Team team, int amount)
	{
		//Debug.Log ("Giving score to team " + team);
		Scores [team] = Mathf.Max (0, Scores [team] + amount);
		if(Scores[team] >= gametype.scoreToWinRound && gametype.scoreToWinRound != 0)
		{
			EndRound();
		}
	}

	public static void HandleDeath(Player victim)
	{
		// Do score
		//Debug.Log (victim);
		Player killer = victim.playableCharacter.ThreateningPlayer;
		if(killer == victim || killer == null) // suicide
		{
			//Debug.Log (victim + " committed suicide");
			victim.hasSuicidePenalty = true; 
			GiveScore (victim.team, gametype.suicideScoreValue);
		}
		else if(killer.team == victim.team) // tk
		{
			//Debug.Log (victim + " was TK'd by " + killer);
			GiveScore (victim.team, gametype.teamkillScoreValue);
			victim.playableCharacter.ThreateningPlayer.hasTeamkillPenalty = true;
		}
		else // normal kill
		{
			//Debug.Log (victim + " was killed by " + killer);
			GiveScore (killer.team, gametype.killScoreValue);
		}

		// Respawn
		if(gametype.lives > 0)
		{
			if(Lives[victim.team] > 0) // Can respawn, so use a life to do so
			{
				Lives[victim.team]--;
				dummy.StartCoroutine(Respawn (victim));
			}
			else // Ran out of lives
			{
				CheckForLastManStandingVictory();
			}
		}
		else // Lives don't matter
		{
			dummy.StartCoroutine(Respawn (victim));
		}

		// Cleanup
		victim.SetPlayableCharacter (null);
	}

	static void CheckForLastManStandingVictory()
	{
		// Get all players who have a character, then get the ones that aren't dead yet, then get their teams
		Team[] remainingTeams = players
			.Where (player => player.playableCharacter != null)
			.Where (player => player.playableCharacter.IsDead == false)
			.Select (player => player.team)
			.ToArray ();
		if(remainingTeams.Length <= 1)
		{
			//Debug.Log("Round ended by LMS");
			EndRound(); // End by LMS
		}
	}

	static IEnumerator Respawn(Player player)
	{
		float respawnLength = gametype.respawnDuration;
		if(player.hasSuicidePenalty)
			respawnLength += gametype.suicideRespawnPenalty;
		if(player.hasTeamkillPenalty)
			respawnLength += gametype.teamkillRespawnPenalty;
		//Debug.Log ("Respawning " + player + " in " + respawnLength);
		yield return new WaitForSeconds (respawnLength);
		SpawnPoint spawnPoint = null;
		do
		{
			spawnPoint = SpawnPoint.FindSafeSpawnPoint (player.team);
			yield return null;
		}while(spawnPoint == null);
		while(paused) yield return null; // Wait if paused
		//Debug.Log ("Respawning " + player + " now");
		player.hasSuicidePenalty = false;
		player.hasTeamkillPenalty = false;
		spawnPoint.Spawn (player).Died += HandleDeath;
		GameGUI.instance.PlayerSpawned(player);
	}

	public static void RoundResultsContinue() // Raised from round results screen
	{
		//Debug.Log ("Round results continue");
		if(roundsRemaining <= 0)
		{
			if(GameEnded != null)
				GameEnded();
			else
				GameResultsContinue();
		}
		else
		{
			Application.LoadLevel("Loading Screen");
		}
	}

	public static void PauseGame()
	{
		if(!paused)
		{
			for(int i = 0; i < players.Length; i++)
			{
				if(players[i].playableCharacter != null)
				{
					rigidbodyTempData[i] = new Tuple<Vector2, float>();
					rigidbodyTempData[i].first = players[i].playableCharacter.rigidbody2D.velocity;
					rigidbodyTempData[i].second = players[i].playableCharacter.rigidbody2D.angularVelocity;
					players[i].playableCharacter.rigidbody2D.isKinematic = true;
					players[i].playableCharacter.Pause();
				}
			}
			Time.timeScale = 0f;
			paused = true;
		}
	}

	public static void ResumeGame()
	{
		if(paused)
		{
			Time.timeScale = 1f;
			for(int i = 0; i < players.Length; i++)
			{
				if(players[i].playableCharacter != null)
				{
					players[i].playableCharacter.rigidbody2D.velocity = rigidbodyTempData[i].first;
					players[i].playableCharacter.rigidbody2D.angularVelocity = rigidbodyTempData[i].second;
					players[i].playableCharacter.rigidbody2D.isKinematic = false;
					players[i].menuControlsEnabled = true;
					players[i].playableCharacter.Unpause();
				}
			}
			paused = false;
		}
	}

	public static void GameResultsContinue() // Raised from game results screen
	{
		//Debug.Log ("Game results continue");
		Application.LoadLevel ("Main Menu");
//		Application.LoadLevel ("Main Menu Level Select Only");
	}

	public static bool CanBeAttacked(Player victim, Player attacker)
	{
		if(gametype == null || victim.team == null || attacker.team == null) 
			return true;
		return gametype.friendlyFire || victim.team != attacker.team;
	}
}

