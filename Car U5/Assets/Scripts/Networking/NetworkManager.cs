using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	const string VERSION = "v0.0.1";
	public string roomName = "CarU5";
	
	public string playerPrefabName = "Car";
	public Transform SpawnPoint;
	
	// Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings(VERSION);
		Debug.Log("NetWork Start");
	}
	
	void OnJoinedLobby(){
		RoomOptions roomOptions = new RoomOptions(){ isVisible =false, maxPlayers = 4};
		PhotonNetwork.JoinOrCreateRoom (roomName, roomOptions, TypedLobby.Default);
	}
	
	void OnJoinedRoom(){
		Debug.Log("instantiating" + playerPrefabName);
		PhotonNetwork.Instantiate(	playerPrefabName,
		 							SpawnPoint.position,
									SpawnPoint.rotation, 0);
		Debug.Log("instantiated" + playerPrefabName);
	}
}

