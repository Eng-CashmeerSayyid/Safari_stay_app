using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationController : MonoBehaviour
{
	private Animator animator;

	private void Awake() {
		animator = GetComponent<Animator>();
	}

	public void SetSpeed(float speed) {
		animator.SetFloat("Speed", speed);
	}
}