using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskEditor : MonoBehaviour {

	public Camera _Camera;
	public RenderTexture MaskTexture;

	public bool _EditMask;
	public bool _PreviewMask;
	public bool _RestartMask;

	public Texture2D _BrushTexture;
	[Range (0, 250)]
	public float _BrushSize = 50f;

	public Texture2D _EraseTexture;
	public bool _Eraser;
	[Range (0, 250)]
	public float _EraserSize = 10f;

	private RaycastHit Hit;

	void Update () {
		if (_EditMask && _RestartMask) {
			MaskTexture.Release ();
			_RestartMask = false;
		}

		if (_EditMask && Input.GetKey (KeyCode.Mouse0)) {
			if (Physics.Raycast (_Camera.ScreenPointToRay (Input.mousePosition), out Hit)) {
				DrawTexture (MaskTexture, Hit.textureCoord.x, Hit.textureCoord.y);
			}
		}

		if (_EditMask){
			_Camera.gameObject.SetActive(true);
		}else{
			_Camera.gameObject.SetActive(false);
		}
	}

	private void OnGUI () {
		if (_EditMask || _PreviewMask) {
			GUI.DrawTexture (new Rect (0, 0, 256, 256), MaskTexture, ScaleMode.ScaleToFit, false, 1);
		}
	}

	void DrawTexture (RenderTexture rt, float posX, float posY) {
		Texture2D Text = (_Eraser) ? _EraseTexture : _BrushTexture;

		float fSize = (_Eraser) ? _EraserSize : _BrushSize;
		float textSize =  MaskTexture.height;

		RenderTexture.active = rt;

		GL.PushMatrix ();
		GL.LoadPixelMatrix (0, textSize, textSize, 0);

		Vector2 coord = new Vector2 (Hit.textureCoord.x * textSize, textSize - Hit.textureCoord.y * textSize);

		Graphics.DrawTexture (new Rect (coord.x - fSize / 2, (coord.y - fSize / 2), fSize, fSize), Text);

		GL.PopMatrix ();
		RenderTexture.active = null;
	}
}