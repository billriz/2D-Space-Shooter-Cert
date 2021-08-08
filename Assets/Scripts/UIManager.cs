using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    
    private int MaxAmmo = 50;
    private int MaxMissile = 3;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Text _gameOver;
    [SerializeField]
    private Text _RestartLevel;
    [SerializeField]
    private Text _ammoCount;
    [SerializeField]
    private Text _missileCount;
    [SerializeField]
    private Slider _thrusterHud;
    [SerializeField]
    private Text _wave;

    private GameManager _gameManager;

    public bool _isSpawning;
    public bool _isWavesCompleted;
            

    // Start is called before the first frame update
    void Start()
    {

        _scoreText.text = "Score: " + 0;
        _gameOver.gameObject.SetActive(false);
        _RestartLevel.gameObject.SetActive(false);
        _wave.gameObject.SetActive(false);
        UpdateAmmo(MaxAmmo);
        UpdateMissile(0);
        UpdateThrusterHud(100.0f);
        

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {

            Debug.LogError("Game Manager is Null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void Updatelives(int  currentLives)
    {
        if (currentLives < 0)
        {
            currentLives = 0;
        }
        _livesImg.sprite = _livesSprites[currentLives];

        if (currentLives == 0)
        {

            GameOverSequence();

        }
    }

    public void UpdateAmmo(int ammoCount)
    {
        _ammoCount.text = "Ammo: " + ammoCount + "/" + MaxAmmo;

    }

    public void UpdateMissile(int missileCount)
    {
        _missileCount.text = "Missile: " + missileCount + "/" + MaxMissile;
    }

    public void UpdateThrusterHud(float thrusterFuel)
    {
        _thrusterHud.value = thrusterFuel;

    }

    private void GameOverSequence()
    {

        _gameOver.gameObject.SetActive(true);
        _RestartLevel.gameObject.SetActive(true);
        StartCoroutine(DisplayGameOverRoutine());
        _gameManager.GameOver();

    }

    public void WavesOver()
    { 
        _isWavesCompleted = true;
        DisplayAllWavesCompleted();
        _gameManager.WavesCompleted();
        
    }


    IEnumerator DisplayGameOverRoutine()
    {
        while (true)
        {

            _gameOver.text = "Game Over";
            yield return new WaitForSeconds(.5f);
            _gameOver.text = "";
            yield return new WaitForSeconds(.5f);

        }

    }

    public IEnumerator DisplayWaveRoutine(int nextwave)
    {
        while (_isSpawning == false)
        {
            _wave.gameObject.SetActive(true);
            _wave.text = "Wave " + nextwave;
            yield return new  WaitForSeconds(.5f);
            _wave.text = " ";
            yield return new WaitForSeconds(.5f); 
        }
        
        _wave.gameObject.SetActive(false);
        _isSpawning = false;

    }

    private void DisplayAllWavesCompleted()
    {
      
          _wave.gameObject.SetActive(true);
          _wave.text = "Waves Complete";
          _RestartLevel.gameObject.SetActive(true);
          //_gameManager.WavesCompleted();

        
      
    }
    

    public void IsSpawning()
    {
        _isSpawning = true;
    }
    
    

   
}


