using UnityEngine;

public class PendulumMovement : MonoBehaviour
{
    public float amplitude = 1f; // U•
    public float frequency = 1f; // ü”g”

    private float startX;

    void Start()
    {
        startX = transform.position.x;
    }

    void Update()
    {
        float x = startX + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}
