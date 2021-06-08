using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private float _speed = 3.0f;
    [SerializeField]
    private AudioClip _ExpolsionSoundClip;

    private AudioSource _audioSource;


    private Player _player;

    private Animator _animator;
    private Collider2D _collider2d;
    [SerializeField]
    private float _fireRate = 3.0f;
    private bool _canFire = true;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private AudioClip _laserSoundClip;

    private bool _isEnemyDestroyed;
    [SerializeField]
    private int _enemyId; // 0=Normal Enemy, 1=Diaganle enemy
    [SerializeField]
    Vector3 _diagDirection = Vector3.right;

    // Start is called before the first frame update
    void Start()
    {

        _player = GameObject.FindWithTag("Player").GetComponent<Player>();

        if (_player == null)
        {

            Debug.LogError("player is Null");
        }

        _animator = GetComponent<Animator>();

        if (_animator == null)
        {

            Debug.LogError("Animator on the Enemy is Null");
        }

        _collider2d = GetComponent<Collider2D>();

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {

            Debug.LogError("Audio Source on the Enemy is Null");
        }

        float randomChange = Random.Range(3.0f, 5.0f);
         
        InvokeRepeating("ChangeDirection" , 5,5);

    }

    // Update is called once per frame
    void Update()
    {

        CalculateMovement();
        
        if (_canFire == true && _isEnemyDestroyed == false)
        {

            FireLaser();
        }

    }

    void CalculateMovement()
    {

        switch(_enemyId)
        {
            case 0:
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                break;
            case 1:
                transform.Translate(Vector3.down * _speed * Time.deltaTime + _diagDirection * _speed * Time.deltaTime);
                break;
            default:
                break;
        }

        if (transform.position.y < -8.0f || transform.position.x > 11.0f || transform.position.x < -11.0f)
        {
            float randomX = Random.Range(-10.45f, 10.45f);
            transform.position = new Vector3(randomX, 9.0f, 0);

        }

    }

    void ChangeDirection()
    {
        if (_diagDirection == Vector3.right)
        {
            _diagDirection = Vector3.left;
        }
        else if (_diagDirection == Vector3.left)
        {
            _diagDirection = Vector3.right;
        }
    }

   
    void FireLaser()
    {
        _canFire = false;

        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {

            lasers[i].IsEnemyLaser();

        }
        _audioSource.clip = _laserSoundClip;
        _audioSource.Play();
        StartCoroutine(FireControlTimer());

    }

   private IEnumerator FireControlTimer()
    {

        yield return new WaitForSeconds(_fireRate);
        _canFire = true;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

            EnemyDeath();

        }
        else if (other.CompareTag("Laser"))
        {
            
            Destroy(other.gameObject);
           
            if (_player != null)
            {
                _player.UpdateScore(10);
            }

            EnemyDeath();          
            
        } 

    }
    private void EnemyDeath()
    {
        _isEnemyDestroyed = true;
        _collider2d.enabled = !_animator.enabled;
        _speed = 0.2f;
        _animator.SetTrigger("OnEnemyDeath");
        _audioSource.clip = _ExpolsionSoundClip;
        _audioSource.Play();
        Destroy(this.gameObject, 2.2f);

    }

}
