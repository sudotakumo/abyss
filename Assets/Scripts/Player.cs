using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject skillShotQPrefab;
    public GameObject skillShotWPrefab;
    public Transform shotPoint;
    public LineRenderer lineRenderer; // ���C�������_���[��ǉ�

    private Animator animator;
    private bool isMoving;
    private bool canMove = true; // �ړ��\�t���O
    private bool canUseSkill = true; // �X�L���g�p�\�t���O
    private bool canDash = true; // �_�b�V���g�p�\�t���O
    private Vector3 targetPosition;
    public float speed = 7f;
    public float dashSpeed = 15f; // �_�b�V���̃X�s�[�h
    public float dashDuration = 0.2f; // �_�b�V���̎�������
    public float qSkillDelay = 0.5f; // Q�X�L���̔��˒x������
    public float wSkillSpeed = 3f; // W�X�L���̒e�̃X�s�[�h
    public float dashCooldown = 2f; // �_�b�V���̃N�[���^�C��
    [SerializeField] GameObject m_guideEffect;
    [SerializeField] Collider2D dashAttackCollider; // �_�b�V�����̍U������p�R���C�_�[

    private bool isDashing = false; // �_�b�V�������ǂ����̃t���O

    void Start()
    {
        animator = GetComponent<Animator>();
        isMoving = false;
        targetPosition = transform.position;

        if (dashAttackCollider != null)
        {
            dashAttackCollider.enabled = false; // �ŏ��͖����ɂ��Ă���
        }

        if (lineRenderer != null)
        {
            lineRenderer.enabled = false; // �ŏ��͖����ɂ��Ă���
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
        if (!canUseSkill) return; // �X�L���g�p���͉������Ȃ�

        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.W))
        {
            if (lineRenderer != null)
            {
                lineRenderer.enabled = true; // ���C�������_���[��L���ɂ���
            }
        }

        if (Input.GetKey(KeyCode.Q))
        {
            UpdateLineRenderer(); // ���C�������_���[���X�V
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            FaceMouseDirection();
            animator.SetTrigger("AttackQ"); // Q�X�L���̍U���A�j���[�V�������Đ�
            StartCoroutine(FireSkillShotWithDelay(skillShotQPrefab, qSkillDelay, speed)); // Q�X�L���̃X�s�[�h��n��
            StopMoving(); // �X�L���g�p���Ɉړ����~
            canUseSkill = false; // �X�L���g�p���͍ēx�g�p�ł��Ȃ��悤�ɂ���

            if (lineRenderer != null)
            {
                lineRenderer.enabled = false; // ���C�������_���[�𖳌��ɂ���
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            UpdateLineRenderer(); // ���C�������_���[���X�V
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            FaceMouseDirection();
            animator.SetTrigger("AttackW"); // W�X�L���̍U���A�j���[�V�������Đ�
            FireSkillShot(skillShotWPrefab, wSkillSpeed, -0.5f); // W�X�L���̃X�s�[�h�ƃI�t�Z�b�g��n��
            StopMoving(); // �X�L���g�p���Ɉړ����~
            canUseSkill = false; // �X�L���g�p���͍ēx�g�p�ł��Ȃ��悤�ɂ���

            if (lineRenderer != null)
            {
                lineRenderer.enabled = false; // ���C�������_���[�𖳌��ɂ���
            }
        }

        if (Input.GetKeyUp(KeyCode.E) && canDash)
        {
            FaceMouseDirection();
            animator.SetTrigger("AttackE"); // E�X�L���̍U���A�j���[�V�������Đ�
            StartCoroutine(Dash());
            canUseSkill = false; // �X�L���g�p���͍ēx�g�p�ł��Ȃ��悤�ɂ���
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
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction); // �e�̉�]��ݒ�
        Vector3 spawnPosition = shotPoint.position - direction * offset; // �I�t�Z�b�g��K�p
        GameObject skillShot = Instantiate(skillShotPrefab, spawnPosition, rotation);
        skillShot.GetComponent<ProjectileBase>().Initialize(direction, speed);
    }

    private void FaceMouseDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; // Z���W���Œ�
        FlipTowards(mousePosition);
    }

    private void StopMoving()
    {
        canMove = false;
        isMoving = false;
        targetPosition = transform.position; // ���݂̈ʒu���^�[�Q�b�g�ʒu�ɐݒ肵�Ē�~
    }

    public void AllowMovement()
    {
        canMove = true;
    }

    public void EnableSkillUse()
    {
        canUseSkill = true; // �X�L���̎g�p���ēx������
    }

    public void EnableDash()
    {
        canDash = true; // �_�b�V���̎g�p���ēx������
    }

    private IEnumerator Dash()
    {

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 dashDirection = (mousePosition - transform.position).normalized;

        // �_�b�V���J�n���ɍU�������L����
        if (dashAttackCollider != null)
        {
            isDashing = true; // �_�b�V�����t���O��ݒ�
            dashAttackCollider.enabled = true;
        }

        float dashEndTime = Time.time + dashDuration;
        while (Time.time < dashEndTime)
        {
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
            yield return null;
        }

        // �_�b�V���I����ɍU������𖳌���
        if (dashAttackCollider != null)
        {
            isDashing = false; // �_�b�V�����t���O������
            dashAttackCollider.enabled = false;
        }

        // �_�b�V���I����Ɍ��݂̈ʒu���^�[�Q�b�g�ʒu�ɍX�V
        targetPosition = transform.position;

        // �_�b�V���̃N�[���_�E�����J�n
        canUseSkill = true; // �X�L���̎g�p���ēx������
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
            // �G�Ƀ_���[�W��^���郍�W�b�N
            other.GetComponent<Enemy>().TakeDamage(100); // �_���[�W�l�͕K�v�ɉ����Ē���
        }
    }
}

