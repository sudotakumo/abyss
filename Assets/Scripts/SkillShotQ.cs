using UnityEngine;

public class SkillShotQ : ProjectileBase
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // �_���[�W��^���郍�W�b�N
            other.GetComponent<Enemy>().TakeDamage(damage);
            // Q�X�L���̒e�͓������Ă������Ȃ�
        }
    }
}
