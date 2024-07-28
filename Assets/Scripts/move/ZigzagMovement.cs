using UnityEngine;

public class ZigzagMovement : MonoBehaviour
{
    public float speed = 5f; // �ړ����x
    public float frequency = 20f; // �W�O�U�O�̎��g��
    public float magnitude = 0.5f; // �W�O�U�O�̕�

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
