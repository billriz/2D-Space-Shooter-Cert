using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour
{

    private float _speed = 3.0f;
    

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }
}
