using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    public Transform center; // íÜêSì_
    public float radius = 5f; // îºåa
    public float angularSpeed = 2f; // äpë¨ìx

    public float angle = 0f;

    void Update()
    {
        angle += angularSpeed * Time.deltaTime;
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        transform.position = new Vector3(center.position.x + x, center.position.y + y, transform.position.z);
    }
}
