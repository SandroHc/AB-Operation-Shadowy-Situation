using System.Collections;
using UnityEngine;

public class PathfindHelper : MonoBehaviour {
	public NavMeshAgent agent;
	public LineRenderer line;

	private bool targetUpdated = false;

	private Vector3 _target;
	public Vector3 target {
		get { return _target; }
		set { _target = value;
			targetUpdated = true;
			agent.enabled = true;
		}
	}
	
	void Start() {
		//agent = gameObject.GetComponent<NavMeshAgent>();
		line = agent.GetComponent<LineRenderer>();
	}

	void Update() {
		if(targetUpdated) {
			targetUpdated = false;

			getPath();
		} else if(agent.enabled && transform.hasChanged) {
			agent.transform.localPosition = Vector3.zero;

			// The agent updates the path to the target automatically
			drawPath(agent.path);
		}
	}

	private void getPath() {
		// Create the path
		agent.SetDestination(target);

		// Wait until path has been generated (on the next frame)
		//yield return null;

		drawPath(agent.path);

		// Add this if you don't want to move the agent
		agent.Stop();
	}

	private void drawPath(NavMeshPath path) {
		// If the path has 1 or no corners, there is no need to draw lines
		if(path.corners.Length < 2) 
			return;

		switch(path.status) {
			//case NavMeshPathStatus.PathComplete:line.SetColors(Color.blue, Color.blue); break;
			case NavMeshPathStatus.PathInvalid:	line.SetColors(Color.red, Color.red); break;
			case NavMeshPathStatus.PathPartial:	line.SetColors(Color.yellow, Color.yellow); break;
		}

		// Re-enable the line
		if(!line.enabled) line.enabled = true;

		// Set the array of positions to the amount of corners
		line.SetVertexCount(path.corners.Length);

		// Set the line's origin
		line.SetPosition(0, transform.position);

		for(var i = 1; i < path.corners.Length; i++) {
			// Lift up a bit, to stay above the ground
			path.corners[i].y += .01f;

			// Go through each corner and set that to the line renderer's position
			line.SetPosition(i, path.corners[i]);
		}
	}

	public void complete() {
		agent.enabled = false;
		line.enabled = false;
	}
}
