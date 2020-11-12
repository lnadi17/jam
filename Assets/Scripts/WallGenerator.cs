using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    public GameObject wall;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 dimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        GameObject bottomWall = Instantiate<GameObject>(wall, new Vector3(0, -dimensions.y - 0.5f, 0), Quaternion.identity);
        GameObject topWall = Instantiate<GameObject>(wall, new Vector3(0, dimensions.y + 0.5f, 0), Quaternion.identity);
        GameObject leftWall = Instantiate<GameObject>(wall, new Vector3(-dimensions.x - 0.5f, 0, 0), Quaternion.identity);
        GameObject rightWall = Instantiate<GameObject>(wall, new Vector3(dimensions.x + 0.5f, 0, 0), Quaternion.identity);
        bottomWall.transform.localScale = new Vector3(2 * dimensions.x, 1);
        topWall.transform.localScale = new Vector3(2 * dimensions.x, 1);
        leftWall.transform.localScale = new Vector3(1, 2 * dimensions.y);
        rightWall.transform.localScale = new Vector3(1, 2 * dimensions.y);
    }
}
