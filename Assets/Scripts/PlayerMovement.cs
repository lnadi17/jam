using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;
    public float force = 100;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private bool lookinLeft = false;
    private float forceX = 0, forceY = 0;
    private bool isDying = false;

    private string currentGroundTag = "Untagged";
    private List<string> groundTags = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        transform.up = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        forceX = 0;
        forceY = 0;
        if (isDying) { rb.velocity = Vector2.zero;  return; }
        if (Input.GetKey(KeyCode.DownArrow) && rb.velocity.y > -speed)
        {
            forceY -= force;
        }
        if (Input.GetKey(KeyCode.UpArrow) && rb.velocity.y < speed)
        {
            forceY += force;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && rb.velocity.x > -speed)
        {
            lookinLeft = true;
            forceX -= force;
        }
        if (Input.GetKey(KeyCode.RightArrow) && rb.velocity.x < speed)
        {
            lookinLeft = false;
            forceX += force;
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector2(forceX, forceY));
        if (rb.velocity.y > speed) rb.velocity = new Vector2(rb.velocity.x, speed);
        else if (rb.velocity.y < -speed) rb.velocity = new Vector2(rb.velocity.x, -speed);
        if (rb.velocity.x > speed) rb.velocity = new Vector2(speed, rb.velocity.y);
        else if (rb.velocity.x < -speed) rb.velocity = new Vector2(-speed, rb.velocity.y);
        sr.flipX = lookinLeft;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        groundTags.Add(collision.gameObject.tag);
        collision.GetComponent<SpriteRenderer>().color = Color.gray;
        currentGroundTag = FindMostFrequent(groundTags);
        //UpdateSpeedAndForce();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        groundTags.Remove(collision.gameObject.tag);
        collision.GetComponent<SpriteRenderer>().color = Color.white;
        if (groundTags.Count == 0)
        {
            // Die
            isDying = true;
            if (collision.transform.position.x < transform.position.x)
            {
                anim.SetTrigger("deathRight");
            } else
            {
                anim.SetTrigger("deathLeft");
            }
            Destroy(gameObject, 2f);
        } else
        {
            //UpdateSpeedAndForce();
            currentGroundTag = FindMostFrequent(groundTags);
        }
        //foreach (string x in groundTags)
        //{
        //    Debug.Log(x);
        //}
        //Debug.Log("----");
    }


    void UpdateSpeedAndforce()
    {

    }

    string FindMostFrequent(List<string> list)
    {
        list.Sort();
        int max = 1;
        int count = 1;
        string currentResult = list[0];
        for (int i = 1; i < list.Count; i++)
        {
            if (!list[i].Equals(list[i - 1]))
            {
                count = 1;
            } else
            {
                count++;
            }

            if (max < count)
            {
                max = count;
                currentResult = list[i - 1];
            }
        }
        return currentResult;
    }
}
