using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Character : MonoBehaviour, ICloudPlatformPassable
{
	public float spawnProtectionDuration;
	public GameObject itemHoldArm;

	[Header("Jump Settings")]
	public float minJumpTime;
	public float maxJumpTime;
	public float minAirJumpTime, maxAirJumpTime;

	[Header("Combat Settings")]
	public float maxPunchChargeTime = 1f;
	public float punchDuration = 1f;
	public float punchCooldown = 0f;
	public float placeDistance = 2.0f;
	public AnimationCurve punchStunDurationVsChargeTime;
	public float stompStunDuration = 1f;
	public float buttonPunchWindow = 0.1f;
	public float minimumThreatenTime = 0f; // The minimum amount of time a player has to be threatened after being punched

	[Header("Time Dilation")]
	public AnimationCurve punchEffectDelayVsChargeTime;
	public AnimationCurve timeDilationDurationVsChargeTime;
	public AnimationCurve timeDilationScaleVsChargeTime;

	[Header("Wall Hug and Grounding Settings")]
	public LayerMask wallHugCheck;
    public LayerMask itemPlaceCheck;
	public float maxWallSlope;
	public LayerMask groundLayers;
	public Transform groundCheckLocation;
	public float groundCheckRadius = 0.1f;

	[Header("Portal Settings")]
	public float teleportCooldown = 0.1f;

	private float threatenTimeRemaining = 0f;
	private HashSet<ThreatenedArea> threatenedAreas = new HashSet<ThreatenedArea>();

	public event Action<Player> Died;

	public Player Player 			{ get; private set; }
	public Vector2 AimDirection		{ get; private set; }
	public Item Item 				{ get; set; } // TODO: Create a method for giving the item (raises OnPickedUpItem())
	public PotatoBomb PotatoBomb	{ get; set; }
	public MissileBeacon Beacon      { get; set; }
	public float Height				{ get; set; } // TODO: Make variable not property?
	public Player ThreateningPlayer { get; set;}

	public float TimeGroundJumping	{ get; private set; }
	public float TimeAirJumping		{ get; private set; }
	public float TimePunchCharging 	{ get; private set; }
	public float TimePunching	 	{ get; private set; }

	public bool CanPunchButton		{ get { return TimePunching < buttonPunchWindow; } }
	public bool CanBePunched 		{ get { return !IsSpawnProtected; } } // TODO: Team stuff
	public bool CanAct 				{ get { return !IsPunching && !IsStunned && !IsExploded && !IsStomped && !IsFrozen && !IsDead && Game.RoundIsPlaying && !Game.paused; } }
	public bool CanMove 			{ get { return CanAct; } }
	public bool CanGroundJump 		{ get { return CanAct && IsGrounded && !IsGroundJumping && !(IsOnCloudPlatform && IsFacingDown); } }
	public bool CanAirJump			{ get { return CanAct && !IsGrounded && !IsAirJumping && !HasAirJumped && !HasAirPunched; } }
	public bool CanPlaceItem 		{ get { return CanAct && HasItem && IsGrounded; } }
	public bool CanThrowItem 		{ get { return CanAct && HasItem && !IsThrowingItem; } }
	public bool CanChargePunch      { get { return CanAct && !IsChargingPunch && !IsPunching && !IsPunchCoolingDown && (IsGrounded || !HasAirPunched); } }
	public bool CanTeleport			{ get; private set; }

	public bool IsSpawnProtected 	{ get; private set; }
	public bool IsGrounded	 		{ get; private set; }
	public bool IsGroundJumping		{ get; private set; }
	public bool IsAirJumping 		{ get; private set; }
	public bool IsFalling 			{ get { return !IsGrounded && rigidbody2D.velocity.y < 0f; } }
	public bool IsThrowingItem 		{ get; private set; }
	public bool IsChargingPunch 	{ get; private set; }
	public bool IsPunching	 		{ get; private set; }
	public bool IsPunchCoolingDown	{ get; private set; }
	public bool IsStunned 			{ get; private set; }
	public bool IsStomped 			{ get; private set; }
	public bool IsFrozen 			{ get; private set; }
	public bool IsExploded			{ get; private set; }
	public bool IsDead 				{ get; private set; }
	public bool IsSlippery			{ get; private set; }
	public bool IsFacingDown		{ get { return AimDirection.y < -0.71f; } } // FIXME: This is for going through cloud platforms, need a better option
	public bool IsOnCloudPlatform	{ get; private set; }
	public bool IsTaggedByPotatoBomb{ get { return PotatoBomb != null; } }
	public bool IsTaggedByBeacon    { get; private set; }
	public bool IsInThreatenedArea	{ get { return threatenedAreas.Count > 0; } }
	public bool IsFacingRight
	{
		get
		{
			return transform.localScale.x > 0f;
		}
		set
		{
			if(value != IsFacingRight)
				transform.localScale = Vector3.Scale(transform.localScale, -Vector3.right + Vector3.up + Vector3.forward);
		}
	}

	public bool HasItem 			{ get { return Item != null; } }
	public bool HasAirJumped		{ get; private set; }
	public bool HasAirPunched		{ get; private set; }
	public bool HasFreezeBomb		{ get; private set; }

	void Start()
	{
		CloudPlatform.checkList.Add (this);
		IsOnCloudPlatform = false;
		IsGroundJumping = false;
		IsStomped = false;
		IsStunned = false;
		IsFrozen = false;
		IsDead = false;
		HasAirPunched = false;
		IsPunchCoolingDown = false;
		IsSpawnProtected = true;
		CanTeleport = true;
		Invoke("RemoveSpawnProtection", spawnProtectionDuration);
		Call ("OnSpawn");
	}

	void OnDestroy()
	{
		CloudPlatform.checkList.Remove (this);
	}

	void RemoveSpawnProtection()
	{
		IsSpawnProtected = false;
		Call ("OnSpawnProtectionEnd");
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(groundCheckLocation.position, groundCheckRadius);
	}

	void FixedUpdate()
	{
		// Grounding
		int oldLayer = gameObject.layer;
		gameObject.SetLayer(LayerMask.NameToLayer("Ignore Raycast"));
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckLocation.position, groundCheckRadius, groundLayers);
		gameObject.SetLayer(oldLayer);
		if(colliders.Length == 0)
		{
			SetGrounded(false);
			IsOnCloudPlatform = false;
		}
		else
		{
			// If we're standing on a bunch of stuff, find the ones we don't ignore collision with
			bool grounded = false;
			IsOnCloudPlatform = false;
			foreach(Collider2D collider in colliders)
			{
				if(collider.GetComponent<CloudPlatform>())
				{
					IsOnCloudPlatform |= true;
				}
				if(!Physics2D.GetIgnoreCollision(collider, collider2D))
				{
					grounded = true;
					break;
				}
			}
			SetGrounded(grounded);
		}

		// Threat
		if(ThreateningPlayer != null && !IsInThreatenedArea && CanAct && Player.input.GetAxis("Horizontal") != 0.0f)
		{
			threatenTimeRemaining -= Time.deltaTime;
			if(IsGrounded && threatenTimeRemaining <= 0f) // Unthreaten
			{
				GetUnthreatened();
			}
		}

		// Aim direction
		float aimX = Player.input.GetAxis("Horizontal");
		float aimY = Player.input.GetAxis("Vertical");
		if(aimX == 0.0f && aimY == 0.0f)
			aimX = (IsFacingRight ? 1 : -1);
		AimDirection = new Vector2(aimX, aimY).normalized;
		
		// Movement input
		if (CanMove)
		{
			float input = Player.input.GetAxis ("Horizontal");
			if(!CanWalkForward ())
				input = 0f;
			Call ("OnMovementInput", input);
			if(input != 0f)
				IsFacingRight = input > 0f;
		}

		// Jump input
		if (CanGroundJump && (Player.input.GetButtonDown("Jump"))) 
		{
			TimeGroundJumping = 0f;
			IsGroundJumping = true;
			IsAirJumping = false;
			HasAirJumped = false;
			Call ("OnJumpStart");
		}
		if(IsGroundJumping)
		{
			if(TimeGroundJumping > maxJumpTime || (TimeGroundJumping > minJumpTime && !Player.input.GetButton("Jump")))
			{
				Call ("OnJumpEnd");
				IsGroundJumping = false;
				TimeGroundJumping = 0f;
			}
			else
			{
				TimeGroundJumping += Time.deltaTime;
			}
		}

		// Air jump
		if(CanAirJump && Player.input.GetButtonDown("Jump"))
		{
			TimeAirJumping = 0f;
			IsGroundJumping = false;
			IsAirJumping = true;
			HasAirJumped = true;
			Call ("OnAirJumpStart");
		}
		if(IsAirJumping)
		{
			if(TimeAirJumping > maxAirJumpTime || (TimeAirJumping > minAirJumpTime && !Player.input.GetButton("Jump")))
			{
				Call ("OnAirJumpEnd");
				IsAirJumping = false;
				TimeAirJumping = 0f;
			}
			else
			{
				TimeAirJumping += Time.deltaTime;
			}
		}

		// Punching
		if(CanChargePunch && Player.input.GetButtonDown ("Punch"))
		{
			TimePunchCharging = 0f;
			IsChargingPunch = true;
			Call("OnPunchChargeStart");
		}
		if(IsChargingPunch)
		{
			if(TimePunchCharging > maxPunchChargeTime)
			{
				Call("OnPunchOverharge");
				Call("OnPunchStart");
				HasAirPunched = !IsGrounded;
				TimePunching = 0f;
				IsChargingPunch = false;
				IsPunching = true;
			}
			else if(Player.input.GetButtonUp ("Punch"))
			{
				Call("OnPunchStart");
				HasAirPunched = !IsGrounded;
				TimePunching = 0f;
				IsChargingPunch = false;
				IsPunching = true;
			}
			else
			{
				TimePunchCharging += Time.deltaTime;
			}
		}
		if(IsPunching)
		{
			if(TimePunching > punchDuration)
			{
				Call ("OnPunchEnd");
				IsPunchCoolingDown = true;
				Invoke("PunchCooldown", punchCooldown);
				IsPunching = false;
				TimePunching = 0f;
			}
			else
			{
				TimePunching += Time.deltaTime;
			}
		}

		// Items
		if(CanThrowItem && Player.input.GetButtonDown ("Throw"))
		{
			IsThrowingItem = true;

			Call ("OnItemThrowStart");
		}
		if(IsThrowingItem)
		{
			if(Player.input.GetButtonUp ("Throw"))
			{
				Call("OnItemThrowEnd");
				IsThrowingItem = false;
				Item = null;
			}
		}

		// Place item
		if(CanPlaceItem && Player.input.GetButtonDown ("Place"))
		{
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, placeDistance, itemPlaceCheck);
			if (hit)
			{
				Call ("OnItemPlace", (Vector3) hit.point + Vector3.forward * transform.position.z);
                StartCoroutine(ParentPlacedItem(Item.transform, hit.transform));
				Item = null;
                
			}
		}

		// Pause
		if (Player.input.GetButtonDown("Pause") && GameGUI.instance.playerPaused == null && GameGUI.instance.currentButton == null)
		{
			Debug.Log ("Pausing!");
			GameGUI.instance.Pause(Player);
		}
		//if (Player.input.GetButtonDown("Pause"))
		//	Debug.Log(GameGUI.instance.currentButton == null);
	}

    // Parent the item to the parent in a frame so it has time to rescale and stuff
    IEnumerator ParentPlacedItem(Transform item, Transform parent)
    {
        yield return new WaitForEndOfFrame();
        item.parent = parent;
    }

	#region Threat
	public void GetThreatened(Player player)
	{
		ThreateningPlayer = player;
		threatenTimeRemaining = minimumThreatenTime;
		Call ("OnGetThreatened", player);
	}

	void GetUnthreatened ()
	{
		ThreateningPlayer = null;
		Call ("OnGetUnthreatened");
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		ThreatenedArea t = collider.GetComponent<ThreatenedArea> ();
		if(t)
		{
			threatenedAreas.Add(t);
		}
	}
	
	void OnTriggerExit2D(Collider2D collider)
	{
		ThreatenedArea t = collider.GetComponent<ThreatenedArea> ();
		if(t)
		{
			threatenedAreas.Remove(t);
		}
	}
	#endregion

	#region ICloudPlatformPassble
	public Vector2 GetCloudPlatformDetectionPoint()
	{
		return collider2D.bounds.center - (Vector3.up * collider2D.bounds.size.y / 2f);
	}

	public bool IgnoreCloudPlatform()
	{
		return ((IsFacingDown && Player.input.GetButton ("Jump")) || IsGroundJumping || IsAirJumping) && !IsFrozen;
	}

	public Collider2D[] GetCloudPlatformColliders()
	{
		return GetComponentsInChildren<Collider2D>();
	}
	#endregion

	void SetGrounded(bool grounded)
	{
		bool wasGrounded = IsGrounded;
		IsGrounded = grounded;
		if(wasGrounded && !IsGrounded)
			Call ("OnUngrounded");
		if(!wasGrounded && IsGrounded)
			Call ("OnGrounded");
	}

	void OnGrounded()
	{
		HasAirPunched = false;
		HasAirJumped = false;
	}

	//shoots a raycast or boxcast or whatever in front of the player 
	// and returns true if there's nothing in the immediate forward of the player
	bool CanWalkForward()
	{
		int oldLayer = gameObject.layer;
		gameObject.SetLayer(LayerMask.NameToLayer("Ignore Raycast"));
		Vector2 dir = Vector2.right * Player.input.GetAxis("Horizontal");
		RaycastHit2D hit = Physics2D.CircleCast (transform.position, .25f, dir, dir.magnitude, wallHugCheck);

		gameObject.SetLayer(oldLayer);
		if (hit)
		{
			float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);
			return slopeAngle < maxWallSlope;
		}
		return true;
	}

	float GetCollisionAngle(Collision2D collision)
	{
		Vector2 normal = collision.contacts [0].normal;
		if((collision.contacts [0].point.y > transform.position.y && normal.y > 0f) ||
		   (collision.contacts [0].point.y < transform.position.y && normal.y < 0f))
		{
			normal *= -1f;
		}
		return Vector2.Angle (normal, Vector2.up);
	}

	public void SetPlayer(Player player)
	{
		Player = player;
	}

	public void Die(DeathType type)
	{
		if (IsDead)
			return;
		IsDead = true;
		foreach(Collider c in GetComponentsInChildren<Collider>())
		{
			c.enabled = false;
		}
		// TODO: Remove other components and shut down player for death
		rigidbody2D.isKinematic = true;
		if(IsFrozen)
			Unfreeze ();
		Call ("OnDie", type);
		if(Died != null)
			Died(Player);
	}

	public void PunchStoppedSuddenly()
	{
		Call ("OnPunchEndSuddenly");
	}

	public void PunchButton(PunchTrigger trigger)
	{
		Call ("OnPunchButton", trigger);
	}

	public void PunchedSomeone(Character punchee)
	{
		Call ("OnPunchSomeone", punchee);
		if (IsTaggedByBeacon)
		{
			IsTaggedByBeacon = false;
		}
//		StartCoroutine (DelayedDilateTime (punchee));
	}

