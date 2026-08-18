using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NPC : MonoBehaviour
{
	public enum States { FREE_ROAMING, BEING_CUSTOMER}

	private Vector3 targetPosition;
	private NavMeshAgent agent;
	private CapsuleCollider capsuleCollider;
	private CharacterAnimationController animationController;
	private CharacterAudioController audioController;

	private bool idle = true;

	[SerializeField] private RandomPositionGenerator positionGenerator;
	
	public States CurrentState { get; private set; } = States.FREE_ROAMING;

	public UnityAction OnTargetReached;

	private void Awake() {
		agent = GetComponent<NavMeshAgent>();
		animationController = GetComponent<CharacterAnimationController>();
		audioController = GetComponent<CharacterAudioController>();
		capsuleCollider = GetComponent<CapsuleCollider>();
		OnTargetReached += OnTargetReachedHandler;
	}

	private void OnDestroy() {
		OnTargetReached -= OnTargetReachedHandler;
	}

	private void Update() {
		CheckTargetReached();
		UpdateWalkAnimation();
		if(CurrentState == States.FREE_ROAMING) {
			if(idle) {
				WalkAround();
			}
		}
	}

	private void CheckTargetReached() {
		const float DISTANCE_THRESHOLD = 0.21f;
		if(Vector3.Distance(agent.destination, transform.position) < DISTANCE_THRESHOLD) {
			OnTargetReached?.Invoke();
		}
	}

	private void OnTargetReachedHandler() {
		if(CurrentState == States.FREE_ROAMING) {
			idle = true;
		}
	}
	private void WalkAround() {
		agent.SetDestination(positionGenerator.GetPosition());
		idle = false;
	}

	private void UpdateWalkAnimation() {
		const float SOUND_THRESHOLD = 0.2f;
		float speed = agent.velocity.sqrMagnitude;
		animationController.SetSpeed(speed);
		audioController.FootstepSfx(speed > SOUND_THRESHOLD);
	}

	public void MoveTo(Vector3 destination) {
		CurrentState = States.BEING_CUSTOMER;
		agent.SetDestination(destination);
	}

	public void SetStateFreeRoam() {
		CurrentState = States.FREE_ROAMING;
		OnTargetReached += OnTargetReachedHandler;
	}

	public void FallAsleep(Transform sleepAnchor) {
		agent.enabled = false;
		capsuleCollider.enabled = false;
		transform.position = sleepAnchor.position;
		transform.rotation = sleepAnchor.rotation;
	}

	public void WakeUp() {
		Vector3 pos = transform.position;
		pos.y = 0;
		pos.z -= 1f;
		transform.position = pos;
		transform.eulerAngles = Vector3.up * -180f;
		agent.enabled = true;
		capsuleCollider.enabled = true;
	}
}