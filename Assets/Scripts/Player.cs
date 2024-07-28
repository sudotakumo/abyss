using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject skillShotQPrefab;
    public GameObject skillShotWPrefab;
    public Transform shotPoint;
    public LineRenderer dashLineRenderer; // �_�b�V���p�̃��C�������_���[
    public LineRenderer skillLineRenderer; // �X�L���p�̃��C�������_���[

    private Animator animator;
    private bool isMoving;
    private bool canAttack = true;
    private bool canMove = true; // �ړ��\�t���O
    private bool canUseSkill = true; // �X�L���g�p�\�t���O
    private bool canDash = true; // �_�b�V���g�p�\�t���O
    private Vector3 targetPosition;
    public float speed = 7f;
    public float dashSpeed = 15f; // �_�b�V���̃X�s�[�h
    public float dashDuration = 0.2f; // �_�b�V���̎�������
    public float attackDuration = 0.2f; // �U���R���C�_�[�̗L������
    public float attackDelay = 0.2f;//�U������̔����x��
    public float qSkillDelay = 0.5f; // Q�X�L���̔��˒x������
    public float dashCooldown = 2f; // �_�b�V���̃N�[���^�C��
    private ProjectileBase projectileBase;
    [SerializeField] GameObject m_guideEffect;
    [SerializeField] Collider2D dashAttackCollider; // �_�b�V�����̍U������p�R���C�_�[
    [SerializeField] Collider2D attackCollider; // �U���p�̃R���C�_�[

    void Start()
    {
        animator = GetComponent<Animator>();
        isMoving = false;
        targetPosition = transform.position;

        if (attackCollider != null)
        {
            attackCollider.enabled = false; // �ŏ��͖����ɂ��Ă���
        }
        if (dashAttackCollider != null)
        {
            dashAttackCollider.enabled = false; // �ŏ��͖����ɂ��Ă���
        }

        if (skillLineRenderer != null)
        {
            skillLineRenderer.enabled = false; // �ŏ��͖����ɂ��Ă���
        }
        if (dashLineRenderer != null)
        {
            dashLineRenderer.enabled = false; // �ŏ��͖����ɂ��Ă���
        }
    }

    void Update()
    {
        Move();
        UpdateAnimation();
        HandleAttack();
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
    private void HandleAttack()
    {
        if(!canAttack) return; //�U�����͉������Ȃ�

        if (Input.GetMouseButton(0)) // ���N���b�N
        {
            FaceMouseDirection();
            animator.SetTrigger("Attack"); // �U���A�j���[�V�������Đ�
            StartCoroutine(PerformAttack(attackCollider,attackDelay)); // �U���R���C�_�[��L���ɂ���R���[�`�����J�n
            StopMoving(); // �ړ����~
            canAttack = false;
            canUseSkill = false;
        }
    }
    private IEnumerator PerformAttack(Collider2D attackCollider,float delay)
    {
        if (attackCollider != null)
        {
            yield return new WaitForSeconds(delay);
            attackCollider.enabled = true; // �U���R���C�_�[��L���ɂ���
            yield return new WaitForSeconds(attackDuration); // �U���̎������ԑ҂�
            attackCollider.enabled = false; // �U���R���C�_�[�𖳌��ɂ���
            //canAttack = true; // �U���I����ɍēx�U���\�ɂ���
        }
    }
    private void HandleSkills()
    {
        if (!canUseSkill) return; // �X�L���g�p���͉������Ȃ�

        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.W))
        {
            if (skillLineRenderer != null)
            {
                skillLineRenderer.enabled = true; // ���C�������_���[��L���ɂ���
            }
        }

        if (Input.GetKey(KeyCode.Q))
        {
            UpdateSkillLineRenderer(); // ���C�������_���[���X�V
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            FaceMouseDirection();
            animator.SetTrigger("AttackQ"); // Q�X�L���̍U���A�j���[�V�������Đ�
            StartCoroutine(FireSkillShotWithDelay(skillShotQPrefab, qSkillDelay, speed)); // Q�X�L���̃X�s�[�h��n��
            StopMoving(); // �X�L���g�p���Ɉړ����~
            canUseSkill = false; // �X�L���g�p���͍ēx�g�p�ł��Ȃ��悤�ɂ���
            canAttack = false;

            if (skillLineRenderer != null)
            {
                skillLineRenderer.enabled = false; // ���C�������_���[�𖳌��ɂ���
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            UpdateSkillLineRenderer(); // ���C�������_���[���X�V
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            FaceMouseDirection();
            animator.SetTrigger("AttackW"); // W�X�L���̍U���A�j���[�V�������Đ�
            FireSkillShot(skillShotWPrefab, -0.5f); // W�X�L���̃X�s�[�h�ƃI�t�Z�b�g��n��
            StopMoving(); // �X�L���g�p���Ɉړ����~
            canUseSkill = false; // �X�L���g�p���͍ēx�g�p�ł��Ȃ��悤�ɂ���
            canAttack = false;

            if (skillLineRenderer != null)
            {
                skillLineRenderer.enabled = false; // ���C�������_���[�𖳌��ɂ���
            }
        }

        if (Input.GetKeyUp(KeyCode.E) && canDash)
        {
            FaceMouseDirection();
            animator.SetTrigger("AttackE"); // E�X�L���̍U���A�j���[�V�������Đ�
            StartCoroutine(Dash());
            canUseSkill = false; // �X�L���g�p���͍ēx�g�p�ł��Ȃ��悤�ɂ���
            canAttack = false;
            canDash = false;
        }
    }

    private void UpdateSkillLineRenderer()
    {
        if (skillLineRenderer != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            skillLineRenderer.SetPosition(0, shotPoint.position);
            skillLineRenderer.SetPosition(1, mousePosition);
        }
    }

    private IEnumerator FireSkillShotWithDelay(GameObject skillShotPrefab, float delay, float speed)
    {
        yield return new WaitForSeconds(delay);
        FireSkillShot(skillShotPrefab, 0f);
    }

    private void FireSkillShot(GameObject skillShotPrefab, float offset)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 direction = (mousePosition - shotPoint.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction); // �e�̉�]��ݒ�
        Vector3 spawnPosition = shotPoint.position - direction * offset; // �I�t�Z�b�g��K�p
        GameObject skillShot = Instantiate(skillShotPrefab, spawnPosition, rotation);
        skillShot.GetComponent<ProjectileBase>().Initialize(direction);
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
    public void EnableAttackUse()
    {
        canAttack = true;
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

        // ���C�������_���[��L���ɂ��A�O����`��
        if (dashLineRenderer != null)
        {
            dashLineRenderer.enabled = true;
            dashLineRenderer.SetPosition(0, transform.position);
            dashLineRenderer.SetPosition(1, transform.position + dashDirection * dashSpeed * dashDuration);
        }
        // �_�b�V���J�n���ɍU�������L����
        if (dashAttackCollider != null)
        {
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
            dashAttackCollider.enabled = false;
        }

        // �_�b�V���I����Ɍ��݂̈ʒu���^�[�Q�b�g�ʒu�ɍX�V
        targetPosition = transform.position;

        // �_�b�V���̃N�[���_�E�����J�n
        canUseSkill = true; // �X�L���̎g�p���ēx������
        StartCoroutine(DashCooldown());
        if (dashLineRenderer != null)
        {
            dashLineRenderer.enabled = false;
        }
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        EnableDash();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((attackCollider != null && attackCollider.enabled) || (dashAttackCollider != null && dashAttackCollider.enabled))
        {
            if (other.CompareTag("Enemy"))
            {
                // �G�Ƀ_���[�W��^���郍�W�b�N
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(100); // �_���[�W�l�͕K�v�ɉ����Ē���
                }
            }
        }
    }
}

