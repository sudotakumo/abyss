using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrontMinion : MonoBehaviour
{
    [Header("移動のスピード")]
    [SerializeField] float _minionMoveSpeed = 1f;
    Transform _playerTransform;
    [Header("これ以上プレイヤーに近づいたら攻撃")]
    [SerializeField] float _attackRange;
    [Header("攻撃のアニメーション")]
    [SerializeField] Animator frontAnim;
    Rigidbody2D _rigidbody;
    public static float _interval = 2f;
    float _timer = 0f;
    WeaponsHP damage;
    minionGenerator MinionGenerator;
    AudioSource _audioSource;
    [SerializeField] GameObject _audioPrefab;
    WeaponsHP _hp;
    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        damage = GetComponentInChildren<WeaponsHP>();
        _rigidbody = GetComponent<Rigidbody2D>();
        //GameObject player = GameObject.Find("PlayerSword");
        MinionGenerator = GameObject.Find("Main Camera").GetComponent<minionGenerator>();
        _audioSource = GetComponentInChildren<AudioSource>();
        if (_playerTransform != null)
        {
            _hp = _playerTransform.GetComponentInChildren<WeaponsHP>();
            if (Vector2.Distance(_playerTransform.position, this.transform.position) < 5)
            {
                Debug.Log("破壊された");
                MinionGenerator._timer = MinionGenerator._interval;
                Destroy(this.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_playerTransform != null)
        {
            var roatation = Quaternion.LookRotation(Vector3.forward, _playerTransform.position - transform.position);
            transform.rotation = roatation;
            float angle = Mathf.Atan2(_playerTransform.position.y - transform.position.y, _playerTransform.position.x - transform.position.x);
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            // 指定した距離より短くなったら攻撃する
            if (Vector2.Distance(_playerTransform.position, transform.position) <= _attackRange)
            {
                if (Vector2.Distance(_playerTransform.position, transform.position) <= 0.2f)
                {
                    _rigidbody.velocity = new Vector2(0, 0);
                }
                if (_timer > _interval)
                {

                    PlayAnim();
                }
            }
            else
            {
                _rigidbody.velocity = new Vector2(direction.x * _minionMoveSpeed, direction.y * _minionMoveSpeed);
            }
        }
        if (_playerTransform == null)
        {
            Quaternion myroa = transform.rotation;
            transform.rotation = myroa;
            _rigidbody.velocity = new Vector2(0, 0);
        }
        if (damage._hp <= 0)
        {
            ScoreManager._score += ScoreManager._screUp;
            Destroy(gameObject);
            Instantiate(_audioPrefab, transform.position, Quaternion.identity);
        }
    }

    public void PlayAnim()
    {
        frontAnim.Play("FrontAtackAnim");
        _timer = 0f;
        _audioSource.Play();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hp.IsInvincible)
        {
            damage.Damage(1);
        }
    }
}