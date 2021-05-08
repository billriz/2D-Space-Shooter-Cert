using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1);

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            Application.Quit();

        }

    }

    public void GameOver()
    {

        _isGameOver = true;

    }
}
