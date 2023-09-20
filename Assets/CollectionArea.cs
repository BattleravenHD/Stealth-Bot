using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CollectionArea : MonoBehaviour
{
    public string updateTextText;
    public TMP_Text updateText;
    public Slider slide;
    public GameObject holdToHack;
    public bool done;
    public GameObject Bomb;
    public float placeTime = 5;
    public AudioSource planted;

    bool playerInside;
    bool placeing = false;
    float currentPlaceTime = 0;

    private void Update()
    {
        if (playerInside && !done)
        {
            holdToHack.SetActive(true);
        }else
        {
            holdToHack.SetActive(false);
        }
        if (placeing && !done)
        {
            holdToHack.SetActive(true);
            currentPlaceTime = Mathf.Clamp(currentPlaceTime + 1 * Time.deltaTime, 0, placeTime);
            slide.value = currentPlaceTime / placeTime;
            slide.gameObject.SetActive(true);
            if (currentPlaceTime >= placeTime)
            {
                done = true;
                holdToHack.SetActive(false);
                slide.gameObject.SetActive(false);
                updateText.text = updateTextText;
                Bomb.SetActive(true);
                planted.Play();
            }
        }else if (!placeing)
        {
            currentPlaceTime = 0;
            slide.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInside = false;
        }
    }

    void OnPlaceBomb(InputValue value)
    {
        if (playerInside)
            placeing = value.Get<float>() > 0;
        else
            placeing = false;
    }
}
