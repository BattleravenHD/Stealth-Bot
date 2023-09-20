using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    public CollectionArea[] areas;
    public Animation shipClip;
    public GameObject cam;
    public GameObject winText;
    public GameObject turnOff;
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (CollectionArea item in areas)
            {
                if (!item.done)
                {
                    return;
                }
            }
            // Win
            player.SetActive(false);
            cam.SetActive(true);
            other.gameObject.SetActive(false);
            shipClip.Play();
            turnOff.SetActive(false);
            winText.SetActive(true);
        }
    }
}
