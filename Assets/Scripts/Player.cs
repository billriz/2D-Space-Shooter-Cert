using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _currentSpeed;
    [SerializeField]
    private float _speedBoostMultiplier = 2.0f;

    private float _thrustMultiplier = 1.5f;
    [SerializeField]
    private float _fireRate = .50f;

    private float _angle = 100.0f;
    [SerializeField]
    private float _powerDownTimer = 5.0f;

    private bool _canFire = true;

    private int _ammoCount;

    
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private AudioClip _fireLaserSound;

    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _hits;

    public bool isPlayerDead;

    private int _score;

    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;

    private bool _isPhotonBlastActive = false;
    [SerializeField]
    private bool _isSpeedBoostActive = false;

    private bool _isThrusterActive = false;
    private bool _isPlayerShieldActive = false;
    [SerializeField]
    private GameObject _PlayerShieldVisualizer;
    [SerializeField]
    private GameObject[] _playerDamage;
    [SerializeField]
    private AudioClip _expolsionSoundClip;

    private UIManager _uIManager;

    private AudioSource _audioSource;

    private Animator _animator;
    [SerializeField]
    private SpriteRenderer _playerShield;

    // Start is called before the first frame update
    void Start()
    {

        transform.position = new Vector3(0, 0, 0);
        _ammoCount = 15;
        
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is Null");
        }

        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uIManager == null)
        {
            Debug.LogError("UI Manager is Null");
        }

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on the Player is Null");

        }
        else
        {
            _audioSource.clip = _fireLaserSound;

        }

        _animator = GetComponent<Animator>();

        if (_animator == null)
        {

            Debug.LogError("Animator on the Player is Null");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _isThrusterActive = true;
        }
        else
        {
            _isThrusterActive = false;
        }
        
        CalculateSpeed();

        
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && _canFire)
        {

            FireLaser();
        }

    }

    void CalculateMovement()
    {
     
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        int checkMovement = Mathf.RoundToInt(horizontalInput);

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        switch (checkMovement)            
        {

            case -1:
                _animator.SetBool("IsTurningRight", false);
                _animator.SetBool("IsTurningLeft", true);
                break;
            case 0:
                _animator.SetBool("IsTurningRight", false);
                _animator.SetBool("IsTurningLeft", false);
                break;
            case 1:
                _animator.SetBool("IsTurningLeft", false);
                _animator.SetBool("IsTurningRight", true);
                break;
            default:
                break;
                  
        }
        

        transform.Translate(direction * _currentSpeed * Time.deltaTime);
       
       
        if(transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if(transform.position.y <= -5.8f)
        {
            transform.position = new Vector3(transform.position.x, -5.8f, 0);           
        }

        if (transform.position.x >= 12.9f)
        {
            transform.position = new Vector3(-12.9f, transform.position.y, 0);
        }
        else if (transform.position.x <= -12.9f)
        {
            transform.position = new Vector3(12.9f, transform.position.y, 0);
        }

    }

    private void CalculateSpeed()
    {
        _currentSpeed = _speed;
        
        if (_isThrusterActive && _isSpeedBoostActive == false)
        {
            _currentSpeed = _speed * _thrustMultiplier;
            
        }
        else if (_isSpeedBoostActive)
        {
            _currentSpeed = _speed * _speedBoostMultiplier;
            
        }

    }

    void FireLaser()
    {

        _canFire = false;
        _ammoCount -= 1;
        _uIManager.UpdateAmmo(_ammoCount);

        if (_isTripleShotActive)
        {
           Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
               
        }
        else if (_isPhotonBlastActive)
        {
            for (int i = 0; i < 9; i++)
            {
                _angle = _angle - 20;
                GameObject photonBlast =  Instantiate(_laserPrefab, transform.position, Quaternion.Euler(0, 0, _angle));
            }

            _angle = 100;
        }
        else
        {
          Instantiate(_laserPrefab, transform.position + new Vector3(0, +.8f, 0), Quaternion.identity);
        }

        _audioSource.Play();

        StartCoroutine(FireControlTimer());

    }

    IEnumerator FireControlTimer()
    {
        yield return new WaitForSeconds(_fireRate);
        _canFire = true;
        if (_ammoCount < 1)
        {
            _canFire = false;
        }
    }

    public void LaserRecharge()
    {
        _ammoCount = 15;
        _uIManager.UpdateAmmo(_ammoCount);
        _canFire = true;

    }

    public void Damage()
    {
        if (_isPlayerShieldActive)
        {
            _hits += 1;
            PlayerShieldStrength(_hits);
           return;
        }

        _lives--;
        _uIManager.Updatelives(_lives);
        
        if (_lives < 1)
        {
            isPlayerDead = true;
            _spawnManager.OnPlayerDeath();
            _audioSource.clip = _expolsionSoundClip;
            _audioSource.Play();
            Destroy(this.gameObject);
        }
        else
        {
            PlayerDamageVisual();
        }
    }

    public void ShipRepair()
    {
        if (_lives < 3)
        {
            _lives++;
            _uIManager.Updatelives(_lives);
            if (_playerDamage[0].activeSelf)
            {
                _playerDamage[0].SetActive(false);
            }
            else
            {
                _playerDamage[1].SetActive(false);
            }
        }
        
    }
    
    public void TripleShotActive()
    {

        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerDownTimer);
        _isTripleShotActive = false;
    }

    public void PhotonBlastActive()
    {
        _isPhotonBlastActive = true;
        StartCoroutine(PhotonBlastPowerDownRoutine());

    }

    IEnumerator PhotonBlastPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerDownTimer);
        _isPhotonBlastActive = false;

    }

    public void SpeedBoostActive()
    {

        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());

    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerDownTimer);
       _isSpeedBoostActive = false;

    }
    

   public void PlayerShieldActive()
    {
        _isPlayerShieldActive = true;
        _PlayerShieldVisualizer.SetActive(true);
        _hits = 0;
        _playerShield.color = new Color(1, 1, 1, 1);
    }

   private void PlayerShieldStrength(int hits)
   {
       switch (hits)
       {
           case 0:
              break;
           case 1:
               _playerShield.color = new Color(1, 1, 1, .50f);
               break;
           case 2:
               _playerShield.color = new Color(1, 1, 1, .10f);
               break;
           case 3:
               _PlayerShieldVisualizer.SetActive(false);
               _isPlayerShieldActive = false;
               _playerShield.color = new Color(1, 1, 1, 1);
               _hits = 0;
               break;
           default:
               break;
       }
   }

   public void UpdateScore(int points)
    {
        _score += points;
        _uIManager.UpdateScore(_score);

    }
    
    private void PlayerDamageVisual()
    {
        switch(_lives)
        {
            case 2:

                int randomDamage = Random.Range(0, 2);
                _playerDamage[randomDamage].SetActive(true);
                break;

            case 1:

                if(_playerDamage[0].activeSelf)
                {
                    _playerDamage[1].SetActive(true);
                }
                else
                {
                    _playerDamage[0].SetActive(true);
                }
                break;

            default:
                break;

        }

    }
}
