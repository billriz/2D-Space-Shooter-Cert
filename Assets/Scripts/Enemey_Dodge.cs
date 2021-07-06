using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemey_Dodge : Enemy
{
    [SerializeField]
    float LaserCastRadius = .5f;
    [SerializeField]
    float LaserCastDistance = 8.0f;

    float DodgeRate = 1.0f;
   

    public override void Start()
    {
        base.Start();
                

    }

    public override void Update()
    {
        base.Update();

        RaycastHit2D Laserhit = Physics2D.CircleCast(transform.position,LaserCastRadius,Vector2.down,LaserCastDistance, LayerMask.GetMask("laser"));
        Debug.DrawRay(transform.position, Vector3.down * LaserCastDistance, Color.red);

        if (Laserhit.collider != null)
        {            

            if(Laserhit.collider.CompareTag("Laser"))
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


