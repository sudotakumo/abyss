using UnityEngine;

public class HomingMovement : MonoBehaviour
{
    public Transform target; // ’Ç”ö‘ÎÛ
    public float speed = 5f; // ‘¬“x

    void Update()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }
}
