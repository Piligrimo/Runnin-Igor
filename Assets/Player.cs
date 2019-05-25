using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
public class Player : MonoBehaviour {
    float BazShotDelay=-1;
    public GameObject Rocket, bullet,inGameUI,menu,cam;
    int rocketCount = 3, bulletCount=5,health=5;
    Vector3 RocketSpot = new Vector3(1.6f, 0.9f, 0),bulletSpot = new Vector3(-2f, 1, 0);
    Rigidbody2D rb;
    public Text rocketLabel, bulletLabel, healthLabel, finalScore,deathLabel;
    AudioSource au;
    public AudioClip jumpSound, bonusSound;
    public void NewGame()
    {
        transform.position = new Vector3(-3.7f, 10, 0);
        menu.SetActive(false);
        rocketCount = 3;
        bulletCount = 5;
        health = 5;
        healthLabel.text = health.ToString();
        bulletLabel.text = bulletCount.ToString();
        rocketLabel.text = rocketCount.ToString();
        inGameUI.SetActive(true);
        cam.GetComponent<Blur>().enabled = false;
        cam.GetComponent<Grayscale>().enabled = false;
        cam.GetComponent<Mover>().score = 0;
        cam.GetComponent<Mover>().scoreLabel.text = "0";

    }
    
  

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = 1;
        deathLabel.text = PlayerPrefs.GetInt("Deaths",0).ToString();
        au = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (health==0)
        {
            PlayerPrefs.SetInt("Deaths", PlayerPrefs.GetInt("Deaths",0) + 1);
            deathLabel.text = PlayerPrefs.GetInt("Deaths").ToString();
            if (cam.GetComponent<Mover>().score > PlayerPrefs.GetInt("Highscore",0))
                PlayerPrefs.SetInt("Highscore", cam.GetComponent<Mover>().score);
            inGameUI.SetActive(false);
            cam.GetComponent<Blur>().enabled=true;
            cam.GetComponent<Grayscale>().enabled = true;
            menu.SetActive(true);
            finalScore.text= "Ты проиграл!\nТвой результат\n"+ cam.GetComponent<Mover>().score.ToString()+ "\nРекорд\n"+ PlayerPrefs.GetInt("Highscore", 0).ToString();
            cam.GetComponent<Mover>().score = 0;
            cam.GetComponent<Mover>().scoreLabel.text = "0";           
            gameObject.SetActive(false);
            Time.timeScale = 1;

        }
		if (Input.GetKeyDown(KeyCode.UpArrow) && rb.velocity.y==0)
        {
            au.clip = jumpSound;
            au.Play();
            rb.velocity = new Vector2(0, 9f);
        }
        if (Input.GetKeyDown(KeyCode.Z) && rb.velocity.y>-0.5 && rb.velocity.y < 0.5 && rocketCount>0 && BazShotDelay<0)
        {
            GetComponent<Animator>().SetTrigger("BazShot");
            BazShotDelay = 0;            

        }
        if (Input.GetKeyDown(KeyCode.X) && rb.velocity.y > -0.5 && rb.velocity.y < 0.5 && bulletCount > 0)
        {
            GetComponent<Animator>().SetTrigger("AkShot");
            bulletCount--;
            bulletLabel.text = bulletCount.ToString();
            Instantiate(bullet, transform.position + bulletSpot, Quaternion.Euler(0, 0, 0));

        }
        if (BazShotDelay >= 0)
        {
            BazShotDelay += Time.deltaTime;

            if (BazShotDelay > 0.75)
            {
                Instantiate(Rocket, transform.position+RocketSpot, Quaternion.Euler(0,0,0));
                BazShotDelay = -1;
                rocketCount--;
                rocketLabel.text = rocketCount.ToString();
            }
        }


       GetComponent<Animator>().SetFloat("VertSpeed", GetComponent<Rigidbody2D>().velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("RocketItem"))
        {
            other.gameObject.transform.parent.gameObject.SetActive(false);
            rocketCount++;
            rocketLabel.text = rocketCount.ToString();
            au.clip = bonusSound;
            au.Play();
        }
        if (other.gameObject.CompareTag("BulletItem"))
        {
            other.gameObject.transform.parent.gameObject.SetActive(false);
            bulletCount++;
            bulletLabel.text = bulletCount.ToString();
            au.clip = bonusSound;
            au.Play();
        }
        if (other.gameObject.CompareTag("HealthItem"))
        {
            other.gameObject.transform.parent.gameObject.SetActive(false);
            health++;
            healthLabel.text = health.ToString();
            au.clip = bonusSound;
            au.Play();
        }
        if (other.gameObject.CompareTag("Wall")|| other.gameObject.CompareTag("Wood")|| other.gameObject.CompareTag("Brick"))
        {
            other.gameObject.GetComponent<Wall>().FX.SetActive(true);          
            other.gameObject.SetActive(false);
            health--;
            healthLabel.text = health.ToString();
        }
    }
}
