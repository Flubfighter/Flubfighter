using UnityEngine;
using System.Collections;

public class CharacterMovement : MoveWithPlatform
{
	[Header("Physics Materials")]
	public PhysicsMaterial2D normalMaterial;
	public PhysicsMaterial2D bouncyMaterial;
	[Header("Speed Settings")]
	public float runSpeed;
	public float runMaxSpeedChange;
	public float maxSlipperySpeedChange;
	public float chargeRunSpeed, chargeRunMaxSpeedChange;
	public float potatoBombRunSpeed, potatobombRunMaxSpeedChange;
	[Header("Jump Settings")]
	public AnimationCurve groundJumpSpeedVsTime;
	public float groundJumpEndSpeed;
	public AnimationCurve airJumpSpeedVsTime;
	public float airJumpEndSpeed;
	[Header("Punch Settings")]
	[Tooltip("The speed you move when you punch (y) vs charge time (x)")]
	public AnimationCurve punchMovementSpeed;
	public float punchEndSpeed;
	[Header("Stomp Settings")]
	public float stompDownwardForce;
	public float stompBounceForce;
	public float stompPunchMultiplier;

	private Character character;
	private float jumpStartSpeed;

	protected override void Awake()
	{
		base.Awake();
		character = GetComponent<Character> ();
	}
	
	void FixedUpdate()
	{
		if(character.IsGroundJumping)
		{
			ApplyJumpForce(jumpStartSpeed, groundJumpSpeedVsTime, character.TimeGroundJumping);
		}
		if(character.IsAirJumping)
		{
			ApplyJumpForce(jumpStartSpeed, airJumpSpeedVsTime, character.TimeAirJumping);
		}
        
	}

	void ApplyJumpForce(float startVelocity, AnimationCurve velocityCurve, float time)
	{
		float desiredVelocity = velocityCurve.Evaluate(time);
		float requiredVelocityChange = desiredVelocity - rigidbody2D.velocity.y;
		AddForceWithMass(Vector2.up * requiredVelocityChange, ForceMode2D.Impulse);
	}

	void AddForceWithMass(Vector2 force, ForceMode2D mode)
	{
		rigidbody2D.AddForce (force * rigidbody2D.mass, ForceMode2D.Impulse);
	}

