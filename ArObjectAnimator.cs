using JsonDeserialize;
using UnityEngine;

public class ArObjectAnimator {
	private readonly AnimationsMaker _animationsMaker;

	public ArObjectAnimator() {
		_animationsMaker = new();
	}
	
	public Animation PrepareAnimation(GameObject objectForAnimate, AnimationRequest animationData) {
		//var animationRequest = SessionCache.instance.animationData;
		var clip = JsonUtility.FromJson<AnimationClipJson>(animationData.clipJson);
		var effects = JsonUtility.FromJson<AnimationEffectsJson>(animationData.effectsJson);
		var steps = JsonUtility.FromJson<AnimationStepsJson>(animationData.stepsJson);

		/*var clipSting = File.ReadAllText(Application.persistentDataPath + "/" + "GEUPS_SGCE6080_WI00000067_anim.json");
		var effectsString = File.ReadAllText(Application.persistentDataPath + "/" + "GEUPS_SGCE6080_WI00000067_effects.json");
		var marlString = File.ReadAllText(Application.persistentDataPath + "/" + "GEUPS_SGCE6080_WI00000067_mark.json");

		var clip = JsonUtility.FromJson<AnimationClipJson>(clipSting);
		var effects = JsonUtility.FromJson<AnimationEffectsJson>(effectsString);
		var steps = JsonUtility.FromJson<AnimationStepsJson>(marlString);*/

		return _animationsMaker.Create(objectForAnimate, clip, effects, steps);
	}
}
