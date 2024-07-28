using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject skillShotQPrefab;
    public GameObject skillShotWPrefab;
    public Transform shotPoint;
    public LineRenderer lineRenderer; // ラインレンダラーを追加

    private Animator animator;
    private bool isMoving;
    private bool canMove = true; // 移動可能フラグ
    private bool canUseSkill = true; // スキル使用可能フラグ
    private bool canDash = true; // ダッシュ使用可能フラグ
    private Vector3 targetPosition;
    public float speed = 7f;
    public float dashSpeed = 15f; // ダッシュのスピード
    public float dashDuration = 0.2f; // ダッシュの持続時間
    public float qSkillDelay = 0.5f; // Qスキルの発射遅延時間
    public float wSkillSpeed = 3f; // Wスキルの弾のスピード
    public float dashCooldown = 2f; // ダッシュのクールタイム
    [SerializeField] GameObject m_guideEffect;
    [SerializeField] Collider2D dashAttackCollider; // ダッシュ時の攻撃判定用コライダー

    private bool isDashing = false; // ダッシュ中かどうかのフラグ

    void Start()
    {
        animator = GetComponent<Animator>();
        isMoving = false;
        targetPosition = transform.position;

        if (dashAttackCollider != null)
        {
            dashAttackCollider.enabled = false; // 最初は無効にしておく
        }

        if (lineRenderer != null)
        {
            lineRenderer.enabled = false; // 最初は無効にしておく
        }
    }

    void Update()
    {
        Move();
        UpdateAnimation();
        HandleSkills();
    }

    private void Move()
    {
        if (Input.GetMouseButton(1) && canMove)
        {
            isMoving = true;
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0;
            targetPosition = clickPosition;
            FlipTowards(clickPosition);
        }

        if (Input.GetMouseButtonDown(1) && canMove)
        {
            if (m_guideEffect)
            {
                Instantiate(m_guideEffect, targetPosition, Quaternion.identity);
            }
        }

        if (canMove && Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StopMoving();
            AllowMovement();
        }
    }

    private void UpdateAnimation()
    {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isIdle", !isMoving);
    }

    private void FlipTowards(Vector3 targetPosition)
    {
        Vector3 objectPosition = transform.position;

        if (targetPosition.x > objectPosition.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void HandleSkills()
    {
        if (!canUseSkill) return; // スキル使用中は何もしない

        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.W))
        {
            if (lineRenderer != null)
            {
                lineRenderer.enabled = true; // ラインレンダラーを有効にする
            }
        }

        if (Input.GetKey(KeyCode.Q))
        {
            UpdateLineRenderer(); // ラインレンダラーを更新
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            FaceMouseDirection();
            animator.SetTrigger("AttackQ"); // Qスキルの攻撃アニメーションを再生
            StartCoroutine(FireSkillShotWithDelay(skillShotQPrefab, qSkillDelay, speed)); // Qスキルのスピードを渡す
            StopMoving(); // スキル使用時に移動を停止
            canUseSkill = false; // スキル使用中は再度使用できないようにする

            if (lineRenderer != null)
            {
                lineRenderer.enabled = false; // ラインレンダラーを無効にする
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            UpdateLineRenderer(); // ラインレンダラーを更新
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            FaceMouseDirection();
            animator.SetTrigger("AttackW"); // Wスキルの攻撃アニメーションを再生
            FireSkillShot(skillShotWPrefab, wSkillSpeed, -0.5f); // Wスキルのスピードとオフセットを渡す
            StopMoving(); // スキル使用時に移動を停止
            canUseSkill = false; // スキル使用中は再度使用できないようにする

            if (lineRenderer != null)
            {
                lineRenderer.enabled = false; // ラインレンダラーを無効にする
            }
        }

        if (Input.GetKeyUp(KeyCode.E) && canDash)
        {
            FaceMouseDirection();
            animator.SetTrigger("AttackE"); // Eスキルの攻撃アニメーションを再生
            StartCoroutine(Dash());
            canUseSkill = false; // スキル使用中は再度使用できないようにする
            canDash = false;
        }
    }

    private void UpdateLineRenderer()
    {
        if (lineRenderer != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            lineRenderer.SetPosition(0, shotPoint.position);
            lineRenderer.SetPosition(1, mousePosition);
        }
    }

    private IEnumerator FireSkillShotWithDelay(GameObject skillShotPrefab, float delay, float speed)
    {
        yield return new WaitForSeconds(delay);
        FireSkillShot(skillShotPrefab, speed, 0f);
    }

    private void FireSkillShot(GameObject skillShotPrefab, float speed, float offset)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 direction = (mousePosition - shotPoint.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction); // 弾の回転を設定
        Vector3 spawnPosition = shotPoint.position - direction * offset; // オフセットを適用
        GameObject skillShot = Instantiate(skillShotPrefab, spawnPosition, rotation);
        skillShot.GetComponent<ProjectileBase>().Initialize(direction, speed);
    }

    private void FaceMouseDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; // Z座標を固定
        FlipTowards(mousePosition);
    }

    private void StopMoving()
    {
        canMove = false;
        isMoving = false;
        targetPosition = transform.position; // 現在の位置をターゲット位置に設定して停止
    }

    public void AllowMovement()
    {
        canMove = true;
    }

    public void EnableSkillUse()
    {
        canUseSkill = true; // スキルの使用を再度許可する
    }

    public void EnableDash()
    {
        canDash = true; // ダッシュの使用を再度許可する
    }

    private IEnumerator Dash()
    {

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 dashDirection = (mousePosition - transform.position).normalized;

        // ダッシュ開始時に攻撃判定を有効化
        if (dashAttackCollider != null)
        {
            isDashing = true; // ダッシュ中フラグを設定
            dashAttackCollider.enabled = true;
        }

        float dashEndTime = Time.time + dashDuration;
        while (Time.time < dashEndTime)
        {
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
            yield return null;
        }

        // ダッシュ終了後に攻撃判定を無効化
        if (dashAttackCollider != null)
        {
            isDashing = false; // ダッシュ中フラグを解除
            dashAttackCollider.enabled = false;
        }

        // ダッシュ終了後に現在の位置をターゲット位置に更新
        targetPosition = transform.position;

        // ダッシュのクールダウンを開始
        canUseSkill = true; // スキルの使用を再度許可する
        StartCoroutine(DashCooldown());
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        EnableDash();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dashAttackCollider != null && dashAttackCollider.enabled && other.CompareTag("Enemy"))
        {
            // 敵にダメージを与えるロジック
            other.GetComponent<Enemy>().TakeDamage(100); // ダメージ値は必要に応じて調整
        }
    }
}

