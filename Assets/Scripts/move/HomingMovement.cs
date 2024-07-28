using UnityEngine;

public class HomingMovement : MonoBehaviour
{
    public Transform target; // �ǔ��Ώ�
    public float speed = 5f; // ���x

    void Update()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }
}
