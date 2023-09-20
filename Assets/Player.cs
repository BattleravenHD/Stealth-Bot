using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public GameObject CameraGameObject;
    public float rotationSpeed = 2f;
    public Animator anim;
    public Animation flyover;
    public AudioSource rain;
    public Transform pointer;

    Vector3 offset;
    Vector2 mousePos;
    NavMeshAgent nav;

    float angle = 0;
    // Start is called before the first frame update
    void Start()
    {
        flyover.Play();
        nav = GetComponent<NavMeshAgent>();
        offset = CameraGameObject.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!flyover.isPlaying)
        {
            rain.volume = Mathf.SmoothStep(rain.volume, 0.25f, 0.1f);
            CameraGameObject.transform.position = transform.position + offset;
            CameraGameObject.transform.Rotate(new Vector3(0, angle * rotationSpeed, 0));
            if (nav.velocity.magnitude > 0.5f)
            {
                anim.SetBool("Walking", true);
            }
            else
            {
                anim.SetBool("Walking", false);
            }
        }
    }

    void OnMoveTo()
    {
        if (!flyover.isPlaying)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out hit))
            {
                nav.SetDestination(hit.point);
                pointer.position = hit.point;
            }
        }
    }

    void OnRotate(InputValue val)
    {
        angle = val.Get<float>();
    }

    void OnStop ()
    {
        nav.SetDestination(transform.position);
        pointer.position = transform.position;
    }

    void OnMousePosition(InputValue val)
    {
        mousePos = val.Get<Vector2>();
    }
}
