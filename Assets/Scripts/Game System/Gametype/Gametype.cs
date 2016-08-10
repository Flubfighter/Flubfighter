using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Contains settings that are refrenced by the Game
public class Gametype : ScriptableObject
{
	public enum TeamMode
	{
		FreeForAll,
		Teams
	}

	public int menuOrder = 0;
	public int scoreToWinRound = 0; // Set to 0 to make score not matter
	public int killScoreValue = 1;
	public int suicideScoreValue = 0;
	public int teamkillScoreValue = -1;
	public float duration = 300f; // Seconds, set to 0 for no timer
	public int lives = 0; // Set to 0 to make infinite lives
	public float respawnDuration = 2f;
	public float suicideRespawnPenalty = 2f;
	public float teamkillRespawnPenalty = 2f;
	public bool allowItems = true;
	public bool friendlyFire = false;
	public TeamMode teamMode = TeamMode.FreeForAll;
}