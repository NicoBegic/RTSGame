using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public int CamSpeed;

	void Start () 
    {
	
	}
	
	void Update () 
    {
        transform.Translate(Input.GetAxis("Horizontal") * CamSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * CamSpeed * Time.deltaTime);
	}
}
