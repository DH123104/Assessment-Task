using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Color[] totalColors;
    [SerializeField] Transform boardParent;
    [SerializeField] Text levelText;
    [SerializeField] Transform winPanel;
    [SerializeField] Transform lossPanel;
    [SerializeField] Image loading;
    public float boardRotateSpeed = 1f;
    bool levelCompleted = false;
    [HideInInspector] public bool canActiveBoards = true;

    // Start is called before the first frame update
    void Start()
    {
        int colorsIndex = Random.Range(0, 5);
        SpehreColoring(colorsIndex);
        int levelNo = PlayerPrefs.GetInt("level");
        levelText.text = levelText.text.Replace("0", (levelNo + 1).ToString());
        StartCoroutine(LoadingFadeOut());
    }

    void SpehreColoring(int clrIndex)
    {
        for (int i = 0; i < boardParent.childCount; i++)
            for (int j = 0; j < boardParent.GetChild(i).childCount; j++)
            {
                Transform beed = boardParent.GetChild(i).GetChild(j);
                beed.GetComponent<MeshRenderer>().material.color = totalColors[clrIndex + j];
                beed.GetChild(0).tag = j.ToString();
            }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void IsCompleteGame()
    {
        Invoke(nameof(CheckCompletion), 0.1f);
    }

    void CheckCompletion()
    {
        bool isLevelComplete = true;
        for (int i = 0; i < boardParent.childCount; i++)
        {
            if (boardParent.GetChild(i).childCount > 0)
            {
                isLevelComplete = false;
                break;
            }
        }

        if (isLevelComplete && !levelCompleted)
        {
            levelCompleted = true;
            GameWin();
        }
        else if (boardParent.GetChild(0).childCount == 0 && boardParent.GetChild(1).childCount == 0 &&
            boardParent.GetChild(2).childCount > 0 && boardParent.GetChild(3).childCount > 0)
            LossGame();
    }

    /// <summary>
    /// Win panel Implementation starts from here...!!!//////////////////////////
    /// </summary>
    void GameWin()
    {
        winPanel.gameObject.SetActive(true);
        StartCoroutine(PanelScaling(winPanel));
    }

    IEnumerator PanelScaling(Transform panel)
    {
        iTween.ScaleTo(panel.GetChild(0).gameObject, iTween.Hash("scale", Vector3.one, "time", 1f, "easetype", iTween.EaseType.spring));
        DeactivateBoards();
        float alpha = 0f;
        while (alpha < 0.7f)
        {
            panel.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, alpha);
            alpha += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void DeactivateBoards()
    {
        for (int i = 0; i < boardParent.childCount; i++)
            boardParent.GetChild(i).GetComponent<BoxCollider>().enabled = false;
    }

    public void ActivateBoards()
    {
        for (int i = 0; i < boardParent.childCount; i++)
            boardParent.GetChild(i).GetComponent<BoxCollider>().enabled = true;
    }


    public void Next()
    {
        int level = PlayerPrefs.GetInt("level");
        level++;
        PlayerPrefs.SetInt("level", level);
        StartCoroutine(LoadingFadeIn());
    }

    public void Again()
    {
        StartCoroutine(LoadingFadeIn());
    }

    /// <summary>
    /// Loading fade out/Fade In...!!!!////////////////////////////////
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadingFadeOut()
    {
        float alpha = 1f;
        while (alpha > -0.1f)
        {
            loading.color = new Color(0f, 0f, 0f, alpha);
            alpha -= (Time.deltaTime * 2f);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator LoadingFadeIn()
    {
        float alpha = 0f;
        while (alpha <= 1f)
        {
            loading.color = new Color(0f, 0f, 0f, alpha);
            alpha += (Time.deltaTime * 2f);
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene("Game Play");
    }

    public void LossGame()
    {
        lossPanel.gameObject.SetActive(true);
        StartCoroutine(PanelScaling(lossPanel));
    }

}
