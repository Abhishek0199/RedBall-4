using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float RespawnDelay;
    public PlayerController Player;
    public int score = 0;
    public int numberofHearts;
    private int health;
    public Image[] Hearts;
    public Text ScoreText;
    public Sprite FullHeart;
    public Sprite EmptyHeart;
    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<PlayerController>();
        health = numberofHearts;
        ScoreText.text = "SCORE: " + score + "/75";
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Respawn()
    {
        health--;
        if(health == 0)
        {
            SceneManager.LoadSceneAsync(2);
        }
        for(int i=0; i<Hearts.Length; i++)
        {
            if(i < health)
            {
                Hearts[i].sprite = FullHeart;
            }
            else{
                Hearts[i].sprite = EmptyHeart;
            }
            if(i < numberofHearts)
            {
                Hearts[i].enabled = true;
            }
            else{
                Hearts[i].enabled = false;
            }
        } 
        Player.gameObject.SetActive(false);
        Player.transform.position = Player.RespawnPoint;
        Player.gameObject.SetActive(true);
        Player.circleCollider.enabled = true;
        Player.inputDisable = false;
        Player.PlayerDied = false;
        Player.rigidBody2D.constraints = RigidbodyConstraints2D.None;
        Player.rigidBody2D.gravityScale = 1;
        Player.rigidBody2D.mass = 1;
    }

    public void UpdateScore(int n)
    {
        score = score + n;
        ScoreText.text = "SCORE: " + score + "/75";
    }
}
