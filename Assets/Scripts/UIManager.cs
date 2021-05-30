using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
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

    private GameManager _gameManager;
            

    // Start is called before the first frame update
    void Start()
    {

        _scoreText.text = "Score: " + 0;
        _gameOver.gameObject.SetActive(false);
        _RestartLevel.gameObject.SetActive(false);
        _ammoCount.text = "Ammo: " + 15;

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
        _ammoCount.text = "Ammo: " + ammoCount;

    }

    private void GameOverSequence()
    {

        _gameOver.gameObject.SetActive(true);
        _RestartLevel.gameObject.SetActive(true);
        StartCoroutine(DisplayGameOverRoutine());
        _gameManager.GameOver();

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

   
}


