using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class WavetableSynthesisOriginal : MonoBehaviour
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


    float sampleRate = 44100;

    public TMP_Dropdown waveformDropdown; 

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
        GenerateWavetableSeno();
        GenerateWavetableSquare();
        GenerateWavetableTriangle();
        GenerateWavetableSawtooth();
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

    private void OnAudioFilterRead(float[] data, int channels)
    {
        // phase = 0.0f;
        for (int i = 0; i < data.Length; i += channels)
        {   
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
        }
    }

    public void ChangeWaveform(int index)
    {
        waveType = (WaveType)index;
    }
}
