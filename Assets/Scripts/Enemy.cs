using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

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
    [SerializeField]
    private float _rRadius = 0.1f , _aSpeed = 2f;
    
    [SerializeField]
    private float posX, posY, angle;
    
    [SerializeField]
    private GameObject _enemyShieldVisual;

    private bool _isEnemeyShieldActive;
    [SerializeField]
    private bool _isEnemyMovingDown = true;

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
        
        
    }
    // Update is called once per frame
    void Update()
    {

        CalculateMovement();
        
        if (_canFire == true && _isEnemyDestroyed == false)
        {

           // FireLaser();
        }

    }

    void CalculateMovement()
    {

        switch (_enemyId)
        {
            case 0:
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                break;
            case 1:
                transform.Translate(Vector3.down * _speed * Time.deltaTime + _diagDirection * _speed * Time.deltaTime);
                break;
            case 2:
                if (_isEnemyMovingDown)
                {
                    transform.Translate(Vector3.down * 2.0f * Time.deltaTime + _diagDirection * 2.0f * Time.deltaTime);
                   // if (transform.position.y <= 3.5f)
                   // {
                  //      _isEnemyMovingDown = false;
                        
                  //  }
                  StartCoroutine(ElipticalMovemntCoolDownRoutine());
                }
                else
                {
                    ElipticalMovement();
                  //  StartCoroutine(ElipticalMovemntCoolDownRoutine());
                }
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
        else
        {
            _diagDirection = Vector3.right;
        }
    }

    void ElipticalMovement()
    {
        posX = transform.position.x + Mathf.Cos (angle) * _rRadius;
        posY = transform.position.y + Mathf.Sin (angle) * _rRadius / 2;
        Vector3 eliptical = new Vector3(posX, posY,0);
        transform.position = eliptical;
        angle += Time.deltaTime * _aSpeed; 
        
    }

    IEnumerator ElipticalMovemntCoolDownRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5.0f);
            _isEnemyMovingDown = false;
            yield return new WaitForSeconds(5.0f);
            _isEnemyMovingDown = true;

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

            if (_isEnemeyShieldActive)
            {
                _isEnemeyShieldActive = false;
                _enemyShieldVisual.SetActive(false);
                return;
            }

            EnemyDeath();

        }
        else if (other.CompareTag("Laser"))
        {
            
            Destroy(other.gameObject);
            
            if (_isEnemeyShieldActive)
            {
                _isEnemeyShieldActive = false;
                _enemyShieldVisual.SetActive(false);
                return;
            }
            
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

    public void ActivateShield()
    {
      
        _enemyShieldVisual.SetActive(true);
        _isEnemeyShieldActive = true;

    }

    public void SetEnemyId(int wave)
    {
        switch (wave)
        {
            case 0:
                _enemyId = 0;
                break;
            case 1:
                _enemyId = Random.Range(0, 2);
                break;
            case 2:
                _enemyId = Random.Range(0, 3);
                break;
            default:
                break;
        }

        if (_enemyId == 1 || _enemyId == 2)
        {
            float randomChange = Random.Range(3.0f, 5.0f);
         
            InvokeRepeating("ChangeDirection" , 5, randomChange);
        }

    }

}
