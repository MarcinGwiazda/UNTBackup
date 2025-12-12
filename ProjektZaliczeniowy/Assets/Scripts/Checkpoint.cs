using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex; // 0 = start, 1 = cp1, 2 = cp2...

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RaceManager.Instance.PlayerHitCheckpoint(checkpointIndex);
        }
    }
}