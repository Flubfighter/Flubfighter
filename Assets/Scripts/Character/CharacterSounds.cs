using UnityEngine;
using System.Collections;

public class CharacterSounds : MonoBehaviour
{
	[System.Serializable]
	public class AudioField : System.Object
	{
		public AudioClip clip;
		[Tooltip("Volume is 0.0-1.0")]
		public float volume = 1f;
	}

	public AudioSource genericAudioSource;

	[Header("Movement Audio")]
	public AudioField movementInput;
	public AudioField footstep;
	public AudioField jumpStart;
	public AudioField jumpEnd;
	public AudioField airJumpStart;
	public AudioField airJumpEnd;
	public AudioField hitHead;
	public AudioField grounded;
	public AudioField ungrounded;
	[Header("Punching Audio")]
	public AudioField punchChargeStart;
	public AudioField punchChargeInterrupted;
	public AudioField punchOvercharge;
	public AudioField punchStart;
	public AudioField punchEnd;
	public AudioField punchEndSuddenly;
	public AudioField punchCooldownEnd;
	public AudioField punchButton;
	public AudioField punchSomeone;
	public AudioField getPunched;
	public AudioField stompSomeone;
	public AudioField getStomped;
	public AudioField getThreatened;
	public AudioField getUnthreatened;
	[Header("Death Audio")]
	public AudioField spawn;
	public AudioField dieGeneric;
	public AudioField dieBurn;
	public AudioField dieCrush;
	public AudioField dieDrown;
	public AudioField dieElectrocute;
	public AudioField dieFreeze;
	public AudioField dieLazer;
	public AudioField dieFall;
	public AudioField deathAnimationEnd;
	[Header("Item Audio")]
	public AudioField pickUpItem;
	public AudioField itemThrowStart;
	public AudioField itemThrowEnd;
	public AudioField itemThrowInterrupted;
	public AudioField itemPlace;
	public AudioField hitByItem;
	public AudioField hitByFlubBall;
	[Header("Item Effect Audio")]
	public AudioField tagByPotatoBomb;
	public AudioField freeze;
	public AudioField unfreeze;
	public AudioField stun;
	public AudioField unstun;
	public AudioField stomp;
	public AudioField unstomp;
	public AudioField explode;
	public AudioField explodeEnd;
	public AudioField spawnProtectionEnd;
	public AudioField teleport;
	[Header("Round Audio")]
	public AudioField pause;
	public AudioField unpause;
	public AudioField roundStart;
	public AudioField roundEnd;
	[Header("Loop Audio")]
	public AudioSource punchChargeLoop;
	public AudioSource punchLoop;
	public AudioSource itemThrowLoop;
	public AudioSource freezeLoop;
	public AudioSource stunLoop;
	public AudioSource stompLoop;
	public AudioSource explodeLoop;

	void Play(AudioField field)
	{
		genericAudioSource.PlayOneShot(field.clip, field.volume);
	}

	void PlayLoop(AudioSource source)
	{
		if(source)
			source.Play ();
	}

	void StopLoop(AudioSource source)
	{
		if(source)
			source.Stop ();
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
		PlayLoop(punchChargeLoop);
	}
	
	void OnPunchChargeInterrupted()
	{
		StopLoop(punchChargeLoop);
		Play (punchChargeInterrupted);
	}
	
	void OnPunchOvercharge()
	{
		StopLoop(punchChargeLoop);
		Play (punchOvercharge);
	}
	
	void OnPunchStart()
	{
		StopLoop(punchChargeLoop);
		Play (punchStart);
		PlayLoop (punchLoop);
	}
	
	void OnPunchEnd()
	{
		StopLoop(punchLoop);
		Play (punchEnd);
	}
	
	void OnPunchEndSuddenly()
	{
		StopLoop(punchLoop);
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
		Play (deathAnimationEnd);
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
		PlayLoop(itemThrowLoop);
	}
	
	void OnItemThrowEnd()
	{
		StopLoop(itemThrowLoop);
		Play (itemThrowEnd);
	}
	
	void OnItemThrowInterrupted()
	{
		StopLoop(itemThrowLoop);
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
		PlayLoop (freezeLoop);
	}
	
	void OnUnfreeze()
	{
		StopLoop(freezeLoop);
		Play (unfreeze);
	}
	
	void OnStun(float duration)
	{
		Play (stun);
		PlayLoop(stunLoop);
	}
	
	void OnUnstun()
	{
		StopLoop(stunLoop);
		Play (unstun);
	}

	void OnStomp(float duration)
	{
		Play (stomp);
		PlayLoop(stompLoop);
	}
	
	void OnUnstomp()
	{
		StopLoop(stompLoop);
		Play (unstomp);
	}
	
	void OnExplode(Vector2 force)
	{
		Play(explode);
		PlayLoop(explodeLoop);
	}
	
	void OnExplodeEnd()
	{
		StopLoop(explodeLoop);
		Play (explodeEnd);
	}
	
	void OnSpawnProtectionEnd()
	{
		Play (spawnProtectionEnd);
	}

	void OnTeleport()
	{
		Play(teleport);
	}
	#endregion
	
	#region Pausing
	void OnPause()
	{
		// TODO: Pause sounds
	}
	
	void OnUnpause()
	{
		// TODO: Resume sounds
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

