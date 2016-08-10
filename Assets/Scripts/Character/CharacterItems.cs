using UnityEngine;
using System.Collections;

public class CharacterItems : MonoBehaviour
{	
	[System.Serializable]
	public class ThrowData
	{
		public Character character;
		public float throwRange, throwTime, throwHeight;
		public float characterHeight;
		[HideInInspector]public bool IsFacingRight;
		[HideInInspector]public bool IsFacingDown;
	}

	private Character character;


	public float ignoreCollisionDuration = 2.0f;
	public float characterHeight = 2.0f;

	[Header("Throw Settings")]
	public ThrowData upwardsThrow;
	public ThrowData downwardsThrow; 
	public ThrowData diagonalThrow;
	public ThrowData downwardsDiagonalThrow;
	public ThrowData forwardThrow;

	void Awake()
	{
		character = GetComponent<Character> ();
	}

	IEnumerator UnignoreOwner(Item item)
	{
		yield return new WaitForSeconds (ignoreCollisionDuration);
		if(item)
			SetCollision (item, true);
	}

	void SetCollision(Item item, bool collide)
	{
		foreach(Collider2D collider in GetComponentsInChildren<Collider2D>())
		{
			Physics2D.IgnoreCollision (collider, item.transform.collider2D, !collide);
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
		if(character.IsTaggedByPotatoBomb)
		{
			character.PotatoBomb.AttachTo(punchee);
		}
		if (character.IsTaggedByBeacon)
			character.Beacon.AttachTo (punchee);
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
		ThrowData throwData;
		if ( character.AimDirection.y == 1)
		{
			throwData = upwardsThrow;
		}
		else if ( character.AimDirection.y == -1 )
		{
			throwData = downwardsThrow;
		}
		else if ( Mathf.Abs (character.AimDirection.x) == 1 )
		{
			throwData = forwardThrow;
		}
		else if ( character.AimDirection.x > 0f && character.AimDirection.x != 1f 
		         && character.AimDirection.y > 0f && character.AimDirection.y != 1f)
		{
			throwData = downwardsDiagonalThrow;
		}
		else
		{
			throwData = diagonalThrow;
		}
		throwData.IsFacingRight = character.IsFacingRight;
		throwData.IsFacingDown = character.IsFacingDown;
		throwData.characterHeight = characterHeight;

		SetCollision (character.Item, false);
		character.Item.SetOwner (character);
		character.Item.Throw (throwData);
		StartCoroutine(UnignoreOwner(character.Item));
	}
	
	void OnItemThrowInterrupted()
	{
	}
	
	void OnItemPlace(Vector3 placePosition)
	{
		SetCollision (character.Item, false);
		character.Item.SetOwner (character);
		character.Item.Place (placePosition);
		StartCoroutine(UnignoreOwner(character.Item));
	}

	void OnHitByItem(Item item)
	{
		if(item is PotatoBomb)
			character.PotatoBomb = item as PotatoBomb;
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

