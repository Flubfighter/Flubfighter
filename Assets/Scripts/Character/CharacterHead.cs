using UnityEngine;
using System.Collections;

public class CharacterHead : MonoBehaviour 
{
	public Character character;

	void OnTriggerEnter2D(Collider2D collider)
	{
		// Bumped head on character
		Character otherCharacter = collider.GetComponent<Character>();
		if(otherCharacter)
		{
			otherCharacter.StompedSomeone(character);
			character.Stomp(otherCharacter);
		}
	}
}
