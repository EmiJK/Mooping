using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_controler : MonoBehaviour
{
    public GameObject player; // UFO
    private Vector3 offset;
    public float rotationSpeed = 5f; // Rotation speed
    private float horizontalInput;
	public bool isControlEnabled = true;
	
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void Update()
    {
        if (isControlEnabled)
		{
			// Kamera sukasi pagal pelės judėjimą
			horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed;
			transform.RotateAround(player.transform.position, Vector3.up, horizontalInput); // Horizontal rotation

			// Maintain the same offset from the player
			transform.position = player.transform.position + offset;
		}
    }
	
	public void RotateCameraUp()
	{
		// Sukome kamerą į viršų
		Vector3 targetRotation = new Vector3(-90f, transform.eulerAngles.y, transform.eulerAngles.z); // 90 laipsnių, kad žiūrėtų į viršų
		transform.eulerAngles = targetRotation;

		// Atjungiam kameros valdymą, kad ji nebejudėtų
		isControlEnabled = false;
	}
}
