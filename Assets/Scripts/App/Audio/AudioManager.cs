using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
	public static AudioManager instance { get; set; }

	private AudioSource musicSource { get; set; }

	private readonly HashSet<AudioSource> playingSfx = new HashSet<AudioSource>();
	private readonly Queue<AudioSource> availableSfx = new Queue<AudioSource>();

	public AudioClip musicClip;
	public AudioClip buttonClip;

	public float sfxVolume { get; private set; }
	public float musicVolume => this.musicSource.volume;

	[Range(0, 1)] public float defaultMusicVolume = .4f;
	[Range(0, 1)] public float defaultSfxVolume = .6f;

	public void Awake()
	{
		if (instance == null) instance = this;
		if (instance != this) Destroy(this);
		else
		{
			this.musicSource = this.gameObject.AddComponent<AudioSource>();
			this.musicSource.volume = this.defaultMusicVolume;
			this.musicSource.clip = this.musicClip;
			this.musicSource.loop = true;
			this.musicSource.Play();

			this.SetSfxVolume(this.defaultSfxVolume);

			for (int i = 0; i < 5; ++i)
			{
				this.availableSfx.Enqueue(this.CreateNewSfxSource());
			}
		}
	}

	private AudioSource CreateNewSfxSource()
	{
		AudioSource sfxSource = new GameObject("SfxSource").AddComponent<AudioSource>();
		sfxSource.transform.SetParent(this.transform);
		sfxSource.volume = this.sfxVolume;
		sfxSource.loop = false;
		return sfxSource;
	}

	public void PlayButtonSfx() { this.PlaySfx(this.buttonClip); }
	public void PlaySfx(AudioClip clip)
	{
		AudioSource src = this.availableSfx.Count > 0 ? this.availableSfx.Dequeue() : this.CreateNewSfxSource();
		src.clip = clip;
		src.gameObject.SetActive(true);
		src.Play();
		this.playingSfx.Add(src);
	}

	public void Update()
	{
		if (this.playingSfx.Count == 0) return;
		foreach (AudioSource finishedSfx in this.playingSfx.Where(t => !t.isPlaying).ToArray())
		{
			this.playingSfx.Remove(finishedSfx);
			this.availableSfx.Enqueue(finishedSfx);
			finishedSfx.gameObject.SetActive(false);
		}
	}

	public void SetMusicVolume(float value)
	{
		this.musicSource.volume = value;
	}

	public void SetSfxVolume(float value)
	{
		this.sfxVolume = value;
		foreach (AudioSource sfxSource in this.playingSfx.Union(this.availableSfx))
		{
			sfxSource.volume = this.sfxVolume;
		}
	}


}
