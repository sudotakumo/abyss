using UnityEngine;

public class SkillShotW : ProjectileBase
{
    private Player player; // �v���C���[�̎Q��
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // �G�Ƀ_���[�W��^����
            other.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject); // �����̒e������
        }
        else if (other.CompareTag("EnemyProjectile"))
        {
            // �G�̒e������
            Destroy(other.gameObject);
            Destroy(gameObject); // �����̒e������
        }
    }
}
