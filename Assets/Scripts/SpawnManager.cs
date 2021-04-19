 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private float _enemySpawnRate = 5.0f;

    private bool _stopSpawning = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        

    }


    IEnumerator SpawnRoutine()
    {

        while (_stopSpawning == false)
        {

            Vector3 PosToSpwan = new Vector3(Random.Range(-11.3f, 11.3f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, PosToSpwan, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnRate);

        }

    }

    public void OnPlayerDeath()
    {

        _stopSpawning = true;

    }
}
