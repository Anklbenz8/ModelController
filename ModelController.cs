using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

[System.Serializable]
public class ModelController
{
	[SerializeField] private ArObjectHandler arObjectHandler;
	[SerializeField] private HorizontalPlacement horizontalPlacement;
	[SerializeField] private ObjectPoseHandler objectPoseHandler;
	[SerializeField] public AnimationsPlayer animationsPlayer; //!public

	[SerializeField] private UIColorToggle cameraSwitchToggle;
	[SerializeField] private AnimatedView modelLoadingView;

	public bool modelObjectVisible {
		set {
			if (_modelObject != null) _modelObject.SetActive(value);
		}
	}

	public bool isReady {
		get; private set; 
		
	}
	private CamerasController _camerasController = new();
	private GameObject _modelObject;
	private bool _modelCameraIsActiveBeforeHide;
   
   public bool uiControlsVisible {
	   set {
		   cameraSwitchToggle.gameObject.SetActive(value);
		   animationsPlayer.animationControlVisible = value;
	   }
   }

   public void Initialize() {
		modelLoadingView.ForceClose();
		cameraSwitchToggle.onValueChanged.AddListener(CameraSwitch);
		animationsPlayer.Initialize();
		horizontalPlacement.Initialize();
		objectPoseHandler.Initialize();

		uiControlsVisible = false;
   }

	public async UniTask Prepare() {
		modelLoadingView.Open();
		_modelObject = await arObjectHandler.Prepare(SessionCache.instance.fbxFileName, SessionCache.instance.animationData);
		modelLoadingView.Close();
		
		_camerasController.modelCamera = arObjectHandler.cadCamera;
		animationsPlayer.targetAnimation = arObjectHandler.cadAnimation;
		objectPoseHandler.target = _modelObject;
		isReady = true;
	}
	
	public async UniTask Bind() {
		uiControlsVisible = false;
		objectPoseHandler.enabled = false;
	
		await horizontalPlacement.PlaceObjectAsync(_modelObject);
		
		objectPoseHandler.enabled = true;
		uiControlsVisible = true;
	}
	
	private void CameraSwitch(bool isOn) {
		objectPoseHandler.enabled = !isOn;
		_camerasController.camera3DEnabled = isOn;
	}

	public void FixedUpdate_SystemCall() {
		horizontalPlacement.FixedUpdate_SystemCall();
		animationsPlayer.FixedUpdate_SystemCall();
	}

	public void Update_SystemCall() {
		horizontalPlacement.Update_SystemCall();
		objectPoseHandler.Update_SystemCall();
	}
	
	public void Hide() {
		uiControlsVisible = false;
		
		_modelCameraIsActiveBeforeHide = _camerasController.camera3DEnabled;
		_camerasController.camera3DEnabled = false;
		modelObjectVisible = false;
	}
	
	public void Show() {
		uiControlsVisible = true;
		_camerasController.camera3DEnabled = _modelCameraIsActiveBeforeHide;
		modelObjectVisible = true;
	}
}