	void ClampVelocity(float magnitude)
	{
		rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, magnitude);
	}

	#region Movement
	void OnMovementInput(float input) // Input in range (-1f, 1f)
	{
		if(input == 0f)
			return;
		if(character.IsChargingPunch)
			ApplyMovementForce(input, chargeRunSpeed, chargeRunMaxSpeedChange);
		else if(character.IsTaggedByPotatoBomb || character.IsTaggedByBeacon)
			ApplyMovementForce(input, potatoBombRunSpeed, potatobombRunMaxSpeedChange);
		else
			ApplyMovementForce(input, runSpeed, runMaxSpeedChange);
	}

	void ApplyMovementForce(float input, float speed, float maxSpeedChange)
	{
		// Fi = mΔv
		float requiredChangeInSpeed = Mathf.Clamp(input * speed - rigidbody2D.velocity.x, -maxSpeedChange, maxSpeedChange);
		Vector2 force = Vector2.right * requiredChangeInSpeed + GetSlopeForce();
		if(character.IsSlippery && character.IsGrounded)
			force = Vector2.ClampMagnitude(force, maxSlipperySpeedChange);
		//rotating by normal
		force = Quaternion.FromToRotation(Vector3.up, normal) * force;
		Debug.DrawRay(transform.position, force * 5, Color.red);
		AddForceWithMass(force, ForceMode2D.Impulse);
	}

	Vector2 GetSlopeForce()
	{
		// Ignore collision with myself
		int oldLayer = gameObject.layer;
		gameObject.SetLayer(LayerMask.NameToLayer("Ignore Raycast"));
		RaycastHit2D hitinfo = Physics2D.Raycast (transform.position, Vector3.down, 1f); // Do cast
		gameObject.SetLayer(oldLayer); // Restore collison layers

		// If thing under me
		if(hitinfo.collider)
		{
			Vector3 slopeVector = Vector3.Cross(hitinfo.normal, Vector3.back); // Get slope vector
			float angle = Vector3.Angle(slopeVector, Vector3.left); // Get angle
			// Correct the slopeVector based on which way the ground is faceing
			if(hitinfo.normal.x < 0)
			{
				slopeVector.y = (Mathf.Abs(slopeVector.y));
				slopeVector.x = (Mathf.Abs(slopeVector.x));
			}
//			Debug.Log("slope: " + slopeVector + "angle: " + angle + " Name: " + hitinfo.collider.name);
			// Apply  force based on slope
			if(angle < 15f || angle > 60f)
				return Vector2.zero;
			else if(angle < 30f)
				return slopeVector * 0.5f;
			else
				return slopeVector * 1f;
			
		}
		return Vector2.zero;
	}
	
	void OnFootstep() // Animation event
	{
	}
	
	void OnJumpStart()
	{
		jumpStartSpeed = rigidbody2D.velocity.y;
	}
	
	void OnJumpEnd()
	{
		ClampVelocity(groundJumpEndSpeed);
	}
	
	void OnAirJumpStart()
	{
		jumpStartSpeed = rigidbody2D.velocity.y;
	}
	
	void OnAirJumpEnd()
	{
		ClampVelocity(airJumpEndSpeed);
	}
	
	void OnHitHead()
	{
	}
	
	void OnGrounded()
	{
//		if (character.IsExploded) 
//		{
//			Debug.LogWarning("Bugfix: Setting velocity to Vector3.zero to try and fix that explosion-sliding (ticket #209), please test me!\nhttps://www.assembla.com/spaces/retora-internship-2014/tickets/209#");
//			rigidbody2D.velocity = Vector2.up * 0.1f;
//		}
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
		rigidbody2D.velocity = Vector2.zero;
		AddForceWithMass(character.AimDirection.normalized * punchMovementSpeed.Evaluate(character.TimePunchCharging), ForceMode2D.Impulse);
	}
	
	void OnPunchStart()
	{
		rigidbody2D.velocity = Vector2.zero;
		AddForceWithMass(character.AimDirection.normalized * punchMovementSpeed.Evaluate(character.TimePunchCharging), ForceMode2D.Impulse);
	}
	
	void OnPunchEnd()
	{
		ClampVelocity(punchEndSpeed);
	}
	
	void OnPunchEndSuddenly()
	{
//		ClampVelocity(punchEndSpeed);
		ClampVelocity(0f);
	}

	void OnPunchCooldownEnd()
	{
	}

	void OnPunchButton(PunchTrigger trigger)
	{
		ClampVelocity (0f);
	}

	void OnPunchSomeone(Character punchee)
	{
		ClampVelocity(punchEndSpeed);
	}
	
	void OnGetPunched(Character puncher)
	{
		Vector2 force = puncher.GetPunchForce();
		if(character.IsStomped)
			force *= stompPunchMultiplier;
		AddForceWithMass(force, ForceMode2D.Impulse);
	}
	
	void OnStompSomeone(Character stompee)
	{
		rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x, 0f);
		AddForceWithMass(Vector2.up * stompBounceForce, ForceMode2D.Impulse);
	}
	
	void OnGetStomped(Character stomper)
	{
		rigidbody2D.velocity = Vector2.zero;
		AddForceWithMass(-Vector2.up * stompDownwardForce, ForceMode2D.Impulse);
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
		case DeathType.Crush:
		case DeathType.Drown:
		case DeathType.Electrocute:
		case DeathType.Fall:
		case DeathType.Freeze:
		case DeathType.Lazer:
//			rigidbody2D.velocity = Vector2.zero;
//			break;
		default:
			rigidbody2D.velocity = Vector2.zero;
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
        rigidbody2D.velocity = force;
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
		SetPhysicsMaterial2D(bouncyMaterial);
	}
	
	void OnUnstun()
	{
		if(!character.IsExploded)
			SetPhysicsMaterial2D(normalMaterial);
	}
	
	void OnStomp(float duration)
	{
	}
	
	void OnUnstomp()
	{
	}
	
	void OnExplode(Vector2 force)
	{
		SetPhysicsMaterial2D(bouncyMaterial);
		AddForceWithMass (force, ForceMode2D.Impulse);
	}
	
	void OnExplodeEnd()
	{
		if(!character.IsStunned)
			SetPhysicsMaterial2D(normalMaterial);
	}
	
	void OnSpawnProtectionEnd()
	{
	}

	void SetPhysicsMaterial2D(PhysicsMaterial2D mat)
	{
		collider2D.sharedMaterial = mat;
		collider2D.enabled = false; // NOTE: Fix for bug where material change doesn't take effect, see http://answers.unity3d.com/questions/579995/physics-material-2d-change-during-runtime-not-taki.html
		collider2D.enabled = true;
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
