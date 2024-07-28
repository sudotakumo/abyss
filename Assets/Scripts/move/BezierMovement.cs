using UnityEngine;

public class BezierMovement : MonoBehaviour
{
    public Transform[] points; // ’Ê‰ßƒ|ƒCƒ“ƒg
    private float t = 0f;

    void Update()
    {
        if (points.Length == 3)
        {
            t += Time.deltaTime * 0.1f;
            if (t > 1f) t = 0f;

            Vector3 m1 = Vector3.Lerp(points[0].position, points[1].position, t);
            Vector3 m2 = Vector3.Lerp(points[1].position, points[2].position, t);
            transform.position = Vector3.Lerp(m1, m2, t);
        }
    }
}
