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
        Debug.DrawRay(transform.position, Vector3.up * CastDistance, Color.red);

        if (hit.collider != null)
        {
            Debug.LogError("player detected");           
           if (_canFireAtPlayer == true)
           {
                FireLaserUp();
                StartCoroutine(FireAtPlayerTimer());
           }
            
           

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            FireLaserUp();

        }
    }

    void FireLaserUp()
    {
        _canFireAtPlayer = false;

        Instantiate(base._laserPrefab, transform.position + new Vector3(0f,1.85f,0f), Quaternion.identity);

        StartCoroutine(FireAtPlayerTimer());
    }

    IEnumerator FireAtPlayerTimer()
    {
        yield return new WaitForSeconds(5.0f);
        _canFireAtPlayer = true;


    }
}
