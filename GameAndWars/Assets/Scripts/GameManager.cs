using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> positions;
    public List<GameObject> grenadeTopLeftPos;
    public List<GameObject> grenadeTopRightPos;
    public List<GameObject> grenadeBottomLeftPos;
    public List<GameObject> grenadeBottomRightPos;

    private List<GameObject> grenadePos;
    public int currentPosition = 2;
    private int grenadePosition = 0;
    private int grenadeMaxPosition = 0;
    private int reversePos = -1;

    //Controles

    private float cooldownControl = 0.7f;
    private float cooldownGrenade = 0.7f;

    private bool isLaunching = false;
    private bool isReverse = false;

    private void Start()
    {
        foreach(GameObject position in positions)
        {
            if (positions.IndexOf(position) != currentPosition) position.GetComponent<Animator>().Play("Deactivating");
        }

        for(int i = 0; i < grenadeTopLeftPos.Count; i++)
        {
            grenadeTopLeftPos[i].GetComponent<Animator>().Play("Deactivating");
            grenadeTopRightPos[i].GetComponent<Animator>().Play("Deactivating");
        }

        for(int j = 0; j < grenadeBottomLeftPos.Count; j++)
        {
            grenadeBottomLeftPos[j].GetComponent<Animator>().Play("Deactivating");
            grenadeBottomRightPos[j].GetComponent<Animator>().Play("Deactivating");
        }
    }

    private void Update()
    { 

        //Grenade
        if(Input.GetKeyDown(KeyCode.Space) && isLaunching == false)
        {
            int random = Random.Range(0, 4);
            switch(random)
            {
                case 0 :
                    grenadePos = grenadeTopLeftPos;
                    grenadeMaxPosition = grenadeTopLeftPos.Count;
                break;
                case 1 :
                    grenadePos = grenadeTopRightPos;
                    grenadeMaxPosition = grenadeTopRightPos.Count;
                break;
                case 2 :
                    grenadePos = grenadeBottomLeftPos;
                    grenadeMaxPosition = grenadeBottomLeftPos.Count;
                break;
                case 3 :
                    grenadePos = grenadeBottomRightPos;
                    grenadeMaxPosition = grenadeBottomRightPos.Count;
                break;
            }
            reversePos = random;
            isLaunching = true;
            cooldownGrenade = 0.7f;
            grenadePos[grenadePosition].GetComponent<Animator>().Play("Activating");
        }

        if(isLaunching == true)
        {
            if(cooldownGrenade > 0)
            {
                cooldownGrenade -= Time.deltaTime;
            }
            else
            {
                if(isReverse == false) CheckGrenade(false);
                else CheckGrenade(true);
            }
        }

        if (cooldownControl > 0)
        {
            cooldownControl -= Time.deltaTime;
            return;
        }

        if(Input.GetAxis("Horizontal") != 0)
        {

            cooldownControl = 0.7f;
            positions[currentPosition].GetComponent<Animator>().Play("Deactivating");

            switch (currentPosition)
            {
                case 0:
                    currentPosition = 1;
                    break;

                case 1:
                    currentPosition = 0;
                    break;

                case 2:
                    currentPosition = 3;
                    break;

                case 3:
                    currentPosition = 2;
                    break;
            }

            positions[currentPosition].GetComponent<Animator>().Play("Activating");
            return;

        }

        if (Input.GetAxis("Vertical") != 0)
        {

            cooldownControl = 0.7f;
            positions[currentPosition].GetComponent<Animator>().Play("Deactivating");

            switch (currentPosition)
            {
                case 0:
                    currentPosition = 2;
                    break;

                case 1:
                    currentPosition = 3;
                    break;

                case 2:
                    currentPosition = 0;
                    break;

                case 3:
                    currentPosition = 1;
                    break;
            }

            positions[currentPosition].GetComponent<Animator>().Play("Activating");

        }
    }

    private void CheckGrenade(bool reverse)
    {
        if(reverse == false)
        {
            if(grenadePosition < grenadeMaxPosition - 1)
            {
                grenadePos[grenadePosition].GetComponent<Animator>().Play("Deactivating");
                grenadePosition += 1;
                cooldownGrenade = 0.7f;
                grenadePos[grenadePosition].GetComponent<Animator>().Play("Activating");
            }
            else
            {
                if(reversePos == currentPosition)
                {
                    if(currentPosition == 0 || currentPosition == 1) grenadePosition = grenadeTopLeftPos.Count - 1;
                    else if( currentPosition == 2 || currentPosition == 3) grenadePosition = grenadeBottomLeftPos.Count - 1;
                    isReverse = true;
                    Debug.Log("REVERSE");
                    CheckGrenade(true);
                }
                else
                {
                    grenadePos[grenadePosition].GetComponent<Animator>().Play("Deactivating");
                    Debug.Log("BOUM");
                    isLaunching = false;
                    grenadePosition = 0;
                    cooldownGrenade = 0.7f;
                }
                reversePos = -1;
            }
        }
        else
        {
            if(grenadePosition > 0)
            {
                grenadePos[grenadePosition].GetComponent<Animator>().Play("Deactivating");
                cooldownGrenade = 0.5f;
                grenadePosition -= 1;
                grenadePos[grenadePosition].GetComponent<Animator>().Play("Activating");
            }
            else
            {
                grenadePosition = 0;
                grenadePos[grenadePosition].GetComponent<Animator>().Play("Deactivating");
                cooldownGrenade = 0.7f;
                isLaunching = false;
                isReverse = false;
            }
        }
    }

}
