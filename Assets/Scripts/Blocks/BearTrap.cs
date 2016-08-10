using UnityEngine;
using System.Collections;

public class BearTrap : MonoBehaviour 
{	
	public Animator animator;
	public Hazard killVolume;
    public float reloadDelay;
    public DeathType type;

	// Already exists in Hazard.cs
    private void OnTriggerEnter2D(Collider2D other)
    {
		if (other.gameObject.GetComponent<Character>())
        {
			animator.SetTrigger("SetOffTrap");
//            collider2D.enabled = false;
//            ActivateTrap(other.gameObject);
            Debug.Log("Hit the trap");
        }
    }

	void AnimationEventEnableHazard()
	{
		killVolume.gameObject.SetActive(true);
	}

	void AnimationEventDisableHazard()
	{
		killVolume.gameObject.SetActive(false);
	}

//    private void ActivateTrap(GameObject character)
//    {
//        animator.SetTrigger("SetOffTrap");
////        character.SendMessage("Die", type, SendMessageOptions.DontRequireReceiver);
//        Debug.Log("Killed player");
//        Invoke("ResetTrap", reloadDelay);
//    }

//    private void ResetTrap()
//    {
//		collider2D.enabled = true;
//    }
}
