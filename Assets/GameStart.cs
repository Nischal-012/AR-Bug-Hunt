using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public TMP_InputField tmpInputField;
    public void EnterButton()
	{
		if (tmpInputField.text == "123456789")
		{
			SceneManager.LoadScene("MainMenu");
		}
	}
}
