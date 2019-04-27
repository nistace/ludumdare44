using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	public static CameraController instance { get; private set; }

	private Camera _camera { get; set; }
	public new Camera camera => this._camera ?? (this._camera = this.GetComponent<Camera>());

	private Display display { get; set; }
	private Vector2Int rendering { get; set; }
	[Range(0, 1)] public float rightPanelSize;


	private void Awake()
	{
		if (instance == null) instance = this;
		if (instance != this) Destroy(this);
		else
		{
			this.display = Display.main;
			this.Refresh();
		}
	}

	private void Update()
	{
		if (this.display.renderingWidth != this.rendering.x || this.display.renderingHeight != this.rendering.y)
		{
			this.Refresh();
		}
		if (EventSystem.current.IsPointerOverGameObject())
		{
			GameController.instance.UnhoverTile();
		}
		else
		{
			Vector3 worldPosition = this.camera.ScreenToWorldPoint(Input.mousePosition);
			Vector2Int tileCoordinates = new Vector2Int((int)(worldPosition.x + .5f), (int)(worldPosition.y + .5f));
			GameController.instance.HoverTile(tileCoordinates);
			if (GameController.instance.selectedRobot != null && Input.GetAxis("Fire1") != 0)
			{
				GameController.instance.PlaceRobot();
			}
		}
		if (Input.GetAxis("Fire2") != 0)
		{
			GameController.instance.SelectRobot(null);
		}
	}

	public void Refresh()
	{
		if (Game.current == null || Game.current.world == null) return;
		this.rendering = new Vector2Int(this.display.renderingWidth, this.display.renderingHeight);

		float w = this.rendering.x * (1 - this.rightPanelSize);
		float h = this.rendering.y;

		float requiredOrthographicOnWidth = h * Game.current.world.width / (2f * w);
		float requiredOrthographicOnHeight = Mathf.Min(w, h) * Game.current.world.height / (2f * h);

		this.camera.orthographicSize = Mathf.Max(requiredOrthographicOnWidth, requiredOrthographicOnHeight);

		this.transform.position = new Vector3(this.camera.orthographicSize * this.rendering.x / this.rendering.y, this.camera.orthographicSize, 0) + new Vector3(-.5f, -.5f, -10);

	}

}
