using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Text scoreText1;
    public Text scoreText2;
    public GameObject labirinth;
    public GameObject labirinthBackground;
    void Start()
    {
        if(PlayerPrefs.GetInt("OpenLabyrinth", 0) == 1)
        {
            labirinth.SetActive(true);
            labirinthBackground.SetActive(true);
        }
        scoreText1.text = PlayerPrefs.GetInt("coin1", 0).ToString();
        scoreText2.text = PlayerPrefs.GetInt("coin2", 0).ToString();
    }

}
