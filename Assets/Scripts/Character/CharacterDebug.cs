using UnityEngine;
using System.Collections;
using System.Linq;

public class CharacterDebug : MonoBehaviour
{
	enum LogLevel
	{
		Normal=0,
		Warning,
		Error,
		Exception
	}
	
	public bool silent = false;
	public bool logAll = false;
	public string[] silencedEvents;
	private Character character;
	
	void Awake()
	{
		character = GetComponent<Character>();
	}
	
	void Log(string method, object message, LogLevel logLevel=LogLevel.Normal)
	{
		if((!silencedEvents.Contains(method) || logAll) && !silent)
		{
			string formattedMessage = string.Format("{0}:\t{1}:\t{2}", character.Player, method, message);
			if(logLevel == LogLevel.Normal)
				Debug.Log(formattedMessage);
			else if(logLevel == LogLevel.Warning)
				Debug.LogWarning(formattedMessage);
			else if(logLevel == LogLevel.Error)
				Debug.LogError(formattedMessage);
			else
				Debug.LogException(new System.Exception(formattedMessage));
		}
	}

//	public object movementInput, footstep, jumpStart, jumpEnd, airJumpStart, airJumpEnd, hitHead, grounded, ungrounded;
//	public object punchChargeStart, punchChargeInterrupted, punchOvercharge, punchStart, punchEnd, punchEndSuddenly, punchSomeone, getPunched, stompSomeone, getStomped;
//	public object spawn, dieGeneric, dieBurn, dieCrush, dieDrown, dieElectrocute, dieFreeze, dieLazer, dieFall, deathAnimationEnd;
//	public object pickUpItem, itemThrowStart, itemThrowEnd, itemThrowInterrupted, itemPlace;
//	public object tagByPotatoBomb, freeze, unfreeze, stun, unstun, stomp, unstomp, explode, explodeEnd, spawnProtectionEnd;
//	public object pause, unpause;
	
	#region Movement
	void OnMovementInput(float input) // Input in range (-1f, 1f)
	{
		Log("OnMovementInput", "Movement Input=" + input);
	}
	
	void OnFootstep() // Animation event
	{
		Log ("OnFootstep", "Footestep");
	}
	
	void OnJumpStart()
	{
		Log ("OnJumpStart", "Jump Started");
	}
	
	void OnJumpEnd()
	{
		Log ("OnJumpEnd", "Jump Ended (released key or timed out)");
	}
	
	void OnAirJumpStart()
	{
		Log ("OnAirJumpStart", "Air Jump Started");
	}
	
	void OnAirJumpEnd()
	{
		Log ("OnAirJumpEnd", "Air Jump Ended (released key or timed out");
	}
	
	void OnHitHead()
	{
		Log ("OnHitHead", "Head Head");
	}
	
	void OnGrounded()
	{
		Log ("OnGrounded", "Became Grounded");
	}
	
	void OnUngrounded()
	{
		Log ("OnUngrounded", "Became Ungrounded");
	}
	#endregion
	
	#region Punch
	void OnPunchChargeStart()
	{
		Log ("OnPunchChargeStart", "Punch Charge Started");
	}
	
	void OnPunchChargeInterrupted()
	{
		Log ("OnPunchChargeInterrupted", "Punch Charge Interrupted");
	}
	
	void OnPunchOvercharge()
	{
		Log ("OnPunchOvercharge", "Punch Overcharged (Held Too Long)");
	}
	
	void OnPunchStart()
	{
		Log ("OnPunchStart", "Punch Started");
	}
	
	void OnPunchEnd()
	{
		Log ("OnPunchEnd", "Punch Ended");
	}
	
	void OnPunchEndSuddenly()
	{
		Log ("OnPunchEndSuddenly", "Punch Ended Suddenly");
	}

	void OnPunchCooldownEnd()
	{
		Log ("OnPunchCooldownEnd", "Punch Cooldown Ended");
	}

