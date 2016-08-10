using UnityEngine;
using System.Collections;

// TODO: Colorize particles
public class CharacterParticles : MonoBehaviour
{
	[System.Serializable]
	public class ParticleBlock : System.Object
	{
		public ParticleSystem particle;
		public bool matchTeamColor;
	}

	public float deathParticleLifespan = 3f;
	[Header("Movement Particles")]
	public ParticleBlock[] movementInput;
	public ParticleBlock[] footstep;
	public ParticleBlock[] jumpStart;
	public ParticleBlock[] jumpEnd;
	public ParticleBlock[] airJumpStart;
	public ParticleBlock[] airJumpEnd;
	public ParticleBlock[] hitHead;
	public ParticleBlock[] grounded;
	public ParticleBlock[] ungrounded;
	[Header("Punch Particles")]
	public ParticleBlock[] punchChargeStart;
	public ParticleBlock[] punchChargeInterrupted;
	public ParticleBlock[] punchOvercharge;
	public ParticleBlock[] punchStart;
	public ParticleBlock[] punchEnd;
	public ParticleBlock[] punchEndSuddenly;
	public ParticleBlock[] punchCooldownEnd;
	public ParticleBlock[] punchButton;
	public ParticleBlock[] punchSomeone;
	public ParticleBlock[] getPunched;
	public ParticleBlock[] stompSomeone;
	public ParticleBlock[] getStomped;
	public ParticleBlock[] getThreatened;
	public ParticleBlock[] getUnthreatened;
	[Header("Death Particles")]
	public ParticleBlock[] spawn;
	public ParticleBlock[] dieGeneric;
	public ParticleBlock[] dieBurn;
	public ParticleBlock[] dieCrush;
	public ParticleBlock[] dieDrown;
	public ParticleBlock[] dieElectrocute;
	public ParticleBlock[] dieFreeze;
	public ParticleBlock[] dieLazer;
	public ParticleBlock[] dieFall;
	public ParticleBlock[] deathAnimationEnd;
	[Header("Item Particles")]
	public ParticleBlock[] pickUpItem;
	public ParticleBlock[] itemThrowStart;
	public ParticleBlock[] itemThrowEnd;
	public ParticleBlock[] itemThrowInterrupted;
	public ParticleBlock[] itemPlace;
	public ParticleBlock[] hitByItem;
	public ParticleBlock[] hitByFlubBall;
	[Header("Item Effect Particles")]
	public ParticleBlock[] tagByPotatoBomb;
	public ParticleBlock[] freeze;
	public ParticleBlock[] unfreeze;
	public ParticleBlock[] stun;
	public ParticleBlock[] unstun;
	public ParticleBlock[] stomp;
	public ParticleBlock[] unstomp;
	public ParticleBlock[] explode;
	public ParticleBlock[] explodeEnd;
	public ParticleBlock[] spawnProtectionEnd;
	public ParticleBlock[] teleport;
	[Header("Round Particles")]
	public ParticleBlock[] pause;
	public ParticleBlock[] unpause;
	public ParticleBlock[] roundStart;
	public ParticleBlock[] roundEnd;
	[Header("Loop Particles")]
	public ParticleBlock[] punchChargeLoop;
	public ParticleBlock[] punchLoop;
	public ParticleBlock[] itemThrowLoop;
	public ParticleBlock[] freezeLoop;
	public ParticleBlock[] stunLoop;
	public ParticleBlock[] stompLoop;
	public ParticleBlock[] explodeLoop;

	private Character character;

	void Awake()
	{
		character = GetComponent<Character> ();
	}

	void Play(ParticleBlock[] systems)
	{
		foreach(ParticleBlock system in systems)
		{
			if(system.particle)
			{
				if(system.matchTeamColor)
					system.particle.SetStartColorRecursively(character.Player.team.particleColor);
				system.particle.Play ();
			}
		}
	}
	
	void Stop(ParticleBlock[] systems)
	{
		foreach(ParticleBlock system in systems)
		{
			if(system.particle)
				system.particle.Stop ();
		}
	}
	
	#region Movement
	void OnMovementInput(float input) // Input in range (-1f, 1f)
	{
		Play (movementInput);
	}
	
	void OnFootstep() // Animation event
	{
		Play (footstep);
	}
	
	void OnJumpStart()
	{
		Play (jumpStart);
	}
	
	void OnJumpEnd()
	{
		Play (jumpEnd);
	}
	
	void OnAirJumpStart()
	{
		Play (airJumpStart);
	}
	
	void OnAirJumpEnd()
	{
		Play (airJumpEnd);
	}
	
	void OnHitHead()
	{
		Play (hitHead);
	}
	
	void OnGrounded()
	{
		Play (grounded);
	}
	
	void OnUngrounded()
	{
		Play (ungrounded);
	}
	#endregion
	
	#region Punch
	void OnPunchChargeStart()
	{
		Play (punchChargeStart);
		Play(punchChargeLoop);
	}
	
