using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour {

	public GameObject myCamera;
	bool isAlive = true;
	Vector3 position;
	Quaternion rotation;
	public float lerpSmoothing = 5f;
	
	void Start(){
		bool isLocalPlayer = photonView.isMine;
		Debug.Log("local player:" + isLocalPlayer.ToString());

		myCamera.SetActive(isLocalPlayer);
		GetComponent<SimpleCarController>().enabled = isLocalPlayer;
		GetComponent<Rigidbody>().useGravity = true; 

        if (!isLocalPlayer)
        {
            position = transform.position;
            rotation = transform.rotation;
            StartCoroutine(Alive());
            Debug.Log("started corouting");
        }

	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonSerializeView");
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            Vector3 newPosition = (Vector3)stream.ReceiveNext();
            Quaternion newRotation = (Quaternion)stream.ReceiveNext();
            if (newPosition != null) position = newPosition;
            if (rotation != null) rotation = newRotation;
            
        }
    }


    IEnumerator Alive()
    {
        while (isAlive)
        {
            transform.position = Vector3.Lerp (transform.position, position, Time.deltaTime * lerpSmoothing);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * lerpSmoothing);
            yield return null;
        }
    }

}
