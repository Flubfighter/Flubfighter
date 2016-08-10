using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
	class RattleValue
	{
		public float remaining;
		public float strength;
		public RattleValue(float remaining, float strength)
		{
			this.remaining = remaining;
			this.strength = strength;
		}
	}

	[Header("Movement Settings")]
	[Tooltip("The minimum amount the camera can zoom in percentages of the entire game space. 1 = 100%")]
	public float minZoomPercent = 1f;
	[Tooltip("The closest distance in meters the camera will allow a player to the edge of the screen before zooming out.")]
	public float playerBorderWidth = 3f;
	[Tooltip("The speed the camera moves to it's desired position")]
	public float cameraTranslateSpeed = 1f;
	[Tooltip("The speed the camera zooms out")]
	public float cameraZoomOutSpeed = 1f;
	[Tooltip("The speed the camera zooms in")]
	public float cameraZoomInSpeed = 1f;
	[HideInInspector] public Rect gameArea;
	[Header("Rattle Settings")]
	[Tooltip("The max distance the camera can rattle from it's desired position")]
	public float maxRattleOffset = 1f;
	[Tooltip("The min distance the camera can rattle from it's desired position")]
	public float minRattleOffset = 0f;

	private Vector3 defaultPosition;
	private Vector3 desiredPosition;
	private Rect playerArea;
	private List<RattleValue> rattleValues = new List<RattleValue>();

	void Awake()
	{
		defaultPosition = transform.position;
		desiredPosition = transform.position;
	}

	void Update()
	{
		// Get players
		Character[] characters = FindObjectsOfType<Character>();

		Vector3 focusPoint = defaultPosition;

		if (characters.Length > 0)
		{
			// Do math to get bounds of players
			playerArea = new Rect(characters[0].transform.position.x, characters[0].transform.position.y, 0f, 0f);
			for (int index = 1; index < characters.Length; index++)
			{
				playerArea = playerArea.Encapsulate(characters[index].transform.position);
			}

			// Add min bounds
			playerArea.x -= playerBorderWidth;
			playerArea.y -= playerBorderWidth;
			playerArea.width += playerBorderWidth * 2f;
			playerArea.height += playerBorderWidth * 2f;

			// Extend bounds to min zoom
			Vector2 minZoomSize = gameArea.size * minZoomPercent;
			float xDifference = minZoomSize.x - playerArea.width;
			if (xDifference > 0)
			{
				playerArea.x -= xDifference / 2f;
				playerArea.width += xDifference;
			}
			float yDifference = minZoomSize.y - playerArea.height;
			if (yDifference > 0)
			{
				playerArea.y -= yDifference / 2f;
				playerArea.height += yDifference;
			}

			// Force 16:9 ratio
			float currentRatio = playerArea.width / playerArea.height;
			if (currentRatio < camera.aspect)
			{
				float newWidth = playerArea.height * camera.aspect;
				playerArea.x += (playerArea.width - newWidth) / 2f;
				playerArea.width = newWidth;
			}
			else if (currentRatio > camera.aspect)
			{
				float newHeight = playerArea.width / camera.aspect;
				playerArea.y += (playerArea.height - newHeight) / 2f;
				playerArea.height = newHeight;
			}

			// Scale down to fit gameArea
			if (playerArea.width > gameArea.width || playerArea.height > gameArea.height)
			{
				playerArea = gameArea;
			}

			// Clamp position within gameArea
			float leftOverlap = gameArea.x - playerArea.x;
			float rightOverlap = (playerArea.x + playerArea.width) - (gameArea.x + gameArea.width);
			float bottomOverlap = gameArea.y - playerArea.y;
			float topOverlap = (playerArea.y + playerArea.height) - (gameArea.y + gameArea.height);

			if (leftOverlap > 0)
			{
				playerArea.x += leftOverlap;
			}else if (rightOverlap > 0)
			{
				playerArea.x -= rightOverlap;
			}

			if (topOverlap > 0)
			{
				playerArea.y -= topOverlap;
			}
			else if (bottomOverlap > 0)
			{
				playerArea.y += bottomOverlap;
			}
			
			focusPoint = playerArea.center;
			focusPoint.z = defaultPosition.z * (playerArea.width / gameArea.width); // Handle zoom
		}

		// Move towards focus point
		float zoomSpeed = (focusPoint.z < desiredPosition.z ? cameraZoomOutSpeed : cameraZoomInSpeed);
		desiredPosition = (Vector3)Vector2.Lerp(desiredPosition, focusPoint, Time.deltaTime * cameraTranslateSpeed) + 
			Vector3.forward * Mathf.Lerp(desiredPosition.z, focusPoint.z, Time.deltaTime * zoomSpeed);
		transform.position = desiredPosition;

		// Add rattle
		float rattleAmount = 0f;
		for (int index = rattleValues.Count - 1; index >= 0; index--) 
		{
			rattleAmount = Mathf.Max(rattleValues[index].strength, rattleAmount);
			rattleValues[index].remaining -= Time.deltaTime;
			if(rattleValues[index].remaining <= 0f)
				rattleValues.RemoveAt(index);
		}
		Vector2 direction = new Vector2 (Random.Range (-1f, 1f), Random.Range (-1f, 1f)).normalized;
		direction *= rattleAmount * Random.Range (minRattleOffset, maxRattleOffset);
		transform.position += (Vector3)direction;
	}

	public void AddRattle (float duration, float strength = 1f)
	{
		rattleValues.Add (new RattleValue (duration, strength));
	}

	public static void Rattle(float duration, float strength = 1f)
	{
		FindObjectOfType<CameraController> ().AddRattle (duration, strength);
	}

	public void StopRattle()
	{
		rattleValues.Clear ();
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(gameArea.center, gameArea.size);

		if (Application.isEditor && Application.isPlaying)
		{
			// Draw running stuff
			Gizmos.color = Color.Lerp(Color.red, Color.green, 0.5f);
			Rect currentArea = GetCurrentCameraRect();
			Gizmos.DrawWireCube(currentArea.center, currentArea.size);

			Gizmos.color = Color.Lerp(Color.red, Color.green, 0.25f);
			Gizmos.DrawWireCube(playerArea.center, playerArea.size);
		}
	}

	Rect GetCurrentCameraRect()
	{
		if (Application.isEditor && !Application.isPlaying)
			desiredPosition = transform.position;

		// Get the plane that represents z=0
		Plane zeroPlane = new Plane(Vector3.forward, Vector3.zero);

		Ray bottomLeftRay = camera.ViewportPointToRay(Vector3.zero);
		Ray topRightRay = camera.ViewportPointToRay(new Vector3(1, 1, 0));

		float bottomLeftDistance;
		float topRightDistance;
		zeroPlane.Raycast(bottomLeftRay, out bottomLeftDistance);
		zeroPlane.Raycast(topRightRay, out topRightDistance);

		Vector3 bottomLeft = desiredPosition + bottomLeftRay.direction * bottomLeftDistance;
		Vector3 topRight = desiredPosition + topRightRay.direction * topRightDistance;

		return new Rect(bottomLeft.x,
			bottomLeft.y,
			topRight.x - bottomLeft.x,
			topRight.y - bottomLeft.y);
	}

	[ContextMenu("Snap to Camera")]
	void SnapToCurrentCamera()
	{
		gameArea = GetCurrentCameraRect();
	}
}
