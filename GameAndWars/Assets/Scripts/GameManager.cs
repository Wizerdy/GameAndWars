using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> positions;
    public int currentPosition = 2;


    //Controles

    private float cooldownControl = 0.7f;

    private void Start()
    {
        foreach(GameObject position in positions)
        {
            if (positions.IndexOf(position) != currentPosition) position.GetComponent<Animator>().Play("Deactivating");
        }
    }

    private void Update()
    { 

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

            cooldownControl = 0.8f;
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

}
