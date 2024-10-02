using UnityEngine;
using TMPro;
using System.Collections;
using System;
public class DialogManager : MonoBehaviour
{
	[SerializeField] GameObject dialogBox;
	[SerializeField] TextMeshProUGUI dialogText;
	[SerializeField] int lettersPerSecond;

	public event Action OnShowDialog;
	public event Action OnHideDialog;

	private int currentLine = 0;
	private Dialog dialog;
	private bool isTyping;

	public static DialogManager Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
		dialogBox.SetActive(false);
	}

	public void HandleUpdate()
	{
		if (Input.GetKeyDown(KeyCode.E) && !isTyping)
		{
			++currentLine;

			if (currentLine < dialog.Lines.Count)
			{
				StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
			}
			else
			{
				dialogBox.SetActive(false);
				OnHideDialog?.Invoke();
				currentLine = 0;
			}
		}
	}

	public IEnumerator ShowDialog(Dialog dialog)
	{
		yield return new WaitForEndOfFrame();
		OnShowDialog?.Invoke();

		this.dialog = dialog;
		dialogBox.SetActive(true);
		StartCoroutine(TypeDialog(dialog.Lines[0]));
	}

	public IEnumerator TypeDialog(string line)
	{
		dialogText.text = "";
		isTyping = true;

		// Show text one character at a time
		foreach (var letter in line.ToCharArray())
		{
			dialogText.text += letter;
			yield return new WaitForSeconds(1f / lettersPerSecond);
		}

		isTyping = false;
	}
}
