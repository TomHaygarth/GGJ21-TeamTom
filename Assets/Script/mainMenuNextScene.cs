using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuNextScene : MonoBehaviour
{
    // Start is called before the first frame update

    public int playerCount;

    public TMPro.TMP_Text bot_text_ui = null;

    [SerializeField]
    private string bot_enabled_text = "Bots Enabled";
    [SerializeField]
    private string bot_disabled_text = "Bots Disabled";

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            print("space");
            //SceneManager.LoadScene("MainGame");
            //setplayer=1
            //playerCount = 1;
        }

        if (Input.GetKeyDown("1"))
        {
            print("1");
            SceneManager.LoadScene("MainGame");
            //set playes =1
            //playerCount = 1;
        }
        if (Input.GetKeyDown("2"))
        {
            print("2");
            //SceneManager.LoadScene("MainGame");
            SceneManager.LoadScene("Level2");
            //set playes =2
            //playerCount = 2;
        }
        if (Input.GetKeyDown("3"))
        {
            print("3");
            //SceneManager.LoadScene("MainGame");
            //set playes =2
            //playerCount = 3;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            GameSettings.EnableAI = !GameSettings.EnableAI;

            if (bot_text_ui != null)
            {
                bot_text_ui.text = GameSettings.EnableAI ? bot_enabled_text : bot_disabled_text;
            }
        }
    }
//public void SetGamePlayerCount(int players)
//{
 //   GameSetupData.PlayerCount = players;
//}
}
