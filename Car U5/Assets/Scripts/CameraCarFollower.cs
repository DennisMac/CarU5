using UnityEngine;
using System.Collections;

public class CameraCarFollower : MonoBehaviour {
    public Transform car;
    public float distance = 10f;
    public float height = 2.0f;
    public float rotationDamping =3f;
    public float heightDamping = 2f;
    public float zoomRatio = 0.5f;
    private float DefaultFOV = 60;
    Camera cam;
    Rigidbody rBody;
    private float rotationVector = 0;
    // Use this for initialization
    void Awake()
    {
        cam = GetComponent<Camera>();
        rBody =car.GetComponent<Rigidbody > ();
    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        float wantedAngle = rotationVector;
        float wantedHeight = car.position.y + height;
        float newAngle = transform.eulerAngles.y;
        float newHeight = transform.position.y;

        newAngle = Mathf.Lerp(newAngle, wantedAngle, rotationDamping * Time.deltaTime);
        newHeight = Mathf.Lerp(newHeight, wantedHeight, heightDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, newAngle, 0);
        transform.position = car.position;
        transform.position -= currentRotation * Vector3.forward * distance;
        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
        transform.LookAt(car);
	}

    void FixedUpdate()
    {
        Vector3 localVelocity = car.InverseTransformDirection(rBody.velocity);
        if (localVelocity.z < -0.5f)
        {
            rotationVector = car.eulerAngles.y + 180;
        }
        else
        {
            rotationVector = car.eulerAngles.y;
        }
            cam.fieldOfView = DefaultFOV + zoomRatio * rBody.velocity.magnitude;
            //Debug.Log(cam.fieldOfView.ToString());
    }
}
