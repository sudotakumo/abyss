using UnityEngine;

public class SpiralMovement : MonoBehaviour
{
    public float spiralSpeed = 1f; // スパイラルの速度
    public float radius = 5f; // 半径
    private float angle = 0f;

    void Update()
    {
        angle += spiralSpeed * Time.deltaTime;
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        transform.position = new Vector3(x, y, transform.position.z);
        radius += 0.1f * Time.deltaTime; // 半径を徐々に増やす
    }
}
