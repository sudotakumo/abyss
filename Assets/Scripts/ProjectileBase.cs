using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;
    public int damage = 10;
    private Vector3 direction;

    void Start()
    {
        // �I�u�W�F�N�g����������Ă���lifeTime�b��Ɏ����I�ɍ폜�����
        Destroy(gameObject, lifeTime);
    }

    public void Initialize(Vector3 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
    }

    void Update()
    {
        // �e���w�肳�ꂽ�����Ɉړ�������
        transform.position += direction * speed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // �G�Ƀ_���[�W��^����
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(100); // �_���[�W�l�͕K�v�ɉ����Ē���
            }

            // �e���폜����
            Destroy(gameObject);
        }
    }
}
