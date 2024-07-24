using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 targetPosition;
    public float speed = 7;
    private Animator animator;
    private bool isMoving;
    private bool isAttacking;
    float m_h;
    float m_scaleX;
    [SerializeField] bool m_flipX = false;

    void Start()
    {
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
        isMoving = false;
        isAttacking = false;
    }

    void Update()
    {
        Move();
        Attack();
        UpdateAnimation();
        if (m_flipX)
        {
            FlipX(m_h);
        }
    }

    // �N���b�N�����ʒu�Ɉړ�����
    private void Move()
    {
        if (Input.GetMouseButton(1))
        {
            isMoving = true;

            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0; // z���W���Œ�
            targetPosition = clickPosition;
        }

        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    // �U���A�N�V�����̎���
    private void Attack()
    {
        if (Input.GetMouseButtonDown(0)|| Input.GetMouseButton(0))
        {
            isAttacking = true;
            // �U�����W�b�N�������ɒǉ�
        }
        else if (!Input.GetMouseButton(0))
        {
            isAttacking = false;
        }
    }

    // �A�j���[�V�����̍X�V
    private void UpdateAnimation()
    {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isAttacking", isAttacking);

        // �ҋ@��Ԃ̔���
        if (!isMoving && !isAttacking)
        {
            animator.SetBool("isIdle", true);
        }
        else
        {
            animator.SetBool("isIdle", false);
        }

        var direction = (targetPosition - transform.position).normalized;
    }
    void FlipX(float horizontal)
    {
        /*
         * ������͂��ꂽ��L�����N�^�[�����Ɍ�����B
         * ���E�𔽓]������ɂ́ATransform:Scale:X �� -1 ���|����B
         * Sprite Renderer �� Flip:X �𑀍삵�Ă����]����B
         * */
        m_scaleX = this.transform.localScale.x;

        if (horizontal > 0)
        {
            this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            this.transform.localScale = new Vector3(-1 * Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }
    }
}
