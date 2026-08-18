using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private CharacterAnimationController animationController;
	private CharacterAudioController audioController;
	private Rigidbody rigidBody;
	private Vector3 inputVector;

	[SerializeField] private float speed = 2.5f;
	[SerializeField] private float rotationDamp = .1f;
	[SerializeField] private Joystick joystick;

	private void Awake() {
		rigidBody = GetComponent<Rigidbody>();
		animationController = GetComponent<CharacterAnimationController>();
		audioController = GetComponent<CharacterAudioController>();
	}

	private void Update() {
		ReadInput();
		RotatePlayer();
		UpdateAnimation();
	}

	private void FixedUpdate() {
		Move();
	}

	private void ReadInput() {
		inputVector = new Vector3(
			Input.GetAxis("Horizontal") + joystick.Horizontal,
			0,
			Input.GetAxis("Vertical") + joystick.Vertical
		).normalized;
	}

	private void Move() {
		rigidBody.AddForce(inputVector * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
	}

	private void RotatePlayer() {
		const float ROTATE_THRESHOLD = 0.2f;
		Quaternion targetRotation = transform.rotation;
		if(inputVector.sqrMagnitude > ROTATE_THRESHOLD) {
			targetRotation = Quaternion.LookRotation(inputVector, Vector3.up);
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationDamp * Time.deltaTime);
	}

	private void UpdateAnimation() {
		if(animationController == null)
			return;

		animationController.SetSpeed(inputVector.sqrMagnitude);
		
		const float SOUND_THRESHOLD = 0.2f;
		audioController.FootstepSfx(inputVector.sqrMagnitude > SOUND_THRESHOLD);
	}
}