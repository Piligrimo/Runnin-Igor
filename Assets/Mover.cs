using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Mover : MonoBehaviour {
    public GameObject[] backGrounds;
    public Sprite rocketSprite, bulletSprite,hpSprite;
    public GameObject player;
    public int score =-2;
    public Text scoreLabel;
    void BonusPlace(Bg bg)
    {
        for (int j = 0; j < 3; j++)
        {
            bg.Bonuses[j].SetActive(false);
        }
        float Dice = Random.value * 80;
        if (Dice < 20)
        {
            bg.Bonuses[0].SetActive(true);
        }
        if (Dice > 15 && Dice < 35)
        {
            bg.Bonuses[1].SetActive(true);
        }
        if (Dice > 30 && Dice < 50)
        {
            bg.Bonuses[2].SetActive(true);
        }
    }

    void BonusType(Bg bg)
    {
        for (int i=0;i< bg.Bonuses.Length;i++)
        {
            SpriteRenderer sr = bg.Bonuses[i].GetComponentInChildren<SpriteRenderer>();
            float   Dice = Random.value * 10;
            if (Dice > 5)
            {
               
                sr.sprite = bulletSprite;
                sr.gameObject.tag = "BulletItem";
            }
            else
            {
                if (Dice < 2)
                {
                    sr.sprite = hpSprite;
                    sr.gameObject.tag = "HealthItem";
                   
                }
                else
                {
                    sr.sprite =rocketSprite;
                    sr.gameObject.tag = "RocketItem";
                }
            }
        }
    }
	// Use this for initialization
	void Start () {
        for (int i = 0; i < backGrounds.Length; i++)
             backGrounds[i].GetComponent<Rigidbody2D>().velocity = new Vector2(-4, 0);
        
    }
	
	// Update is called once per frame
	void Update () {
        
        for (int i = 0; i < backGrounds.Length; i++)
        {
            if (backGrounds[i].transform.position.x < player.transform.position.x && !backGrounds[i].GetComponent<Bg>().completed)
            {
                backGrounds[i].GetComponent<Bg>().completed = true;
                score++;
                scoreLabel.text = score.ToString();
                Debug.Log(Time.timeScale);
                if (score < 10000)
                    Time.timeScale = 1 + Mathf.Sqrt(Mathf.Max(score * .005f, 0));
                else
                    Time.timeScale = 11;
            }
            if (backGrounds[i].transform.position.x < -10)
            {
                backGrounds[i].transform.position = new Vector3(backGrounds[(i + 3) % 4].transform.position.x + 7.67f, 0.07f, 0);
                backGrounds[i].GetComponent<Bg>().completed = false;
                foreach (var item in backGrounds[i].GetComponent<Bg>().Walls)
                    item.GetComponent<Wall>().FX.SetActive(false);
                float[] chances = { 10, 20, 50, 20 };
                float Dice = Random.value * (chances[0] + chances[1] + chances[2] + chances[3]);
                for (int j = 0; j < 3; j++)
                {
                    backGrounds[i].GetComponent<Bg>().Walls[j].SetActive(false);
                }
                if (Dice < chances[0])
                    backGrounds[i].GetComponent<Bg>().Walls[2].SetActive(true);
                if (Dice >= chances[0] && Dice < chances[0] + chances[1])
                    backGrounds[i].GetComponent<Bg>().Walls[1].SetActive(true);
                if (Dice >= chances[0] + chances[1] && Dice < chances[0] + chances[1] + chances[2])
                {
                    backGrounds[i].GetComponent<Bg>().Walls[0].SetActive(true);
                    BonusPlace(backGrounds[i].GetComponent<Bg>());
                    BonusType(backGrounds[i].GetComponent<Bg>());
                }

            }
        }
	}
}
