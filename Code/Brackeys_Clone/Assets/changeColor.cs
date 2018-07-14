using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine;

public class changeColor : MonoBehaviour
{
    static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
    Button colorButton;
    //  [SyncVar(hook = "OnMyColor")]

    Color playerColor;

    /*
        public void OnMyColor(Color newColor)
        {
            playerColor = newColor;
            colorButton.GetComponent<Image>().color = newColor;

            foreach (var item in gameObject.GetComponents<GameObject>())
            {
                if (item.tag == "ColorChange") item.GetComponent<MeshRenderer>().material.color = changeColor.playerColor;
            }
        }*/


    public void ChangeColor()
    {
        int idx = System.Array.IndexOf(Colors, UserAccountManager.LoggedIn_color);
        if (idx < 0) idx = 0;

        idx = (idx + 1) % Colors.Length;

        UserAccountManager.LoggedIn_color = Colors[idx];
        colorButton.GetComponent<Image>().color = UserAccountManager.LoggedIn_color;
    }


    // Use this for initialization
    void Start()
    {
        colorButton = GetComponent<Button>();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
