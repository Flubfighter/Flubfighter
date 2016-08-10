using UnityEngine;
using System.Collections;

public class Player
{
	public static Player One = new Player(1);
	public static Player Two = new Player(2);
	public static Player Three = new Player(3);
	public static Player Four = new Player(4);
	public static Player[] All = new Player[] { One, Two, Three, Four };

	public int index;
	public InputState input;
	public Team team;
	public Character playableCharacter;
	public bool enabled;
	public bool menuControlsEnabled = true;
	public bool hasSuicidePenalty = false;
	public bool hasTeamkillPenalty = false;

	public bool CharacterIsAlive
	{
		get
		{
			if(playableCharacter != null)
				return !playableCharacter.IsDead;
			return false;
		}
	}

	private Player(int index)
	{
		this.index = index;
		input = new InputState(index);
	}

	public override string ToString ()
	{
		return "Player " + index.ToString();
	}

	public void SetPlayableCharacter(Character character)
	{
		playableCharacter = character;
	}
}
