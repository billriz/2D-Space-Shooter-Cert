using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    private float _fireRate = .15f;

    private bool _canFire = true;

    [SerializeField]
    private GameObject _laserPrefab;
    
    // Start is called before the first frame update
    void Start()
    {

        transform.position = new Vector3(0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {

        CalculateMovement();

        // when space key is press
        if (Input.GetKeyDown(KeyCode.Space) && _canFire)
        {

            // fire laser at the position of the player
            Instantiate(_laserPrefab, transform.position + new Vector3(0, +.85f, 0), Quaternion.identity);
            _canFire = false;
            StartCoroutine(FireControlTimer());



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


    IEnumerator FireControlTimer()
    {

        yield return new WaitForSeconds(_fireRate);
        _canFire = true;


    }


}
