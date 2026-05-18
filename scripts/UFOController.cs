using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UFOController : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    public GameObject beamObject;
    public float cowLiftSpeed = 2f;
    public float detectionRange = 10f;
    public float minSize = 0.1f;
    public float sizeDecreaseSpeed = 0.01f;
    private bool isLifting = false;
    private bool isStealing = false;
    private List<GameObject> cowsInRange = new List<GameObject>();
    public Camera mainCamera;
    public Camera stealingCamera;
    private Vector3 moveDirection;
    public TextMeshProUGUI cowCountText; // UI text showing collected cows
    private int cowsStealed = 0; // Number of stolen cows
    public GameObject missionCompleteText; // Mission complete text
	public TextMeshProUGUI gameOverText;
	public GameObject restartButton;
	public GameObject MainmenuButton;
	
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (beamObject != null) beamObject.SetActive(false);

        if (mainCamera != null) mainCamera.enabled = true;
        if (stealingCamera != null) stealingCamera.enabled = false;

        missionCompleteText.SetActive(false); // Hide mission complete text initially
		gameOverText.gameObject.SetActive(false);
		restartButton.SetActive(false);
		MainmenuButton.SetActive(false);
    }

    void FixedUpdate()
    {
        // Only move the UFO forward if 'W' or 'UpArrow' is pressed
        if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))
        {
            // Get movement direction based on camera rotation
            Vector3 cameraForward = mainCamera.transform.forward;
            cameraForward.y = 0f; // Ensure movement is only horizontal
            cameraForward.Normalize();  // Normalize direction for consistent movement

            moveDirection = cameraForward;

            // Apply movement
            rb.AddForce(moveDirection * speed * 0.99f);
        }
    }

    void Update()
    {
		
		if (cowsStealed == 6)
		{
			FindObjectOfType<camera_controler>().RotateCameraUp();
			MoveUFOUp();

			// Po kelių sekundžių pereiname į meniu
			StartCoroutine(MissionCompleteSequence());
		}
        cowsInRange.Clear();
        
        // Detect cows in range for stealing
        GameObject[] cows = GameObject.FindGameObjectsWithTag("Cow");
		GameObject[] badCows = GameObject.FindGameObjectsWithTag("badcow");
		
        foreach (var cow in cows)
        {
            CowMovement cowMovement = cow.GetComponent<CowMovement>();
            if (Vector3.Distance(transform.position, cow.transform.position) <= detectionRange)
            {
                cowsInRange.Add(cow);
                PlayMooSound(cow);
            }
        }
		foreach (var badCow in badCows)
		{
            CowMovement cowMovement = badCow.GetComponent<CowMovement>();
            if (Vector3.Distance(transform.position, badCow.transform.position) <= detectionRange)
            {
                cowsInRange.Add(badCow);
                PlayMooSound(badCow);
            }
        }

        // Start stealing if there are cows in range
        if (cowsInRange.Count > 0)
        {
            foreach (var cow in cowsInRange)
            {
                ActivateBeam(cow);
                StartStealing(cow);
            }
        }
        else
        {
            DeactivateBeam();
        }

        if (isLifting && cowsInRange.Count > 0)
        {
            foreach (var cow in cowsInRange)
            {
                DecreaseCowSize(cow);
            }
        }
    }

    void PlayMooSound(GameObject cow)
    {
        AudioSource audioSource = cow.GetComponent<AudioSource>();
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void StartStealing(GameObject cow)
    {
        rb.velocity = Vector3.zero;

        Vector3 targetPosition = new Vector3(cow.transform.position.x, transform.position.y, cow.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f && !isStealing)
        {
            isStealing = true;

            if (mainCamera != null) mainCamera.enabled = false;
            if (stealingCamera != null) stealingCamera.enabled = true;
            ActivateBeam(cow);
        }
    }

    void ActivateBeam(GameObject cow)
    {
        if (beamObject != null && !beamObject.activeSelf)
            beamObject.SetActive(true);

        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        cow.transform.position = Vector3.MoveTowards(cow.transform.position, targetPosition, cowLiftSpeed * Time.deltaTime);

        if (Vector3.Distance(cow.transform.position, targetPosition) < 0.1f)
        {
            CowMovement cowMovement = cow.GetComponent<CowMovement>();
            if (!cowMovement.isStolen)
            {
                cowMovement.isStolen = true;
                cowsStealed++;
                UpdateCowCountText();
            }
			if (cow.CompareTag("badcow"))
			{
				GameOver();
			}
            Destroy(cow);
            EndStealing();
        }

        isLifting = true;
    }

    void DeactivateBeam()
    {
        if (beamObject != null && beamObject.activeSelf) beamObject.SetActive(false);
        isLifting = false;
    }

    void DecreaseCowSize(GameObject cow)
    {
        if (cow.transform.localScale.x > minSize && cow.transform.localScale.y > minSize)
        {
            float newScale = Mathf.Max(cow.transform.localScale.x - sizeDecreaseSpeed * Time.deltaTime, minSize);
            cow.transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }

    void EndStealing()
    {
        isStealing = false;

        if (mainCamera != null) mainCamera.enabled = true;
        if (stealingCamera != null) stealingCamera.enabled = false;

        DeactivateBeam();
    }

    void UpdateCowCountText()
    {
        cowCountText.text = "The radar detects cows...\nWe need to collect them: " + cowsStealed + "/6";
    }
	
	public void MoveUFOUp()
	{
		Vector3 startPosition = transform.position;
		Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + 100, startPosition.z); // Kėlimo aukštis
		StartCoroutine(MoveUFO(startPosition, endPosition, 5f)); // 5 sekundės pakilti
	}

	IEnumerator MoveUFO(Vector3 start, Vector3 end, float time)
	{
		float elapsedTime = 0;
		while (elapsedTime < time)
		{
			transform.position = Vector3.Lerp(start, end, elapsedTime / time);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		transform.position = end;
	}
	
	IEnumerator MissionCompleteSequence()
	{
		cowCountText.gameObject.SetActive(false);
		yield return new WaitForSeconds(2f); // Laiko 2 sekundes su užrašu
		missionCompleteText.SetActive(true); // Rodyti užrašą
		yield return new WaitForSeconds(4f); // Laiko užrašui
		SceneManager.LoadScene("Menu"); // Pereis į meniu
	}
	
	void GameOver()
	{
		FindObjectOfType<camera_controler>().RotateCameraUp();
		gameOverText.gameObject.SetActive(true);
		restartButton.SetActive(true);
		MainmenuButton.SetActive(true);
		Time.timeScale = 0f; // Sustabdyti žaidimą
		cowCountText.gameObject.SetActive(false);
	}
	
	public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		Time.timeScale = 1f;
    }
	
	public void Mainmenu()
    {
        SceneManager.LoadScene("Menu");
    }
}