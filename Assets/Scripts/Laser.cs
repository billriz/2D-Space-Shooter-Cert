using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.5f;
    private float _playerLaserSpeed = 10.0f;
    [SerializeField]
    private bool _isEnemyLaser;
    [SerializeField]
    private bool _isSmartLaser;

    [SerializeField] private bool _isBossLaser;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();

        }
        

        void MoveUp()
        {
            transform.Translate(Vector3.up * _playerLaserSpeed * Time.deltaTime);

            if (transform.position.y > 8.0f || transform.position.x > 12.0f || transform.position.x < -12.0f)
            {

                if (transform.parent != null)
                {
                    Destroy(this.transform.parent.gameObject);
                }
                else
                {

                    Destroy(this.gameObject);

                }

            }

        }

        void MoveDown()
        {
            if (_isBossLaser)
            {
                _speed = 12.0f;

            }

            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y <= -8.0f)
            {

                if (transform.parent != null)
                {
                    Destroy(this.transform.parent.gameObject);
                }
                else
                {

                    Destroy(this.gameObject);

                }

            }

        }
    }

    public void IsEnemyLaser()
    {

        _isEnemyLaser = true;

    }

    public void IsSmartLaser()
    {

        _isSmartLaser = true;
    }

    public void IsBossLaser()
    {
        _isEnemyLaser = true;
        _isBossLaser = true;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player"  && _isEnemyLaser == true || _isSmartLaser == true)
        {

            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                if (player.isPlayerDead == false)
                {
                    
                    player.Damage();

                    if (transform.parent != null)
                    {
                        Destroy(this.transform.parent.gameObject);
                    }
                    else
                    {

                        Destroy(this.gameObject);

                    }
                } 
            }
        }        
    }
}
