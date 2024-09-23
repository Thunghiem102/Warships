using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public Animator animator;


    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(Loading(sceneIndex));
    }

    IEnumerator Loading(int sceneIndex)
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(sceneIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
