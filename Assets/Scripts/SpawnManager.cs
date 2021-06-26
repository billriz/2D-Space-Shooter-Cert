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
    [SerializeField] private float _timeBetweenWaves = 1.5f;
    private int _nextWave = 0;

    private bool _isCountingenemies = false;
    private bool _isWaitingNextWave = false;
    private bool _canSpawnEnemy = false;

    private UIManager _uiManager;
    
    
    [System.Serializable]
    public class EnemyWaves
    {
        public string name;
        public GameObject[] enemyToSpawn;
        public int enemyCount;
        public float spawnRate;
        

    }

    public EnemyWaves[] enemyWaves;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager on SpawnManager is Null");
            
        }
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
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnRate);

        }
    }

    IEnumerator SpawnEnemyWave(EnemyWaves _enemyWaves)
    {
        _canSpawnEnemy = true;
        _uiManager.StartCoroutine(_uiManager.DisplayWaveRoutine(_nextWave + 1));
        yield return new WaitForSeconds(1.0f);
        _uiManager.IsSpawning();
        StartCoroutine(PowerUpSpawnRoutine());
        
        for (int i = 0; i < _enemyWaves.enemyCount; i++)
        {
            int randomMax = _enemyWaves.enemyToSpawn.Length;
            int randomEnemy = Random.Range(0, randomMax);
            SpawnEnemy(_enemyWaves.enemyToSpawn[randomEnemy]);
            
            yield return new WaitForSeconds(_enemyWaves.spawnRate);
        }
        _canSpawnEnemy = false;
        CountingEnemies();
        yield break;
    }

    void CountingEnemies()
    {
        _isCountingenemies = true;
       
        StartCoroutine(CountingEnemieRoutine());

    }

    IEnumerator CountingEnemieRoutine()
    {
        while (_isCountingenemies)
        {
            yield return new WaitForSeconds(0.5f);
            if (GameObject.FindWithTag("Enemy") == null)
            {
                _isCountingenemies = false;
                
            }
        }

        _isWaitingNextWave = true;
        StartCoroutine(WaitForNextWaveRouting());
        yield break;

    }

    IEnumerator WaitForNextWaveRouting()
    {
        while (_isWaitingNextWave)
        {
            _canSpawnEnemy = true;
            _nextWave++;
            yield return new WaitForSeconds(_timeBetweenWaves);
           
            if (_nextWave > (enemyWaves.Length - 1))
            {
                _nextWave = 0;
                Debug.LogError("waves complete");
                _stopSpawning = true;
                _canSpawnEnemy = false;
                _uiManager.WavesOver();
            }
            
            _isWaitingNextWave = false;
        }

        if (_canSpawnEnemy == true)
        {
            StartCoroutine(SpawnEnemyWave(enemyWaves[_nextWave]));
        }
        

    }
    
    void SpawnEnemy(GameObject _enemy)
    {
        Vector3 PosToSpwan = new Vector3(Random.Range(-10.45f, 10.45f), 7, 0);
        GameObject newEnemy = Instantiate(_enemy, PosToSpwan, Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;
        newEnemy.GetComponent<Enemy>().SetEnemyId(_nextWave);
        
    }

    IEnumerator PowerUpSpawnRoutine()
    {

        while (_stopSpawning == false && _canSpawnEnemy == true)
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
       // StartCoroutine(SpawnRoutine());
       _canSpawnEnemy = true;
       _stopSpawning = false;
       StartCoroutine(SpawnEnemyWave(enemyWaves[_nextWave]));
       // StartCoroutine(PowerUpSpawnRoutine());
    }
}
