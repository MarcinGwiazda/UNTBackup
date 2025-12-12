using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    [Header("Wheel Meshes")]
    public Transform meshFL;
    public Transform meshFR;
    public Transform meshRL;
    public Transform meshRR;

    [Header("Car Settings")]
    public float motorForce = 2000f;
    public float maxSteerAngle = 25f;

    [Header("Stability Settings")]
    public float antiRollForce = 8000f;

    [Header("Braking Settings")]
    public float handbrakeForce = 5000f;

    private float motorInput;
    private float steerInput;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        motorInput = Input.GetAxisRaw("Vertical");
        steerInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        ApplyDrive();
        ApplyBrakingAndDrag();
        UpdateWheelMeshes();
        ApplyHandbrake();
    }

    // ===============================
    //       DRIVE LOGIC
    // ===============================
    private void ApplyDrive()
    {
        float steer = steerInput * maxSteerAngle;

        wheelFL.steerAngle = steer;
        wheelFR.steerAngle = steer;

        float torque = motorInput * motorForce;

        // wolniejsze cofanie
        if (motorInput < 0)
            torque *= 0.4f;

        // brak inputu → brak napędu
        if (motorInput == 0)
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
            wheelRL.motorTorque = 0;
            wheelRR.motorTorque = 0;
        }
        else
        {
            wheelFL.motorTorque = torque;
            wheelFR.motorTorque = torque;
            wheelRL.motorTorque = torque;
            wheelRR.motorTorque = torque;
        }
    }

    // ===============================
    //      BRAKING + ENGINE BRAKING
    // ===============================
    private void ApplyBrakingAndDrag()
    {
        float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
        float brakeForce = 9000f;
        float EngineBrakeForce = 3000f;

        // --- NATYCHMIASTOWE HAMOWANIE (S podczas jazdy do przodu) ---
        if (motorInput < 0 && forwardSpeed > 0f)
        {
            wheelFL.brakeTorque = brakeForce;
            wheelFR.brakeTorque = brakeForce;
            wheelRL.brakeTorque = brakeForce;
            wheelRR.brakeTorque = brakeForce;

            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
            wheelRL.motorTorque = 0;
            wheelRR.motorTorque = 0;

            rb.linearDamping = 1.5f;
            return;
        }

        // --- NATYCHMIASTOWE COFANIE (S gdy stoimy) ---
        if (Mathf.Abs(forwardSpeed) < 0.2f && motorInput < 0)
        {
            wheelFL.brakeTorque = 0;
            wheelFR.brakeTorque = 0;
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;

            rb.linearDamping = 0.05f;
            return;
        }

        // --- NATYCHMIASTOWE HAMOWANIE PODCZAS COFANIA (W) ---
        if (motorInput > 0 && forwardSpeed < 0f)
        {
            wheelFL.brakeTorque = brakeForce;
            wheelFR.brakeTorque = brakeForce;
            wheelRL.brakeTorque = brakeForce;
            wheelRR.brakeTorque = brakeForce;

            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
            wheelRL.motorTorque = 0;
            wheelRR.motorTorque = 0;

            rb.linearDamping = 1.0f;
            return;
        }

        // --- STAŁE HAMOWANIE SILNIKIEM (puszczony gaz) ---
        if (motorInput == 0)
        {
            wheelFL.brakeTorque = EngineBrakeForce;
            wheelFR.brakeTorque = EngineBrakeForce;
            wheelRL.brakeTorque = EngineBrakeForce;
            wheelRR.brakeTorque = EngineBrakeForce;

            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
            wheelRL.motorTorque = 0;
            wheelRR.motorTorque = 0;

            rb.linearDamping = 0.5f;
            
            return;
        }

        // --- NORMALNA JAZDA ---
        wheelFL.brakeTorque = 0;
        wheelFR.brakeTorque = 0;
        wheelRL.brakeTorque = 0;
        wheelRR.brakeTorque = 0;

        rb.linearDamping = 0.05f;
    }

    // ===============================
    //       UPDATE WHEEL MESHES
    // ===============================
    private void UpdateWheelMeshes()
    {
        UpdateWheelPose(wheelFL, meshFL);
        UpdateWheelPose(wheelFR, meshFR);
        UpdateWheelPose(wheelRL, meshRL);
        UpdateWheelPose(wheelRR, meshRR);
    }

    private void UpdateWheelPose(WheelCollider col, Transform mesh)
    {
        col.GetWorldPose(out Vector3 pos, out Quaternion rot);
        mesh.position = pos;
        mesh.rotation = rot;
    }

    // ===============================
    //          HAND BRAKE
    // ===============================
    private void ApplyHandbrake()
    {
        bool handbrake = Input.GetKey(KeyCode.Space);

        if (handbrake)
        {
            wheelRL.brakeTorque = handbrakeForce;
            wheelRR.brakeTorque = handbrakeForce;

            wheelFL.brakeTorque = 0f;
            wheelFR.brakeTorque = 0f;

            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
            wheelRL.motorTorque = 0;
            wheelRR.motorTorque = 0;
        }
    }
}
