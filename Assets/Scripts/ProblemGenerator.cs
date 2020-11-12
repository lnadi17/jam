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

    int[] GetPossibleAnswers(string op, int firstNumber, int secondNumber, int arraySize)
    {
        return null;
    }

    void DrawArithmetic(string op, Vector2Int numbers)
    {
        Transform parent = new GameObject("NumbersParent").transform;
        int count = 0;
        float xPosition = 0f;
        // Draw first
        string firstStr = numbers.x.ToString();
        foreach (char ch in firstStr)
        {
            Debug.Log(ch);
            GameObject obj = Instantiate<GameObject>(numberObject, new Vector2(xPosition, 0), Quaternion.identity, parent);
            obj.GetComponent<SpriteRenderer>().sprite = numberSprites[(int)char.GetNumericValue(ch)];
            xPosition += xOffset;
            count++;
        }
        // Draw operator
        if (op == "+")
        {
            GameObject obj = Instantiate<GameObject>(numberObject, new Vector2(xPosition, 0), Quaternion.identity, parent);
            obj.GetComponent<SpriteRenderer>().sprite = plus;
            xPosition += xOffset;
            count++;
        }
        if (op == "-")
        {
            GameObject obj = Instantiate<GameObject>(numberObject, new Vector2(xPosition, 0), Quaternion.identity, parent);
            obj.GetComponent<SpriteRenderer>().sprite = minus;
            xPosition += xOffset;
            count++;
        }
        // Draw second
        string secondStr = numbers.y.ToString();
        foreach (char ch in secondStr)
        {
            GameObject obj = Instantiate<GameObject>(numberObject, new Vector2(xPosition, 0), Quaternion.identity, parent);
            obj.GetComponent<SpriteRenderer>().sprite = numberSprites[(int)char.GetNumericValue(ch)];
            xPosition += xOffset;
            count++;

        }
        // Draw equal sign
        GameObject eq = Instantiate<GameObject>(numberObject, new Vector2(xPosition, 0), Quaternion.identity, parent);
        eq.GetComponent<SpriteRenderer>().sprite = equals;
        count++;
        parent.position = new Vector2(xOffset / 2 - count * xOffset / 2, 4.5f);
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
}
