using UnityEngine;
using System.Collections;

public class SelectableButton : MonoBehaviour
{
	public bool mouseable = true;
	public Transform up, down, left, right, activated;
	public MainMenu.MenuArea areaToChangeTo;

	public virtual void Activate() {
		Debug.Log("default activate called");
	}
}
