using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnakeContoller : MonoBehaviour
{
    [SerializeField]
    private GameObject foodPrefab, tailPrefab;
    private GameObject food;
    private float stepRate, currentAngleZ;
    private Vector2 move;
    private List<Transform> tail = new List<Transform>();
    private bool isFoodEaten;
    private Vector2 lBorderPos, rBorderPos, uBorderPos, dBorderPos;

    private GameController gameController;
    private bool isGameOver, canTurn;
    private float turnTime;

    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        turnTime = Time.time;
        isGameOver = false;

        currentAngleZ = 0.0f;
        stepRate = 0.3f;
        isFoodEaten = false;
        lBorderPos = GameObject.Find("WallL").transform.position;
        rBorderPos = GameObject.Find("WallR").transform.position;
        uBorderPos = GameObject.Find("WallU").transform.position;
        dBorderPos = GameObject.Find("WallD").transform.position;
        InvokeRepeating("Movement", 0.1f, stepRate);
    }

    private void SpawnFood()
    {
        float x = (int)Random.Range(lBorderPos.x + 2f, rBorderPos.x - 2f);
        float y = (int)Random.Range(dBorderPos.y + 2f, uBorderPos.y - 2f);
        food = Instantiate(foodPrefab, new Vector3(x, y, 5f), Quaternion.identity);
    }

    private void Movement()
    {
        Vector2 v = transform.position;
        transform.Translate(move);
        if (isFoodEaten)
        {
            GameObject g = Instantiate(tailPrefab, v, Quaternion.identity);
            g.GetComponent<Renderer>().material.color = this.GetComponent<Renderer>().material.color;
            tail.Insert(0, g.transform);
            isFoodEaten = false;
        }
        else if (tail.Count > 0)
        {
            tail.Last().position = v;
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
    }

    private void SetDirection()
    {
        if(Input.GetKeyDown(KeyCode.D) && currentAngleZ != 90.0f)
        {
            currentAngleZ = 270.0f;
        }
        else if (Input.GetKeyDown(KeyCode.A) && currentAngleZ != 270.0f)
        {
            currentAngleZ = 90.0f;
        }
        else if (Input.GetKeyDown(KeyCode.W) && currentAngleZ != 180.0f)
        {
            currentAngleZ = 0.0f;
        }
        else if (Input.GetKeyDown(KeyCode.S) && currentAngleZ != 0.0f)
        {
            currentAngleZ = 180.0f;
        }
    }

    private void SnakeBehaviour(float prevAngleZ)
    {
        if (canTurn)
        {
            SetDirection();
        }
        transform.localEulerAngles =new Vector3(0f, 0f, currentAngleZ);
        if(Time.time > turnTime + 0.2f) 
        {
            canTurn = true;
        }
        if(prevAngleZ != currentAngleZ)
        {
            turnTime = Time.time;
            canTurn = false;
        }
    }

    private void Restart()
    {
        foreach (Transform tailChunk in tail)
        {
            Destroy(tailChunk);
        }
        tail.Clear();
        transform.position = Vector2.zero;
        gameController.score = 0;
    }

    private void Update()
    {
        SnakeBehaviour(currentAngleZ);
        if(Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space))
        {
            move = Vector2.up;
        }
        if(food == null)
        {
            SpawnFood();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Food")
        {
            Destroy(col.gameObject);
            isFoodEaten = true;
            SpawnFood();
            gameController.score++;
        }
        if (col.gameObject.name == "WallR" && currentAngleZ == 270.0f)
        {
            transform.position = new Vector2(lBorderPos.x, transform.position.y);
        }
        else if (col.gameObject.name == "WallL" && currentAngleZ == 90.0f)
        {
            transform.position = new Vector2(rBorderPos.x, transform.position.y);
        }
        else if (col.gameObject.name == "WallU" && currentAngleZ == 0.0f)
        {
            transform.position = new Vector2(transform.position.x, dBorderPos.y);
        }
        else if (col.gameObject.name == "WallD" && currentAngleZ == 180.0f)
        {
            transform.position = new Vector2(transform.position.x, uBorderPos.y);
        }
        if (col.gameObject.tag == "Tail")
        {
            Restart();
        }
    }
}
