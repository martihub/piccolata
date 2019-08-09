using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Net;
using System.IO;

[RequireComponent(typeof(AudioSource))]
public class SplashAudio : MonoBehaviour
{
    public Text textBox;
    public string apiKey;

    private AudioSource goAudioSource;
    private float MicLoudness;
    private bool mute_flag = false;
    private float mute_time = 0.0f;
    private bool recording = false;
    private float recording_time = 0.0f;

    private int minFreq;
    private int maxFreq;

    // Start is called before the first frame update
    // scale the text position fit to the screensize
    void Start()
    {
        Vector2 myVector1 = new Vector2(Screen.width / (-5), Screen.height / 5);
        transform.gameObject.GetComponent<RectTransform>().anchoredPosition = myVector1;
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

            goAudioSource = this.GetComponent<AudioSource>();
            goAudioSource.clip = Microphone.Start(null, true, 600, 44100);
        }
    }



    // Update is called once per frame
    void Update()
    {
        Vector2 myVector1 = new Vector2(Screen.width / (-5), Screen.height / 5);
        transform.gameObject.GetComponent<RectTransform>().anchoredPosition = myVector1;

        MicLoudness = LevelMax();
        if (Microphone.IsRecording(null))
        {
            //if it first hears a loud sound, start recording to save 
            if (MicLoudness >= 0.0002 && !recording)
            {
                Microphone.End(null);
                startRecording();
                recording = true;
                recording_time += Time.deltaTime;
            }
            else if (MicLoudness >= 0.0002f && recording)
            {
                textBox.text = "Recording...";
                mute_flag = false;
                mute_time = 0.0f;
                recording_time += Time.deltaTime;
                if (recording_time >= 3.0f)
                    stopRecording();
            }

            else if (MicLoudness < 0.0002 && recording && !mute_flag)
            {
                mute_flag = true;
            }
            //if it hears nothing for one second, stop recording
            else if (MicLoudness < 0.0002 && recording && mute_flag)
            {
                recording_time += Time.deltaTime;
                textBox.text = "Mute";
                mute_time += Time.deltaTime;
                if (mute_time >= 1.0f || recording_time >=3.0f)
                {
                    stopRecording();
                    mute_time = 0.0f;
                    recording_time = 0.0f;
                }
            }
            else
            {
                textBox.text = "Say start";
            }
        }
        else
        {
            goAudioSource.clip = Microphone.Start(null, true, 600, 44100);
        }
         
        
    }



    //Start recording
    void startRecording()
    {
        goAudioSource.clip = Microphone.Start(null, true, 3, maxFreq);
    }



    //Stop recording and Start audio recognition
    void stopRecording()
    {
        float filenameRand = UnityEngine.Random.Range(0.0f, 10.0f);
        string filename = "testing" + filenameRand;
        Microphone.End(null); //Stop the audio recording
     
        if (!filename.ToLower().EndsWith(".wav"))
        {
            filename += ".wav";
        }

        var filePath = Path.Combine("testing/", filename);
        filePath = Path.Combine(Application.persistentDataPath, filePath);
        Debug.Log("Created filepath string: " + filePath);

        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        SavWav.Save(filePath, goAudioSource.clip); //Save a temporary Wav File
        goAudioSource.clip = Microphone.Start(null, true, 3, maxFreq);

        string apiURL = "https://speech.googleapis.com/v1/speech:recognize?&key=" + apiKey;
        string Response;
        Debug.Log("Uploading " + filePath);
        Response = HttpUploadFile(apiURL, filePath, "file", "audio/wav; rate=44100");
        //Debug.Log("Response String: " + Response);

        var jsonresponse = SimpleJSON.JSON.Parse(Response);

        if (jsonresponse != null)
        {
            string resultString = jsonresponse["results"][0].ToString();
            var jsonResults = SimpleJSON.JSON.Parse(resultString);
            string transcripts = "";
            if (jsonResults == null)
                transcripts = null;
            else
                transcripts = jsonResults["alternatives"][0]["transcript"].ToString();
            if (transcripts == null)
            {
                textBox.text = "NULL! Say again";
                Debug.Log("NULL");
            }
            else
            {
                if (transcripts == "\"start\"")
                {
                    SceneManager.LoadScene("MainGame");
                }
            }
            //File.Delete(filePath); //Delete the Temporary Wav file
        }
    }



    public string HttpUploadFile(string url, string file, string paramName, string contentType)
    {

        System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
        Debug.Log(string.Format("Uploading {0} to {1}", file, url));

        Byte[] bytes = File.ReadAllBytes(file);
        String file64 = Convert.ToBase64String(bytes, Base64FormattingOptions.None);

        Debug.Log(file64);

        try
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{ \"config\": { \"languageCode\" : \"en-US\" , \"speechContexts\": [{\"phrases\": [\"start\"]}]}, \"audio\" : { \"content\" : \"" + file64 + "\"}}";

                //Debug.Log(json);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Debug.Log(httpResponse);

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

    int _sampleWindow = 128;

    float LevelMax()
    {
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
        return levelMax;
    }

}
