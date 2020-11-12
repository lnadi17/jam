using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemGenerator : MonoBehaviour
{
    public enum Type { Arithmetic, Equation };
    public GameObject numberObject;
    public Sprite[] numberSprites;
    public Sprite plus;
    public Sprite minus;
    public Sprite equals;
    public float xOffset;

    // Start is called before the first frame update
    void Start()
    {
        DrawArithmetic("+", AdditionProblem(20, 20));
    }

    void DrawArithmetic(string op, Vector2Int numbers)
    {
        float xPosition = 0f;

        // Draw first
        string firstStr = numbers.x.ToString();
        foreach (char ch in firstStr)
        {
            Debug.Log(ch);
            GameObject obj = Instantiate<GameObject>(numberObject, new Vector2(xPosition, 0), Quaternion.identity);
            Debug.Log((int)char.GetNumericValue(ch));
            obj.GetComponent<SpriteRenderer>().sprite = numberSprites[(int)char.GetNumericValue(ch)];
            xPosition += xOffset;
        }
        // Draw operator
        if (op == "+")
        {
            GameObject obj = Instantiate<GameObject>(numberObject, new Vector2(xPosition, 0), Quaternion.identity);
            obj.GetComponent<SpriteRenderer>().sprite = plus;
            xPosition += xOffset;
        }
        if (op == "-")
        {
            GameObject obj = Instantiate<GameObject>(numberObject, new Vector2(xPosition, 0), Quaternion.identity);
            obj.GetComponent<SpriteRenderer>().sprite = minus;
            xPosition += xOffset;
        }
        // Draw second
        string secondStr = numbers.y.ToString();
        foreach (char ch in secondStr)
        {
            Debug.Log(ch);
            GameObject obj = Instantiate<GameObject>(numberObject, new Vector2(xPosition, 0), Quaternion.identity);
            Debug.Log((int)char.GetNumericValue(ch));
            obj.GetComponent<SpriteRenderer>().sprite = numberSprites[(int)char.GetNumericValue(ch)];
            xPosition += xOffset;
        }
    }

    Vector2Int AdditionProblem(int minSum, int maxSum)
    {
        int sum = Random.Range(minSum, maxSum);
        int firstNumber = Random.Range(0, sum + 1);
        int secondNumber = sum - firstNumber;
        return new Vector2Int(firstNumber, secondNumber);
    }

    Vector2Int SubtractionProblem(int minFirst, int maxFirst)
    {
        Vector2Int temp = AdditionProblem(minFirst, maxFirst);
        int firstNumber = temp.x + temp.y;
        int secondNumber = temp.x;
        return new Vector2Int(firstNumber, secondNumber);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
