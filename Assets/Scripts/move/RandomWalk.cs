using UnityEngine;

public class RandomWalk : MonoBehaviour
{
    public float speed = 2f; // ‘¬“x
    public float changeInterval = 2f; // •ûŒü“]Š·‚ÌŠÔŠu

    private Vector3 direction;

    void Start()
    {
        InvokeRepeating("ChangeDirection", 0, changeInterval);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void ChangeDirection()
    {
        float angle = Random.Range(0f, 360f);
        direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0).normalized;
    }
}
