using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayAnimationControl : MonoBehaviour {
	[SerializeField] private UIColorButton playButton;
	[SerializeField] private Image progressImage;

	public event Action PlayClickedEvent;

	private void Awake() {
		playButton.onClick.AddListener(delegate { PlayClickedEvent?.Invoke(); });
	}

	public void Refresh(bool isPlaying, float progress) {
		playButton.isHighlighted = !isPlaying;
		progressImage.fillAmount = progress;
	}
}
