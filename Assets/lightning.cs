using UnityEngine;
using System.Collections;

public class lightning : MonoBehaviour {
	public Transform startPoint;
	public Transform endPoint;
	LineRenderer lightningLine;
	// Use this for initialization
	void Start () {
		lightningLine = GetComponent<LineRenderer> ();
		lightningLine.SetWidth (10f, 10f);
	
	}
	// Update is called once per frame
	void Update () {
		lightningLine.SetPosition (0, startPoint.position);
		lightningLine.SetPosition (1, endPoint.position);
	
	}
}
