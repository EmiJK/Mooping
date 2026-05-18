using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowMovement : MonoBehaviour
{
    public float swaySpeed = 1.0f;  // Supimo greitis
    public float swayAmount = 10.0f; // Supimo kampas (laipsniais)
	public bool isStolen = false; // Flag to mark whether the cow has been stolen
    
    void Update()
    {
		Vector3 initialRotation = transform.eulerAngles; // Gaukite rankiniu būdu nustatytą pradinę rotaciją
		float swayRotation = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
		transform.rotation = Quaternion.Euler(initialRotation.x, initialRotation.y, swayRotation);
    }
}