//	IEnumerator DelayedDilateTime(Character punchee)
//	{
//		yield return new WaitForSeconds (punchEffectDelayVsChargeTime.Evaluate(TimePunchCharging));
//		Call ("OnPunchSomeone", punchee);
//		if (IsTaggedByBeacon)
//		{
//			IsTaggedByBeacon = false;
//		}
//		PunchTimeDilation.DilateTime (timeDilationScaleVsChargeTime.Evaluate(TimePunchCharging), timeDilationDurationVsChargeTime.Evaluate(TimePunchCharging));
//	}

	void PunchCooldown()
	{
		IsPunchCoolingDown = false;
		Call ("OnPunchCooldownEnd");
	}

	public void Stomp(Character stomper)
	{
		if(!IsStomped && Game.CanBeAttacked(Player, stomper.Player))
		{
			GetThreatened(stomper.Player);
			IsStomped = true;
			CancelInvoke("Unstomp");
			Invoke("Unstomp", stompStunDuration);
			Call ("OnGotStomped", stomper);
			Call ("OnStomp", stompStunDuration);
		}
	}

	public void Unstomp()
	{
		IsStomped = false;
		Call ("OnUnstomp");
	}

	public void StompedSomeone(Character other)
	{
		Call ("OnStompSomeone", other);
	}

	public void BecomeExploded(Vector2 force, float stunTime)
	{
		IsExploded = true;
		CancelInvoke("ExplodeEnd");
		Invoke ("ExplodeEnd",stunTime);
		Call ("OnExplode", force);
	}

	public void ExplodeEnd()
	{
		IsExploded = false;
		Call("OnExplodeEnd");
	}

	public void Freeze(float duration)
	{
		IsFrozen = true;
		if(IsPunching)
			PunchEndSuddenly ();
		if (IsChargingPunch)
			PunchChargeInterrupted ();
		CancelInvoke("Unfreeze");
		Invoke ("Unfreeze", duration);
		Call ("OnFreeze", duration);
	}

	public void Unfreeze()
	{
		IsFrozen = false;
		Call ("OnUnfreeze");
	}

	public void Stun(float stunTime)
	{
		IsStunned = true;
		CancelInvoke("Unstun");
		Invoke ("Unstun", stunTime);
		Call("OnStun",stunTime);
	}

	public void Unstun()
	{
		IsStunned = false;
		Call ("OnUnstun");
	}

	public void Pause()
	{
//		IsFrozen = true; // FIXME: Why? Trevor...
	}

	public void Unpause()
	{
//		IsFrozen = false;
	}

	public void GotPunched(Character puncher)
	{
		if(Game.CanBeAttacked(Player, puncher.Player))
		{
			GetThreatened(puncher.Player);
			Stun (puncher.GetPunchStunDuration());
			if (puncher.IsTaggedByPotatoBomb)
				HitByItem(puncher.PotatoBomb);
			if (puncher.IsTaggedByBeacon)
				GotTaggedByMissileBeacon();
			Call ("OnGetPunched", puncher);
		}
	}

    public void HitByFlubBall(Vector2 force)
    {
        Call("OnHitByFlubBall", force);
        Stun(0.5f);
    }

	public void BecomeSlippery()
	{
		IsSlippery = true;
		Call ("OnBecomeSlippery");
	}

	public void BecomeNotSlippery()
	{
		IsSlippery = false;
		Call ("OnBecomeNotSlippery");
	}

	public void HitByItem(Item item)
	{
		Call ("OnHitByItem", item);
		if (item is MissileBeacon)
			Beacon = item as MissileBeacon;
	}

	public void GotTaggedByMissileBeacon()
	{
		IsTaggedByBeacon = true;
	}

	public float GetPunchStunDuration()
	{
		return punchStunDurationVsChargeTime.Evaluate(TimePunchCharging);
	}

	public Vector2 GetPunchForce()
	{
		// TODO: Do we do this or like GetPunchStunDuration, where the variables are kept in Character and just referenced in other classes?
		CharacterPunch punch = GetComponent<CharacterPunch> ();
		if(punch)
			return AimDirection * punch.PunchForce;
		return AimDirection * 0f;
	}

	public void PunchChargeInterrupted()
	{
		TimePunchCharging = 0f;
		IsChargingPunch = false;
		Call ("OnPunchChargeInterrupted");
	}

	public void PunchEndSuddenly()
	{
		TimePunching = 0f;
		IsPunching = false;
		Call ("OnPunchEndSuddenly");
	}

	void OnDeathAnimationEnded()
	{
		Destroy (gameObject);
	}

	void Teleport()
	{
		// Change aim dir
		AimDirection = rigidbody2D.velocity.normalized;

		// Flip left/right
		float dot = Vector3.Dot(((Vector3)rigidbody2D.velocity).normalized, (Vector3.right * (IsFacingRight ? 1f : -1f)).normalized);
		if (dot < 0f)
			IsFacingRight = !IsFacingRight;

		CanTeleport = false;
		Call ("OnTeleport");
		Invoke ("EndTeleportCooldown", teleportCooldown);
	}

	void EndTeleportCooldown()
	{
		CanTeleport = true;
	}
	
	void Call(string message)
	{
		BroadcastMessage(message, SendMessageOptions.DontRequireReceiver);
	}
	
	void Call(string message, object args)
	{
		BroadcastMessage(message, args, SendMessageOptions.DontRequireReceiver);
	}

	public void RoundStart()
	{
		Call ("OnRoundStart");
	}

	public void RoundEnd()
	{
		Call ("OnRoundEnd");
	}
}
