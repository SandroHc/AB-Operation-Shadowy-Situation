using UnityEngine;
using System.Collections;

public class PathfindHelper : MonoBehaviour {
	private NavMeshAgent agent;
	private LineRenderer lineRenderer;

	private Vector3 destination;
	public Transform destTransform;
	
	void Start() {
		agent = gameObject.GetComponent<NavMeshAgent>();
		lineRenderer = gameObject.GetComponent<LineRenderer>();

		if(destTransform != null) {
			destination = destTransform.position;
			agent.SetDestination(destination);
		}
	}

	public void setDestination(Vector3 destination) {
		this.destination = destination;
		// Reset the NavAgent position to the parent
		//gameObject.transform.position = Vector3.zero;

		//agent.CalculatePath(destination, agent.path);
		agent.SetDestination(destination);
		//agent.Stop();


		updateLine();
	}

	public void updateLine() {
		lineRenderer.enabled = true;
		
		switch(agent.path.status) {
		case NavMeshPathStatus.PathComplete: lineRenderer.SetColors(Color.blue, Color.blue); break;
		case NavMeshPathStatus.PathInvalid:  lineRenderer.SetColors(Color.red, Color.red); break;
		case NavMeshPathStatus.PathPartial:  lineRenderer.SetColors(Color.yellow, Color.yellow); break;
		}
		
		lineRenderer.SetVertexCount(agent.path.corners.Length);
		for(int i=0; i < agent.path.corners.Length; i++)
			lineRenderer.SetPosition(i, agent.path.corners[i]);
	}
}
