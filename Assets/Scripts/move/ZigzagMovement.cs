using UnityEngine;

public class ZigzagMovement : MonoBehaviour
{
    public float speed = 5f; // 移動速度
    public float frequency = 20f; // ジグザグの周波数
    public float magnitude = 0.5f; // ジグザグの幅

    private Vector3 axis;
    private Vector3 pos;

    void Start()
    {
        pos = transform.position;
        axis = transform.right;
    }

    void Update()
    {
        pos += transform.up * Time.deltaTime * speed;
        transform.position = pos + axis * Mathf.Sin(Time.time * frequency) * magnitude;
    }
}
