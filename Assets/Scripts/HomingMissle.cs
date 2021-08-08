using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour
{

    private float _speed = 3.0f;

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
       
       // transform.Translate(Vector3.up * _speed * Time.deltaTime);

     //  transform.position =
     //      Vector2.MoveTowards(transform.position, _closestEnemy.transform.position, _speed * Time.deltaTime);

       Vector3 direction = _closestEnemy.transform.position - transform.position;
       Debug.DrawRay(transform.position,_closestEnemy.transform.position, Color.blue);
       Debug.LogError("direction: " + direction);
       float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Deg2Rad - 90f;

      Debug.LogError("angle: " + angle);
       //Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
       //transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * 50);
       transform.eulerAngles = Vector3.forward * angle;
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



