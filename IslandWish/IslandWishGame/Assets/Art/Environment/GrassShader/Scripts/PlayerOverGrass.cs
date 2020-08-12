using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverGrass : MonoBehaviour {
    public LayerMask _LayerMask;
    public Color _PathColor;

    [Range (0.001f, 0.5f)]
    public float _PressureResetAmount;

    [Range (0, 0.05f)]
    public float _PressureResetOpacity;
    public Transform[] _Objects;
    public float[] _BrushSizes;

    [Range (0, 1)]
    public float[] _BrushStrengths;
    
    public float[] _RayDistances;

    // --------- //
    private Shader _DrawShader;
    private Shader _ResetPressureShader;
    private const string TextureName = "_PressureTex";
    private Material _PathMaterial;
    private Material _ResetPathMaterial;
    RaycastHit _GrassHit;
    private Dictionary<string, RenderTexture> _MySplats;

    void Start () {
        _DrawShader = Shader.Find ("Hidden/Keronius/DrawPath");
        _ResetPressureShader = Shader.Find ("Hidden/Keronius/RandomDots");

        _PathMaterial = new Material (_DrawShader);
        _PathMaterial.SetVector ("_Color", _PathColor);

        _ResetPathMaterial = new Material (_ResetPressureShader);

        _MySplats = new Dictionary<string, RenderTexture> ();
    }
    void LateUpdate () {
        for (int i = 0; i < _Objects.Length; i++) {
            if (_Objects[i].gameObject.activeInHierarchy && Physics.Raycast (_Objects[i].position, -Vector3.up, out _GrassHit, _RayDistances[i], _LayerMask.value)) {
                RenderTexture search = null;
                if (_MySplats.TryGetValue (_GrassHit.transform.gameObject.name, out search)) {
                    _PathMaterial.SetVector ("_PosToDraw", new Vector4 (_GrassHit.textureCoord.x, _GrassHit.textureCoord.y, 0, 0));
                    _PathMaterial.SetFloat ("_BrushStrength", _BrushStrengths[i]);
                    _PathMaterial.SetFloat ("_BrushSize", _BrushSizes[i]);

                    RenderTexture temp = RenderTexture.GetTemporary (search.width, search.height, 0, RenderTextureFormat.ARGBFloat);
                    Graphics.Blit (search, temp);
                    Graphics.Blit (temp, search, _PathMaterial);
                    RenderTexture.ReleaseTemporary (temp);
                } else {
                    RenderTexture newSplatmap = new RenderTexture (1024, 1024, 0, RenderTextureFormat.ARGBFloat);
                    Material newMat = _GrassHit.transform.gameObject.GetComponent<MeshRenderer> ().material;
                    newMat.SetTexture (TextureName, newSplatmap);

                    _MySplats.Add (_GrassHit.transform.gameObject.name, newSplatmap);
                }
            }
        }

        foreach (string key in _MySplats.Keys) {
            RenderTexture search = _MySplats[key];

            _ResetPathMaterial.SetFloat ("_DotsAmount", _PressureResetAmount);
            _ResetPathMaterial.SetFloat ("_DotsOpacity", _PressureResetOpacity);

            RenderTexture pressureTemp = RenderTexture.GetTemporary (search.width, search.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit (search, pressureTemp);
            Graphics.Blit (pressureTemp, search, _ResetPathMaterial);
            RenderTexture.ReleaseTemporary (pressureTemp);
        }
    }
}