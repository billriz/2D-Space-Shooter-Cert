using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Aggressive : Enemy
{
    [SerializeField]
    private float _playerDistance = 5.0f;
    
    private float _distanceToPlayer;
    [SerializeField]
    private float _chaseMultiplyer;
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }


    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        PlayerDetect();
    }

    void PlayerDetect()
    {
        _distanceToPlayer = Vector2.Distance(_player.transform.position, this.transform.position);
        
        if (_distanceToPlayer <= _playerDistance)
        {
            Debug.LogError("player Found");
            Vector3 dir = this.transform.position - _player.transform.position;
            dir = dir.normalized;
            this.transform.position -= dir * Time.deltaTime * (_speed * _chaseMultiplyer);

        }
    }
}

