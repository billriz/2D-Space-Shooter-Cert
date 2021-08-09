using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : MonoBehaviour
{

    private float _speed = 1.0f;

    public float start;
    public float end;
    public float t = 0;
    public float temp;

    public float PosX;
    [SerializeField]
    private int _hits = 50;
    [SerializeField]
    private int _shieldHits = 3;

    private bool _isInPosition;
    [SerializeField]
    private GameObject _bossLaserPrefab;
    [SerializeField]
    private GameObject _bossLaser2Prefab;
    [SerializeField]
    private GameObject _bossShieldVisual;

    private bool _canFire;
    private bool _canFire2;

    private Animator _animator;
    [SerializeField]
    private AudioClip _explosionClip;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FireControlTimer());
        StartCoroutine(Fire2ControlTimer());

        _animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= 2.8f)
        {
            _speed = 0.0f;
            if (!_isInPosition)
            {
                start = transform.position.x;
                end = 8.0f;
                _isInPosition = true;
                
            }
            t += .15f * Time.deltaTime;
            if (t >= 1.0f)
            {
                start = transform.position.x;
                end = -transform.position.x;
                t = 0.0f;
            }          
          
            SideMovement();  

        }

        FireBossLaser();
        FireBossLaser2();       

    }
    private void SideMovement()
    {
        
        PosX = Mathf.Lerp(start, end, t);
        transform.position = new Vector3(PosX, transform.position.y, transform.position.z);


    }

    private void FireBossLaser()
    {
        if (_canFire == true)
        {
            _canFire = false;
            GameObject bossLaser = Instantiate(_bossLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = bossLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].IsBossLaser();
            }

            StartCoroutine(FireControlTimer());
        }

      
    }

    IEnumerator FireControlTimer()
    {

        yield return new WaitForSeconds(5.0f);
        _canFire = true;
    }

    private void FireBossLaser2()
    {
        if (_canFire2 == true)
        {
            _canFire2 = false;
            GameObject bosslaser2 = Instantiate(_bossLaser2Prefab, transform.position, Quaternion.identity);
            Laser[] lasers = bosslaser2.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].IsBossLaser();
            }
            StartCoroutine(Fire2ControlTimer());
        }

    }
    IEnumerator Fire2ControlTimer()
    {
        yield return new WaitForSeconds(3.5f);
        _canFire2 = true;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.CompareTag("Laser"))
       {
           if (_shieldHits > 0)
           {
               _shieldHits -= 1;
               if (_shieldHits <= 0)
               {
                   _bossShieldVisual.SetActive(false);
               }
               return;
           }
           
           _hits--;
            Destroy(other.gameObject);
            if (_hits <= 0)
            {
                _animator.SetTrigger("OnBossDeath");
                Destroy(this.gameObject,1.5f);
            }
            

       }
    }
}
