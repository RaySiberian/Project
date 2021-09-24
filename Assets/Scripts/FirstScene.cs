using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstScene : MonoBehaviour
{

    public Text seedText; 
    
    private void Start()
    {
        SharedData.Seed = 100;
    }

    public void LoadLoadingScene()
    {
        if (seedText.text == string.Empty)
        {
            seedText.text = "0";
        }
        int.TryParse(seedText.text, out var x);
        
        SharedData.Seed = x;
        SceneManager.LoadScene(1);
    }
}
