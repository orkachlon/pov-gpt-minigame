// Script for having a typewriter effect for UI - Version 2
// Prepared by Nick Hwang (https://www.youtube.com/nickhwang)
// Want to get creative? Try a Unicode leading character(https://unicode-table.com/en/blocks/block-elements/)
// Copy Paste from page into Inspector

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITypeWriterEffect : MonoBehaviour
{
	[SerializeField] private Text text;
	[SerializeField] private TMP_Text tmpProText;
	private string _writer;
	[SerializeField] private Coroutine _coroutine;

	[SerializeField] private float delayBeforeStart = 0f;
	[SerializeField] private float timeBtwChars = 0.1f;
	[SerializeField] private string leadingChar = "";
	[SerializeField] private bool leadingCharBeforeDelay = false;
	[Space(10)] [SerializeField] private bool startOnEnable = false;
	
	[Header("Collision-Based")]
	[SerializeField] private bool clearAtStart = false;
	[SerializeField] private bool startOnCollision = false;

	private enum Options {Clear, Complete}
	[SerializeField] private Options collisionExitOptions;

	// Use this for initialization
	private void Awake()
	{
		if(text != null)
		{
			_writer = text.text;
		}
		
		if (tmpProText != null)
		{
			_writer = tmpProText.text;
		}
	}

	private void Start()
	{
		if (!clearAtStart ) return;
		if(text != null)
		{
			text.text = "";
		}
		
		if (tmpProText != null)
		{
			tmpProText.text = "";
		}
	}

	private void OnEnable()
	{
		print("On Enable!");
		if(startOnEnable) StartTypewriter();
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		print("Collision!");
		if (startOnCollision)
		{
			StartTypewriter();
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		if (collisionExitOptions == Options.Complete)
		{
			if (text != null)
			{
				text.text = _writer;
			}

			if (tmpProText != null)
			{
				tmpProText.text = _writer;
			}
		}
		// clear
		else
		{
			if (text != null)
			{
				text.text = "";
			}

			if (tmpProText != null)
			{
				tmpProText.text = "";
			}
		}
		
		StopAllCoroutines();
	}


	private void StartTypewriter()
	{
		StopAllCoroutines();

		if(text != null)
		{
			text.text = "";

			StartCoroutine("TypeWriterText");
		}

		if (tmpProText == null)
			return;
		tmpProText.text = "";

		StartCoroutine("TypeWriterTMP");
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private IEnumerator TypeWriterText()
	{
		text.text = leadingCharBeforeDelay ? leadingChar : "";

		yield return new WaitForSeconds(delayBeforeStart);

		foreach (var c in _writer)
		{
			if (text.text.Length > 0)
			{
				text.text = text.text.Substring(0, text.text.Length - leadingChar.Length);
			}
			text.text += c;
			text.text += leadingChar;
			yield return new WaitForSeconds(timeBtwChars);
		}

		if(leadingChar != "")
        {
			text.text = text.text.Substring(0, text.text.Length - leadingChar.Length);
		}

		yield return null;
	}

	private IEnumerator TypeWriterTMP()
    {
	    tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

		foreach (var c in _writer)
		{
			if (tmpProText.text.Length > 0)
			{
				tmpProText.text = tmpProText.text[..^leadingChar.Length];
			}
			tmpProText.text += c;
			tmpProText.text += leadingChar;
			yield return new WaitForSeconds(timeBtwChars);
		}

		if (leadingChar != "")
		{
			tmpProText.text = tmpProText.text[..^leadingChar.Length];
		}
	}
}