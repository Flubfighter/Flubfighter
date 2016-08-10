using UnityEngine;
using System.Collections;
using System.Linq;

public class ItemToggle : SelectableButton
{
	public GameObject effectObject;
	public Color disabledColor;
	public string itemName;

	[HideInInspector] public bool itemEnabled = true;

	private Color startColorMaterial;
	private Color startColorText;
	private TextMesh textMesh;
	private Renderer itemRenderer;
	private Item item;

	void Start()
	{
		textMesh = GetComponent<TextMesh>();
		startColorText = textMesh.color;
		if(effectObject != null)
		{
			MeshRenderer mr = effectObject.GetComponentInChildren<MeshRenderer>();
			SkinnedMeshRenderer smr = effectObject.GetComponentInChildren<SkinnedMeshRenderer>();
			if(mr != null)
				itemRenderer = mr;
			else if(smr != null)
				itemRenderer = smr;
			else
				Debug.LogWarning("cant find an appropriate renderer for " + name);

			startColorMaterial = itemRenderer.material.color;
		}
		else
			Debug.LogWarning(name + " does not have an effect object!");
		try
		{
			item = Assets.Items.Single(x => x.name == itemName);
//			Debug.Log("Added: " + item.name);
		}
		catch
		{
			item = null;
			Debug.LogError(name + " does not have a valid itemName or Assets is missing it.");
		}

		if(item != null)
		{
			Debug.Log("CustomItem added: " + item.name);
			MainMenu.Instance.customItems.Add(item);
		}

		UpdateDisplay();
	}
	
	public override void Activate()
	{
//		Debug.Log("activate from ItemToggle");
		Toggle ();
		FindObjectOfType<MainMenu>().currentMenu = areaToChangeTo;
	}
	
	void Toggle()
	{
		Debug.Log("Toggle");
		if(MainMenu.Instance.customItems.Contains(item))
		{
			MainMenu.Instance.customItems.Remove(item);
			Debug.Log("removing: " + item.name);
		}
		else
			MainMenu.Instance.customItems.Add(item);
		itemEnabled = !itemEnabled;
		
		UpdateDisplay();
	}
	
	void UpdateDisplay()
	{
		if(effectObject != null)
		{
			textMesh.color = itemEnabled ? startColorText : disabledColor;
			itemRenderer.material.color = itemEnabled ? startColorMaterial : disabledColor;
			Animator itemAnimator = effectObject.GetComponent<Animator>();
			Animation itemAnimation = effectObject.animation;
			if(itemAnimator != null)
			{
				if(!itemEnabled)
					// I know wierd right?
					itemAnimator.StartPlayback();
				else
					itemAnimator.StopPlayback();
			}
			else if(itemAnimation != null)
			{
				itemAnimation[itemAnimation.clip.name].speed = itemEnabled ? 1f : 0f;
			}
			ParticleSystem particles = effectObject.GetComponentInChildren<ParticleSystem>();
			if(particles)
				particles.enableEmission = itemEnabled;
		}
	}
	
	void OnMouseDown()
	{
		FindObjectOfType<Selector>().SendActivate();
	}
}
