using System;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
	public const string SAVE_KEY = "Player progress";

	public static Progress playerProgress;

	private void Awake() {
		LoadProgress();
	}

	public void LoadProgress() {
		playerProgress = JsonUtility.FromJson<Progress>(PlayerPrefs.GetString(SAVE_KEY));
		if(playerProgress == null) {
			playerProgress = new Progress();
		}
	}

	public void SaveProgress() {
		playerProgress.money = MoneyController.Instance.Money;
		string progress = JsonUtility.ToJson(playerProgress);
		PlayerPrefs.SetString(SAVE_KEY, progress);
	}

}

public class Progress {
	public float money;
	public RoomStates[] rooms;

	public Progress() {
		money = 0;
		rooms = new RoomStates[2];
		for (int i = 0; i < rooms.Length; i++) {
			RoomStates room = new RoomStates();
			room.isUnlocked = i == 0;
			room.isOccupied = false;
			room.isDirty = false;
			rooms[i] = room;
		}
	}
}

[Serializable]
public class RoomStates {
	public bool isUnlocked = false;
	public bool isOccupied = false;
	public bool isDirty = false;
}