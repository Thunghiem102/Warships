using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        Time.timeScale = 0f;
    }
    public void TryAgain(int sceneID)
    {
        Time.timeScale = 1;
        StartCoroutine(Tryagain(sceneID));

    }

    IEnumerator Tryagain(int sceneID)
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(sceneID);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();  
    }
}
