using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour {

    public void setToNone()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public IEnumerator setToFlat()
    {
        GetComponent<MeshRenderer>().enabled = true;

        float r_x = transform.eulerAngles.x;
        float r_z = transform.eulerAngles.z;

        for (float r = r_x; r >= 0; r += 0.1f)
        {
            
            transform.rotation = Quaternion.Euler(0, 0, 0);
            yield return new WaitForSeconds(.02f);
        }
    }

    public IEnumerator setToTiltOnX()
    {
        GetComponent<MeshRenderer>().enabled = true;

        // get current rotation on the x
        float r_x = transform.eulerAngles.x;

        // every 0.1s, change the angle to be 1 closer to the desired rotation
        for (float r = r_x; r > 30; r -= 1f)
        {

            transform.rotation = Quaternion.Euler(r, 0, 0);
            yield return new WaitForSeconds(.02f);
        }
    }

    public IEnumerator setToTiltOnZ()
    {
        GetComponent<MeshRenderer>().enabled = true;

        // get current rotation on the y
        float r_y = transform.eulerAngles.y;

        // every 0.1s, change the angle to be 1 closer to the desired rotation
        for (float r = r_y; r > 30; r -= 1f)
        {

            transform.rotation = Quaternion.Euler(0, 0, r);
            yield return new WaitForSeconds(.02f);
        }
    }
}
