using UnityEngine;

public class SinWaveMovement : MonoBehaviour
{
    public float amplitude = 1f; // U•
    public float frequency = 1f; // ü”g”
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * frequency) * amplitude;
    }
}
