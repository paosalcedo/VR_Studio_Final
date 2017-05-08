using UnityEngine;
using System.Collections;
using UnityEngine.Audio; // required for dealing with audiomixers

[RequireComponent(typeof(AudioSource))]
public class MicrophoneListener : MonoBehaviour
{
    public bool startRecordingAtStart = true;

    //allows start and stop of listener at run time within the unity editor
    public bool stopMic = false;
    public bool startMic = false;

    private bool micOn = false;

    //public to allow temporary listening over the speakers if you want of the mic output
    public bool disableOutput = false; 
 
     //an audio source also attached to the same object as this script is
     AudioSource src;

    public AudioMixer masterMixer;

    float timeSinceRestart = 0;

    void Start()
    {
        //start the microphone listener
        if (startRecordingAtStart)
        {
            RestartMic();
            StartMicrophoneListener();
        }
    }

    void Update()
    {
        if (stopMic)
        {
            StopMicrophoneListener();
        }
        if (startMic)
        {
            StartMicrophoneListener();
        }
        //reset paramters to false because only want to execute once
        stopMic = false;
        startMic = false;

        PlayMicInSource(micOn);
        DisableOutput(!disableOutput);
    }


    //stops everything and returns audioclip to null
    public void StopMicrophoneListener()
    {
        micOn = false;
        disableOutput = false;
        src.Stop();
        src.clip = null;
        Microphone.End(null);
    }

    public void StartMicrophoneListener()
    {
        micOn = true;
        disableOutput = false;
        RestartMic();
    }

    //controls whether the volume is on or off, use "off" for mic input (dont want to hear your own voice input!) 
    public void DisableOutput(bool SoundOn)
    {
        float volume = 0;
        if (SoundOn)
        {
            volume = 0.0f;
        }
        else
        {
            volume = -80.0f;
        }
        masterMixer.SetFloat("MasterVolume", volume);
    }



    // restart microphone removes the clip from the audiosource
    public void RestartMic()
    {
        src = GetComponent<AudioSource>();
        src.clip = null;
        timeSinceRestart = Time.time;
    }

    //puts the mic into the audiosource
    void PlayMicInSource(bool MicrophoneListenerOn)
    {
        if (MicrophoneListenerOn)
        {
            //pause a little before setting clip to avoid lag and bugginess
            if (Time.time - timeSinceRestart > 0.5f && !Microphone.IsRecording(null))
            {
                src.clip = Microphone.Start(null, true, 10, 44100);
                //wait until microphone position is found (?)
                while (!(Microphone.GetPosition(null) > 0))
                {
                }
                src.Play(); // Play the audio source
            }
        }
    }

}