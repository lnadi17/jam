using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Color correctColor;
    public Color incorrectColor;

    private Text correctText;
    private Text incorrectText;

    private float speed = 10;
    private float force = 100;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private bool lookinLeft = false;
    private float forceX = 0, forceY = 0;
    private bool isDying = false;
    private ProblemGenerator problemGenerator;
    private GameObject progressBarObject;
    private GameObject currentProgressBar;

    private string currentGroundTag = "Untagged";
    private string lastGroundTag = "Untagged";
    private List<string> groundTags = new List<string>();
    private bool ignoreTriggerExit = false;
    public static int correctAns = 0, incorrectAns = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        problemGenerator = GameObject.Find("Generator").GetComponent<ProblemGenerator>();
        transform.up = new Vector2(0, 0);
        correctText = GameObject.Find("Correct_Score").GetComponent<Text>();
        incorrectText = GameObject.Find("Incorrect_Score").GetComponent<Text>();
        progressBarObject = GameObject.Find("ProgressBar");
    }

    // Update is called once per frame
    void Update()
    {
        forceX = 0;
        forceY = 0;
        if (isDying) { rb.velocity = Vector2.zero; return; }
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
        string otherTag = collision.gameObject.tag;
        if (otherTag == "A_1" || otherTag == "A_2" || otherTag == "A_3")
        {
            ignoreTriggerExit = true;
            if (problemGenerator.correctTag == otherTag)
            {
                SoundManagerScript.PlaySound("score");
                correctAns++;
                correctText.text = correctAns.ToString();
                UpdateLevelProgress(true);
            }
            else
            {
                incorrectAns++;
                Debug.Log(incorrectAns);
                incorrectText.text = incorrectAns.ToString();
                SoundManagerScript.PlaySound("score");
                UpdateLevelProgress(false);
            }
            problemGenerator.secondsPassed = problemGenerator.timerSeconds;
            problemGenerator.DrawAll();
            Destroy(gameObject);
            Debug.Log("After destroy");
            return;
        }
        groundTags.Add(collision.gameObject.tag);
        //collision.GetComponent<SpriteRenderer>().color = Color.gray;
        lastGroundTag = currentGroundTag;
        currentGroundTag = FindMostFrequent(groundTags);
        if (currentGroundTag != lastGroundTag) UpdateSpeedAndForce();
    }

    // Draws correct level bar if passed true
    private void UpdateLevelProgress(bool isCorrect)
    {
        if (ProblemGenerator.number_of_question == 1)
        {
            progressBarObject.SetActive(true);
            // 3 is the width of the board
            progressBarObject.transform.localScale = new Vector2(3f / problemGenerator.numberOfProblems, 1);
            currentProgressBar = progressBarObject;
        } else
        {
            Vector2 newPosition = new Vector2(progressBarObject.transform.position.x + progressBarObject.transform.localScale.x * (ProblemGenerator.number_of_question - 1), 
                                              progressBarObject.transform.position.y);
            currentProgressBar = Instantiate(progressBarObject, newPosition, Quaternion.identity);
            currentProgressBar.transform.localScale = new Vector2(3f / problemGenerator.numberOfProblems, 1);
        }
        if (isCorrect)
        {
            currentProgressBar.GetComponent<SpriteRenderer>().color = correctColor;
        }
        else
        {
            currentProgressBar.GetComponent<SpriteRenderer>().color = incorrectColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string otherTag = collision.gameObject.tag;
        if (ignoreTriggerExit) {
            return;
        }
        groundTags.Remove(collision.gameObject.tag);
        //collision.GetComponent<SpriteRenderer>().color = Color.white;
        if (groundTags.Count == 0) {
            // Die
            UpdateLevelProgress(false);
            SoundManagerScript.PlaySound("death");
            isDying = true;
            if (collision.transform.position.x < transform.position.x)
            {
                anim.SetTrigger("deathRight");
            } else
            {
                anim.SetTrigger("deathLeft");
            }
            Destroy(gameObject, 2f);
            incorrectAns++;
            incorrectText.text = incorrectAns.ToString();
            Debug.Log("Increased Incorrect Answer");
            Invoke(nameof(ResetGame), 1.5f);
        } else
        {
            lastGroundTag = currentGroundTag;
            currentGroundTag = FindMostFrequent(groundTags);
            if(lastGroundTag != currentGroundTag) UpdateSpeedAndForce();
        }
        //foreach (string x in groundTags)
        //{
        //    Debug.Log(x);
        //}
        //Debug.Log("----");
    }

    void UpdateSpeedAndForce()
    {
        if (currentGroundTag == "Grass_1")
        {
            speed = 2.5f;
            force = 100;
            rb.drag = 10;
        }
        else if (currentGroundTag == "Grass_4") 
        {
            speed = 0.5f;
            force = 50;
            rb.drag = 20;
        }
        else if (currentGroundTag == "Grass_7" || currentGroundTag == "Grass_8")
        {
            speed = 1.6f;
            force = 30;
            rb.drag = 0.1f;
        }
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

    void ResetGame()
    {
        problemGenerator.secondsPassed = problemGenerator.timerSeconds;
        problemGenerator.DrawAll();
    }
}
