using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    private float _speed = 8.5f;

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8.0f)
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
