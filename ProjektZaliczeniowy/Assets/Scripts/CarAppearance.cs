using UnityEngine;

public class CarAppearance : MonoBehaviour
{
    public Material car1Material;
    public Material car2Material;
    public Renderer carRenderer;

    void Start()
    {
        if (GameProgress.Car2Unlocked)
            carRenderer.material = car2Material;
        else
            carRenderer.material = car1Material;
    }
}