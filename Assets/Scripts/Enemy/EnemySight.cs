using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {
	public float fieldOfViewAngle = 110f;           // Number of degrees, centred on forward, for the enemy see.
	public bool playerInSight;                      // Whether or not the player is currently sighted.
	public Vector3 personalLastSighting;            // Last place this enemy spotted the player.

	private NavMeshAgent nav;                       // Reference to the NavMeshAgent component.
//	private SphereCollider col;                     // Reference to the sphere collider trigger component.
	private Vector3 previousSighting;               // Where the player was sighted last frame.

	public float currentTime;

	void Awake() {
		// Setting up the references.
		nav = GetComponent<NavMeshAgent>();
//		col = GetComponent<SphereCollider>();
	}
	
	void Update() {
		// TODO Temp code to follow the enemy
		if(playerInSight)
			nav.SetDestination(personalLastSighting);

		DrawPath(nav.path);
	}
	
	
	void OnTriggerStay(Collider other) {
		// Only check for new player sights twice a second
		if(currentTime < .5f) {
			currentTime += Time.deltaTime;
			return;
		}

		// If the player has entered the trigger sphere...
		if(other.gameObject.tag == Tags.player) {
			// By default the player is not in sight.
			playerInSight = false;
			
			// Create a vector from the enemy to the player and store the angle between it and forward.
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);
			
			// If the angle between forward and where the player is, is less than half the angle of view...
			if(angle < fieldOfViewAngle) {
//				RaycastHit hit;

//				Debug.Log("IN FIELD OF VIEW!!");

				// ... the player is in sight.
				playerInSight = true;
				
				// Set the last global sighting is the players current position.
				//lastPlayerSighting.position = player.transform.position;
				personalLastSighting = other.transform.position;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		// If the player enters the triger zone, activate the GameObject
		if(other.gameObject.tag == Tags.player) {
			enabled = true;
		}
	}
	
	
	void OnTriggerExit(Collider other) {
		// If the player leaves the trigger zone the player is not in sight.
		if(other.gameObject.tag == Tags.player) {
			playerInSight = false;

			// Also, deactivate the GameObject to prevent unneeded calls
			enabled = false;

			// Reset the timer
			currentTime = 0f;
		}
	}
	
	
	float CalculatePathLength(Vector3 targetPosition) {
		// Create a path and set it based on a target position.
		NavMeshPath path = new NavMeshPath();
		if(nav.enabled)
			nav.CalculatePath(targetPosition, path);
		
		// Create an array of points which is the length of the number of corners in the path + 2.
		Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
		
		// The first point is the enemy's position.
		allWayPoints[0] = transform.position;
		
		// The last point is the target position.
		allWayPoints[allWayPoints.Length - 1] = targetPosition;
		
		// The points inbetween are the corners of the path.
		for(int i = 0; i < path.corners.Length; i++)
			allWayPoints[i + 1] = path.corners[i];
		
		// Create a float to store the path length that is by default 0.
		float pathLength = 0;
		
		// Increment the path length by an amount equal to the distance between each waypoint and the next.
		for(int i = 0; i < allWayPoints.Length - 1; i++)
			pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
		
		return pathLength;
	}

	private Color c = Color.blue;

	void DrawPath(NavMeshPath path) {
		if(path.corners.Length < 2)
			return;

		switch (path.status) {
		case NavMeshPathStatus.PathComplete:
			c = Color.blue;
			break;
		case NavMeshPathStatus.PathInvalid:
			c = Color.red;
			break;
		case NavMeshPathStatus.PathPartial:
			c = Color.yellow;
			break;
		}

		Vector3 previousCorner = path.corners[0];

		int i = 1;
		while (i < path.corners.Length) {
			Vector3 currentCorner = path.corners[i];
			Debug.DrawLine(previousCorner, currentCorner, c);
			previousCorner = currentCorner;
			i++;
		}

	}
}