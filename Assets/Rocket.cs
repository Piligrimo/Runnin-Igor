using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
    Rigidbody2D rb;
    public GameObject FX;
    public bool isRocket;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        if (isRocket)
            rb.velocity = new Vector2(6, 0);
        else
            rb.velocity = new Vector2(10, 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isRocket)
        {
            if (other.gameObject.CompareTag("Wood") || other.gameObject.CompareTag("Brick"))
            {
                Instantiate(FX, transform.position, transform.rotation);
                other.gameObject.GetComponent<Wall>().FX.SetActive(true);
                other.gameObject.SetActive(false);
                Destroy(gameObject);

            }
        }
        else
        {
            if (other.gameObject.CompareTag("Wood") )
            {
                other.gameObject.GetComponent<Wall>().FX.SetActive(true);
                other.gameObject.SetActive(false);
                Destroy(gameObject);

            }
            if (other.gameObject.CompareTag("Brick"))
                Destroy(gameObject);
        }
    }
}
