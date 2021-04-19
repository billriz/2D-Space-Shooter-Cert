using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
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
    private int _lives = 3;

    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive;


    // Start is called before the first frame update
    void Start()
    {

        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is Null");

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

           Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
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

        StartCoroutine(FireControlTimer());


    }

    IEnumerator FireControlTimer()
    {

          yield return new WaitForSeconds(_fireRate);
          _canFire = true;


    }

    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
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



   

    
}
