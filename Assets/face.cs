using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class face : MonoBehaviour
{
    public GameObject cam;
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(cam.transform.position - transform.position, Vector3.up);
    }
}
