using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using JsonDeserialize;
using TriLibCore.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public class ArObject {
	private const string CAMERA_NAME = "Camera";

	[SerializeField] private GameObject tilePrefab;
	[SerializeField] private Camera injectedCameraPrefab;
	[Min(0.01f)]
	[SerializeField] private float sideInMeters = 0.4f;
	[SerializeField] private float tileSizeMultiplier = 1.3f;
	[SerializeField] private CadHandler cadHandler;
	public Camera camera { get; private set; }
	public GameObject cadModel { get; private set; }

	public async UniTask<GameObject> PrepareObject(string fbxFilename) {
		var fbxData = new FbxData() {fbxUrl = fbxFilename, isActive = true};
		var arObject = await cadHandler.CreateAsync(fbxData);

		cadModel = arObject.gameObject;
		CameraInject();

		var bounds = cadModel.RecalculateBounds();
		var maxBoundsElement = (new float[] {bounds.size.x, bounds.size.y, bounds.size.z}).Max();
		var scaleFactor = sideInMeters / maxBoundsElement;

		var contentGameObject = new GameObject("Content");
		cadModel.transform.SetParent(contentGameObject.transform);
		
		var tile = Object.Instantiate(tilePrefab, contentGameObject.transform);
		tile.transform.position = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z); //new Vector3(boundsCenter.x, bounds.min.y, boundsCenter.z);
		tile.transform.localScale = new Vector3(bounds.size.x * tileSizeMultiplier, 0.001f, bounds.size.z * tileSizeMultiplier);
		
		contentGameObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

		var parentGameObject = new GameObject("arObject");
		contentGameObject.transform.SetParent(parentGameObject.transform);

		return parentGameObject;
	}
	
	private void CameraInject() {
		var cameraParent = cadModel.transform.Find(CAMERA_NAME);

		if (!cameraParent) return;
		camera = Object.Instantiate(injectedCameraPrefab,  cameraParent,false);
		camera.transform.localPosition = Vector3.zero;
		camera.transform.localRotation = Quaternion.Euler(0, 180, 0);
		camera.enabled = false;
	}
}