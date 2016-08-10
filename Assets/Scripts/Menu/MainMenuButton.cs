using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuButton : SelectableButton
{
	public Actions action = Actions.animate;
	public string animation = "";
	public Transform interperlerpTarget;
	public List<GameObject> extraAnimationObjects = new List<GameObject>();
	public bool backButton;

	[Tooltip("If the action is a player modifier, which player is modified?")]
	public int playerIndex = 1;

	private static Animation cameraAnimation;

	public enum Actions
	{
		animate,
		interpolate,
		startMatch,
		exit
	}

	void Awake()
	{
		if(cameraAnimation == null)
			cameraAnimation = Camera.main.transform.parent.gameObject.animation;
	}

	public override void Activate()
	{
//		Debug.Log("activate from MainMenuButton");
		FindObjectOfType<MainMenu>().currentMenu = areaToChangeTo;
		if(action == Actions.animate && animation != "")
		{
			Animate();
		}
		else if(action == Actions.startMatch)
		{
			FindObjectOfType<MainMenu>().BeginGame();
		}
		else if(action == Actions.exit)
			Application.Quit();
	}

	void Animate()
	{
		if(backButton)
		{
			foreach(GameObject o in extraAnimationObjects)
			{
				foreach(AnimationState s in o.animation)
				{
					s.time = s.length;
					s.speed = -1f;
					o.animation.Play(s.name);
				}
			}
			cameraAnimation[animation].time = cameraAnimation[animation].length;
			cameraAnimation[animation].speed = -1f;
		}
		else
		{
			foreach(GameObject o in extraAnimationObjects)
			{
				foreach(AnimationState s in o.animation)
				{
					s.speed = 1f;
					o.animation.Play(s.name);
				}
			}
			cameraAnimation[animation].speed = 1f;
		}
		cameraAnimation.Play(animation);
	}

	void OnMouseDown()
	{
		if(action == Actions.interpolate)
			Camera.main.GetComponent<Interperlerp>().target = interperlerpTarget;
		else
			FindObjectOfType<Selector>().SendActivate();
	}

}
