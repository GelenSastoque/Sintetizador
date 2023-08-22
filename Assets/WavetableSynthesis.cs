using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavetableSynthesis : MonoBehaviour
{
    public int wavetableSize = 1024;
    public float frequency = 440f;
    public float amplitude = 0.5f;
    public WaveType waveType = WaveType.Sine;

    private AudioSource audioSource;
    private float[] wavetable;
    private float phase;

    float sampleRate;

    public enum WaveType
    {
        Sine,
        Square,
        Triangle,
        Sawtooth
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sampleRate = AudioSettings.outputSampleRate;
        GenerateWavetable();
    }

    private void GenerateWavetable()
    {
        wavetable = new float[wavetableSize];

        for (int i = 0; i < wavetableSize; i++)
        {
            float t = (float)i / wavetableSize;

            switch (waveType)
            {
                case WaveType.Sine:
                    wavetable[i] = Mathf.Sin(2f * Mathf.PI * t);
                    break;
                case WaveType.Square:
                    wavetable[i] = Mathf.Sign(Mathf.Sin(2f * Mathf.PI * t));
                    break;
                case WaveType.Triangle:
                    wavetable[i] = 2f * Mathf.Abs(2f * t - 1f) - 1f;
                    break;
                case WaveType.Sawtooth:
                    wavetable[i] = 2f * t - 1f;
                    break;
            }
        }
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        GenerateWavetable(); // Generar la tabla de ondas en cada llamada de audio (puede optimizarse)

        for (int i = 0; i < data.Length; i += channels)
        {
            float currentSample = wavetable[(int)(phase * wavetableSize)] * amplitude;

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
}
