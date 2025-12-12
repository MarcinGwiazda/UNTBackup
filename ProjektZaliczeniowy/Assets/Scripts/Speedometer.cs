using UnityEngine;
using TMPro;

public class Speedometer : MonoBehaviour
{
    public Rigidbody targetRb;     // Rigidbody samochodu
    public TextMeshProUGUI speedText;

    void Update()
    {
        if (targetRb == null) return;

        // Prędkość w m/s → km/h
        float speed = targetRb.linearVelocity.magnitude * 3.6f;

        speedText.text = Mathf.RoundToInt(speed) + " km/h";
    }
}