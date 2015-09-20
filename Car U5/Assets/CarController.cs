using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {
    public Transform centerOfMass;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelBL;
    public WheelCollider wheelBR;

    public float maxTorque = 40;
    public float maxSteerAngle = 20;
    Rigidbody rBody;
    void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }
    // Use this for initialization
    void Start () {
        rBody.centerOfMass = centerOfMass.localPosition;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        wheelBR.motorTorque = maxTorque * Input.GetAxis("Vertical");
	    wheelBL.motorTorque = maxTorque * Input.GetAxis("Vertical");

        wheelFL.steerAngle = maxSteerAngle * Input.GetAxis("Horizontal");
        wheelFR.steerAngle = maxSteerAngle * Input.GetAxis("Horizontal");
    }
}
