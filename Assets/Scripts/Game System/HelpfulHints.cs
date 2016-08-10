using UnityEngine;
using System.Collections;

public class HelpfulHints : MonoBehaviour {

//    public LoadingScreenReferences hints;
    private TextMesh tm;
    private string text;

    private void Start()
    {
        tm = GetComponent<TextMesh>();
        text = Assets.HelpfulHints[Random.Range(0, Assets.HelpfulHints.Length)].message;
        tm.text = text;
    }
}
