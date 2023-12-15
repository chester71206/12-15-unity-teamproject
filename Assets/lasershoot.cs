using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaycastGun : MonoBehaviour
{
    public float gunRange = 50f;
    public float fireRate = 8f;
    public float moveDuration = 3f; // ���ʮɶ�
    public float laserDuration = 0.05f;
    public Transform player;

    LineRenderer laserLine;
    float fireTimer = 8f;
    public life_change life_controler;

    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        // laserLine.material.color = Color.white;

    }
    private void Start()
    {
        Debug.Log(laserLine.widthCurve);
    }
    void Update()
    {
        laserLine.SetPosition(0, this.transform.position);
        fireTimer += Time.deltaTime;

        if (fireTimer > fireRate)
        {
            fireTimer = 0;

            laserLine.SetPosition(0, this.transform.position);
            Vector3 temp = player.position;
            temp.y = -4.25f;
            StartCoroutine(MoveLaser(this.transform.position, temp)); // �}�l���ʮg�u
        }
    }

    IEnumerator MoveLaser(Vector3 startPos, Vector3 endPos)
    {
        laserLine.enabled = true;
        float elapsedTime = 0;
        bool hitPlayer = false;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / moveDuration;
            Vector3 currentPos = Vector3.Lerp(this.transform.position, endPos, t);

            laserLine.SetPosition(1, currentPos);

            //RaycastHit2D hit = Physics2D.Raycast(currentPos, Vector2.down); // ���U�g�u�˴��a�O
            RaycastHit2D hit = Physics2D.Linecast(this.transform.position, currentPos);
            if (hit.collider != null && hit.collider.CompareTag("ground"))
            {
                endPos = hit.point; // �p�G�g��a�O�A��s���I���a�O�I���I
            }

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                hitPlayer = true;
                life_controler.minusDisplay();
            }

            yield return null;
        }
        // �p�G�S���������a�A�~��b�a���W���ʤ@�q�ɶ�
        float extraTime = 0;
        Vector3 lastPos = endPos;

        while (extraTime < 4f)
        {
            extraTime += Time.deltaTime;
            float playerX = player.position.x;
            if (this.transform.position.x > player.position.x)
            {
                playerX -= 0.5f;
            }
            else
            {
                playerX += 0.5f;
            }


            Vector3 newLaserPos = new Vector3(playerX, lastPos.y, lastPos.z);


            lastPos = Vector3.MoveTowards(lastPos, newLaserPos, Time.deltaTime * 7f);

            laserLine.SetPosition(1, lastPos);
            //RaycastHit2D hit = Physics2D.Raycast(lastPos, Vector2.down); // ���U�g�u�˴��a�O
            RaycastHit2D hit = Physics2D.Linecast(this.transform.position, lastPos);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                hitPlayer = true;
                life_controler.minusDisplay();
            }

            yield return null;
        }

        laserLine.enabled = false;
    }
}