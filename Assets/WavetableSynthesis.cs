using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class WavetableSynthesis : MonoBehaviour
{
    public int wavetableSize = 1024;
    public float frequency = 440f;
    public float sampleRate = 44100;
    public float amplitude = 0.5f;

    [Range(0,1)]
    public float[] Amplitudes = {0.9f,0.57f,0.9f,0.94f,0.94f,0.73f,0.63f,0.7f,0.87f,0.77f};

    public WaveType waveType = WaveType.Sine;

    private AudioSource audioSource;
    private float[] wavetable;
    private float phase;
    private float[] wavetableSeno;
    private float[] wavetableSquare;
    private float[] wavetableTriangle;
    private float[] wavetableSawtooth;

    // public TMP_Dropdown waveformDropdown; 

    public enum WaveType
    {
        Sine,
        Square,
        Triangle,
        Sawtooth
    }

    private string csvFileName = "audio_data_guitar.csv";
    public float[] valoresAmplitud;

    private void Start()
    {
        // Se lee el CSV
        string csvPath = Application.dataPath + "/" + csvFileName;
        if (File.Exists(csvPath))
        {
            List<string> lines = new List<string>(File.ReadAllLines(csvPath));
            List<float> timeValues = new List<float>();
            List<float> amplitudeValues = new List<float>();

            // Crear una cultura específica que use el punto como separador decimal
            CultureInfo culture = new CultureInfo("en-US"); 

            for (int i = 1; i < lines.Count; i++) // Ignorar la primera línea de encabezado
            {
                // Identifica separador
                string[] values = lines[i].Split(',');

                // Especifica la cultura
                float time = float.Parse(values[0], culture); 
                float amplitude = float.Parse(values[1], culture);

                timeValues.Add(time);
                amplitudeValues.Add(amplitude);
            }
            // Convierte las listas a arreglos
            float[] timeArray = timeValues.ToArray();
            valoresAmplitud = amplitudeValues.ToArray();
        }
        else{
            Debug.Log("No ta el archivo unu");
        }

        //Se generan los wavetables
        GenerateWavetableSeno();
        GenerateWavetableSquare();
        GenerateWavetableTriangle();
        GenerateWavetableSawtooth();

        //Se crea un audioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0;
        audioSource.Stop();

        // int waveformIndex = (int)waveType;
        // waveformDropdown.value = waveformIndex;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(!audioSource.isPlaying)
            {
                frequency=261.626f;
                ADSRindex = 0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(!audioSource.isPlaying)
            {
                frequency=277.183f;
                ADSRindex = 0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(!audioSource.isPlaying)
            {
                frequency=293.665f;
                ADSRindex = 0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!audioSource.isPlaying)
            {
                frequency=311.127f;
                ADSRindex = 0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if(!audioSource.isPlaying)
            {
                frequency=329.628f;
                ADSRindex = 0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(!audioSource.isPlaying)
            {
                frequency=349.228f;
                ADSRindex = 0;
                audioSource.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(!audioSource.isPlaying)
            {
                frequency=369.994f;
                ADSRindex = 0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if(!audioSource.isPlaying)
            {
                frequency=391.995f;
                ADSRindex = 0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if(!audioSource.isPlaying)
            {
                frequency=415.305f;
                ADSRindex = 0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            if(!audioSource.isPlaying)
            {
                frequency=440.000f;
                ADSRindex = 0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            if(!audioSource.isPlaying)
            {
                frequency=466.164f;
                ADSRindex = 0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if(!audioSource.isPlaying)
            {
                frequency=493.883f;
                ADSRindex = 0;
                audioSource.Play();
            }
    
        }
    
        if (Input.GetKeyUp(KeyCode.A)||Input.GetKeyUp(KeyCode.W)||Input.GetKeyUp(KeyCode.S)||Input.GetKeyUp(KeyCode.E)||Input.GetKeyUp(KeyCode.D)||Input.GetKeyUp(KeyCode.F)||Input.GetKeyUp(KeyCode.T)||Input.GetKeyUp(KeyCode.G)||Input.GetKeyUp(KeyCode.Y)||Input.GetKeyUp(KeyCode.H)||Input.GetKeyUp(KeyCode.U)||Input.GetKeyUp(KeyCode.J))
        {
            audioSource.Stop();
        }
    }

    private float CreateSeno(int timeindex, float frecuencia)
    {
        return Mathf.Sin(2f * Mathf.PI * timeindex * frecuencia / sampleRate);
    }

    private void GenerateWavetableSeno()
    {
        wavetableSeno = new float[wavetableSize];
        float f = sampleRate / wavetableSize;
        for (int i = 0; i < wavetableSize; i++)
        {
            wavetableSeno[i] = CreateSeno(i, f);
        }
    }

    private float CreateSquare(int timeindex, float frecuencia)
    {
        return Mathf.Sign(Mathf.Sin(2 * Mathf.PI * timeindex * frecuencia / sampleRate));
    }

    private void GenerateWavetableSquare()
    {
        wavetableSquare = new float[wavetableSize];
        float f = sampleRate / wavetableSize;
        for (int i = 0; i < wavetableSize; i++)
        {
            wavetableSquare[i] = CreateSquare(i, f);
        }
    }
      
    private float CreateTriangle(int timeIndex, float frecuencia)
    {
        int Tm = (int)(sampleRate/frecuencia);
        float m1 = 1 / ((Tm / 4.0f));
        float m2 = -2 / ((Tm * (3 / 4.0f)) - ((Tm / 4.0f)));
        float m3 = 1 / (Tm - ((Tm * 3) / 4.0f));

        float b1 = 1 - (m1 * (Tm / 4));
        float b2 = 1 - (m2 * (Tm / 4));
        float b3 = 0 - (m3 * Tm);
        int x = timeIndex - ((int)(timeIndex / Tm) * Tm);

        if (x <= (Tm / 4))
            return (m1 * x + b1);
        else if (x > (Tm / 4) && x <= ((Tm * 3) / 4))
            return (m2 * x + b2);
        else
            return (m3 * x + b3);
    }

    private void GenerateWavetableTriangle()
    {
        wavetableTriangle = new float[wavetableSize];
        float f = sampleRate / wavetableSize;
        for (int i = 0; i < wavetableSize; i++)
        {
            wavetableTriangle[i] = CreateTriangle(i, f);
        }
    }

    private float CreateSawTooth(int timeIndex, float frecuencia)
    {
        int Tm = (int)(sampleRate/frecuencia);
        float m1 = 1 / ((Tm / 2.0f));
        float m2 = 1 / (Tm - ((Tm) / 2.0f));

        float b1 = 1 - (m1 * (Tm / 2));
        float b2 = 0 - (m2 * Tm);

        int x = timeIndex - ((int)(timeIndex / Tm) * Tm);

        if (x <= (Tm / 2))
            return (m1 * x + b1);
        else
            return (m2 * x + b2);
    }

    private void GenerateWavetableSawtooth()
    {
        wavetableSawtooth = new float[wavetableSize];
        float f = sampleRate / wavetableSize;
        for (int i = 0; i < wavetableSize; i++)
        {
            wavetableSawtooth[i] = CreateSawTooth(i, f);
        }
    }

    int nA = 10;
    int ADSRindex = 0;

    private void OnAudioFilterRead(float[] data, int channels)
    {
        // ADSRindex = 0;
        for (int i = 0; i < data.Length; i += channels)
        {   
            float currentSample = 0.0f;

            for(int n = 1; n <= nA; n++)
            {
                switch (waveType)
                {
                    case WaveType.Sine:
                        currentSample += wavetableSeno[(int)(phase * wavetableSize)] * Amplitudes[n-1];
                        break;
                    case WaveType.Square:
                        currentSample += wavetableSquare[(int)(phase * wavetableSize)] * Amplitudes[n-1];
                        break;
                    case WaveType.Triangle:
                        currentSample += wavetableTriangle[(int)(phase * wavetableSize)] * Amplitudes[n-1];
                        break;
                    case WaveType.Sawtooth:
                        currentSample += wavetableSawtooth[(int)(phase * wavetableSize)] * Amplitudes[n-1];
                        break;
                }
            }

            float E;

            if (ADSRindex < valoresAmplitud.Length)
            {
                E = valoresAmplitud[ADSRindex];
            }
            else{
                E = 0f;
            }

            for (int channel = 0; channel < channels; channel++)
            {
                data[i + channel] = currentSample * E / (float)nA;
            }

            phase += frequency / sampleRate;

            if (phase > 1f)
            {
                phase -= 1f;
            }

            ADSRindex++;
        }
    }

    public void ChangeWaveform(int index)
    {
        waveType = (WaveType)index;
    }
}

