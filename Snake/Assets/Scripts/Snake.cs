using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public enum Direction {Right, Left, Up, Down}
    public Direction _snakeDirection;
    public List<Transform> Bones;
    [System.NonSerialized] public Transform headBone;
    public float _stepTime = 0.4f;
    public float _nextStep = 0f;
    public bool Dead = false;
    public GameObject bonePrefab;
    void Awake()
    {
        headBone = Bones[0];
    }
    void Update()
    {
       if(!Dead) {
            DirectSnake();
            if(Time.time > _nextStep ) {
                Move();
            }
        }
    }
    void Move()
    {
        RaycastHit hit;
        if(Physics.Raycast(headBone.position, headBone.forward, out hit , 1)) {
            if(hit.transform.tag == "Snake") {
               Die();
               GameManager.reference.audioSource.Play();
               return;
            }else if(hit.transform.tag== "Food"){
                Eat(hit.transform.gameObject);
            }
        }
        for(int i = Bones.Count - 1 ; i > -1; i--) {
            Transform curBone = Bones[i];
            if( i > 0 ){
                curBone.position = Bones[i - 1].position; 
            }  
        }
        headBone.position += headBone.forward;

        Vector3 minBound = GameManager.reference.minScreenPoint;
        Vector3 maxBound = GameManager.reference.maxScreenPoint;

        if ((int)headBone.position.x > maxBound.x) {
            headBone.position = new Vector3(minBound.x, headBone.position.y, headBone.position.z);
        } else if ((int)headBone.position.x < minBound.x) {
            headBone.position = new Vector3(maxBound.x, headBone.position.y, headBone.position.z);
        } else if ((int)headBone.position.z > maxBound.z) {
            headBone.position = new Vector3(headBone.position.x, headBone.position.y, minBound.z);
        } else if ((int) headBone.position.z < minBound.z) {
            headBone.position = new Vector3(headBone.position.x, headBone.position.y, maxBound.z);
        }

        _nextStep = Time.time + _stepTime;
    }
    void DirectSnake()
    {
        if(Input.GetButtonDown("Up") && _snakeDirection != Direction.Up && _snakeDirection != Direction.Down) {
            _snakeDirection = Direction.Up;
            headBone.eulerAngles = Vector3.zero;
            Move();
        }
        else if(Input.GetButtonDown("Down") && _snakeDirection != Direction.Up && _snakeDirection != Direction.Down) {
            _snakeDirection = Direction.Down;
            headBone.eulerAngles = new Vector3(0,180,0);
            Move();
        }
        else if(Input.GetButtonDown("Right") && _snakeDirection != Direction.Right && _snakeDirection != Direction.Left) {
            _snakeDirection = Direction.Right;
            headBone.eulerAngles = new Vector3(0,90,0);
            Move();
        }
        else if(Input.GetButtonDown("Left") && _snakeDirection != Direction.Right && _snakeDirection != Direction.Left) {
            _snakeDirection = Direction.Left;
            headBone.eulerAngles = new Vector3(0,270,0);
            Move();
        }
    }
    void Eat(GameObject food)
    {
        Transform _lastBone = Bones[Bones.Count - 1];
        GameObject _newBone = Instantiate(bonePrefab, _lastBone.position - _lastBone.forward, bonePrefab.transform.rotation) as GameObject;
        _newBone.transform.parent = transform;
        Bones.Add(_newBone.transform);
        GameManager.reference.SpawnFood();
        Destroy(food);
        GameManager.reference.score++;
        GameManager.reference.scoreText.text = "Score : " + GameManager.reference.score.ToString();
    }
    void Die()
    {
        Dead = true;
        GameManager.reference.SessionEnd();
    }
}
