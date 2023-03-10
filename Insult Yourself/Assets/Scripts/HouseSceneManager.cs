using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseSceneManager : MonoBehaviour
{
    public string sceneToLoad;
    public Transform playerPos;
    public GameObject spriteF;

    private void Awake()
    {
        spriteF.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            spriteF.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                playerPos.position = new Vector3(0, 0, 0);
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        spriteF.SetActive(false);
    }
}