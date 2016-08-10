using UnityEngine;
using System.Collections;

public class DestructablePlatform : MonoBehaviour {

    public enum BreakMode
    {
        WaitThenBreak,
        AfterContinuousContact
    }

    public bool respawns = true;
    public float respawnDelay;
    public float checkPadding = 0.1f;
    public BreakMode breakMode;
    public float health;

    private bool isBroken;
    private float tempHealth;

    private void Start()
    {
        tempHealth = health;
    }

    private void Update()
    {
        if (health <= 0.0f)
        {
            Break();
        }
    }

    private Vector2 BottomLeft
    {
        get
        {
            return collider2D.bounds.min - Vector3.one * checkPadding;
        }
    }

    private Vector2 TopRight
    {
        get
        {
            return collider2D.bounds.max + Vector3.one * checkPadding;
        }
    }

    void Break()
    {
        if (!isBroken)
        {
            isBroken = true;
            if (respawns)
                StartCoroutine("Repair");
            Broken();
        }
    }

    IEnumerator Repair()
    {
        yield return new WaitForSeconds(respawnDelay);
        while (!RepairAreaIsClear())
            yield return null;
        isBroken = false;
        Repaired();
    }

    bool RepairAreaIsClear()
    {
        Collider2D[] collidersInArea = Physics2D.OverlapAreaAll(BottomLeft, TopRight);
        foreach (Collider2D otherCollider in collidersInArea)
        {
            if (otherCollider.GetComponent<Character>())
                return false;
        }
        return true;
    }


    void Broken()
    {
        renderer.enabled = false;
        collider2D.enabled = false;
    }

    void Repaired()
    {
        renderer.enabled = true;
        collider2D.enabled = true;
        health = tempHealth;
    }
}