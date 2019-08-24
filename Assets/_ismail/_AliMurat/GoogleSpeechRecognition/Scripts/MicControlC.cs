using UnityEngine;
using System.Collections;

//The GameObject requires a AudioSource component
[RequireComponent(typeof(AudioSource))]

public class MicControlC : MonoBehaviour
{

    private string selectedDevice;
    private int minFreq = 0;
    private float ListenerDistance;
    private Vector3 ListenerPosition;
    private bool micSelected = false;
    private bool recording = true;
    private bool focused = false;
    private bool Initialised = false;

    private float[] freqData;
    private int nSamples = 1024;
    private float fMax;

    private int position = 0;
    private int sampleRate = 0;
    private float frequency = 440;
    private int fallbackMaxFreq = 44100;
    AudioSource audioSource;

    MicControlD micControlD;

    //if false the below will override and set the mic selected in the editor
    //Select the microphone you want to use (supported up to 6 to choose from). If the device has number 1 in the console, you should select default as it is the first defice to be found.
    public enum Devices
    {
        DefaultDevice,
        Second,
        Third,
        Fourth,
        Fifth,
        Sixth
    }

    public Devices InputDevice;
    public Transform audioListener;
    public float loudness = 0.0f;
    public int amountSamples = 256;
    public int maxFreq = 44100;//48000;
    public float sensitivity = 0.4f;
    public float sourceVolume = 100f;
    public bool SelectIngame = false;
    public bool ThreeD = false;
    public float VolumeFallOff = 1.0f;
    public float PanThreshold = 1.0f;
    public bool Mute = true;
    public bool debug = false;
    public bool ShowDeviceName = false;

    public static MicControlC instance;

    private void Awake()
    {
        if (!instance) instance = this;
    }

    void Start()
    {
        if (!audioSource)
        {
            audioSource = GetComponent<AudioSource>();
            //audioSource.mute = true;
            audioSource.playOnAwake = false;
        }
        InitMic();
        Initialised = true;
        micControlD = GetComponent<MicControlD>();
    }

    void Update()
    {


        if (!Application.isPlaying)
        {
            StopMicrophone();
            Initialised = false;
        }
        else
        {
            if (!Initialised)
            {
                InitMic();
                Initialised = true;
            }
        }

        loudness = GetDataStream();
        if (sourceVolume > 100)
        {
            sourceVolume = 100;
        }

        if (sourceVolume < 0)
        {
            sourceVolume = 0;
        }
        audioSource.volume = (sourceVolume / 100);
    }

    float GetDataStream()
    {
        float[] samples = new float[amountSamples]; //Converts to a float
                                                    //float[] samples = new float[audioSource.clip.samples * audioSource.clip.channels];

        audioSource.clip.GetData(samples, 0);
        return Sum(samples) / amountSamples;

    }


    private float Sum(params float[] samples)
    {
        float result = 0.0f;
        for (int i = 0; i < samples.Length; i++)
        {
            result += Mathf.Abs(samples[i]);
        }
        return result;
    }

    private float Average(params float[] samples)
    {
        float sum = Sum(samples);
        float result = (float)sum / samples.Length;
        return result;
    }



    private void InitMic()
    {
        selectedDevice = Microphone.devices[0];
        audioSource.clip = Microphone.Start(selectedDevice, true, 5, maxFreq);
        audioSource.loop = true;
        audioSource.Play();
        recording = true;
    }

    public void StartMicrophone()
    {
        //Starts recording
        audioSource.clip = Microphone.Start(selectedDevice, true, 5, maxFreq);

        if (debug)
        {
            Debug.Log("Selected device: " + selectedDevice);
        }

        // Wait until the recording has started
        while (!(Microphone.GetPosition(selectedDevice) > 0))
        {
            if (debug)
            {
                Debug.Log("Waiting on recording to start...");
            }
        }

        if (debug)
        {
            Debug.Log("Playing the recorded audio...");
        }
        // Play the audio recording
        audioSource.Play();
    }

    public void StopMicrophone()
    {
        if (debug)
        {
            Debug.Log("Stopping the microphone...");
        }

        //Stops the audio
        audioSource.Stop();

        //Stops the recording of the device
        Microphone.End(selectedDevice);

    }

    void GetMicCaps()
    {
        //Gets the frequency of the device
        Microphone.GetDeviceCaps(selectedDevice, out minFreq, out maxFreq);

        //These 2 lines of code are mainly for windows computers
        if ((minFreq + maxFreq) == 0)
        {
            maxFreq = fallbackMaxFreq;
        }
    }


    /*
     * Create a gui button in another script that calls to this script
     */
    public void MicDeviceGUI(float left, float top, float width, float height, float buttonSpaceTop, float buttonSpaceLeft)
    {
        //If there is more than one device, choose one.
        if (Microphone.devices.Length > 1 && micSelected == false)
        {
            for (int i = 0; i < Microphone.devices.Length; ++i)
            {
                if (GUI.Button(new Rect(left + (buttonSpaceLeft * i), top + (buttonSpaceTop * i), width, height), Microphone.devices[i].ToString()))
                {
                    StopMicrophone();
                    selectedDevice = Microphone.devices[i].ToString();
                    GetMicCaps();
                    StartMicrophone();
                    micSelected = true;
                }
            }
        }

        //If there is only 1 microphone make it default
        if (Microphone.devices.Length < 2 && micSelected == false)
        {
            selectedDevice = Microphone.devices[0].ToString();
            GetMicCaps();
            micSelected = true;
        }
    }

    /*
     * Flush the data through the custom created audio clip. This controls the data flow of that clip
     * Creates a 1 sec long audioclip, with a 440hz sinoid
     */

    public Transform tr;
    public float f2 = 0;
    void OnAudioFilterRead(float[] data, int channels)
    {

        for (var i = 0; i < data.Length; ++i)
        {

            if (data[i] > f2) f2 = data[i];
        }
    }

    private void FixedUpdate()
    {

        if (f2 > 0) f2 -= Time.deltaTime;

        //tr.position = new Vector3(0, f2 * 10, 0);
        micControlD.micLevel = f2;
    }

    void PCMReaderCallback(float[] data)
    {
        if (debug)
        {
            Debug.Log("PCMReaderCallback()");
            Debug.Log(data);
            Debug.Log("-----");
        }
    }

    void PCMSetPositionCallback(int newPosition)
    {
        if (debug)
        {
            Debug.Log("PCMSetPositionCallback()");
            Debug.Log(newPosition);
            Debug.Log("=====");
        }
        position = newPosition;
    }

    /*
     * Start or stop the script from running when the state is paused or not.
     */
    void OnApplicationFocus(bool focus)
    {
        focused = focus;
    }

    void OnApplicationPause(bool focus)
    {
        focused = focus;
    }

}