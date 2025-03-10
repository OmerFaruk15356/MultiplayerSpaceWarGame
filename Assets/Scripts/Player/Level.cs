using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField] TMP_Text levelText;
    [SerializeField] PhotonView photonView;
    [SerializeField] GameObject xpBar;
    [SerializeField] Slider xpState;
    CanvasGroup statsPanel;
    GameObject[] buttonObjects;
    private float currentXp = 0;
    private float xpForLevelUp = 100;
    private int level = 1;
    private int statPoint = 0;
    private bool state = false;
    UI uI;
    GetButtons getButtons;
    
    private List<Button> buttons = new List<Button>();  

    private void Start() 
    {
        uI = FindObjectOfType<UI>();
        getButtons = FindObjectOfType<GetButtons>();
        statsPanel = uI.statsPanel;

        if (photonView.IsMine) 
        {
            xpBar.gameObject.SetActive(true);
        }
        
        levelText.text = level.ToString();

        buttonObjects = getButtons.GetAllButtons();

        foreach (GameObject buttonObj in buttonObjects) 
        {
            Button button = buttonObj.GetComponent<Button>();
            buttons.Add(button);
        }

        for (int i = 0; i < buttons.Count; i++) 
        {
            int index = i;  // 
            buttons[i].onClick.AddListener(() => GetLevelUp(index));
        }
        SetStatPanel();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.C))
        {
            state = !state;
            SetStatPanel();
        }
    }

    public void SetXp(int xp) {
        if ((currentXp + xp) >= xpForLevelUp) 
        {
            LevelUp(xp);
        } 
        else 
        {
            currentXp += xp;
        }

        xpState.value = currentXp / xpForLevelUp; 
    }

    private void LevelUp(int xp)
    {
        level++;
        levelText.text = level.ToString();
        currentXp = (currentXp + xp) - xpForLevelUp;
        xpForLevelUp *= 2;
        statPoint++;

        if(photonView.IsMine)
        {
            uI.SetStats(statPoint,photonView);
            state = true;
            SetStatPanel();  
        }
    }


    public void GetLevelUp(int index) {

        photonView.RPC("SetLevelUp",RpcTarget.AllBuffered, index);
        statPoint--;
        uI.SetStats(statPoint,photonView);
        if(statPoint == 0)
        {
            state = true;
            SetStatPanel();
        }
    }

    private void SetStatPanel()
    {
        statsPanel.alpha = state ? 1 : 0;
        statsPanel.blocksRaycasts = state;
        foreach (GameObject button in buttonObjects)
        {
            if(statPoint > 0)
            {
                button.SetActive(true);
            }
            else
            {
                button.SetActive(false);
            }
        }
        uI.SetStats(statPoint,photonView);
    }
}