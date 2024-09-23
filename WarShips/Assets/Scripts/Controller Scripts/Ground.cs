
using UnityEngine;

public class Ground : MonoBehaviour
{

    public Renderer meshRender;
    public float speed = 0.1f;
    // Update is called once per frame
    void Update()
    {
        meshRender.material.mainTextureOffset += new Vector2(0, speed * Time.deltaTime);
    }
}
