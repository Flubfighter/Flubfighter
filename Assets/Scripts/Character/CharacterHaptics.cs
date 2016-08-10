using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Needs variables and input from designers
// TODO: Make Vibration amount (curve evaluate) mapped to 0-1 range so designers can have it easy (0-1::0-duration)
public class CharacterHaptics : MonoBehaviour
{
	public enum VibrationMode
	{
		Strongest,
		Average,
		Total
	}

	[System.Serializable]
	public class Vibration : System.Object
	{
		public bool silenced = false;
		public AnimationCurve left;
		public AnimationCurve right;
		public float duration = 0f;
		public float elapsed = 0f;
		public float scale = 1f;
	}

	public VibrationMode vibrationMode = VibrationMode.Total;

	[Header("Movement Haptics")]
	public Vibration movementInput;
	public Vibration footstep;
	public Vibration jumpStart;
	public Vibration jumpEnd;
	public Vibration airJumpStart;
	public Vibration airJumpEnd;
	public Vibration hitHead;
	public Vibration grounded;
	public Vibration ungrounded;
	[Header("Punch Haptics")]
	public Vibration punchChargeStart;
	public Vibration punchChargeInterrupted;
	public Vibration punchOvercharge;
	public Vibration punchStart;
	public Vibration punchEnd;
	public Vibration punchEndSuddenly;
	public Vibration punchCooldownEnd;
	public Vibration punchButton;
	public Vibration punchSomeone;
	public Vibration getPunched;
	public Vibration stompSomeone;
	public Vibration getStomped;
	public Vibration getThreatened;
	public Vibration getUnthreatened;
	[Header("Death Haptics")]
	public Vibration spawn;
	public Vibration dieGeneric;
	public Vibration dieBurn;
	public Vibration dieCrush;
	public Vibration dieDrown;
	public Vibration dieElectrocute;
	public Vibration dieFreeze;
	public Vibration dieLazer;
	public Vibration dieFall;
	public Vibration deathAnimationEnd;
	[Header("Item Haptics")]
	public Vibration pickUpItem;
	public Vibration itemThrowStart;
	public Vibration itemThrowEnd;
	public Vibration itemThrowInterrupted;
	public Vibration itemPlace;
	public Vibration hitByItem;
	public Vibration hitByFlubBall;
	[Header("Item Effects Haptics")]
	public Vibration tagByPotatoBomb;
	public Vibration freeze;
	public Vibration unfreeze;
	public Vibration stun;
	public Vibration unstun;
	public Vibration stomp;
	public Vibration unstomp;
	public Vibration explode;
	public Vibration explodeEnd;
	public Vibration spawnProtectionEnd;
	public Vibration teleport;
	[Header("Round Haptics")]
	public Vibration pause;
	public Vibration unpause;
	public Vibration roundStart;
	public Vibration roundEnd;
	[Header("Loop Haptics")]
	public Vibration punchChargeLoop;
	public Vibration punchLoop;
	public Vibration itemThrowLoop;
	public Vibration freezeLoop;
	public Vibration stunLoop;
	public Vibration stompLoop;
	public Vibration explodeLoop;
	
	private bool silenced = false;
	private Character character;
	private HashSet<Vibration> vibrations = new HashSet<Vibration>();
	
	void Awake()
	{
		character = GetComponent<Character>();
	}
	
	void OnDestroy()
	{
		StopAll();
	}

	void FixedUpdate()
	{
		foreach(Vibration v in vibrations)
		{
			v.elapsed += Time.deltaTime;
		}
		UpdateVibrations();
	}
	
	void UpdateVibrations()
	{
		float totalLeft = 0f;
		float totalRight = 0f;

		if(!silenced)
		{
			if(vibrationMode == VibrationMode.Strongest)
			{
				foreach(Vibration vibration in vibrations)
				{
					totalLeft = Mathf.Max(totalLeft, vibration.left.Evaluate(vibration.elapsed / vibration.duration) * vibration.scale);
					totalRight = Mathf.Max(totalRight, vibration.right.Evaluate(vibration.elapsed / vibration.duration) * vibration.scale);
				}
			}
			else
			{
				foreach(Vibration vibration in vibrations)
				{
					totalLeft += vibration.left.Evaluate(vibration.elapsed / vibration.duration) * vibration.scale;
					totalRight += vibration.right.Evaluate(vibration.elapsed / vibration.duration) * vibration.scale;
				}
				if(vibrationMode == VibrationMode.Average)
				{
					totalLeft /= (float)vibrations.Count;
					totalRight /= (float)vibrations.Count;
				}
			}
			totalLeft = Mathf.Clamp01(totalLeft);
			totalRight = Mathf.Clamp01(totalRight);
		}
		SetVibration(totalLeft, totalRight);
	}

	void SetVibration(float left, float right)
	{
		try
		{
			character.Player.input.SetVibration(left, right);
		}
		#pragma warning disable 168
		catch(System.Exception e) { }
		#pragma warning restore 168
	}
	
	void Play(Vibration vibration, float scale = 1f)
	{
		if(vibration.silenced)
			return;
		vibration.scale = scale;
		vibration.elapsed = 0f;
		vibrations.Add(vibration);
		StartCoroutine(StopAfterTime(vibration, vibration.duration));
	}
	
	IEnumerator StopAfterTime(Vibration vibration, float duration)
	{
		yield return new WaitForSeconds(duration);
		Stop(vibration);
	}
	
	void PlayLoop(Vibration vibration, float scale=1f)
	{
		if(vibration.silenced)
			return;
		vibration.scale = scale;
		vibration.elapsed = 0f;
		vibrations.Add(vibration);
	}
	
	void Stop(Vibration vibration)
	{
		if(vibrations.Contains(vibration))
			vibrations.Remove(vibration);
	}
	
	void StopAll()
	{
		StopAllCoroutines();
		vibrations = new HashSet<Vibration>();
		SetVibration(0f, 0f);
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
		PlayLoop (punchLoop);
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
		// TODO: Scale based on punch force
		Play (getPunched, 1f);
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
		PlayLoop (freezeLoop);
	}
	
	void OnUnfreeze()
	{
		Stop(freezeLoop);
		Play (unfreeze);
	}
	
	void OnStun(float duration)
	{
		Play (stun);
		PlayLoop(stunLoop);
	}
	
	void OnUnstun()
	{
		Stop(stunLoop);
		Play (unstun);
	}
	
	void OnStomp(float duration)
	{
		Play (stomp);
		PlayLoop (stompLoop);
	}
	
	void OnUnstomp()
	{
		Stop (stompLoop);
		Play (unstomp);
	}
	
	void OnExplode(Vector2 force)
	{
		// TODO: Scale based on explode force
		Play(explode, 1f);
		PlayLoop(explodeLoop, 1f);
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
		silenced = true;
	}
	
	void OnUnpause()
	{
		silenced = false;
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