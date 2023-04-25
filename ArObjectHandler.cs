using Cysharp.Threading.Tasks;
using JsonDeserialize;
using UnityEngine;

[System.Serializable]
public class ArObjectHandler {
	[SerializeField] private ArObject arObject;
	public GameObject cadModel => arObject.cadModel;
	public bool hasModel => arObject != null;
	public GameObject createdArObject { get; private set; }
	public Animation cadAnimation { get; private set; }
	public Camera cadCamera => arObject.camera;

	private ArObjectAnimator _arObjectAnimator = new();
	private ThreeJsRename _threeJsRename = new();
	
	public async UniTask<GameObject> Prepare(string fbxFileName, AnimationRequest animationData) {
		createdArObject = await arObject.PrepareObject(fbxFileName);
		_threeJsRename.Rename(createdArObject);
		cadAnimation = _arObjectAnimator.PrepareAnimation(cadModel, animationData);

		return createdArObject;
	}
}