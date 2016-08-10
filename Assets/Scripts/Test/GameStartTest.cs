using UnityEngine;
using System.Collections;

public class GameStartTest : MonoBehaviour 
{
	public Gametype gametype;
	public Map map;
	public World world;
	public Team[] teams;

	void Start()
	{
		// Copy gametype
		Game.roundsRemaining = 2;
		Game.gametype = Object.Instantiate (gametype) as Gametype;
		Game.map = map;
		Game.world = world;
		Game.teams = teams;
		Player.One.enabled = true;
		Player.Two.enabled = true;
		Game.players = new Player[] { Player.One, Player.Two };
		Player.One.team = Game.teams [0];
		Player.Two.team = Game.teams [1];
		Game.StartNewGame ();
		Application.LoadLevel ("Loading Screen");
	}
}
