using UnityEngine;
using System.Collections;

public class FlubBall : Item {


	public float lifespan = 10f;
    [Header("Multiplier Settings")]
	[Tooltip("Multiplier for when a player punches the ball")]
    public float ballPunchMultiplier = 1f;
    [Tooltip("How hard the ball applies it's velocity when it hits stuff")]
    public float ballHitMultiplier = 2f;
    [Tooltip("How long the ball lasts in the game")]
    
	[Header("Velocity Settings")]
	public float initialVelocity = 2;
    public float minVelocityToPuch = 5;
    public float maxVelocity = 30f; // m/s
    public float theta;

    protected override void Awake()
    {
		base.Awake ();
		Destroy(gameObject, lifespan);
		collider2D.isTrigger = false;
    }

    private void Start()
    {
        rigidbody2D.velocity = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)).normalized*initialVelocity;
    }

    public override void GotPunched(Character puncher)
    {
        itemOwner = puncher;
        rigidbody2D.AddForce(puncher.GetPunchForce() * rigidbody2D.mass * ballPunchMultiplier, ForceMode2D.Impulse);
        puncher.PunchStoppedSuddenly();
    }

    void FixedUpdate()
    {
        rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, maxVelocity);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (rigidbody2D.velocity.magnitude < minVelocityToPuch)
            return;
        Character character = collision.gameObject.GetComponent<Character>();
        if (character)
        {
            Vector2 targetDir = transform.position - character.transform.position; 
            Vector2 fwd = rigidbody2D.velocity;
            float angle = Vector2.Angle(targetDir, fwd);
            Debug.Log(angle);
            if (angle > theta)
            {
                //Debug.Log("Behind");

            }
            else
            {
                //Debug.Log("Forward");
                character.HitByFlubBall(-rigidbody2D.velocity * ballHitMultiplier); // TODO: Test that it's calculating the right force
            }
        }
    }
}
