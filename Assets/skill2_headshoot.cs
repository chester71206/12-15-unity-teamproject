using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill2_headshoot : MonoBehaviour
{
    public GameObject particle;
    public count_score monsterlife_controller;
    public GameObject spotlight;
    LineRenderer laserLine;
    public Transform monster_position;
    float Rate =12f;
    float Timer = 12f;
    public mpbar_player mpbar_Player;
    //public gamemannager Gamemannager;
    // Start is called before the first frame update
    void Start()
    {
       laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        gamemannager gameManagerInstance = FindObjectOfType<gamemannager>();
        Timer += Time.deltaTime;
        Vector3 temp = this.transform.position;
        temp.y += 1f;
        laserLine.SetPosition(0, temp);   //為了讓射線跑到頭頂用的
        if (gameManagerInstance.player_skills.Count >= 1 && gameManagerInstance.player_skills[0] == 1)
        {
            if (Input.GetKeyDown(KeyCode.J) && Timer >= Rate && mpbar_Player.MP >= 10000)
            {
                for (int i = 0; i < 10000; i++)
                {
                    mpbar_Player.minusMP();
                }
                StartCoroutine(shoot());
            }
        }
        else if (gameManagerInstance.player_skills.Count >= 2 && gameManagerInstance.player_skills[1] == 1)
        {
            if (Input.GetKeyDown(KeyCode.K) && Timer >= Rate && mpbar_Player.MP >= 10000)
            {
                for (int i = 0; i < 10000; i++)
                {
                    mpbar_Player.minusMP();
                }
                StartCoroutine(shoot());
            }
        }
        else if (gameManagerInstance.player_skills.Count >= 3 && gameManagerInstance.player_skills[2] == 1)
        {
            if (Input.GetKeyDown(KeyCode.L) && Timer >= Rate && mpbar_Player.MP >= 10000)
            {
                for (int i = 0; i < 10000; i++)
                {
                    mpbar_Player.minusMP();
                }
                StartCoroutine(shoot());
            }
        }
    }
    IEnumerator shoot()
    {
        particle.SetActive(true);
        yield return new WaitForSeconds(1f);
        particle.SetActive(false);
        spotlight.SetActive(true);
        yield return new WaitForSeconds(5f);
        laserLine.SetPosition(0, this.transform.position);
        StartCoroutine(MoveLaser() ); // 開始移動射線

        particle.SetActive(false);
    }
    IEnumerator MoveLaser()
    {
        laserLine.enabled = true;
        float elapsedTime = 0;
        float moveDuration = 1f;
        while (elapsedTime < moveDuration/10)
        {
            Vector3 endPos = monster_position.position;
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / moveDuration;
            Vector3 currentPos = Vector3.Lerp(this.transform.position, endPos, t);

            laserLine.SetPosition(1, currentPos);
            RaycastHit2D hit = Physics2D.Linecast(this.transform.position, currentPos);

            yield return null;
        }
        while (elapsedTime < 5*moveDuration)
        {
            Vector3 endPos = monster_position.position;
            elapsedTime += Time.deltaTime;

            laserLine.SetPosition(1, endPos);
            RaycastHit2D hit = Physics2D.Linecast(this.transform.position, endPos);
            for(int i = 0; i < 4; i++)
            {
                monsterlife_controller.minus_monster_life();
            }

            yield return null;
        }

        laserLine.enabled = false;
        spotlight.SetActive(false);
    }
}
