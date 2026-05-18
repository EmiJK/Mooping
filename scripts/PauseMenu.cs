using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public GameObject pauseMenuUI;
	private bool isPaused = false;
	
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Tikrina, ar paspaustas ESC
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
	
	public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Rodyti meniu
        Time.timeScale = 0f; // Sustabdyti žaidimą
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Paslėpti meniu
        Time.timeScale = 1f; // Atstatyti žaidimo greitį
        isPaused = false;
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f; // Užtikrinti, kad žaidimo greitis būtų normalus
        SceneManager.LoadScene("Menu"); // Perkelti į pagrindinį meniu
    }
}
