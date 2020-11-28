using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager reference;
    public GameObject foodPrefab;
    public float margin = 1f;
    public Vector3 minScreenPoint;
    public Vector3 maxScreenPoint;
    public int score;
    public int hightestScore;
    public GameObject gameOverPanel;
    public Text scoreText;
    public Text highScoreText;
    [System.NonSerialized] public AudioSource audioSource;
    void Awake()
    {
        reference = this;
        scoreText.text = "Score : " + score.ToString();
        audioSource = GetComponent <AudioSource>();
        LoadData();
        SpawnFood();
    }
    
   
    void Update()
    {
        // if(Input.GetButtonDown("f"))
        //     SpawnFood();
    }
    void CalculateScreenBoundaries()
    {
        Vector3 minPoint = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 maxPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        minScreenPoint = VectorToIntVector(minPoint);
        maxScreenPoint = VectorToIntVector(maxPoint);
    }
    Vector3 VectorToIntVector(Vector3 point)
    {
        return new Vector3((int)point.x, (int)point.y, (int)point.z);
    }
    public void SpawnFood()
    {
        Vector3 spawnPoint = Vector3.zero;
        CalculateScreenBoundaries();
        do{
            spawnPoint = new Vector3( Random.Range((int)(minScreenPoint.x + margin), (int)(maxScreenPoint.x - margin)), 0,
                                      Random.Range((int)(minScreenPoint.z + margin), (int)(maxScreenPoint.z - margin)));
        }while(Physics.Raycast(spawnPoint + Vector3.up * 10, Vector3.down));
        
        Instantiate(foodPrefab, spawnPoint, Quaternion.identity);
    }
    public void SessionEnd()
    {
        if(score > hightestScore){
            hightestScore = score;
            PlayerPrefs.SetInt("HighestScore", hightestScore);
        }  
        gameOverPanel.SetActive(true);   
        highScoreText.text = "Highest Score  " + hightestScore.ToString();
    }
    void LoadData()
    {
        hightestScore = PlayerPrefs.GetInt("HighestScore");
        highScoreText.text = "Highest Score  " + hightestScore.ToString();
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
