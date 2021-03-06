﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public string name;
    public Sprite trueForm; //what the fish actually looks like
    public int rarity;
    public int value;
    public float ringRadius;
    public float ringSpeed;

    public double stamina;

    public double difficulty;
    public float speed;
    public double power;
    private double swim_time;
    private double wait_time;
    public double bite_time;

    public SpriteRenderer spriteRenderer;

    private Vector3 direction;
    private bool hooked;
    private Shop shop;

    private Fisher fisherman;
    private Vector3 mousepos;
    // Start is called before the first frame update
    void Start()
    {
        swim_time = Random.Range(15, 30) + difficulty;
        wait_time = Random.Range(300, 500) - difficulty;
        bite_time = Random.Range(300, 600);
        direction = new Vector3(Random.Range(-10, 11), Random.Range(-10, 11), 0).normalized / 100 * speed;
        hooked = false;
        stamina = 50;
        shop = GameObject.FindGameObjectWithTag("shop").GetComponent<Shop>();
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
                direction = new Vector3(Random.Range(-10, 11), Random.Range(-10, 11), 0).normalized / 100 * speed;
            }
        }
        else //Code to run for minigame
        {
            stamina += power;
            transform.position += direction * 2; //change position of fish
            transform.position = (transform.position - fisherman.transform.position) * (float) 0.95 + fisherman.transform.position; //make position drift towards hook
            mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float dist = Vector3.Distance(transform.position, new Vector3(mousepos.x, mousepos.y, 100));
            if (Input.GetMouseButtonUp(0) && dist < ringRadius)
            {
                stamina -= fisherman.currentRod.efficiency; //done
                Debug.Log("ow");
            }
            if (stamina <= 0)
            {
                Debug.Log("i've been caught >:C");
                fisherman.hooked = false;
                fisherman.newCatch();
                fisherman.money += value;
                Destroy(this.gameObject); //Destroy(this.line) later
            }
            else if (stamina > 100)
            {
                Debug.Log("i'm free B)");
                fisherman.hooked = false;
                Destroy(this.gameObject);
            }
            swim_time -= 1;
            if (swim_time <= 0)
            {
                direction = new Vector3(Random.Range(-10, 11), Random.Range(-10, 11), 0).normalized / 100 * speed;
                swim_time = Random.Range(10, 20);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.GetComponent<Fisher>().hooked && !shop.shopOpen)
        {
            fisherman = collision.gameObject.GetComponent<Fisher>();
            fisherman.hooked = true;
            fisherman.target = this;
            Debug.Log("ive been hooked :(");
            hooked = true;
            spriteRenderer.sprite = trueForm;
        }
    }

    void OnGUI()
    {
        if (hooked)
        {
            GUI.color = Color.yellow;
            GUI.Label(new Rect(300,10,100,200), "Stamina: " + ((int)stamina).ToString());
        }
    }
}
