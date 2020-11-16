using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    private void Start()
    {
        Instantiate(player, transform.position, Quaternion.identity);
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (ProblemGenerator.gameOver)
            {
                // do nothing
            } else if (GameObject.Find("Player(Clone)") == null)
            {
                Debug.Log("Before Spawn");
                Instantiate(player, transform.position, Quaternion.identity);
                Debug.Log("After Spawn");
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
