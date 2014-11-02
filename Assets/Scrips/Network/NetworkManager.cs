using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	public GameObject playerPrefab;

	private const string uniqueName = "ABOperationShadowySituation";
	public HostData[] hostList;

	void OnMasterServerEvent(MasterServerEvent msEvent) {
		if(msEvent == MasterServerEvent.HostListReceived) {
			hostList = MasterServer.PollHostList();
		}
	}
	
	public void StartServer(string name) {
		// number players, listen port, use NAT
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(uniqueName, name);
	}

	public void JoinServer(HostData hostData) {
		Network.Connect(hostData);
	}
	
	public void RefreshHostList() {
		MasterServer.RequestHostList(uniqueName);
	}

	private void SpawnPlayer() {
		Network.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, 0);
	}

	void OnServerInitialized() {
		Debug.Log("Server Initializied");
		//SpawnPlayer();
	}

	void OnConnectedToServer() {
		Debug.Log("Server Joined");
		Application.LoadLevel(1); // Load main scene
		SpawnPlayer();
	}
}
