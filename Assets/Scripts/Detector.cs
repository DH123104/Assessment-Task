using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    GameManager managerScript;
    void Start()
    {
        managerScript = Camera.main.GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (transform.CompareTag(other.tag))
            StartCoroutine(PopBubble());
    }

    IEnumerator PopBubble()
    {
        managerScript.canActiveBoards = false;
        transform.GetComponent<BoxCollider>().enabled = false;
        float scale = 0.003f;
        Transform coloredBeed = transform.parent;
        while (scale <= 0.004f)
        {
            coloredBeed.localScale = new Vector3(scale, scale, scale);
            scale += 0.0001f;
            yield return new WaitForSeconds(0.001f);
        }

        yield return new WaitForEndOfFrame();
        managerScript.canActiveBoards = true;
        managerScript.ActivateBoards();

        if (coloredBeed.parent.childCount < 2)
            managerScript.IsCompleteGame();

        Destroy(coloredBeed.gameObject);
    }
}
