using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardMovement : MonoBehaviour
{
    GameManager managerScript;

    void Start()
    {
        managerScript = Camera.main.GetComponent<GameManager>();
    }
    private void OnMouseUp()
    {
        //  GetComponent<BoxCollider>().enabled = false;
        managerScript.DeactivateBoards();
        StartCoroutine(MoveBoard());
    }

    IEnumerator MoveBoard()
    {
        float zRot = 0f;
        float rotateSpeed = managerScript.boardRotateSpeed;
        while (zRot < 60f)
        {
            transform.localEulerAngles = new Vector3(0f, 0f, transform.localEulerAngles.z + rotateSpeed);
            zRot += rotateSpeed;
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForEndOfFrame();
        if (managerScript.canActiveBoards)
            managerScript.ActivateBoards();
        //  GetComponent<BoxCollider>().enabled = true;
    }
}
