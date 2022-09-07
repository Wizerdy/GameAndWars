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
    
    public List<GameObject> reverseGrenadeTopLeftPos;
    public List<GameObject> reverseGrenadeTopRightPos;
    public List<GameObject> reverseGrenadeBottomLeftPos;
    public List<GameObject> reverseGrenadeBottomRightPos;
    public int currentPosition = 2;
    [SerializeField]private List<int> grenadePosition;
    [SerializeField]private List<int> reverseGrenadePosition;
    private List<int> grenadeMaxPosition = new List<int>(4){4, 4, 3, 3};
    private List<int> reversePos = new List<int>(4){0, 1, 2, 3};

    public float cooldownGrenadeLaunch = 5f;
    public float nextGrenadeCooldown = 4.9f;

    //Controles

    private float cooldownControl = 0.7f;
    [SerializeField]private List<float> cooldownGrenade;
    [SerializeField]private List<float> reverseCooldownGrenade = new List<float>(4);

    [SerializeField]private List<bool> isReverse;
    [SerializeField]private List<bool> isLaunching;

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
            
            reverseGrenadeTopLeftPos[i].GetComponent<Animator>().Play("Deactivating");
            reverseGrenadeTopRightPos[i].GetComponent<Animator>().Play("Deactivating");
        }

        for(int j = 0; j < grenadeBottomLeftPos.Count; j++)
        {
            grenadeBottomLeftPos[j].GetComponent<Animator>().Play("Deactivating");
            grenadeBottomRightPos[j].GetComponent<Animator>().Play("Deactivating");

            reverseGrenadeBottomLeftPos[j].GetComponent<Animator>().Play("Deactivating");
            reverseGrenadeBottomRightPos[j].GetComponent<Animator>().Play("Deactivating");
        }

    }

    private void Update()
    {

        //Grenade
        cooldownGrenadeLaunch -= Time.deltaTime;
        if(cooldownGrenadeLaunch <= 0)
        {
            SendGrenade();
            cooldownGrenadeLaunch = nextGrenadeCooldown;
            if (nextGrenadeCooldown > 0.8f) nextGrenadeCooldown -= 0.1f;
        }

        //cooldown grenade before check
        for(int i = 0; i < isLaunching.Count; i++)
        {
            if(isLaunching[i] == true)
            {
                if(cooldownGrenade[i] > 0)
                {
                    cooldownGrenade[i] -= Time.deltaTime;
                }
                else
                {
                    CheckGrenade(i);
                }
            }
        }

        for(int j = 0; j < isReverse.Count; j++)
        {
            if(isReverse[j] == true)
            {
                if(reverseCooldownGrenade[j] > 0)
                {
                    reverseCooldownGrenade[j] -= Time.deltaTime;
                }
                else
                {
                    ReverseGrenade(j);
                }
            }
        }
        

        if (cooldownControl > 0)
        {
            cooldownControl -= Time.deltaTime;
            return;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            SetPosition(0);
            cooldownControl = 0.7f;
            return;
        } else if (Input.GetKey(KeyCode.LeftArrow))
        {
            SetPosition(2);
            cooldownControl = 0.7f;
            return;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            SetPosition(3);
            cooldownControl = 0.7f;
            return;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            SetPosition(1);
            cooldownControl = 0.7f;
            return;
        }
    }

    public void SendGrenade()
    {
        int random = Random.Range(0, 4);
        List<GameObject> localGrenade = new List<GameObject>();
        if(isLaunching[random] == false)
        {
            switch (random)
            {
                case 0:
                    localGrenade = grenadeTopLeftPos;
                    break;
                case 1:
                    localGrenade = grenadeTopRightPos;
                    break;
                case 2:
                    localGrenade = grenadeBottomLeftPos;
                    break;
                case 3:
                    localGrenade = grenadeBottomRightPos;
                    break;
            }
            reversePos[random] = random;
            cooldownGrenade[random] = 0.7f;
            isLaunching[random] = true;
            localGrenade[grenadePosition[random]].GetComponent<Animator>().Play("Activating");
        }
        else
        {
            SendGrenade();
        }
    }

    private void CheckGrenade(int position)
    {
        List<GameObject> localGrenade = new List<GameObject>();
        switch(position)
        {
            case 0:
                localGrenade = grenadeTopLeftPos;
            break;
            case 1:
                localGrenade = grenadeTopRightPos;
            break;
            case 2:
                localGrenade = grenadeBottomLeftPos;
            break;
            case 3:
                localGrenade = grenadeBottomRightPos;
            break;
        }

        if(grenadePosition[position] < grenadeMaxPosition[position] - 1)
        {
            localGrenade[grenadePosition[position]].GetComponent<Animator>().Play("Deactivating");
            grenadePosition[position] += 1;
            cooldownGrenade[position] = 0.7f;
            localGrenade[grenadePosition[position]].GetComponent<Animator>().Play("Activating");
        }
        else
        {
            if(reversePos[position] == currentPosition)
            {
                if(currentPosition == 0 || currentPosition == 1) reverseGrenadePosition[position] = reverseGrenadeTopLeftPos.Count - 1;
                else if(currentPosition == 2 || currentPosition == 3) reverseGrenadePosition[position] = reverseGrenadeBottomLeftPos.Count - 1;
                Debug.Log("REVERSE");
                isReverse[position] = true;
                isLaunching[position] = false;
                cooldownGrenade[position] = 0.7f;
                reverseCooldownGrenade[position] = 0.5f;
                localGrenade[grenadePosition[position]].GetComponent<Animator>().Play("Deactivating");
                switch(position)
                {
                    case 0 :
                        reverseGrenadeTopLeftPos[reverseGrenadePosition[position]].GetComponent<Animator>().Play("Activating");
                    break;
                    case 1 :
                        reverseGrenadeTopRightPos[reverseGrenadePosition[position]].GetComponent<Animator>().Play("Activating");
                    break;
                    case 2 :
                        reverseGrenadeBottomLeftPos[reverseGrenadePosition[position]].GetComponent<Animator>().Play("Activating");
                    break;
                    case 3 :
                        reverseGrenadeBottomRightPos[reverseGrenadePosition[position]].GetComponent<Animator>().Play("Activating");
                    break;
                }
                grenadePosition[position] = 0;
            }
            else
            {
                localGrenade[grenadePosition[position]].GetComponent<Animator>().Play("Deactivating");
                Debug.Log("BOUM");
                grenadePosition[position] = 0;
                cooldownGrenade[position] = 0.7f;
                isLaunching[position] = false;
            }
        }
    }

    private void ReverseGrenade(int position)
    {
        List<GameObject> localGrenade = new List<GameObject>();
        switch(position)
        {
            case 0:
                localGrenade = reverseGrenadeTopLeftPos;
            break;
            case 1:
                localGrenade = reverseGrenadeTopRightPos;
            break;
            case 2:
                localGrenade = reverseGrenadeBottomLeftPos;
            break;
            case 3:
                localGrenade = reverseGrenadeBottomRightPos;
            break;
        }

        if(reverseGrenadePosition[position] > 0)
        {
            localGrenade[reverseGrenadePosition[position]].GetComponent<Animator>().Play("Deactivating");
            reverseGrenadePosition[position] -= 1;
            localGrenade[reverseGrenadePosition[position]].GetComponent<Animator>().Play("Activating");
        }
        else
        {
            localGrenade[reverseGrenadePosition[position]].GetComponent<Animator>().Play("Deactivating");
            isReverse[position] = false;
        }
        reverseCooldownGrenade[position] = 0.5f;
    }

    public void SetPosition(int position)
    {
        positions[currentPosition].GetComponent<Animator>().Play("Deactivating");
        currentPosition = position;
        positions[currentPosition].GetComponent<Animator>().Play("Activating");
    }

}
