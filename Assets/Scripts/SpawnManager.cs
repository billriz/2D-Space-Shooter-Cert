 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    
    [SerializeField]
    private GameObject [] _enemyPrefab;
    [SerializeField]
    private GameObject[] _powerUpPrefab;

    [SerializeField]
    private GameObject _enemyContainer;
    


    [SerializeField]
    private float _enemySpawnRate = 5.0f;
   
    private bool _stopSpawning = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }


    IEnumerator SpawnRoutine()
    {

        while (_stopSpawning == false)
        {
            int randomEnemy = Random.Range(0, 2);
            Vector3 PosToSpwan = new Vector3(Random.Range(-10.45f, 10.45f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab[randomEnemy], PosToSpwan, Quaternion.identity);
            //GameObject newEnemy = Instantiate(_enemyPrefab, PosToSpwan, Quaternion.Euler(0,0,100.0f));
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnRate);

        }

    }

    IEnumerator PowerUpSpawnRoutine()
    {

        while (_stopSpawning == false)
        {

            Vector3 PosToSpawn = new Vector3(Random.Range(-10.45f, 10.45f), 7, 0);
            int randomPowerUp = Random.Range(0, 6);
            if (randomPowerUp == 5 && Random.value > 0.7f)
            {
                GameObject newRarePowerUp = Instantiate(_powerUpPrefab[randomPowerUp], PosToSpawn, Quaternion.identity);
            }
            else
            {
                randomPowerUp = Random.Range(0, 5);
            }
            GameObject newpowerUp = Instantiate(_powerUpPrefab[randomPowerUp], PosToSpawn, Quaternion.identity);            
            yield return new WaitForSeconds(Random.Range(3, 8));

        }


    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());

        StartCoroutine(PowerUpSpawnRoutine());
    }
}
