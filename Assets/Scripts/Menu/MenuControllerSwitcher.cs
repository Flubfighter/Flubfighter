using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]

public class MenuControllerSwitcher : MonoBehaviour
{
    public static MenuControllerSwitcher instance;

    public Sprite[] images;

    private SpriteRenderer spriteRenderer;
    private int index;

    void Start()
    {
        instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateDisplay()
    {
        index++;
        if (index >= images.Length)
            index = 0;
        spriteRenderer.sprite = images[index];
    }
}
