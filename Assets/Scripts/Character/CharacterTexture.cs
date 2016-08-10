using UnityEngine;
using System.Collections;

public class CharacterTexture : MonoBehaviour
{
	public SkinnedMeshRenderer skin;
	public TrailRenderer[] fistTrails;

	Character character;

	void Awake()
	{
		character = GetComponent<Character> ();
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
	}
	
	void OnPunchEnd()
	{
	}
	
	void OnPunchEndSuddenly()
	{
	}
	
	void OnPunchCooldownEnd()
	{
	}

	void OnPunchButton(PunchTrigger trigger)
	{
	}
	
	void OnPunchSomeone(Character punchee)
	{
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
		if(character.Player.team)
		{
			skin.material = character.Player.team.characterMaterial;
			foreach(TrailRenderer r in fistTrails)
			{
				r.material.SetColor("_TintColor", character.Player.team.fistTrailColor);
			}
		}
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

