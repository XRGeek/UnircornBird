﻿using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BirdControl : MonoBehaviour {

	public int rotateRate = 10;
	public float upSpeed = 10;
    public GameObject scoreMgr;
    public GameObject Game_Over_image;
    public GameObject Game_Over_RenderCanvas;
    public GameObject uiCamera;
    public GameObject MainMenu;
    public GameObject Restart;
    public GameObject Panel;
    public GameObject medal1;
    public GameObject medal2;
    public GameObject medal3;
    public GameObject CurrentScore;
    public GameObject HighScore;


    //public ScoreMgr scoreMgr;

    public AudioClip jumpUp;
    public AudioClip hit;
    public AudioClip score;

    public bool inGame = false;

	private bool dead = false;
	private bool landed = false;

    private Sequence birdSequence;

    // Use this for initialization
    void Start () {
        float birdOffset = 0.05f;
        float birdTime = 0.3f;
        float birdStartY = transform.position.y;

        birdSequence = DOTween.Sequence();

        birdSequence.Append(transform.DOMoveY(birdStartY + birdOffset, birdTime).SetEase(Ease.Linear))
            .Append(transform.DOMoveY(birdStartY - 2 * birdOffset, 2 * birdTime).SetEase(Ease.Linear))
            .Append(transform.DOMoveY(birdStartY, birdTime).SetEase(Ease.Linear))
            .SetLoops(-1);
    }
	
	// Update is called once per frame
	void Update () {
        if (!inGame)
        {
            return;
        }
        birdSequence.Kill();

		if (!dead)
		{
			if (Input.GetButtonDown("Fire1"))
			{
                JumpUp();
			}
		}

		if (!landed)
		{
			float v = transform.GetComponent<Rigidbody2D>().velocity.y;
			
			float rotate = Mathf.Min(Mathf.Max(-90, v * rotateRate + 60), 30);
			
			transform.rotation = Quaternion.Euler(0f, 0f, rotate);
		}
		else
		{
			transform.GetComponent<Rigidbody2D>().rotation = -90;
		}
	}
    public bool canTriggerEnter=false;
	void OnTriggerEnter2D (Collider2D other)
	{

        if (!canTriggerEnter)
            return;

        Debug.Log(other.name);
		if (other.name == "land" || other.name == "pipe_up" || other.name == "pipe_down")
		{
            if (!dead)
            {
                GameObject[] objs = GameObject.FindGameObjectsWithTag("movable");
                foreach (GameObject g in objs)
                {
                    g.BroadcastMessage("GameOver");
                }

                GetComponent<Animator>().SetTrigger("die");
                AudioSource.PlayClipAtPoint(hit, Vector3.zero);
            }

			

			if (other.name == "land")
			{
				transform.GetComponent<Rigidbody2D>().gravityScale = 0;
				transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

				landed = true;
			}
		}

        if (other.name == "pass_trigger")
        {
            scoreMgr.GetComponent<ScoreMgr>().AddScore();
            AudioSource.PlayClipAtPoint(score, Vector3.zero);
        }


	}

    public void JumpUp()
    {
        transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, upSpeed);
        AudioSource.PlayClipAtPoint(jumpUp, Vector3.zero);
    }
	
	public void GameOver()
	{

        Debug.Log("Game Over");
		dead = true;
        Game_Over_image.SetActive(true);
        Game_Over_RenderCanvas.SetActive(true);
        uiCamera.SetActive(true);
        MainMenu.SetActive(true);
        Restart.SetActive(true);
        Panel.SetActive(true);
        CurrentScore.SetActive(true);
        HighScore.SetActive(true);
       



            if (ScoreMgr.Instance.nowScore>4 && ScoreMgr.Instance.nowScore < 10)
        {
            medal1.SetActive(true);
        }

       else if (ScoreMgr.Instance.nowScore > 9 && ScoreMgr.Instance.nowScore < 15)
        {
            medal2.SetActive(true);
        }

        else if (ScoreMgr.Instance.nowScore > 14)
        {
            medal3.SetActive(true);
        }
            else
                {
            print("infinity");

        }

        ScoreMgr.Instance.SetHighScore();
    }
}
