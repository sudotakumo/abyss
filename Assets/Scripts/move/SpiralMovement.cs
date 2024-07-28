using UnityEngine;

public class SpiralMovement : MonoBehaviour
{
    public float spiralSpeed = 1f; // �X�p�C�����̑��x
    public float radius = 5f; // ���a
    private float angle = 0f;

    void Update()
    {
        angle += spiralSpeed * Time.deltaTime;
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        transform.position = new Vector3(x, y, transform.position.z);
        radius += 0.1f * Time.deltaTime; // ���a�����X�ɑ��₷
    }
}
