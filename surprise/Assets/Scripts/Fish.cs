﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public string name;
    public int rarity;
    public float ringRadius;
    public float ringSpeed;

    public double stamina;

    public double difficulty;
    public float speed;
    private double swim_time;
    private double wait_time;
    private Vector3 direction;
    private bool hooked;

    private Vector3 mousepos;
    // Start is called before the first frame update
    void Start()
    {
        swim_time = Random.Range(15, 30) + difficulty;
        wait_time = Random.Range(300, 500) - difficulty;
        direction = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0).normalized / 100 * speed;
        hooked = false;
        stamina = 50;
    }

    // Update is called once per frame
    void Update()
    {
        if(!hooked) //Code to run when fish is going about its life
        {
            if(wait_time > 0)
            {
                wait_time -= 1;
            }
            else if(swim_time > 0)
            {
                transform.position += direction;
                swim_time -= 1;
            }
            else
            {
                swim_time = Random.Range(0, 30);
                wait_time = Random.Range(300, 500);
                direction = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0).normalized / 100 * speed;
            }
        }
        else //Code to run for minigame
        {
            stamina += difficulty / 10;
            transform.position += new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0).normalized / 50 * speed;
            mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float dist = Vector3.Distance(transform.position, new Vector3(mousepos.x, mousepos.y, 0));
            if (Input.GetMouseButtonUp(0) && dist < ringRadius)
            {
                stamina -= 30; //change to value of rod
            }
            if (stamina <= 0)
            {
                Debug.Log("i've been caught >:(");
                Destroy(this.gameObject); //Destroy(this.line) later
            }
            else if (stamina > 100)
            {
                Debug.Log("i'm free B)");
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.GetComponent<Fisher>().hooked)
        {
            Debug.Log("ive been hooked :(");
            hooked = true;
        }
    }

}
