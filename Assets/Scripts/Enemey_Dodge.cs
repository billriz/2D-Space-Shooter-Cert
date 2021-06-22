using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemey_Dodge : Enemy
{
    [SerializeField]
    float CastRadius = .5f;
    [SerializeField]
    float CastDistance = 8.0f;

    float DodgeRate = 1.0f;
   

    public override void Start()
    {
        base.Start();
                

    }

    public override void Update()
    {
        base.Update();

        RaycastHit2D hit = Physics2D.CircleCast(transform.position,CastRadius,Vector2.down,CastDistance, LayerMask.GetMask("laser"));
        Debug.DrawRay(transform.position, Vector3.down * CastDistance, Color.red);

        if (hit.collider != null)
        {            

            if(hit.collider.CompareTag("Laser"))
            {               
                transform.position = new Vector3(transform.position.x - DodgeRate, transform.position.y, transform.position.z);
                DodgeRate -= .3f;
                if (DodgeRate <= 0f)
                {
                    DodgeRate = .05f;

                }
            }
        }
    }






}


