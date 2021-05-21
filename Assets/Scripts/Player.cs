using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedBoostMultiplier = 2.0f;
    [SerializeField]
    private float _fireRate = .50f;
    [SerializeField]
    private float _powerDownTimer = 5.0f;

    private bool _canFire = true;

    
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private AudioClip _fireLaserSound;

    [SerializeField]
    private int _lives = 3;

    public bool isPlayerDead;

    private int _score;

    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]

    private bool _isSpeedBoostActive = false;
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

    // Start is called before the first frame update
    void Start()
    {

        transform.position = new Vector3(0, 0, 0);

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
        
                  
        
        transform.Translate(direction * _speed * Time.deltaTime);
       
       
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

    void FireLaser()
    {

        _canFire = false;

        if (_isTripleShotActive)
        {
           Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
               
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


    }

    public void Damage()
    {
        if (_isPlayerShieldActive == true)
        {
            _isPlayerShieldActive = false;
            _PlayerShieldVisualizer.SetActive(false);
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

    public void SpeedBoostActive()
    {

        _isSpeedBoostActive = true;
        _speed *= _speedBoostMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());

    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerDownTimer);
        _speed /= _speedBoostMultiplier;
        _isSpeedBoostActive = false;

    }

   public void PlayerShieldActive()
    {
        _isPlayerShieldActive = true;
        _PlayerShieldVisualizer.SetActive(true);
        StartCoroutine(PlayerShieldPowerDownRoutine());
        
    }

    IEnumerator PlayerShieldPowerDownRoutine()
    {

        yield return new WaitForSeconds(_powerDownTimer);
        _PlayerShieldVisualizer.SetActive(false);
        _isPlayerShieldActive = false;

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
