using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class LoadingScreen : MonoBehaviour
{
	public TMPro.TMP_Text progressInfo;
	public Gauge gauge;

	private Animator _animator { get; set; }
	private Animator animator { get { return this._animator ?? (this._animator = this.GetComponent<Animator>()); } }

	private AsyncOperation loadingOperation;

	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		App.instance.OnSceneChange += this.HandleSceneChange;
	}


	private void Update()
	{
		if (loadingOperation != null)
		{
			this.progressInfo.text = (int)(loadingOperation.progress * 100) + "%";
			this.gauge.SetProgress(this.loadingOperation.progress);
			if (this.loadingOperation.isDone)
			{
				this.animator.SetTrigger("FadeOut");
				this.loadingOperation = null;
			}
		}
	}

	private void HandleSceneChange(AsyncOperation asOp)
	{
		this.loadingOperation = asOp;
		this.animator.SetTrigger("FadeIn");
	}
}
