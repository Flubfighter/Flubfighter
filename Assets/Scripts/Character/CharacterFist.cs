using UnityEngine;
using System.Collections;


/// <summary>
/// FlubFist- 
/// Controlled by FlubPunch
/// </summary>
public class CharacterFist : MonoBehaviour
{
	public Character character;
	public CharacterPunch characterPunch;
	public TrailRenderer trail;

	public void TurnOn()
	{
		collider2D.enabled = true;
		if(trail)
			trail.enabled = true;
	}

	public void TurnOff()
	{
		collider2D.enabled = false;
		if(trail)
			trail.enabled = false;
	}
	
	private void OnTriggerEnter2D(Collider2D collider)
	{
		// FIXME: Bad code, reorganize
		Character punchee = collider.GetComponent<Character> ();
		if(punchee)
		{
			if(Game.CanBeAttacked(character.Player, punchee.Player))
			{
				character.PunchedSomeone (punchee);
				collider.SendMessage ("GotPunched", character, SendMessageOptions.DontRequireReceiver);
//				TurnOff();
			}
		}
		// Buttons
		if(character.CanPunchButton)
		{
			PunchTrigger trigger = collider.GetComponent<PunchTrigger> ();
			if(trigger)
			{
				collider.SendMessage ("GotPunched", character, SendMessageOptions.DontRequireReceiver);
				character.PunchButton(trigger);
			}
		}
		if(collider.GetComponent<Item>())
			collider.SendMessage ("GotPunched", character, SendMessageOptions.DontRequireReceiver);
	}
}
