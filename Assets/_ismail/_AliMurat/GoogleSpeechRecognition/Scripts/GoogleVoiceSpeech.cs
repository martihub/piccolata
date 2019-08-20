//	Copyright (c) 2016 steele of lowkeysoft.com
//        http://lowkeysoft.com
//
//	This software is provided 'as-is', without any express or implied warranty. In
//	no event will the authors be held liable for any damages arising from the use
//	of this software.
//
//	Permission is granted to anyone to use this software for any purpose,
//	including commercial applications, and to alter it and redistribute it freely,
//	subject to the following restrictions:
//
//	1. The origin of this software must not be misrepresented; you must not claim
//	that you wrote the original software. If you use this software in a product,
//	an acknowledgment in the product documentation would be appreciated but is not
//	required.
//
//	2. Altered source versions must be plainly marked as such, and must not be
//	misrepresented as being the original software.
//
//	3. This notice may not be removed or altered from any source distribution.
//
//  =============================================================================
//
// Acquired from https://github.com/steelejay/LowkeySpeech
//
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class GoogleVoiceSpeech : MonoBehaviour
{

    int listenDuration = 2;
    public Text text;
    const int HEADER_SIZE = 44;
    private int minFreq;
    private int maxFreq;
    private bool micConnected = false;
    private AudioSource goAudioSource;
    public string apiKey;

    public static GoogleVoiceSpeech instance;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    void Start()
    {
        if (Microphone.devices.Length <= 0)
        {
            Debug.LogWarning("Microphone not connected!");
        }
        else //At least one microphone is present
        {
            micConnected = true;
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);
            if (minFreq == 0 && maxFreq == 0)
            {
                maxFreq = 44100;
            }
            goAudioSource = this.GetComponent<AudioSource>();
        }
    }

    public void MicStart()
    {
        if (micConnected)
        {
            if (!Microphone.IsRecording(null))
            {
                goAudioSource.clip = Microphone.Start(null, true, listenDuration, maxFreq); //Currently set for a 7 second clip
                Debug.Log("mic started");
            }
        }
        StartCoroutine(SendRecord());
    }

    IEnumerator SendRecord()
    {
        yield return new WaitForSeconds(listenDuration);
        MicStopAndSend();
    }

    public void MicStopAndSend()
    {
        float filenameRand = UnityEngine.Random.Range(0.0f, 10.0f);
        string filename = "testing" + filenameRand;
        Microphone.End(null); //Stop the audio recording
        if (!filename.ToLower().EndsWith(".wav"))
        {
            filename += ".wav";
        }
        // var filePath = Path.Combine("testing/", filename);
        //filePath = Path.Combine(Application.persistentDataPath, filePath);
        // Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        string filePath = Application.streamingAssetsPath + filename;
        SavWav.Save(filePath, goAudioSource.clip); //Save a temporary Wav File
        string apiURL = "https://speech.googleapis.com/v1/speech:recognize?&key=" + apiKey;
        string Response;
        Response = HttpUploadFile(apiURL, filePath, "file", "audio/wav; rate=44100");
        File.Delete(filePath); //Delete the Temporary Wav file
    }


    //void OnGUI()
    //{
    //    if (micConnected)
    //    {
    //        //If the audio from any microphone isn't being recorded
    //        if (!Microphone.IsRecording(null))
    //        {
    //            //Case the 'Record' button gets pressed
    //            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "Record"))
    //            {
    //                //Start recording and store the audio captured from the microphone at the AudioClip in the AudioSource
    //                goAudioSource.clip = Microphone.Start(null, true, 7, maxFreq); //Currently set for a 7 second clip
    //                Debug.Log("Recording Started");
    //            }
    //        }
    //        else //Recording is in progress
    //        {

    //            //Case the 'Stop and Play' button gets pressed
    //            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "Stop and Play!"))
    //            {
    //                float filenameRand = UnityEngine.Random.Range(0.0f, 10.0f);

    //                string filename = "testing" + filenameRand;

    //                Microphone.End(null); //Stop the audio recording

    //                Debug.Log("Recording Stopped");

    //                if (!filename.ToLower().EndsWith(".wav"))
    //                {
    //                    filename += ".wav";
    //                }

    //                var filePath = Path.Combine("testing/", filename);
    //                filePath = Path.Combine(Application.persistentDataPath, filePath);
    //                Debug.Log("Created filepath string: " + filePath);

    //                // Make sure directory exists if user is saving to sub dir.
    //                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
    //                SavWav.Save(filePath, goAudioSource.clip); //Save a temporary Wav File
    //                Debug.Log("Saving @ " + filePath);
    //                //Insert your API KEY here.
    //                string apiURL = "https://speech.googleapis.com/v1/speech:recognize?&key=" + apiKey;
    //                string Response;

    //                Debug.Log("Uploading " + filePath);
    //                Response = HttpUploadFile(apiURL, filePath, "file", "audio/wav; rate=44100");

    //                File.Delete(filePath); //Delete the Temporary Wav file
    //            }
    //            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 25, 200, 50), "Recording in progress...");
    //        }
    //    }
    //    else // No microphone
    //    {
    //        //Print a red "Microphone not connected!" message at the center of the screen
    //        GUI.contentColor = Color.red;
    //        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "Microphone not connected!");
    //    }
    //}

    public string HttpUploadFile(string url, string file, string paramName, string contentType)
    {
        System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
        Byte[] bytes = File.ReadAllBytes(file);
        String file64 = Convert.ToBase64String(bytes, Base64FormattingOptions.None);
        try
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{ \"config\": { \"languageCode\" : \"en-US\" }, \"audio\" : { \"content\" : \"" + file64 + "\"}}";
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                ShowResult(result);
            }
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
        }
        return "empty";
    }

    public void ShowResult(string str)
    {
        MicInput.instance.RestartMic();
        var jsonresponse = SimpleJSON.JSON.Parse(str);
        string resultString = jsonresponse["results"][0].ToString();
        var jsonResults = SimpleJSON.JSON.Parse(resultString);
        string transcripts = jsonResults["alternatives"][0]["transcript"].ToString();
        if (transcripts != null) text.text = transcripts;


    }
}

struct ClipData
{
    public int samples;
}