	void OnPunchChargeInterrupted()
	{
		Stop(punchChargeLoop);
		Play (punchChargeInterrupted);
	}
	
	void OnPunchOvercharge()
	{
		Stop(punchChargeLoop);
		Play (punchOvercharge);
	}
	
	void OnPunchStart()
	{
		Stop(punchChargeLoop);
		Play (punchStart);
		Play (punchLoop);
	}
	
	void OnPunchEnd()
	{
		Stop(punchLoop);
		Play (punchEnd);
	}
	
	void OnPunchEndSuddenly()
	{
		Stop(punchLoop);
		Play (punchEndSuddenly);
	}

	void OnPunchCooldownEnd()
	{
		Play (punchCooldownEnd);
	}

	void OnPunchButton(PunchTrigger trigger)
	{
		Play (punchButton);
	}
	
	void OnPunchSomeone(Character punchee)
	{
		Play (punchSomeone);
	}
	
	void OnGetPunched(Character puncher)
	{
		Play (getPunched);
	}
	
	void OnStompSomeone(Character stompee)
	{
		Play (stompSomeone);
	}
	
	void OnGetStomped(Character stomper)
	{
		Play (getStomped);
	}
	
	void OnGetThreatened(Player threatener)
	{
		Play (getThreatened);
	}
	
	void OnGetUnthreatened()
	{
		Play (getUnthreatened);
	}
	#endregion
	
	#region Spawning/Death
	void OnSpawn()
	{
		Play (spawn);
	}
	
	void OnDie(DeathType type)
	{
		switch(type)
		{
		case DeathType.Burn:
			Play (dieBurn);
			break;
		case DeathType.Crush:
			Play (dieCrush);
			break;
		case DeathType.Drown:
			Play (dieDrown);
			break;
		case DeathType.Electrocute:
			Play (dieElectrocute);
			break;
		case DeathType.Fall:
			Play (dieFall);
			break;
		case DeathType.Freeze:
			Play (dieFreeze);
			break;
		case DeathType.Lazer:
			Play (dieLazer);
			break;
		default:
			Play (dieGeneric);
			break;
		}
	}
	
	void OnDeathAnimationEnd() // Animation event
	{
		Play(deathAnimationEnd);
		GameObject particleGroup = new GameObject (name + " Particle Jettison Group");
		foreach(ParticleSystem system in transform.GetComponentsInChildren<ParticleSystem>())
			system.transform.parent = particleGroup.transform;
		Destroy (particleGroup, deathParticleLifespan);
	}
	#endregion
	
	#region Items
	void OnPickUpItem(Item item)
	{
		Play (pickUpItem);
	}
	
	void OnItemThrowStart()
	{
		Play (itemThrowStart);
		Play(itemThrowLoop);
	}
	
	void OnItemThrowEnd()
	{
		Stop(itemThrowLoop);
		Play (itemThrowEnd);
	}
	
	void OnItemThrowInterrupted()
	{
		Stop(itemThrowLoop);
		Play (itemThrowInterrupted);
	}
	
	void OnItemPlace(Vector3 placePosition)
	{
		Play (itemPlace);
	}

	void OnHitByItem(Item item)
	{
		Play (hitByItem);
	}

    void OnHitByFlubBall(Vector2 force)
    {
        Play(hitByFlubBall);
    }
	#endregion
	
	#region Status Effects
	void OnTagByPotatoBomb(Character tagger)
	{
		Play (tagByPotatoBomb);
	}
	
	void OnFreeze(float duration)
	{
		Play (freeze);
		Play (freezeLoop);
	}
	
	void OnUnfreeze()
	{
		Stop(freezeLoop);
		Play (unfreeze);
	}
	
	void OnStun(float duration)
	{
		Play (stun);
		Play(stunLoop);
	}
	
	void OnUnstun()
	{
		Stop(stunLoop);
		Play (unstun);
	}
	
	void OnStomp(float duration)
	{
		Play (stomp);
		Play (stompLoop);
	}
	
	void OnUnstomp()
	{
		Stop (stompLoop);
		Play (unstomp);
	}
	
	void OnExplode(Vector2 force)
	{
		Play(explode);
		Play(explodeLoop);
	}
	
	void OnExplodeEnd()
	{
		Stop(explodeLoop);
		Play (explodeEnd);
	}
	
	void OnSpawnProtectionEnd()
	{
		Play (spawnProtectionEnd);
	}

	void OnTeleport()
	{
		Play (teleport);
	}
	#endregion
	
	#region Pausing
	void OnPause()
	{
		// TODO: Pause particles
	}
	
	void OnUnpause()
	{
		// TODO: Resume particles
	}
	
	void OnRoundStart()
	{
		Play (roundStart);
	}
	
	void OnRoundEnd()
	{
		Play (roundEnd);
	}
	#endregion
}
