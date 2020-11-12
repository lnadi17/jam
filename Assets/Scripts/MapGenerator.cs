using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject hexagon;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 dimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        for (int j = 1; j < 5 * dimensions.y + 3; j++)
        {
            for (int i = 0; i < Mathf.CeilToInt((dimensions.x)) + Mathf.CeilToInt((dimensions.x / 2)); i++)
            {
                Instantiate(hexagon, new Vector3((j % 2 != 0 ? 0.7f : 0) - dimensions.x + i * 1.4f, dimensions.y - j * 0.4f, 0), Quaternion.identity);
            }
        }
    }
}
