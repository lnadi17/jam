using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
    [Header("Normal Settings (Speed/Force/Drag)")]
    public Vector3 normalSettings;
    [Header("Slow Settings (Speed/Force/Drag)")]
    public Vector3 slowSettings;
    [Header("Slippery Settings (Speed/Force/Drag)")]
    public Vector3 slipperySettings;

    public Color correctColor;
    public Color incorrectColor;

    private Text correctText;
    [HideInInspector]
    public Text incorrectText;

    private float speed = 10;
    private float force = 100;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private bool lookinLeft = false;
    private float forceX = 0, forceY = 0;
    [HideInInspector]
    public static bool isDying = false;
    private ProblemGenerator problemGenerator;
    private GameObject progressBarObject;
    private GameObject currentProgressBar;

    private string currentGroundTag = "Untagged";
    private string lastGroundTag = "Untagged";
    private List<string> groundTags = new List<string>();
    [HideInInspector]
    public bool ignoreTriggerExit = false;
    public static int correctAns = 0, incorrectAns = 0;
    private Animator curtainAnimator;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        problemGenerator = GameObject.Find("Generator").GetComponent<ProblemGenerator>();
        transform.up = new Vector2(0, 0);
        correctText = GameObject.Find("Correct_Score").GetComponent<Text>();
        incorrectText = GameObject.Find("Incorrect_Score").GetComponent<Text>();
        progressBarObject = GameObject.Find("ProgressBar");
        curtainAnimator = GameObject.Find("Curtain").GetComponent<Animator>();
        isDying = false;
        //problemGenerator.DrawLevel();
        //problemGenerator.secondsPassed = 0;
    }

    // Update is called once per frame
    void Update() {
        forceX = 0;
        forceY = 0;
        if (isDying) { rb.velocity = Vector2.zero; return; }

        // Code for keyboard

        // if (Input.GetKey(KeyCode.DownArrow) && rb.velocity.y > -speed) {
        //     forceY -= force;
        // }
        // if (Input.GetKey(KeyCode.UpArrow) && rb.velocity.y < speed) {
        //     forceY += force;
        // }
        // if (Input.GetKey(KeyCode.LeftArrow) && rb.velocity.x > -speed) {
        //     lookinLeft = true;
        //     forceX -= force;
        // }
        // if (Input.GetKey(KeyCode.RightArrow) && rb.velocity.x < speed) {
        //     lookinLeft = false;
        //     forceX += force;
        // }
        // forceX *= 0.5f;
        // forceY *= 0.5f;

        // Code For accelerometer

        Vector3 dir = Input.acceleration;

        // clamp acceleration vector to unit sphere
        if (dir.sqrMagnitude > 1)
           dir.Normalize();

        if (dir.y < 0 && rb.velocity.y > -speed) {
           forceY = force * dir.y;
        }
        if (dir.y > 0 && rb.velocity.y < speed) {
           forceY = force * dir.y;
        }
        if (dir.x < 0 && rb.velocity.x > -speed) {
           lookinLeft = true;
           forceX = force * dir.x;
        }
        if (dir.x > 0 && rb.velocity.x < speed) {
           lookinLeft = false;
           forceX = force * dir.x;
        }
    }

    private void FixedUpdate() {
        rb.AddForce(new Vector2(forceX, forceY));
        if (rb.velocity.y > speed)
            rb.velocity = new Vector2(rb.velocity.x, speed);
        else if (rb.velocity.y < -speed)
            rb.velocity = new Vector2(rb.velocity.x, -speed);

        if (rb.velocity.x > speed)
            rb.velocity = new Vector2(speed, rb.velocity.y);
        else if (rb.velocity.x < -speed)
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        sr.flipX = lookinLeft;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        string otherTag = collision.gameObject.tag;
        if (otherTag == "A_1" || otherTag == "A_2" || otherTag == "A_3") {
            isDying = true;
            ignoreTriggerExit = true;
            if (problemGenerator.correctTag == otherTag) {
                SoundManagerScript.PlaySound("score");
                correctAns++;
                correctText.text = correctAns.ToString();
                UpdateLevelProgress(true);
            } else {
                incorrectAns++;
                incorrectText.text = incorrectAns.ToString();
                SoundManagerScript.PlaySound("score");
                UpdateLevelProgress(false);
            }
            if (ProblemGenerator.number_of_question == problemGenerator.numberOfProblems) {
                Invoke("Fall", 2f);
                sr.enabled = false;
                transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            } else {
                Fall();
            }
        } else {
            groundTags.Add(collision.gameObject.tag);
            // collision.GetComponent<SpriteRenderer>().color = Color.gray; //
            lastGroundTag = currentGroundTag;
            currentGroundTag = FindMostFrequent(groundTags);
            if (currentGroundTag != lastGroundTag)
                UpdateSpeedAndForce();
        }
    }

    // Draws correct level bar if passed true
    public void UpdateLevelProgress(bool isCorrect) {
        if (ProblemGenerator.number_of_question == 1) {
            progressBarObject.SetActive(true);
            // 3 is the width of the board
            progressBarObject.transform.localScale = new Vector2(3f / problemGenerator.numberOfProblems, 1);
            currentProgressBar = progressBarObject;
        } else {
            Vector2 newPosition = new Vector2(progressBarObject.transform.position.x + progressBarObject.transform.localScale.x * (ProblemGenerator.number_of_question - 1),
                                              progressBarObject.transform.position.y);
            currentProgressBar = Instantiate(progressBarObject, newPosition, Quaternion.identity);
            currentProgressBar.transform.localScale = new Vector2(3f / problemGenerator.numberOfProblems, 1);
        }
        if (isCorrect) {
            currentProgressBar.GetComponent<SpriteRenderer>().color = correctColor;
        } else {
            currentProgressBar.GetComponent<SpriteRenderer>().color = incorrectColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (ignoreTriggerExit) {
            return;
        }
        groundTags.Remove(collision.gameObject.tag);
        collision.GetComponent<SpriteRenderer>().color = Color.white; //
        if (groundTags.Count == 0) {
            isDying = true;
            ignoreTriggerExit = true;
            if (collision.transform.position.x < transform.position.x) {
                anim.SetTrigger("deathRight");
            } else {
                anim.SetTrigger("deathLeft");
            }
            SoundManagerScript.PlaySound("death");
            PlayerMovement.incorrectAns++;
            incorrectText.text = incorrectAns.ToString();
            UpdateLevelProgress(false);
            Invoke("Fall", 2f);
        } else {
            lastGroundTag = currentGroundTag;
            currentGroundTag = FindMostFrequent(groundTags);
            if (lastGroundTag != currentGroundTag)
                UpdateSpeedAndForce();
        }
        //foreach (string x in groundTags)
        //{
        //    Debug.Log(x);
        //}
        //Debug.Log("----");
    }

    void Fall() {
        problemGenerator.secondsPassed = 0;
        ProblemGenerator.number_of_question++;
        if (ProblemGenerator.number_of_question > problemGenerator.numberOfProblems) {
            Destroy(GameObject.Find("Spawner"));
            curtainAnimator.SetTrigger("FadeOut");
            Invoke("LoadNextScene", 2f);
        } else {
            problemGenerator.DrawLevel();
            Destroy(gameObject);
        }
    }

    private void LoadNextScene() {
        Debug.Log("Invoked");
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % 6);
    }

    void UpdateSpeedAndForce() {
        if (currentGroundTag == "Normal") {
            speed = normalSettings.x;
            force = normalSettings.y;
            rb.drag = normalSettings.z;
        } else if (currentGroundTag == "Slow") {
            speed = slowSettings.x;
            force = slowSettings.y;
            rb.drag = slowSettings.z;
        } else if (currentGroundTag == "Slippery") {
            speed = slipperySettings.x;
            force = slipperySettings.y;
            rb.drag = slipperySettings.z;
        }
    }

    string FindMostFrequent(List<string> list) {
        list.Sort();
        int max = 1;
        int count = 1;
        string currentResult = list[0];
        for (int i = 1; i < list.Count; i++) {
            if (!list[i].Equals(list[i - 1])) {
                count = 1;
            } else {
                count++;
            }

            if (max < count) {
                max = count;
                currentResult = list[i - 1];
            }
        }
        return currentResult;
    }
}
