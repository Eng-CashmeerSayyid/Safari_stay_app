using UnityEngine;

public class SwipeUp : MonoBehaviour
{
	private RectTransform rectTransform;

	private void Awake() {
		rectTransform = GetComponent<RectTransform>();
	}

	private void OnEnable() {
		Animate();
	}

	private void Animate() {
		const float ANIMATION_TIME = .4f;
		LTDescr tween = LeanTween.value(gameObject, 0, 2000f, ANIMATION_TIME);
		tween.setEase(LeanTweenType.easeOutQuad);
		tween.setOnUpdate((float newValue) => {
			rectTransform.anchoredPosition = new Vector2(0, newValue);
		});
		tween.setOnComplete(() => Destroy(gameObject));
	}
}