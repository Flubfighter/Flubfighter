using UnityEngine;
using System.Collections;
using System;

public class CharacterPunch : MonoBehaviour 
{
	public Character character;
	[Tooltip("The amount of time after a punch when you can still hit buttons.")]
	public float buttonInteractionWindow = 0.075f; 
	[Tooltip("The curve defining the charge time versus how much force your punch has.")]
	public AnimationCurve chargeTimeVsPunchForce;
	public CharacterFist[] fists;

	public float PunchForce
	{
		get
		{
			return chargeTimeVsPunchForce.Evaluate(character.TimePunchCharging);
		}
	}

	private void TurnFistsOn()
	{
		foreach(CharacterFist fist in fists)
		{
			fist.TurnOn();
		}
	}
	
	private void TurnFistsOff()
	{
		foreach(CharacterFist fist in fists)
		{
			fist.TurnOff();
		}
	}

	#region Movement
	void OnMovementInput(float input) // Input in range (-1f, 1f)
	{
	}
	
	void OnFootstep() // Animation event
	{
	}
	
	void OnJumpStart()
	{
	}
	
	void OnJumpEnd()
	{
	}
	
	void OnAirJumpStart()
	{
	}
	
	void OnAirJumpEnd()
	{
	}
	
	void OnHitHead()
	{
	}
	
	void OnGrounded()
	{
	}
	
	void OnUngrounded()
	{
	}
	#endregion
	
	#region Punch
	void OnPunchChargeStart()
	{
	}
	
	void OnPunchChargeInterrupted()
	{
	}
	
	void OnPunchOvercharge()
	{
	}
	
	void OnPunchStart()
	{
		TurnFistsOn ();
	}
	
	void OnPunchEnd()
	{
		TurnFistsOff ();
	}
	
	void OnPunchEndSuddenly()
	{
		TurnFistsOff ();
	}

	void OnPunchCooldownEnd()
	{
	}

	void OnPunchButton(PunchTrigger trigger)
	{
		TurnFistsOff ();
	}
	
	void OnPunchSomeone(Character punchee)
	{
		TurnFistsOff();
	}
	
	void OnGetPunched(Character puncher)
	{
	}
	
	void OnStompSomeone(Character stompee)
	{
	}
	
	void OnGetStomped(Character stomper)
	{
	}
	
	void OnGetThreatened(Player threatener)
	{
	}
	
	void OnGetUnthreatened()
	{
	}
	#endregion
	
	#region Spawning/Death
	void OnSpawn()
	{
	}
	
	void OnDie(DeathType type)
	{
		switch(type)
		{
		case DeathType.Burn:
			break;
		case DeathType.Crush:
			break;
		case DeathType.Drown:
			break;
		case DeathType.Electrocute:
			break;
		case DeathType.Fall:
			break;
		case DeathType.Freeze:
			break;
		case DeathType.Lazer:
			break;
		default:
			break;
		}
	}
	
	void OnDeathAnimationEnd() // Animation event
	{
		
	}
	#endregion
	
	#region Items
	void OnPickUpItem(Item item)
	{
	}
	
	void OnItemThrowStart()
	{
	}
	
	void OnItemThrowEnd()
	{
	}
	
	void OnItemThrowInterrupted()
	{
	}
	
	void OnItemPlace(Vector3 placePosition)
	{
	}

	void OnHitByItem(Item item)
	{
	}

    void OnHitByFlubBall(Vector2 force)
    {
    }
	#endregion
	
	#region Status Effects
	void OnTagByPotatoBomb(Character tagger)
	{
	}
	
	void OnFreeze(float duration)
	{
	}
	
	void OnUnfreeze()
	{
	}
	
	void OnStun(float duration)
	{
	}
	
	void OnUnstun()
	{
	}
	
	void OnStomp(float duration)
	{
	}
	
	void OnUnstomp()
	{
	}
	
	void OnExplode(Vector2 force)
	{
	}
	
	void OnExplodeEnd()
	{
	}
	
	void OnSpawnProtectionEnd()
	{
	}

	void OnTeleport()
	{
	}
	#endregion
	
	#region Pausing
	void OnPause()
	{
	}
	
	void OnUnpause()
	{
	}
	
	void OnRoundStart()
	{
	}
	
	void OnRoundEnd()
	{
	}
	#endregion
}
