using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemGenerator : MonoBehaviour
{

    private enum Operator
    {
        PLUS,
        MINUS
    }

    public static readonly int MIN_SUM = 20;
    public static readonly int MAX_SUM = 20;
    public static readonly int TIMER_SECS = 10;

    public enum Type { Arithmetic, Equation };
    public GameObject numberObject;
    public Sprite[] numberSprites;
    public Sprite[] timerSprites;
    public Sprite plus;
    public Sprite minus;
    public Sprite equals;
    public float xOffset;
    private List<GameObject> numbersPresentObjects;
    private GameObject timerObject;
    private int secondsPassed;
    private List<Vector2> numbersPresentRangesX;
    private List<Vector2> otherRangesX;
    private List<Vector2> numbersPresentRangesY;
    private List<Vector2> otherRangesY;
    private List<GameObject> answerObjects;

    // Start is called before the first frame update
    void Start()
    {
        answerObjects = new List<GameObject>() {
            GameObject.FindGameObjectWithTag("A_1"),
            GameObject.FindGameObjectWithTag("A_2"),
            GameObject.FindGameObjectWithTag("A_3"),
        };
        numbersPresentRangesX = new List<Vector2>();
        otherRangesX = new List<Vector2>();
        numbersPresentRangesY = new List<Vector2>();
        otherRangesY = new List<Vector2>();
        numbersPresentObjects = new List<GameObject>();
        timerObject = new GameObject("NumbersPresent");
        secondsPassed = 0;
        drawObjects();
        InvokeRepeating("DrawAll", 0, 1.0f);
    }

    void DrawAll()
    {
        if (secondsPassed == TIMER_SECS)
        {
            foreach (GameObject numbersPresentObjcet in numbersPresentObjects)
            {
                Destroy(numbersPresentObjcet);
            }
            numbersPresentObjects.Clear();
            numbersPresentRangesX.Clear();
            numbersPresentRangesY.Clear();
            otherRangesX.Clear();
            otherRangesY.Clear();
            secondsPassed = 0;
            drawObjects();
        } else
        {
            secondsPassed++;
        }
        drawTimer();
    }

    void drawObjects()
    {
        Operator op = getRandomOperator();
        Vector2Int numbers = new Vector2Int(0, 0);
        switch (op)
        {
            case Operator.PLUS:
                numbers = AdditionProblem(MIN_SUM, MAX_SUM);
                break;
            case Operator.MINUS:
                numbers = SubtractionProblem(MIN_SUM, MAX_SUM);
                break;
        }
        DrawArithmetic(op, numbers);

        HashSet<int> possibleAnswers = GetPossibleAnswers(op, numbers, 3);
        int index = 0;
        foreach (int number in possibleAnswers)
        {
            DrawNumber(number, index++);
        }
    }

    HashSet<int> GetPossibleAnswers(Operator op, Vector2Int numbers, int arraySize)
    {
        HashSet<int> result = new HashSet<int>();
        int resultNum = 0;
        switch (op) {
            case Operator.PLUS:
                resultNum = numbers.x + numbers.y;
                break;
            case Operator.MINUS:
                resultNum = numbers.x - numbers.y;
                break;
        }
        result.Add(resultNum);
        while (result.Count != arraySize)
        {
            result.Add(Random.Range(resultNum - 7, resultNum + 8));
        }
        return result;
    }

    private Operator getRandomOperator()
    {
        int len = 2;
        return (Operator) Random.Range(0, len);
    }

    void DrawArithmetic(Operator op, Vector2Int numbers)
    {
        GameObject numbersPresent = new GameObject("NumbersPresent");
        Transform parent = numbersPresent.transform;
        int count = 0;
        float xPosition = 0f;
        // Draw first
        string firstStr = numbers.x.ToString();
        foreach (char ch in firstStr)
        {
            GameObject obj = Instantiate<GameObject>(numberObject, new Vector2(xPosition, 0), Quaternion.identity, parent);
            obj.GetComponent<SpriteRenderer>().sprite = numberSprites[(int)char.GetNumericValue(ch)];
            xPosition += xOffset;
            count++;
        }
        // Draw operator
        if (op == Operator.PLUS)
        {
            GameObject obj = Instantiate<GameObject>(numberObject, new Vector2(xPosition, 0), Quaternion.identity, parent);
            obj.GetComponent<SpriteRenderer>().sprite = plus;
            xPosition += xOffset;
            count++;
        }
        if (op == Operator.MINUS)
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
        otherRangesX.Add(new Vector2(parent.position.x, parent.position.x + count * xOffset));
        otherRangesY.Add(new Vector2(parent.position.y, parent.position.y + numberSprites[0].bounds.size.x));
        numbersPresentObjects.Add(numbersPresent);
    }

    void DrawNumber(int number, int index)
    {
        GameObject numbersPresent = new GameObject("NumbersPresent");
        Transform parent = numbersPresent.transform;
        float xPosition = 0f;
        int count = 0;
        if (number < 0)
        {
            GameObject obj = Instantiate<GameObject>(numberObject, new Vector2(xPosition, 0), Quaternion.identity, parent);
            obj.GetComponent<SpriteRenderer>().sprite = minus;
            obj.GetComponent<SpriteRenderer>().sortingOrder = 10;
            xPosition += xOffset;
            count++;
            number = -number;
        }
        foreach (char ch in number.ToString())
        {
            GameObject obj = Instantiate<GameObject>(numberObject, new Vector2(xPosition, 0), Quaternion.identity, parent);
            obj.GetComponent<SpriteRenderer>().sprite = numberSprites[(int)char.GetNumericValue(ch)];
            obj.GetComponent<SpriteRenderer>().sortingOrder = 10;
            xPosition += xOffset;
            count++;
        }
        parent.position = new Vector2(answerObjects[index].transform.position.x, answerObjects[index].transform.position.y);
        //numbersPresentRangesX.Add(new Vector2(parent.position.x, parent.position.x + count * xOffset));
        //numbersPresentRangesY.Add(new Vector2(parent.position.y, parent.position.y + numberSprites[0].bounds.size.x));
        numbersPresentObjects.Add(numbersPresent);
    }

    void drawTimer()
    {
        Destroy(timerObject);
        timerObject = new GameObject("NumbersPresent");
        Transform parent = timerObject.transform;
        float xPosition = 0f;
        int count = 0;
        foreach (char ch in (TIMER_SECS - secondsPassed).ToString())
        {
            GameObject obj = Instantiate<GameObject>(numberObject, new Vector2(xPosition, 0), Quaternion.identity, parent);
            obj.GetComponent<SpriteRenderer>().sprite = timerSprites[(int)char.GetNumericValue(ch)];
            obj.GetComponent<SpriteRenderer>().sortingOrder = 10;
            xPosition += xOffset;
            count++;
        }
        parent.position = new Vector2(xOffset / 2 - count * xOffset / 2, -4.5f);
        otherRangesX.Add(new Vector2(parent.position.x, parent.position.x + count * xOffset));
        otherRangesY.Add(new Vector2(parent.position.y, parent.position.y + numberSprites[0].bounds.size.x));
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
