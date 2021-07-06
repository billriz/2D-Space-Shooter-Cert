using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Smart : Enemy
{
    [SerializeField]
    float CastRadius = .5f;
    [SerializeField]
    float CastDistance = 5.0f;

    private bool _canFireAtPlayer = true;
        

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }


    public override void Update()
    {
        base.Update();

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, CastRadius, Vector2.up, CastDistance, LayerMask.GetMask("player"));
        
        if (hit.collider != null)
        {
                    
           if (_canFireAtPlayer == true)
           {
                FireLaserUp();
                StartCoroutine(FireAtPlayerTimer());
           }          
           
        }
       
    }

    void FireLaserUp()
    {
        _canFireAtPlayer = false;

        GameObject smartLaser = Instantiate(base._laserPrefab, transform.position + new Vector3(0f,1.85f,0f), Quaternion.identity);
        Laser[] lasers = smartLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {

            lasers[i].IsSmartLaser();

        }

        StartCoroutine(FireAtPlayerTimer());
    }

    IEnumerator FireAtPlayerTimer()
    {
        yield return new WaitForSeconds(5.0f);
        _canFireAtPlayer = true;

    }
}
