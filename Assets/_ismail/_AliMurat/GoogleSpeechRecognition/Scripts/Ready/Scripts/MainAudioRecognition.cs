using UnityEngine;
using System;
using System.IO;
using System.Net;
using UnityEngine.UI;

public class MainAudioRecognition : MonoBehaviour
{
    public Text textBox;
    public Text textBoxLang;
    public Text textRecBox;
    public Text textSentence;
    public string apiKey;

    public static string lang = "English";
    private readonly string lang_code = "\"tr\"";
    private AudioSource goAudioSource;
    private float MicLoudness;
    private bool mute_flag = false;
    private float mute_time = 0.0f;
    private bool recording = false;
    private float recording_time = 0.0f;

    private readonly float sensitivity = 0.25f;

    private int minFreq;
    private int maxFreq;

    public static bool speechOut = false;


    void Start()
    {
        if (Microphone.devices.Length <= 0)
        {
            Debug.LogWarning("Microphone not connected!");
        }
        else
        {
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);
            if (minFreq == 0 && maxFreq == 0)
            {
                maxFreq = 44100;
            }

            goAudioSource = GetComponent<AudioSource>();
            goAudioSource.clip = Microphone.Start(null, true, 60, 44100);
        }
    }

    void Update()
    {
        lang = textBoxLang.text;
        MicLoudness = LevelMax();

        if (Microphone.IsRecording(null))
        {
            if (MicLoudness < sensitivity && !recording)
                textBox.text = "Say something...";
            //if it first hears a loud sound, start recording to save 
            if (MicLoudness >= sensitivity && !recording)
            {
                // Debug.Log(MicLoudness);
                mute_flag = false;
                mute_time = 0.0f;
                Microphone.End(null);
                startRecording();
                recording = true;
                recording_time += Time.deltaTime;
            }
            else if (MicLoudness >= sensitivity && recording)
            {
                textBox.text = "Recording...";
                mute_flag = false;
                mute_time = 0.0f;
                recording_time += Time.deltaTime;
                if (recording_time >= 59.0f)
                    stopRecording();
            }

            else if (MicLoudness < sensitivity && recording && !mute_flag)
            {
                mute_flag = true;
                recording_time += Time.deltaTime;
            }
            //if it hears nothing for one second, stop recording
            else if (MicLoudness < sensitivity && recording && mute_flag)
            {
                mute_time += Time.deltaTime;
                recording_time += Time.deltaTime;
                if (mute_time >= 1.9f)
                    textBox.text = "Recording has stopped.";
                else if (mute_time >= 0.5f && mute_time < 1.9f)
                    textBox.text = "Mute";

                if (mute_time >= 2.0f)
                {
                    textBox.text = "Mute";
                    //Microphone.End(null);
                    stopRecording();

                    mute_time = 0.0f;
                    recording_time = 0.0f;
                    mute_flag = false;
                    recording = false;
                }
            }
            else
            {
                textBox.text = "Say words";
            }
        }
        else
        {
            goAudioSource.clip = Microphone.Start(null, true, 60, 44100);
        }
    }

    void startRecording()
    {
        goAudioSource.clip = Microphone.Start(null, true, 60, maxFreq);
    }

    void stopRecording()
    {
        var position = Microphone.GetPosition(null);
        Microphone.End(null);
        var soundData = new float[goAudioSource.clip.samples * goAudioSource.clip.channels];
        goAudioSource.clip.GetData(soundData, 0);
        var newData = new float[position * goAudioSource.clip.channels];
        for (int i = 0; i < newData.Length; i++)
        {
            newData[i] = soundData[i];
        }
        var newClip = AudioClip.Create(goAudioSource.clip.name, position, goAudioSource.clip.channels, goAudioSource.clip.frequency, false, false);
        newClip.SetData(newData, 0);
        Destroy(goAudioSource.clip);
        goAudioSource.clip = Microphone.Start(null, true, 60, maxFreq);
        float filenameRand = UnityEngine.Random.Range(0.0f, 10.0f);
        string filename = "testing" + filenameRand;
        if (!filename.ToLower().EndsWith(".wav"))
        {
            filename += ".wav";
        }
        var filePath = Path.Combine("testing/", filename);
        filePath = Path.Combine(Application.persistentDataPath, filePath);
        //   Debug.Log("Created filepath string: " + filePath);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        SavWav.Save(filePath, newClip); //Save a temporary Wav File
        string apiURL = "https://speech.googleapis.com/v1/speech:recognize?&key=" + apiKey;
        string Response;
        //Debug.Log("Uploading " + filePath);
        Response = HttpUploadFile(apiURL, filePath, "file", "audio/wav; rate=44100", textBoxLang.text);
        GetFromJson(Response);
        File.Delete(filePath); //Delete the Temporary Wav file
    }

    void GetFromJson(string Response)
    {
        var jsonresponse = SimpleJSON.JSON.Parse(Response);
        if (jsonresponse != null)
        {
            string resultString = jsonresponse["results"][0].ToString();
            var jsonResults = SimpleJSON.JSON.Parse(resultString);
            textSentence.text = "Key Words ";
            string transcripts;
            if (jsonResults == null)
                transcripts = null;
            else
                transcripts = jsonResults["alternatives"][0]["transcript"].ToString();
            if (transcripts == null)
            {
                textRecBox.text = "NULL! Say again";
                //  Debug.Log("NULL");
            }
            else
            {
                if (transcripts == null)
                {
                    textRecBox.text = "NULL";
                    textBox.text = "Say again";
                }
                else
                {
                    textBox.text = "Say again";
                    textRecBox.text = transcripts;
                    speechOut = true;
                    ManageGoogleWords.instance.SetWords(transcripts);
                    // TestTTS.sound_text = transcripts;
                }
            }
        }
    }

    public string HttpUploadFile(string url, string file, string paramName, string contentType, string lang)
    {
        // Debug.Log("Language:" + lang);
        ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
        byte[] bytes = File.ReadAllBytes(file);
        string file64 = Convert.ToBase64String(bytes, Base64FormattingOptions.None);
        try
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{ \"config\": { \"languageCode\" :" + lang_code + "}, \"audio\" : { \"content\" : \"" + file64 + "\"}}";
                //   Debug.Log("json-------" + json);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result;
            }
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            Debug.Log("!!!" + resp);
        }

        return "empty";
    }

    float LevelMax()
    {
        int _sampleWindow = 128;  //Check mic level from 128 samples
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1);
        if (micPosition < 0) return 0;
        goAudioSource.clip.GetData(waveData, micPosition);

        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return Mathf.Sqrt(Mathf.Sqrt(levelMax));
    }

}
