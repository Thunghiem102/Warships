using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddScore : MonoBehaviour
{
    public static AddScore Instance { get; private set; }
    private Text scoreText;
    private float scoreTotal;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    // Start is called before the first frame update
    private void Start()
    {
        scoreText = GetComponent<Text>();
        if (scoreText != null)
        {
            scoreText.text = scoreTotal.ToString();
        }
        else
        {
            Debug.LogError("Text component not found on AddScore GameObject.");
        }
    }
    // Update is called once per frame
    public void Scoring(float scoreAmount)
    {
        scoreTotal += scoreAmount;
        //Debug.Log("Adding score: " + scoreAmount + ". Total score: " + scoreTotal);
        if (scoreText != null)
        {
            scoreText.text = scoreTotal.ToString();
        }
    }
  
}
