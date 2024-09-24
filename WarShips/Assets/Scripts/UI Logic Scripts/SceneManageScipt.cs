using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneManageScript : MonoBehaviour
{
    public Slider slider1;
    public Text progressText;
    public Animator animator;
    public Button continueButton;
    public int sceneIndex = 2;
    private AsyncOperation operation;
    public GameObject slider;
    public GameObject loadingText;
    public float fakeLoadingSpeed = 0.1f;

    private void Start()
    {
        LoadScene(sceneIndex);
    }
    public void LoadScene ( int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
       
        operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;
        float fakeProgress = 0;
        while (!operation.isDone)
        {
            //yield return new WaitForSeconds(0.5f);
            float progress = Mathf.Clamp01(operation.progress / .9f);
            while (fakeProgress < progress)
            {
                fakeProgress += Time.deltaTime * fakeLoadingSpeed;
                fakeProgress = Mathf.Min(fakeProgress, progress);
                slider1.value = fakeProgress;
                progressText.text = Mathf.FloorToInt(fakeProgress * 100f) + "%";
                yield return null;
            }
            
            // Nếu đã tải xong (operation.progress >= 0.9f)
            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                // Hiển thị button cho người dùng nhấn vào
                slider.SetActive(false);
                loadingText.SetActive(false);
                continueButton.gameObject.SetActive(true);               
                break;
            }
            yield return null;
        }
    }
    public void OnContinueButtonPressed()
    {
        StartCoroutine(Transition());
        // Cho phép chuyển scene
        
        
    }
    IEnumerator Transition()
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        operation.allowSceneActivation = true;
    }

}
