using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class Enemy : MonoBehaviour
{

    protected float _speed = 3.0f;
    [SerializeField]
    private AudioClip _ExpolsionSoundClip;

    private AudioSource _audioSource;


    protected Player _player;

    private Animator _animator;
    private Collider2D _collider2d;
    [SerializeField]
    private float _fireRate = 3.0f;
    protected bool _canFire = true;
    [SerializeField]
    protected GameObject _laserPrefab;
    [SerializeField]
    protected AudioClip _laserSoundClip;

    private bool _isEnemyDestroyed;
    [SerializeField]
    private int _enemyId; // 0=Normal Enemy, 1=Diaganle enemy 2 = side to side
    [SerializeField]
    Vector3 _diagDirection = Vector3.right;
    
    private float posX, posY, PosZ;
    
    [SerializeField]
    private GameObject _enemyShieldVisual;

    private bool _isEnemeyShieldActive;
    [SerializeField]
    private bool _isEnemyMovingDown = false;

    [SerializeField]
    float PowerUpCastRadius = .5f;
    [SerializeField]
    float PowerUpCastDistance = 5.0f;
    [SerializeField]
    float start;
    [SerializeField]
    float end;
    float t = 0.0f;
    private bool _isMovingside = false;

    

    // Start is called before the first frame update
    public virtual void Start()
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

        if (Random.value < .20f)
        {
            ActivateShield();
        }            
        

    }
    // Update is called once per frame
    public virtual void Update()
    {

        CalculateMovement();
        
        if (_canFire == true && _isEnemyDestroyed == false)
        {

          FireLaser();
        }

        CheckForPowerUp();

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
                    transform.Translate(Vector3.down * _speed * Time.deltaTime + _diagDirection * _speed * Time.deltaTime);
                }
                else
                {
                    
                    ElipticalMovement();
                }                
                break;
            default:
                break;
        }

        if (transform.position.y < -8.0f || transform.position.x > 11.0f || transform.position.x < -11.0f)
        {
            float randomX = Random.Range(-10.45f, 10.45f);
           _isEnemyMovingDown = true;
            transform.position = new Vector3(randomX, 9.0f, 0);
        }

    }

    void ChangeDirection()
    {
        Debug.LogError("change direction called");
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
                
        if (_isMovingside)
        {
            start = transform.position.x;
            end = transform.position.x + 10;
            t = 0.0f;
            StartCoroutine(ElipticalMovemntCoolDownRoutine());
            _isMovingside = false;
        }
        

        posX = Mathf.Lerp(start, end, t);
        
        posY = transform.position.y; 
        PosZ = transform.position.z;
        transform.position = new Vector3(posX, posY, PosZ);
        t += 0.6f * Time.deltaTime;
        if (t > 1.0f)
        {
            float temp = end;
            end = start;
            start = temp;
            t = 0.0f;
        }
        
    }

    IEnumerator ElipticalMovemntCoolDownRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(4.0f);
            _isEnemyMovingDown = false;
            _isMovingside = true;
                       
            yield return new WaitForSeconds(4.0f);
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

   protected IEnumerator FireControlTimer()
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
        else if (other.CompareTag("Laser") || other.CompareTag("HomMissile"))
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
        // _collider2d.enabled = !_animator.enabled;
        _collider2d.enabled = !_collider2d.enabled;
        _speed = 0.01f;
        _animator.SetTrigger("OnEnemyDeath");
        _audioSource.clip = _ExpolsionSoundClip;
        _audioSource.Play();
        Destroy(this.gameObject, 1.5f);

    }

    public void ActivateShield()
    {
      
        _enemyShieldVisual.SetActive(true);
        _isEnemeyShieldActive = true;

    }

    public void SetEnemyId(int wave)

    {
        switch(wave)
        {

            case 0:
                _enemyId = 0;
                break;
            case 1:
                int RandomEnemy1 = Random.Range(0, 2);
                _enemyId = RandomEnemy1;
                break;
            case 2:
                int RandomEnemy2 = Random.Range(0, 3);
                _enemyId = RandomEnemy2;
                break;

        }

        if (_enemyId == 1 || _enemyId == 2)
        {
            float randomChange = Random.Range(3.0f, 5.0f);
            InvokeRepeating("ChangeDirection", 5, randomChange);
        }

        if (_enemyId == 2)
        {
            _isEnemyMovingDown = true;
            StartCoroutine(ElipticalMovemntCoolDownRoutine());
        }

    }

    void CheckForPowerUp()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, PowerUpCastRadius, Vector2.down, PowerUpCastDistance, LayerMask.GetMask("powerups"));
        Debug.DrawRay(transform.position, Vector3.down * PowerUpCastDistance, Color.red);

        if (hit.collider != null && _canFire == true)
        {
           
            FireLaser();
        }
    }

   

}
