using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public AudioSource audioSource;    // AudioSource komponentas
    public AudioClip clickSound;       // Paspaudimo garsas
    public float delay = 1f;           // Vėlavimas prieš scenos keitimą (garsui groti)
    public Image fadeImage;            // Fade efektui naudoti (Image komponentas)
    public float fadeDuration = 1f;    // Fade trukmė
	
	private void Start()
    {
        // Pradžioje paslėpime fadeImage
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false);
        }
    }
	
    public void StartGame()
    {
        StartCoroutine(PlaySoundAndLoadScene("Main")); // Užtikrinkite, kad turite tokią sceną
    }

    private IEnumerator PlaySoundAndLoadScene(string sceneName)
    {
        if (fadeImage != null)
        {
            // Įjungiame fadeImage ir pradedame fade efektą
            fadeImage.gameObject.SetActive(true); // Užtikriname, kad Image aktyvus
            StartCoroutine(Fade(1)); // Fade iš 0 į 1
        }

        // Groja paspaudimo garsą
        if (audioSource && clickSound)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // Palaukia garsui baigtis
        yield return new WaitForSeconds(delay);

        // Įkelia naują sceną
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a; // Pradinis alpha
        float elapsedTime = 0f;

        // Fade animacija
        while (elapsedTime < fadeDuration)
        {
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, currentAlpha); // Keičiamas alfa (permatomumas)
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Užtikriname, kad galutinė reikšmė būtų pasiekta
        fadeImage.color = new Color(0, 0, 0, targetAlpha);
    }

    public void ExitGame()
    {
        Application.Quit(); // Uždarome žaidimą
    }
}
