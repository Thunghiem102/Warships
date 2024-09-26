using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject confirm;
    public Animator animator;
    public void Pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void BackToMenu()
    {
        confirm.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void No()
    {
        pauseMenu.SetActive(true);
        confirm.SetActive(false);
    }

    public void Yes()
    {
        Time .timeScale = 1f;
        StartCoroutine(Loadscene());
    }

    IEnumerator Loadscene()
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        SceneManager.LoadScene(0);
    }
}
