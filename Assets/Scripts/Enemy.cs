using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private float _speed = 4.0f;

    private Player _player;

    private Animator _animator;
    private Collider2D _collider2d;


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


    }

    // Update is called once per frame
    void Update()
    {

        
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < - 8.0f)
        {
            float randomX = Random.Range(-11.0f, 11.0f);
            transform.position = new Vector3(randomX, 9.0f, 0); 

        }        

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

            EnemyDeath();

            //_animator.SetTrigger("OnEnemyDeath");
            //Destroy(this.gameObject, 2.3f);

        } 
        else if (other.tag == "Laser")
        {
            
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.UpdateScore(10);
            }

            EnemyDeath();
            //_animator.SetTrigger("OnEnemyDeath");
            //Destroy(this.gameObject, 2.3f); 
            
        } 

    }
    private void EnemyDeath()
    {
        _collider2d.enabled = !_animator.enabled;
        _speed = 0.2f;
        _animator.SetTrigger("OnEnemyDeath");
        Destroy(this.gameObject, 2.2f);

    }

}
