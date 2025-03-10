using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetButtons : MonoBehaviour
{
    [SerializeField] List<Button> buttons; 

    public GameObject[] GetAllButtons()
    {
        GameObject[] buttonObjects = new GameObject[buttons.Count];

        for (int i = 0; i < buttons.Count; i++)
        {
            buttonObjects[i] = buttons[i].gameObject; 
        }

        return buttonObjects;
    }
}
