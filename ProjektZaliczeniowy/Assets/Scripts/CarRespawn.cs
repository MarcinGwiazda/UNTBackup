using UnityEngine;

public class CarRespawn : MonoBehaviour
{
    public float fallLimitY = -5f; // jeśli auto spadnie niżej, respawn
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Ręczny reset (R)
        if (Input.GetKeyDown(KeyCode.T))
            Respawn();

        // Auto spadło w przepaść
        if (transform.position.y < fallLimitY)
            Respawn();
    }

    void Respawn()
    {
        Vector3 pos = RaceManager.Instance.lastCheckpointPosition + Vector3.up * 1.5f;
        transform.position = pos;

        // reset prędkości
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // ustaw kierunek jazdy w stronę następnego checkpointa
        transform.rotation = RaceManager.Instance.lastCheckpointRotation;
    }
}