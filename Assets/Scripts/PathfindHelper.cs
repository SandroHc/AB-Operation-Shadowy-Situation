using UnityEngine;

public class PathfindHelper : MonoBehaviour {
	private NavMeshAgent agent;
	private LineRenderer lineRenderer;

	private Vector3 destination;
	private bool hasDestination;

	public float minDistance = 5f;
	
	void Start() {
		agent = gameObject.GetComponent<NavMeshAgent>();
		lineRenderer = gameObject.GetComponent<LineRenderer>();
	}

	void Update() {
		if(hasDestination && Vector3.Distance(transform.position, destination) < minDistance)
			hasDestination = false;
	}

	public void setDestination(Vector3 destination) {
		this.destination = destination;

		// Reset the NavAgent position to the parent
		//gameObject.transform.position = Vector3.zero;

		//agent.CalculatePath(destination, agent.path);
		agent.SetDestination(destination);
		//agent.Stop();


		updateLine();

		hasDestination = true;
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
