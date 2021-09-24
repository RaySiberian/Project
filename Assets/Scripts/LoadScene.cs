using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    private AsyncOperation asyncOperation;
    public Image LoadImage;
    public Image LoadBar;
    public Text BatTxt;

    public int SceneID;

    private Vector3 rotateTo;
    
    private void Start()
    {
        //StartCoroutine(LoadSceneCor());
        rotateTo = new Vector3(0, 0, -90);
    }

    private void Update()
    {
        LoadImage.rectTransform.Rotate(rotateTo * Time.deltaTime);
    }

    // IEnumerator LoadSceneCor()
    // {
    //     yield return new WaitForSeconds(1f);
    //     asyncOperation = SceneManager.LoadSceneAsync(SceneID);
    //     while (!Test.GameSceneLoaded)
    //     {
    //         float progress = asyncOperation.progress / 0.9f;
    //         LoadBar.fillAmount = progress;
    //         BatTxt.text = "Загрузка " + $"{progress * 100f:0}%";
    //         yield return 0;
    //     }
    // }
}
