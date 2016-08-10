using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour, ICloudPlatformPassable
{
	public enum State
	{
		WaitingToBePickedUp,		// To Held
		Held,						// To Thrown/Placed
		Thrown,						// To Placed/Used
		Placed,						// To Used
		Used
	}

	public bool threatenWhenUsed = true;
	public ParticleSystem thrownParticleSystem;
	protected Character itemOwner;
    protected Vector3 initialScale;
	public State CurrentState { get; protected set; }

	protected virtual void Awake()
	{
		CloudPlatform.checkList.Add (this);
		CurrentState = State.WaitingToBePickedUp;
		collider2D.enabled = true;
		collider2D.isTrigger = true;
        initialScale = transform.localScale;
	}

    void LateUpdate()
    {
        Vector3 newScale = initialScale;
        Transform parent = transform.parent;
        while (parent != null)
        {
            newScale.x /= parent.localScale.x;
            newScale.y /= parent.localScale.y;
            newScale.z /= parent.localScale.z;
            parent = parent.parent;
        }
        transform.localScale = newScale;
    }

	void OnDestroy()
	{
		CloudPlatform.checkList.Remove (this);
	}

	public void GetPickedUp(Character character)
	{
		CurrentState = State.Held;
		collider2D.enabled = false;

		character.Item = this;
		transform.parent = character.itemHoldArm.transform;
		transform.localPosition = Vector3.zero;
		OnBecamePickedUp (character);
	}

	public virtual void OnBecamePickedUp(Character character) {	}

	public void Throw(CharacterItems.ThrowData throwData)
	{
		CurrentState = State.Thrown;
		collider2D.enabled = true;
		collider2D.isTrigger = false;

//		Debug.Log (thrownParticleSystem);
		if(thrownParticleSystem)
		{
			Debug.Log ("Playing");
			thrownParticleSystem.gameObject.SetActive(false);
			thrownParticleSystem.gameObject.SetActive(true);
			thrownParticleSystem.Play ();
		}

		transform.parent = null;
        transform.localScale = initialScale;
		gameObject.AddComponent<Rigidbody2D> ();
		OnBecameThrown (throwData);
	}

	public Character GetItemOwner()
	{
		return itemOwner;
	}

	public void SetOwner(Character owner)
	{
		itemOwner = owner;
	}

	public virtual void OnBecameThrown(CharacterItems.ThrowData throwData)
	{
		float characterHeight = throwData.characterHeight;

		Vector2 itemVelocity = Vector2.zero;
		itemVelocity.y = Mathf.Sqrt (2.0f*(characterHeight-throwData.throwHeight)*Physics2D.gravity.y);
		itemVelocity.x = throwData.throwRange / throwData.throwTime;
		if (!throwData.IsFacingRight)
		{
			itemVelocity.x *= -1;
		}
		if (throwData.IsFacingDown)
		{
			itemVelocity.y *= -1;
		}
		rigidbody2D.velocity = itemVelocity;
	}

	public void Place(Vector3 placePosition)
	{
        CurrentState = State.Placed;
		collider2D.enabled = true;
		collider2D.isTrigger = true;
		if (rigidbody2D)
			Destroy (rigidbody2D);
		transform.parent = null;
		transform.position = placePosition;
        transform.rotation = Quaternion.identity;
        transform.localScale = initialScale;
		OnBecamePlaced (placePosition);
	}

	public virtual void OnBecamePlaced(Vector3 placePosition) 
	{
		Animation anim = GetComponentInChildren<Animation> ();
//		ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem> ();
		if (anim)
            anim.Stop ();
//		particleSystem.Stop ();
	}	
	
	public void BecomeUsed(Character target)
	{
		CurrentState = State.Used;
		collider2D.enabled = false;
		if (rigidbody2D)
			Destroy (rigidbody2D);
		if (threatenWhenUsed)
		{
            //Debug.Log (itemOwner);
			target.GetThreatened(itemOwner.Player);
		}
		OnBecameUsed (target);
	}

	public virtual void OnBecameUsed(Character target) {	}

	public virtual void GotPunched(Character puncher)
	{
		if(CurrentState == State.WaitingToBePickedUp && !puncher.HasItem )
		{
			puncher.PunchStoppedSuddenly ();
			GetPickedUp (puncher);
		}
	}

	protected virtual void OnTriggerEnter2D(Collider2D collider)
	{
		if(CurrentState == State.Placed)
		{
			Character character = collider.GetComponent<Character>();
			if(character)
			{
				BecomeUsed(character);
			}
		}
	}

	protected virtual void OnCollisionEnter2D(Collision2D collision)
	{
		if(CurrentState == State.Thrown)
		{
			Character character = collision.gameObject.GetComponent<Character>();
			if(thrownParticleSystem)
				thrownParticleSystem.Stop ();
			if(character)
			{
				BecomeUsed(character);
			}
			else
			{
				Place (transform.position);
			}
		}
	}

	public virtual void OnDie() // Raised from Character when attached
	{
		Destroy (this);
	}
	
	#region ICloudPlatformPassble
	public Vector2 GetCloudPlatformDetectionPoint()
	{
		return collider2D.bounds.center - (Vector3.up * collider2D.bounds.size.y / 2f);
	}
	
	public bool IgnoreCloudPlatform()
	{
		return false;
	}
	
	public Collider2D[] GetCloudPlatformColliders()
	{
		return GetComponentsInChildren<Collider2D>();
	}
	#endregion
}
