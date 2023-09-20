using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public GameObject Player;
    public GameObject GameOver;
    public Transform[] points;
    public MeshRenderer lightObject;
    public float detectionRange = 5;
    public float detectionAngle = 30;
    public float detectionTime = 3;
    public Light lightForEye;

    Animator anim;
    NavMeshAgent navMeshAgent;
    public int currentPoint = 0;
    float currentDetetionTime = 0;
    bool seenPlayer = false;
    Vector3 lastSeen = Vector3.zero;
    public int direction = 1;
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (points.Length < 1)
        {
            points = new Transform[2] { transform, transform };
        }
        mat = Material.Instantiate(lightObject.material);
        lightObject.material = mat;
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent.velocity.magnitude > 1)
        {
            anim.SetBool("Moving", true);
        }else
        {
            anim.SetBool("Moving", false);
        }
        currentDetetionTime = Mathf.Clamp(currentDetetionTime - 0.5f * Time.deltaTime, 0, detectionTime + 1);
        DetectPlayer();
        mat.SetColor("_EmissionColor", Color.Lerp(Color.white, Color.red, currentDetetionTime / detectionTime));
        lightForEye.color = Color.Lerp(Color.white, Color.red, currentDetetionTime / detectionTime);

        if (seenPlayer)
        {
            if (Vector3.Distance(transform.position, lastSeen) < 1)
            {
                seenPlayer = false;
            }
            else
            {
                navMeshAgent.SetDestination(lastSeen);
            }
        }else
        {
            if (Vector3.Distance(transform.position, points[currentPoint].position) < 2)
            {
                if (currentPoint + direction >= points.Length || currentPoint + direction < 0)
                {
                    direction *= -1;
                }
                currentPoint += direction;
            }else
            {
                navMeshAgent.SetDestination(points[currentPoint].position);
            }
        }
        if (currentDetetionTime > detectionTime)
        {
            GameOver.SetActive(true);
            StartCoroutine(reload());
        }
    }

    void DetectPlayer()
    {

        if (Vector3.Angle(transform.forward, Player.transform.position - transform.position) < detectionAngle)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position + Vector3.up/2, Player.transform.position - transform.position + Vector3.up/2, out hit, detectionRange))
            {
                if (hit.collider.tag == "Player")
                {
                    seenPlayer = true;
                    lastSeen = hit.point;
                    currentDetetionTime = Mathf.Clamp(currentDetetionTime + 2 * Time.deltaTime, 0, detectionTime + 1);
                }
            }
        }
    }

    IEnumerator reload()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
