using UnityEngine;
using System.Collections;
using System.Linq;

public class ResolutionChanger : SelectableButton
{
	private Resolution nextRes;

	void Start()
	{
		UpdateDisplay();
		nextRes = Screen.currentResolution;
		Debug.Log("Res: " + Screen.resolutions.Length.ToString());
	}

	public override void Activate ()
	{
		Next();
	}

	public void ChangeDisplay()
	{
		if(nextRes.height != Screen.currentResolution.height)
			Screen.SetResolution(nextRes.width, nextRes.height, Screen.fullScreen);
	}

	void Next()
	{
		int currentIndex = Screen.resolutions.ToList().FindIndex(x => x.height == Screen.currentResolution.height);
		nextRes = Screen.resolutions[(int)Mathf.Repeat(currentIndex, Screen.resolutions.Length) + 1];
		UpdateDisplay();
	}

	void Previous()
	{
		int currentIndex = Screen.resolutions.ToList().FindIndex(x => x.height == Screen.currentResolution.height);
		nextRes = Screen.resolutions[(int)Mathf.Repeat(currentIndex, Screen.resolutions.Length) - 1];
		UpdateDisplay();
	}

	void UpdateDisplay()
	{
		GetComponent<TextMesh>().text = "Resolution: " + Screen.currentResolution.width + "x" + Screen.currentResolution.height + "\nNote: Press A to apply";
	}
}
