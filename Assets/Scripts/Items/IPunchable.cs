using UnityEngine;
using System.Collections;

public interface IPunchable 
{
    void GetPunched(Character puncher, Vector2 punchForce);
}
