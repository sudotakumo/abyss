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

    // クリックした位置に移動する
    private void Move()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0; // z座標を固定
            targetPosition = clickPosition;
        }

        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            isMoving = true;

            // 向きの変更
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(2.5f, 2.5f, 2.5f); // 右向き
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(2.5f, 2.5f, 2.5f); // 左向き
            }
        }
        else
        {
            isMoving = false;
        }
    }

    // 攻撃アクションの実装
    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
            // 攻撃ロジックをここに追加
        }
        else
        {
            isAttacking = false;
        }
    }

    // アニメーションの更新
    private void UpdateAnimation()
    {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isAttacking", isAttacking);

        // 待機状態の判定
        if (!isMoving && !isAttacking)
        {
            animator.SetBool("isIdle", true);
        }
        else
        {
            animator.SetBool("isIdle", false);
        }

        Vector3 direction = (targetPosition - transform.position).normalized;
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }
    void FlipX(float horizontal)
    {
        /*
         * 左を入力されたらキャラクターを左に向ける。
         * 左右を反転させるには、Transform:Scale:X に -1 を掛ける。
         * Sprite Renderer の Flip:X を操作しても反転する。
         * */
        m_scaleX = this.transform.localScale.x;

        if (horizontal > 0)
        {
            this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }
        else if (horizontal < 0)
        {
            this.transform.localScale = new Vector3(-1 * Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }
    }
}
