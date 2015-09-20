using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
    public bool eBraking;
    public bool normalBraking;
}


public class SimpleCarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos; 
    public float maxMotorTorque;
    public float maxSteeringAngle;

    public Transform centerOfMass;
    Rigidbody rBody;

    void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        rBody.centerOfMass = centerOfMass.localPosition;
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    bool IsMovingForwared()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rBody.velocity);
        if (localVelocity.z > 0f)
        {
            return true;
        }
        return false;
    }
    bool IsBrakingNormal(float throttleBrake)
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rBody.velocity);
        if ((localVelocity.z > 0f && throttleBrake < 0) || (localVelocity.z < 0f && throttleBrake > 0))
            return true;
        return false;
    }
    public void FixedUpdate()
    {
        float motorTorque = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        bool braking = IsBrakingNormal(motorTorque);
        bool eBraking = Input.GetKey(KeyCode.Space);



       

        foreach (AxleInfo axleInfo in axleInfos)
        {
            axleInfo.leftWheel.motorTorque = 0f;
            axleInfo.rightWheel.motorTorque = 0f;
            axleInfo.leftWheel.brakeTorque = 0f;
            axleInfo.rightWheel.brakeTorque = 0f;

            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motorTorque;
                axleInfo.rightWheel.motorTorque = motorTorque;
                
            }
            if (axleInfo.normalBraking)
            {
                if (braking)
                {
                    //axleInfo.leftWheel.motorTorque = 0f;
                    //axleInfo.rightWheel.motorTorque = 0f;
                    axleInfo.leftWheel.brakeTorque = Mathf.Abs(motorTorque);
                    axleInfo.rightWheel.brakeTorque = Mathf.Abs(motorTorque);
                }
            }
            if (axleInfo.eBraking)
            {
                if (eBraking)
                {
                    //axleInfo.leftWheel.motorTorque = 0;
                    //axleInfo.rightWheel.motorTorque = 0;
                    axleInfo.leftWheel.brakeTorque = maxMotorTorque * 8f;
                    axleInfo.rightWheel.brakeTorque = maxMotorTorque * 8f;
                }
            }


            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }
}