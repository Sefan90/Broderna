using UnityEngine;

public class physicshands2 : MonoBehaviour
{
    [Header("PID")]
    [SerializeField] float frequency = 50f;
    [SerializeField] float damping = 1f;
    [SerializeField] float rotfreqency = 100f;
    [SerializeField] float rotdamping = 0.9f;
    [SerializeField] Rigidbody playerRigbody;
    [SerializeField] Transform target;
    
    [Space]
    [Header("Springs")]
    [SerializeField] float climbForce = 1000f;
    [SerializeField] float climbDrag = 500f;
    
    Vector3 _previousPosition;
    Rigidbody _rigdbody;
    bool _isColliding;

    void Start()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
        _rigdbody = GetComponent<Rigidbody>();
        _rigdbody.maxAngularVelocity = float.PositiveInfinity;
        _previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        PIDMovment();
        PIDRotation();
        if (_isColliding) HookesLaw();
        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    void PIDMovment()
    {  
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Vector3 force = (target.position - transform.position) * ksg + (playerRigbody.velocity - _rigdbody.velocity) * kdg;
        _rigdbody.AddForce(force,ForceMode.Acceleration);
    }

    void PIDRotation()
    {  
        float kp = (6f * rotfreqency) * (6f * rotfreqency) * 0.25f;
        float kd = 4.5f * rotfreqency * rotdamping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Quaternion q = target.rotation * Quaternion.Inverse(transform.rotation);
        if (q.w < 0)
        {
            q.x = -q.x;
            q.y = -q.y;
            q.z = -q.z;
            q.w = -q.w;
        }
        q.ToAngleAxis(out float angle, out Vector3 axis);
        axis.Normalize();
        axis *= Mathf.Deg2Rad;
        Vector3 torque = ksg * axis * angle + -_rigdbody.angularVelocity * kdg;
        _rigdbody.AddTorque(torque, ForceMode.Acceleration);
    }

    void HookesLaw()
    {
        Vector3 displacementFromResting = transform.position;
        Vector3 force = displacementFromResting * climbForce;
        float drag = GetDrag();

        playerRigbody.AddForce(force, ForceMode.Acceleration);
        playerRigbody.AddForce(drag * -playerRigbody.velocity * climbDrag, ForceMode.Acceleration);
    }

    float GetDrag()
    {
        Vector3 handVelocity = (target.localPosition - _previousPosition) / Time.fixedDeltaTime;
        float drag = 1 / handVelocity.magnitude + 0.01f;
        drag = drag > 1 ? 1 : drag;
        drag = drag < 0.03f ? 0.03f : drag;
        _previousPosition = transform.position;
        return drag;
    }

    void OnCollisionEnter(Collision collision)
    {
        _isColliding = true;
    }

    void OnCollisionExit(Collision collision)
    {
        _isColliding = false;
    }
}
