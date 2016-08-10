using UnityEngine;
using System.Collections;

public class VolumeController : SelectableButton
{
	void Start ()
	{
		UpdateDisplay();
	}

	public override void Activate ()
	{
		Previous();
	}

	public void Next()
	{
		AudioListener.volume += 0.09999f; // Do not use 0.1f #GodDamnItFloatingPointPresision
		AudioListener.volume = Mathf.Clamp(AudioListener.volume, 0, 1);
		UpdateDisplay();
	}

	public void Previous()
	{
		AudioListener.volume -= 0.09999f; // Do not use 0.1f #GodDamnItFloatingPointPresision
		AudioListener.volume = Mathf.Clamp(AudioListener.volume, 0, 1);
		UpdateDisplay();
	}

	void UpdateDisplay()
	{
		GetComponent<TextMesh>().text = "Volume: " + ((int)(AudioListener.volume * 10)).ToString();
	}

	void OnMouseDown()
	{
		Activate();
	}
}