	void OnPunchButton(PunchTrigger trigger)
	{
		Log ("OnPunchButton", "Punched Button=" + trigger);
	}

	void OnPunchSomeone(Character punchee)
	{
		Log ("OnPunchSomeone", "Punched Someone=" + punchee);
	}
	
	void OnGetPunched(Character puncher)
	{
		Log ("OnGetPunched", "Punched by " + puncher);
	}
	
	void OnStompSomeone(Character stompee)
	{
		Log ("OnStompSomeone", "Stomped " + stompee);
	}
	
	void OnGetStomped(Character stomper)
	{
		Log("OnGetStomped", "Stomped by " + stomper);
	}

	void OnGetThreatened(Player threatener)
	{
		Log ("OnGetThreatened", "Threatened by " + threatener);
	}

	void OnGetUnthreatened()
	{
		Log ("OnGetUnthreatened", "Became unthreatened");
	}
	#endregion
	
	#region Spawning/Death
	void OnSpawn()
	{
		Log ("OnSpawn", "Spawned");
	}
	
	void OnDie(DeathType type)
	{
		Log("OnDie", "Die=" + type);
	}
	
	void OnDeathAnimationEnd() // Animation event
	{
		Log ("OnDeathAnimationEnd", "Death Animation Ended");
	}
	#endregion
	
	#region Items
	void OnPickUpItem(Item item)
	{
		Log ("OnPickUpItem", "Picked up " + item);
	}
	
	void OnItemThrowStart()
	{
		Log ("OnItemThrowStart", "Item Throw Started");
	}
	
	void OnItemThrowEnd()
	{
		Log ("OnItemThrowEnd", "Item Throw Ended (Key Released)");
	}
	
	void OnItemThrowInterrupted()
	{
		Log ("OnItemThrowInterrupted", "Item Throw Interrupted");
	}
	
	void OnItemPlace(Vector3 placePosition)
	{
		Log ("OnItemPlace", "Item Placed=" + placePosition);
	}

	void OnHitByItem(Item item)
	{
		Log ("OnHitByItem", "Hit By Item=" + item);
	}

    void OnHitByFlubBall(Vector2 force)
    {
        Log("OnHitByFlubBall", "Hit by Flub Ball for " + force);
    }
	#endregion
	
	#region Status Effects
	void OnTagByPotatoBomb(Character tagger)
	{
		Log ("OnTagByPoatoBomb", "Tagged With Potato Bomb By " + tagger);
	}
	
	void OnFreeze(float duration)
	{
		Log ("OnFreeze", "Frozen for " + duration);
	}
	
	void OnUnfreeze()
	{
		Log ("OnUnfreeze", "Unfrozen");
	}
	
	void OnStun(float duration)
	{
		Log ("OnStun", "Stunned for " + duration);
	}
	
	void OnUnstun()
	{
		Log ("OnUnstun", "Unstunned");
	}
	
	void OnStomp(float duration)
	{
		Log ("OnStomp", "Stomped for " + duration);
	}
	
	void OnUnstomp()
	{
		Log ("OnUnstomp", "Unstomped");
	}
	
	void OnExplode(Vector2 force)
	{
		Log ("OnExplode", "Exploded, Force=" + force);
	}
	
	void OnExplodeEnd()
	{
		Log ("OnExplodeEnd", "Explosion Ended (No Longer Ragdolled)");
	}
	
	void OnSpawnProtectionEnd()
	{
		Log ("OnSpawnProtectionEnd", "Spawn Protection Ended");
	}

	void OnTeleport()
	{
		Log("OnTeleport", "Teleported");
	}
	#endregion
	
	#region Pausing
	void OnPause()
	{
		Log ("OnPause", "Paused");
	}
	
	void OnUnpause()
	{
		Log ("OnUnpause", "Unpaused");
	}
	
	void OnRoundStart()
	{
		Log ("OnRoundStart", "Round Started");
	}
	
	void OnRoundEnd()
	{
		Log ("OnRoundEnd", "Round Ended");
	}
	#endregion
}