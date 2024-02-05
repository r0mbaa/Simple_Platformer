using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boomboxplaying : MonoBehaviour
{
    private AudioSource music;
    private void Awake()
    {
        //Получить компоненты
        music = GetComponent<AudioSource>();

    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player")
            music.Play();

    }
    void OnTriggerExit2D (Collider2D other)
    {
        if (other.tag == "Player")
            music.Pause();

    }
    

}
