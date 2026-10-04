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
		// Drive the velocity directly so the player has a constant, controllable speed.
                // (AddForce + VelocityChange used to accumulate force every physics step, which
                // made the character accelerate up to very high speeds.)
                Vector3 targetVelocity = inputVector * speed;
                rigidBody.velocity = Vector3.MoveTowards(rigidBody.velocity, targetVelocity, speed * 4f * Time.fixedDeltaTime);
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
