using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    private Player player; // �v���C���[�̎Q��

    void Start()
    {
        player = FindObjectOfType<Player>(); // �v���C���[�������ĎQ�Ƃ��擾
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // �G��|�����Ƃ��Ƀv���C���[�̃_�b�V������
        if (player != null)
        {
            player.EnableDash();
        }

        // �G�I�u�W�F�N�g���폜
        Destroy(gameObject);
    }
}
