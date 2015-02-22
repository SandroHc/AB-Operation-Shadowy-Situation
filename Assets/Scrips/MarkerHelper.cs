using UnityEngine;
using System.Collections;

public class MarkerHelper : MonoBehaviour {
	private NavMeshAgent agent;
	private Color c = Color.blue;

	public Transform target;

	void Start() {
		agent = gameObject.GetComponent<NavMeshAgent>();
		agent.SetDestination(target.position);
		agent.CalculatePath(target.position, agent.path);
		agent.Stop();
	}

	void Update() {
		DrawPath(agent.path);
	}

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
			Debug.DrawLine(previousCorner, currentCorner, c, 2, false);
			previousCorner = currentCorner;
			i++;
		}

	}
}
