using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour
{

    private float _speed = 12.0f;

    private float _minDistance;

    private Vector3 _currentPosition;
    private GameObject _closestEnemy;
    [SerializeField]
    GameObject[] _availableEnemyTargets;

    


// Start is called before the first frame update
    void Start()
    {
        GetClosestEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (_closestEnemy != null)
        {
            if (Vector3.Distance(transform.position ,_closestEnemy.transform.position) != 0)
            {
                
                transform.position = Vector2.MoveTowards(transform.position, _closestEnemy.transform.position, _speed * Time.deltaTime);

                Vector3 direction = _closestEnemy.transform.position - transform.position;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

                transform.eulerAngles = Vector3.forward * angle;
            }

        }
        else
        {
            _closestEnemy = GetClosestEnemy();

        }
    }   

    private GameObject GetClosestEnemy()
    {
        _availableEnemyTargets = GameObject.FindGameObjectsWithTag("Enemy");

        _currentPosition = this.transform.position;
        _minDistance = Mathf.Infinity;

        foreach (GameObject target in _availableEnemyTargets)
        {
            float distance = Vector3.Distance(target.transform.position, _currentPosition);
            if (distance < _minDistance)
            {
                _closestEnemy = target;
                _minDistance = distance;
            }
        }

        return _closestEnemy;

    }
}



