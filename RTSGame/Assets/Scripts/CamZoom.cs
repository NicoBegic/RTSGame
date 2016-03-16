using UnityEngine;
using System.Collections;

public class CamZoom : MonoBehaviour 
{
    public int ZoomSpeed;
	
	void Update () 
    {
        transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * Time.deltaTime);
	}
}
