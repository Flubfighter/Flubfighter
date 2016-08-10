using UnityEngine;
using System.Collections;

public class CharacterAnimations : MonoBehaviour
{
	private Character character;
	private Animator animator;

	void Awake()
	{
		character = GetComponent<Character> ();
		animator = GetComponent<Animator> ();
	}

	void Update()
	{
		animator.SetFloat ("Vertical Velocity", rigidbody2D.velocity.y);
		animator.SetFloat ("Aim Y", character.AimDirection.y);
		if(character.IsChargingPunch)
		{
			animator.SetFloat("Charge Time", character.TimePunchCharging);
		}
	}

	#region Movement
	void OnMovementInput(float input) // Input in range (-1f, 1f)
	{
		animator.SetFloat ("Horizontal Input", Mathf.Abs(input));
	}
	
	void OnFootstep() // Animation event
	{
	}
	
	void OnJumpStart()
	{
		animator.SetTrigger ("Jump");
	}
	
	void OnJumpEnd()
	{
	}
	
	void OnAirJumpStart()
	{
		animator.SetTrigger ("Air Jump");
	}
	
	void OnAirJumpEnd()
	{
	}
	
	void OnHitHead()
	{
	}
	
	void OnGrounded()
	{
		animator.SetBool ("Grounded", true);
	}
	
	void OnUngrounded()
	{
		animator.SetBool ("Grounded", false);
	}
	#endregion
	
	#region Punch
	void OnPunchChargeStart()
	{
		animator.SetTrigger ("Charge Punch");
	}
	
	void OnPunchChargeInterrupted()
	{
		animator.SetTrigger ("Punch Interrupted");
	}
	
	void OnPunchOvercharge()
	{
		animator.SetTrigger ("Release Punch");
	}
	
	void OnPunchStart()
	{
		animator.SetTrigger ("Release Punch");
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
		animator.SetTrigger ("Knockback");
	}
	
	void OnStompSomeone(Character stompee)
	{
		animator.SetTrigger ("Stomp Someone");
	}
	
	void OnGetStomped(Character stomper)
	{
		animator.SetTrigger ("Get Stomped");
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
//		animator.speed = 1f;
		animator.SetInteger("Death Type", (int)type);
		animator.SetTrigger("Died");
	}
	
	void OnDeathAnimationEnd() // Animation event
	{
		
	}
	#endregion
	
	#region Items
	void OnPickUpItem(Item item)
	{
		animator.SetTrigger ("Take Item");
	}
	
	void OnItemThrowStart()
	{
		animator.SetTrigger("Item Throw Hold");
	}
	
	void OnItemThrowEnd()
	{
		animator.SetTrigger ("Item Throw Release");
	}
	
	void OnItemThrowInterrupted()
	{
		animator.SetTrigger ("Item Throw Interrupted");
	}
	
	void OnItemPlace(Vector3 placePosition)
	{
		animator.SetTrigger ("Place Item");
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
		animator.speed = 0.0f;
	}
	
	void OnUnfreeze()
	{
		animator.speed = 1.0f;
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
		animator.SetTrigger ("Exploded");
		animator.SetFloat ("Horizontal Input", 0f);
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
		// TODO: Pause
	}
	
	void OnUnpause()
	{
		// TODO: Resume
	}
	
	void OnRoundStart()
	{
	}
	
	void OnRoundEnd()
	{
		animator.SetFloat ("Horizontal Input", 0f);
	}
	#endregion
}