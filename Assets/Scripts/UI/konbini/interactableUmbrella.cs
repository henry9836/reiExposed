using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactableUmbrella : MonoBehaviour
{
    public float mouseSpeed = 1.0f;
	private float x = 90;
	private float y;
	
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            y += Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;
            x += Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
			
			if (x > 160.0f)
			{
				x = 160.0f;	
			}
			else if (x < 20.0f)
			{
				x = 20.0f;
			}
			
			if (y > 70.0f)
			{
				y = 70.0f;	
			}
			else if (y < -70.0f)
			{
				y = -70.0f;
			}
			
        }
		else
		{
			float xdis = Mathf.Abs(x - 90) / 3.0f;
			float ydis = Mathf.Abs(y - 0) / 3.0f;

			if (x < 89)
			{
				x += xdis;
			}
			else if (x > 91)
			{
				x -= xdis;		
			}

			if (y < -1)
			{
				y += ydis;
			}
			else if (y > 1)
			{
				y -= ydis;		
			}
		}
		
        Vector3 desired = new Vector3(y, -x, 0);
		
		transform.rotation = Quaternion.Euler(desired);
    }
}
