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
            if (GameObject.Find("Player(Clone)") == null)
            {
                Instantiate(player, transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
