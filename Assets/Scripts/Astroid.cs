using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;

    private SpawnManager _spawnmanager;
    [SerializeField]
    private GameObject _explosionPreFab;


       

    // Start is called before the first frame update
    void Start()
    {
        _spawnmanager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnmanager == null)
        {

            Debug.LogError("Spawn Manager is Null");
        }
                
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(Vector3.back * _speed * Time.deltaTime);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {

            Destroy(other.gameObject);
            Instantiate(_explosionPreFab, transform.position, Quaternion.identity);
            _spawnmanager.StartSpawning();
            Destroy(this.gameObject);
        }
    }
}
