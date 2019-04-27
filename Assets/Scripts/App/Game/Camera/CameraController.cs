using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	public static CameraController instance { get; private set; }

	private Camera _camera { get; set; }
	public new Camera camera => this._camera ?? (this._camera = this.GetComponent<Camera>());

	private Display display { get; set; }
	private Vector2Int rendering { get; set; }
	public float rightPanelWidth = 300;

	public RectOffset uiOffset;

	private void Awake()
	{
		if (instance == null) instance = this;
		if (instance != this) Destroy(this);
		else
		{
			this.display = Display.main;
		}
	}

	private void Update()
	{
		if (this.display.renderingWidth != this.rendering.x || this.display.renderingHeight != this.rendering.y)
		{
			this.Refresh();
		}
	}

	public void Refresh()
	{
		if (Game.current == null || Game.current.world == null) return;
		this.rendering = new Vector2Int(this.display.renderingWidth, this.display.renderingHeight);

		float w = this.rendering.x - rightPanelWidth;
		float h = this.rendering.y;

		float requiredOrthographicOnWidth = h * Game.current.world.width / (2f * w);
		float requiredOrthographicOnHeight = Mathf.Min(w, h) * Game.current.world.height / (2f * h);

		this.camera.orthographicSize = Mathf.Max(requiredOrthographicOnWidth, requiredOrthographicOnHeight);

		this.transform.position = new Vector3(this.camera.orthographicSize * this.rendering.x / this.rendering.y, this.camera.orthographicSize, 0) + new Vector3(-.5f, -.5f, -10);

	}



}
