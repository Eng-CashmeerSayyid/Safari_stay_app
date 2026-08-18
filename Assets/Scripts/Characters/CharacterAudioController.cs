using UnityEngine;

public class CharacterAudioController : MonoBehaviour
{
	[SerializeField] private Vector2 pitchRange = new Vector2(-.2f, .2f);
	[SerializeField] private AudioSource footsteps;

	private void Start() {
		RandomizeFootstepPitch();
	}

	private void RandomizeFootstepPitch() {
		footsteps.pitch = 1f + Random.Range(pitchRange.x, pitchRange.y);
	}

	public void FootstepSfx(bool play) {
		if(!play) {
			footsteps.Stop();
			return;
		}

		if(!footsteps.isPlaying ) {
			footsteps.Play();
		}
	}
}