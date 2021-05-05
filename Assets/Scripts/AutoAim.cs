using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAim : MonoBehaviour
{
    public Transform aimPosition;
    public GameObject currentGun;
    GameObject currentTarget;
    public float distance = 10f;

    bool isAiming;
    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        //set position of gun
        
        currentGun.transform.position = aimPosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckTarget();
        if (isAiming)
        {
            AutoAiming();
        }
    }

    //cast a ray
    private void CheckTarget()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, distance))
        {
            if (hit.transform.gameObject.tag == "Target")
            {
                if (!isAiming)
                    Debug.Log("Target found");

                currentTarget = hit.transform.gameObject;
                
                isAiming = true;
            }
            else
            {
                currentTarget = null;
                isAiming = false;
                
            }

        }
         
    }

    private void AutoAiming()
    {
        //Vector3 pos = currentTarget.transform.position;
        //currentTarget.transform.position=new Vector3()
        //Vector3 pos = currentTarget.transform.Translate(0, 1, 0);
        //currentGun.transform.position += new Vector3(0, 0.5f, 0);
        pos = currentTarget.transform.position;
        pos.y += 1f;
        //currentGun.transform.LookAt(currentTarget.transform);
        currentGun.transform.LookAt(pos);

    }
}
