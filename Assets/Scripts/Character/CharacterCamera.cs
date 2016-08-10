using UnityEngine;
using System.Collections;

public class CharacterCamera : MonoBehaviour
{
	[Header("Rattle Type")]
	[EnumFlag] public DeathType deathRattleTypes;
	[Header("Rattle Settings")]
	public float deathRattleStrength;
	public float deathRattleDuration;
	public AnimationCurve punchRattleStrengthVsPunchChargeTime;
	public AnimationCurve punchRattleDurationVsPunchChargeTime;
	public float stompRattleStrength;
	public float stompRattleDuration;
    public float hitByFlubBallDuration, hitByFlubBallStrength;
	
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
		CameraController.Rattle (
			punchRattleDurationVsPunchChargeTime.Evaluate (puncher.TimePunchCharging),
			punchRattleStrengthVsPunchChargeTime.Evaluate (puncher.TimePunchCharging));
	}
	
	void OnStompSomeone(Character stompee)
	{
	}
	
	void OnGetStomped(Character stomper)
	{
		CameraController.Rattle (stompRattleDuration, stompRattleStrength);
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
		if((type & deathRattleTypes) == type)
			CameraController.Rattle (deathRattleDuration, deathRattleStrength);
//		switch(type)
//		{
//		case DeathType.Burn:
//			break;
//		case DeathType.Crush:
//			break;
//		case DeathType.Drown:
//			break;
//		case DeathType.Electrocute:
//			break;
//		case DeathType.Fall:
//			break;
//		case DeathType.Freeze:
//			break;
//		case DeathType.Lazer:
//			break;
//		default:
//			break;
//		}
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
        CameraController.Rattle(hitByFlubBallDuration, hitByFlubBallStrength);
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
