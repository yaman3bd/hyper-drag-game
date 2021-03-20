using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Sound
{
    public string Name;
    public AudioClip Audio;
    public float Volume;
    public float Pitch;
    public bool PlayOnAwake;
    [HideInInspector]
    public AudioSource Source;

}
public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;
    public GameObject Template;
    public Sound[] Sounds;
    private void Awake()
    {
        Instance = this;
        foreach (var sound in Sounds)
        {
            var gm = Instantiate(Template, Vector3.zero, Quaternion.identity, this.transform);
            gm.SetActive(true);
            gm.name = sound.Name;
            
            var s = gm.AddComponent<AudioSource>();
            s.clip = sound.Audio;
            s.volume = sound.Volume;
            s.pitch = sound.Pitch;
            s.playOnAwake = sound.PlayOnAwake;
            sound.Source = s;
        }
    }
    public void Play(string name)
    {

        var s = Array.Find(Sounds, sound => sound.Name == name);

        s.Source.Play();
    }

}
