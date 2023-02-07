using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
	[SerializeField] GameObject background_1;
	[SerializeField] GameObject background_2;
	[SerializeField] GameObject areYouSure;
	[SerializeField] GameObject pauseButton;
	[SerializeField] GameObject playButton;
	[SerializeField] AudioSource audioSource;
	[SerializeField] TMP_Text currentTime;
	[SerializeField] TMP_Text finalTime;
	[SerializeField] Slider musicLength;
	[SerializeField] AudioClip myAudioClip;
	[SerializeField] TMP_InputField teamName;
	[SerializeField] TMP_InputField teamLead;

	private void Start()
	{
		EndTime();
		if (audioSource.clip == null)
		{
			audioSource.clip = myAudioClip;
		}
	}

	void Update()
	{
		GetTime();
		if (audioSource.isPlaying)
		{
			musicLength.value = audioSource.time / audioSource.clip.length;
		}
		if (audioSource.clip.length == audioSource.time)
		{
			audioSource.time = 0;
			musicLength.value = 0;
			playButton.SetActive(true);
			pauseButton.SetActive(false);
		}
	}
	public void OnSliderChange()
	{
		audioSource.time = Mathf.Clamp(musicLength.value * audioSource.clip.length, 0, audioSource.clip.length);
	}

	public void OpenAreYouSure()
	{
		areYouSure.SetActive(true);
	}

	public void CloseAreYouSure()
	{
		areYouSure.SetActive(false);
	}

	public void StartGame()
	{
		SceneManager.LoadScene("Game");
	}

	public void PlayAudio()
	{
		if (!audioSource.isPlaying) audioSource.Play();
	}

	public void PauseAudio()
	{
		audioSource.Pause();
	}
	public void GetTime()
	{
		float curTime = audioSource.time;

		int minutes = Mathf.FloorToInt(curTime / 60);
		int seconds = Mathf.FloorToInt(curTime % 60);

		string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

		currentTime.text = timeString;
	}
	public void EndTime()
	{
		float totalTime = audioSource.clip.length;

		int minutes = Mathf.FloorToInt(totalTime / 60);
		int seconds = Mathf.FloorToInt(totalTime % 60);

		string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

		finalTime.text = timeString;
	}
	public void SaveTeamName()
	{
		string path = Application.persistentDataPath + "bugResult.txt";
		if (!(File.Exists(path)))
		{
			string content = "Team Name:" + teamName.text + "\n Team Lead:" + teamLead.text + "\n";
			File.WriteAllText(path, content);
		}

	}
}
