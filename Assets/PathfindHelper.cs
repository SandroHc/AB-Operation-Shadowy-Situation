using UnityEngine;
using System.Collections;

public class PathfindHelper : MonoBehaviour {
	private NavMeshAgent agent;
	private LineRenderer lineRenderer;
	
	void Start() {
		agent = gameObject.GetComponent<NavMeshAgent>();
		lineRenderer = gameObject.GetComponent<LineRenderer>();
	}

	public void setDestination(Vector3 dest) {
		// Reset the NavAgent position to the parent
		//gameObject.transform.position = Vector3.zero;

		//agent.CalculatePath(dest, agent.path);
		agent.SetDestination(dest);
		//agent.Stop();


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
