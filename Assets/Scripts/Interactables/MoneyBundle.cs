using UnityEngine;

public class MoneyBundle : Interactable
{
	private AudioSource collectMoney;
	private BoxCollider boxCollider;
	
	public float money = 20f;

	[SerializeField] private ParticleSystem moneyParticles;

	private void Awake() {
		transform.localScale = Vector3.zero;
		collectMoney = GetComponent<AudioSource>();
		boxCollider = GetComponent<BoxCollider>();
	}

	private void OnEnable() {
		const float SPAWN_TIME = .2f;
		LTDescr tween = LeanTween.scale(gameObject, Vector3.one, SPAWN_TIME);
		tween.setEase(LeanTweenType.easeOutQuart);
		tween.setOnComplete(() => collectMoney.Play());
	}

	protected override void PlayerInteracted(PlayerController player) {
		Destroy(boxCollider);
		ParticleSystem particles = Instantiate(moneyParticles, transform.position, Quaternion.identity);
		particles.Play();
		const float DESTROY_DELAY = 2f;
		Destroy(particles.gameObject, DESTROY_DELAY);
		MoneyController.Instance.AddMoney(money);
		foreach(Transform child in transform) {
			child.gameObject.SetActive(false);
		}
		collectMoney.Play();
		Destroy(gameObject, DESTROY_DELAY);
	}
}