using UnityEngine;
using System.Collections;

public class Hazard : MonoBehaviour
{
	public DeathType type;
	public AudioClip itemDestroyClip;
	private AudioSource audioSource;

	private void Start()
	{
		audioSource = GetComponent<AudioSource> ();
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		Kill (other.gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Kill (other.gameObject);
	}

	void Kill(GameObject gameObject)
	{
		gameObject.SendMessage("Die", type, SendMessageOptions.DontRequireReceiver);
		Item item = gameObject.GetComponent<Item> ();
		if (item)
		{
			if (itemDestroyClip == null)
			{
				if ( type == DeathType.Generic)
					itemDestroyClip = Assets.HazardSounds.generic;
				else if ( type == DeathType.Burn)
					itemDestroyClip = Assets.HazardSounds.burn;
				else if ( type == DeathType.Crush)
					itemDestroyClip = Assets.HazardSounds.crush;
				else if ( type == DeathType.Drown)
					itemDestroyClip = Assets.HazardSounds.drown;
				else if ( type == DeathType.Electrocute)
					itemDestroyClip = Assets.HazardSounds.electrocute;
				else if ( type == DeathType.Fall)
					itemDestroyClip = Assets.HazardSounds.fall;
				else if ( type == DeathType.Freeze)
					itemDestroyClip = Assets.HazardSounds.freeze;
				else if ( type == DeathType.Lazer)
					itemDestroyClip = Assets.HazardSounds.lazer;
			}
			if(audioSource)
				audioSource.PlayOneShot(itemDestroyClip);
			Destroy(item.gameObject);
		}
	}
}
