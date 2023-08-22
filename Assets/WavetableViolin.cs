using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class WavetableViolin : MonoBehaviour
{
    public int wavetableSize = 1024;
    public float frequency = 440f;
    public float amplitude = 0.5f;
    public WaveType waveType = WaveType.Sine;

    private AudioSource audioSource;
    private float[] wavetable;
    private float phase;
    private float[] wavetableSeno;
    private float[] wavetableSquare;
    private float[] wavetableTriangle;
    private float[] wavetableSawtooth;


    float sampleRate = 44100f;

    public TMP_Dropdown waveformDropdown; 

    private string csvFileName = "audio_data_promedio.csv";
    private float[] valoresAmplitud;
    List<float> amplitudeValues = new List<float>();

    public int ADSRindex = 0;

    public enum WaveType
    {
        Sine,
        Square,
        Triangle,
        Sawtooth
    }

    private void Start()
    {
        // audioSource = GetComponent<AudioSource>();
        // sampleRate = AudioSettings.outputSampleRate;
        string csvPath = Application.dataPath + "/" + csvFileName;
        if (File.Exists(csvPath))
        {
            List<string> lines = new List<string>(File.ReadAllLines(csvPath));
            List<float> timeValues = new List<float>();
            List<float> amplitudeValues = new List<float>(); // No necesitas declarar esto arriba, ya que se creará aquí

            // Crear una cultura específica que use el punto como separador decimal
            CultureInfo culture = new CultureInfo("en-US"); 

            for (int i = 1; i < lines.Count; i++) // Ignorar la primera línea de encabezado
            {
                string[] values = lines[i].Split(',');
                float time = float.Parse(values[0], culture); // Especifica la cultura aquí
                float amplitude = float.Parse(values[1], culture); // Especifica la cultura aquí

                timeValues.Add(time);
                amplitudeValues.Add(amplitude);
            }
            // Convierte las listas a arreglos si lo necesitas
            float[] timeArray = timeValues.ToArray();
            valoresAmplitud= amplitudeValues.ToArray();
        }
        else{
            Debug.Log("No ta el archivo unu");
        }
        GenerateWavetable();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0;
        audioSource.Stop();

        int waveformIndex = (int)waveType;
        waveformDropdown.value = waveformIndex;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(!audioSource.isPlaying)
            {
                frequency=261.626f;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(!audioSource.isPlaying)
            {
                frequency=277.183f;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(!audioSource.isPlaying)
            {
                frequency=293.665f;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!audioSource.isPlaying)
            {
                frequency=311.127f;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if(!audioSource.isPlaying)
            {
                frequency=329.628f;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(!audioSource.isPlaying)
            {
                frequency=349.228f;
                audioSource.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(!audioSource.isPlaying)
            {
                frequency=369.994f;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if(!audioSource.isPlaying)
            {
                frequency=391.995f;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if(!audioSource.isPlaying)
            {
                frequency=415.305f;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            if(!audioSource.isPlaying)
            {
                frequency=440.000f;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            if(!audioSource.isPlaying)
            {
                frequency=466.164f;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if(!audioSource.isPlaying)
            {
                frequency=493.883f;
                audioSource.Play();
            }
    
        }
        
    
        if (Input.GetKeyUp(KeyCode.A)||Input.GetKeyUp(KeyCode.W)||Input.GetKeyUp(KeyCode.S)||Input.GetKeyUp(KeyCode.E)||Input.GetKeyUp(KeyCode.D)||Input.GetKeyUp(KeyCode.F)||Input.GetKeyUp(KeyCode.T)||Input.GetKeyUp(KeyCode.G)||Input.GetKeyUp(KeyCode.Y)||Input.GetKeyUp(KeyCode.H)||Input.GetKeyUp(KeyCode.U)||Input.GetKeyUp(KeyCode.J))
        {
            audioSource.Stop();
        }
    }

    private void GenerateWavetable()
    {
        wavetableSeno = new float[wavetableSize];
        wavetableSquare = new float[wavetableSize];
        wavetableTriangle = new float[wavetableSize];
        wavetableSawtooth = new float[wavetableSize];

        for (int i = 0; i < wavetableSize; i++)
        {
            float t = (float)i / wavetableSize;
            wavetableSeno[i] = Mathf.Sin(2f * Mathf.PI * t);
            wavetableSquare[i] = Mathf.Sign(Mathf.Sin(2f * Mathf.PI * t));
            wavetableTriangle[i] = 2f * Mathf.Abs(2f * t - 1f) - 1f;
            wavetableSawtooth[i] = 2f * t - 1f;
        }
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        // phase = 0.0f;
        ADSRindex = 0;
        for (int i = 0; i < data.Length; i += channels)
        {   
            if (ADSRindex<valoresAmplitud.Length)
            {
                amplitude = valoresAmplitud[ADSRindex];
            }
            else{
                amplitude = 0f;
            }
            float currentSample = 0.0f;
            switch (waveType)
            {
                case WaveType.Sine:
                    currentSample = wavetableSeno[(int)(phase * wavetableSize)] * amplitude;
                    break;
                case WaveType.Square:
                    currentSample = wavetableSquare[(int)(phase * wavetableSize)] * amplitude;
                    break;
                case WaveType.Triangle:
                    currentSample = wavetableTriangle[(int)(phase * wavetableSize)] * amplitude;
                    break;
                case WaveType.Sawtooth:
                    currentSample = wavetableSawtooth[(int)(phase * wavetableSize)] * amplitude;
                    break;
            }

            for (int channel = 0; channel < channels; channel++)
            {
                data[i + channel] = currentSample;
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
