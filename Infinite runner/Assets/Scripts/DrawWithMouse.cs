using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWithMouse : MonoBehaviour
{
    public Camera cam;
    private RenderTexture splatmap;
    public Shader drawShader;
    private Material drawMaterial;
    private Material myMaterial;
    [Range(1, 500)]
    public float _brushSize;
    [Range(0, 1)]
    public float _brushStrength;



    // Use this for initialization
    void Start()
    {

        drawMaterial = new Material(drawShader);
        drawMaterial.SetVector("_Color", Color.red);

        myMaterial = GetComponent<MeshRenderer>().material;
        myMaterial.SetTexture("_Splat", splatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat));

    }
    RaycastHit _hit;
    // Update is called once per frame
    void FixedUpdate()
    {


        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
        {
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                //Debug.Log(hitInfo.textureCoord.ToString("F4"));
                drawMaterial.SetVector("_Coordinate", new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0, 0));
                drawMaterial.SetFloat("_Strength", _brushStrength);
                drawMaterial.SetFloat("_Size", _brushSize);
                RenderTexture temp = RenderTexture.GetTemporary(splatmap.width, splatmap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(splatmap, temp);
                Graphics.Blit(temp, splatmap, drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
            }
        }
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 256, 256), splatmap, ScaleMode.ScaleToFit, false, 1);
    }
}