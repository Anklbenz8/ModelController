using UnityEngine;

public class CamerasController {
	public bool camera3DEnabled {
		get => _modelCamera && _modelCamera.enabled;
		set { if(_modelCamera) _modelCamera.enabled = value; }
	}
	
	public Camera modelCamera {
		set => _modelCamera = value;
		get => _modelCamera;
	}
	private Camera _modelCamera;
	
}
