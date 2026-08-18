using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneHandler : MonoBehaviour
{
	[SerializeField] private Button continueButton;

	private void Awake() {
        continueButton.interactable = PlayerPrefs.HasKey(SaveSystem.SAVE_KEY);
    }

	public void OnNewGameClicked() {
		PlayerPrefs.DeleteAll();
		SceneManager.LoadScene(1);
	}

	public void OnContinueClicked() {
		SceneManager.LoadScene(1);
	}
}