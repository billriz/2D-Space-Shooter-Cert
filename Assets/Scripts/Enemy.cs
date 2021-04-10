using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private float _speed = 4.0f;

    private Player _player;

    // Start is called before the first frame update
    void Start()
    {

        _player = GameObject.FindWithTag("Player").GetComponent<Player>();

        if (_player == null)
        {

            Debug.LogError("player is Null");
        }

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

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

            Destroy(this.gameObject);

        } 
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);

        } 

    }
}
