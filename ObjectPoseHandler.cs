using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class ObjectPoseHandler {
	[SerializeField] private float rotateMultiplier = 1, moveMultiplier = 0.0005f;
	public GameObject target { set => _targetObject = value; }
	public bool enabled { get; set; }

	private Touches _touches = new();
	private GameObject _targetObject;
	private Camera _camera;

	public void Initialize() {
		_touches.PinchRotateEvent += OnRotate;
		_touches.PinchSizeChangedEvent += OnScale;
		_touches.MovedEvent += OnMove;
		_camera = Camera.main;
	}

	public void Update_SystemCall() {
		if (enabled && _targetObject)
			_touches.Update_SystemCall();
	}
	private void OnMove(Vector2 delta) {
		var scaledDelta = delta * moveMultiplier;
		_targetObject.transform.Translate(new Vector3(scaledDelta.x, scaledDelta.y, 0), _camera.transform);
	}
	private void OnScale(float scaleFactor) =>
			_targetObject.transform.localScale *= scaleFactor;

	private void OnRotate(float angle) =>
			_targetObject.transform.Rotate(Vector3.up, angle * rotateMultiplier);
}