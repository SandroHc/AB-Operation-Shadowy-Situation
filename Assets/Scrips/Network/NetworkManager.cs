using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	public GameObject playerPrefab;

	private const string uniqueName = "ABOperationShadowySituation";
	private HostData[] hostList;

	void OnMasterServerEvent(MasterServerEvent msEvent) {
		if(msEvent == MasterServerEvent.HostListReceived) {
			hostList = MasterServer.PollHostList();
		}
	}
	
	private void StartServer(string name) {
		// number players, port, 
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(uniqueName, name);
	}

	private void JoinServer(HostData hostData) {
		Network.Connect(hostData);
	}
	
	private void RefreshHostList() {
		MasterServer.RequestHostList(uniqueName);
	}

	private void SpawnPlayer() {
		Network.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, 0);
	}

	void OnServerInitialized() {
		Debug.Log("Server Initializied");
		SpawnPlayer();
	}

	void OnConnectedToServer() {
		Debug.Log("Server Joined");
		SpawnPlayer();
	}
}
