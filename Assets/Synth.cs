using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.IO;

public class Synth : MonoBehaviour
{

    public AudioSource audioSource;


    
    public float FM = 44100;

    public enum WaveformType
        {
            Sine,
            Square,
            Triangle,
            Sawtooth,
            WNoise,
            Padwave,
            Kick
        }

    public WaveformType waveformType = WaveformType.Square;


    public float SineWave(int timeindex, float frecuencia)
    {
        return Mathf.Sin(2 * Mathf.PI * timeindex * frecuencia / FM);        
    }

    public float SquareWave(int timeindex, float frecuencia)
    {
        return Mathf.Sign(Mathf.Sin(2 * Mathf.PI * timeindex * frecuencia / FM));
    }

    public float TriangleWave(int timeIndex, float frecuencia)
    {
        int Tm = (int)(FM / frecuencia);
        float m1 = 1 / ((Tm / 4.0f));
        float m2 = -2 / ((Tm * (3 / 4.0f)) - ((Tm / 4.0f)));
        float m3 = 1 / (Tm - ((Tm * 3) / 4.0f));

        float b1 = 1 - (m1 *(Tm / 4));
        float b2 = 1 - (m2 *(Tm / 4)) ;
        float b3 = 0 - (m3 * Tm);

        int x = timeIndex - ((int)(timeIndex / Tm) * Tm);

        if (x <= (Tm / 4)) return (m1 * x + b1);
        else if (x> (Tm / 4) && x<= ((Tm * 3) / 4)) return (m2 * x + b2);
        else return (m3 * x + b3);
    }

    
    public float SawtoothWave(int timeIndex, float frecuencia)
    {
        int Tm = (int)(FM / frecuencia);
        float m1 = 1 / ((Tm / 2.0f));
        float m2 = 1 / (Tm - ((Tm) / 2.0f));

        float b1 = 1 - (m1 *(Tm / 4));
        float b2 = 0 - (m2 * Tm);

        int x = timeIndex - ((int)(timeIndex / Tm) * Tm);

        if (x <= (Tm / 2)) return ((m1 * x + b1));
        else return ((m2 * x + b2));
    }

    private float WhiteNoise()
    {
        System.Random random = new System.Random();
        return (float)(random.NextDouble() * 2.0 - 1.0);
        
    }

    public float PadWave(int timeindex, float frecuencia)
    {
        if (timeindex < wavetableSize) return Mathf.Sign(Mathf.Sin(2 * Mathf.PI * timeindex * frecuencia / FM));
        else return Mathf.Sin(2 * Mathf.PI * timeindex * frecuencia / FM);
    }


    public float[] wavetable;
    public int wavetableSize = 2048;

    private void GenerateWavetable()
    {
        wavetable = new float[wavetableSize];
        env = ADSR();
        float f = FM / wavetableSize;
        for(int i = 0; i < wavetableSize; i ++){
            switch (waveformType)
            {
                case WaveformType.Sine:
                    wavetable[i] += SineWave(i,f);
                    break;
                case WaveformType.Square:
                    wavetable[i] += SquareWave(i,f);
                    break;
                case WaveformType.Triangle:
                    wavetable[i] += TriangleWave(i,f);
                    break;
                case WaveformType.Sawtooth:
                    wavetable[i] += SawtoothWave(i,f);
                    break;
                case WaveformType.WNoise:
                    wavetable[i] += WhiteNoise();
                    break;
            }
        }
    }


    float Amplitud;
    [Range(-60,0)]
    public float dbfsValue = -20f;
    private float DBFSToLinearValue(float dbfs){
        return Mathf.Pow(10f, dbfs/20f);
    }


    private void Awake(){
        
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.playOnAwake = true;
        audioSource.spatialBlend = 0;
        GenerateWavetable();
        env = ADSR();
        Application.runInBackground = true;
        audioSource.Play();
    }

     void Start()
    {
        Amplitud = DBFSToLinearValue(dbfsValue);
        // incialización para versión polifónica
        frecuencias.Add(0f);
      
    }
    

    public List<float> frecuencias = new List<float>();
    void Update()
    {

        
        if (Input.GetKeyDown(KeyCode.A))
        {
            frecuencia = 261.626f;
            frecuencias.Add(261.626f);
            ADSRindex = 0;
            if (!audioSource.isPlaying)
            {
                
                audioSource.Play();
            }
           
        }
        else if(Input.GetKeyUp(KeyCode.A))
        {
            if (audioSource.isPlaying)
            {
                frecuencias.Remove(261.626f);
                frecuencia = 0;
            }
        if (Input.GetKeyDown(KeyCode.W))
        {
                frecuencia = 277.183f;
                frecuencias.Add(277.183f);
            ADSRindex = 0;
            if (!audioSource.isPlaying)
            {
            
                audioSource.Play();
            }
            
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            if (audioSource.isPlaying)
            {
                frecuencias.Remove(277.183f);
                    frecuencia = 0;
                    LFOAindex = 0;
                }
        }

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            frecuencia = 293.665f;
            frecuencias.Add(293.665f);
            ADSRindex = 0;
            
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            if (audioSource.isPlaying)
            {
                frecuencias.Remove(293.665f);
                frecuencia = 0;
                 LFOAindex = 0;
            }

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            frecuencia = 311.117f;
            frecuencias.Add(311.117f);
            ADSRindex = 0;
            
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            if (audioSource.isPlaying)
            {
                frecuencias.Remove(311.117f);
                    frecuencia =0;
                     LFOAindex = 0;
            }

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            frecuencia = 311.117f;
            frecuencias.Add(329.628f);
            ADSRindex = 0;
           
            
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            if (audioSource.isPlaying)
            {
               frecuencias.Remove(329.628f);
                LFOAindex = 0;
            }

        }
       

        if (Input.GetKeyDown(KeyCode.M))
        {
            GenerateWavetable();
        }
        
    }

    
    public float frecuencia;
    
    float[] phase = new float[20];
    float phaseM;
    public bool isMono = false;
    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (isMono){
            for (int i = 0; i < data.Length; i += channels)
            {

                float E;
                if (ADSRindex < env.Length) E = env[ADSRindex];
                else E = 0f;

                float LFOF = LFOSineWave(LFOF_index , LFOF_Frecuency,LFOF_Amplitud);

                float LFOA = LFOSineWave(LFOAindex , LFOFrecuency,LFOAmplitud);
                float currentsample = 0;
            
                    try
                    {
                        currentsample += wavetable[(int)(phaseM * wavetableSize)] * Amplitud * E*LFOA;
                        for (int channel = 0; channel < channels; channel++) data[i + channel] = currentsample;

                    }
                    catch (System.IndexOutOfRangeException ex)
                    {
                        print("An IndexOutOfRangeException occurred.");
                        print("Error message: " + ex.Message);
                    }


                    try
                    {
                        phaseM += (frecuencia * LFOF*LFOADSR ) / FM;
                        if (phaseM > 1f) phaseM -= 1f;

                    }
                    catch (System.IndexOutOfRangeException ex)
                    {
                        print("An IndexOutOfRangeException occurred.");
                        print(" Error message: " + ex.Message);
                    }

                ADSRindex++;
                LFOAindex++;
                LFOF_index++; 
                
            }

        }
        else{
                for (int i = 0; i < data.Length; i += channels)
            {
                float E;
                if (ADSRindex < env.Length) E = env[ADSRindex];
                else E = 0f;

                float LFOF = LFOSineWave(LFOF_index , LFOF_Frecuency,LFOF_Amplitud);

                float LFOA = LFOSineWave(LFOAindex , LFOFrecuency, LFOAmplitud);
                float currentsample = 0;
                for (int j = 0; j < frecuencias.Count;j++)
                {

                    try
                    {
                        if (j != 0) currentsample += wavetable[(int)(phase[j] * wavetableSize)] * Amplitud*E*LFOA;
                        for (int channel = 0; channel < channels; channel++) data[i + channel] = currentsample;

                    }
                    catch (System.IndexOutOfRangeException ex)
                    {
                        print("An IndexOutOfRangeException occurred.");
                        print(j.ToString() + "Error message: " + ex.Message);
                    }


                    try
                    {
                        phase[j] += (frecuencias[j]*LFOF*LFOADSR)  / FM;
                        if (phase[j] > 1f) phase[j] -= 1f;

                    }
                    catch (System.IndexOutOfRangeException ex)
                    {
                        print("An IndexOutOfRangeException occurred.");
                        print(j.ToString() + " Error message: " + ex.Message);
                    }

                }
                ADSRindex++;
                LFOAindex++;
                LFOF_index++; 
            } 


        }  
        
    }
    

    
    
   

    [Range(0.01f, 0.9f)]
    public float A = 0.1f;

    [Range(0.1f, 1.5f)]
    public float D = 0.1f;

    [Range(0.1f, 4f)]
    public float S = 0.1f;

    [Range(0.1f, 1f)]
    public float R = 0.1f;

    [Range(0.1f, 1f)]
    public float SustainLevel = 0.5f;
    public int ADSRindex = 0;



    float[] env;
    float[] ADSR() {
        int totalADSRSize = (int)(FM * (A + D + R + S));
        float[] envelope = new float[totalADSRSize];

        int ASamples = (int)(FM * A);
        int DSamples = (int)(FM * D);
        int SSamples = (int)(FM * S);
        int RSamples = (int)(FM * R);

        for (int i = 0; i < totalADSRSize; i++)
        {
            float value = 0f;
            if (i < ASamples) value = Mathf.Lerp(0f, 1f, (float)i / ASamples);

            else if (i < ASamples + DSamples) value = Mathf.Lerp(1f, SustainLevel, (float)i / DSamples);

            else if (i < ASamples + DSamples + SSamples) value = SustainLevel;

            else if (i < ASamples + DSamples + SSamples + RSamples) value =  Mathf.Lerp( SustainLevel, 0f, (float)i / RSamples);

            envelope[i] = value;
        }


        return envelope;
    }
    //para el LFO de amplitud
    [Range(1f, 15f)]
    public float LFOFrecuency = 10;

    [Range(0f, 1f)]
    public float LFOAmplitud = 1;


    public int LFOAindex =0;
    public float LFOSineWave(int timeindex, float frecuencia, float Amplitud)
    {
        return 1 + Mathf.Sin(2 * Mathf.PI * timeindex * frecuencia / FM) * Amplitud;        
    }

    //Para el LFO de frecuencia
    [Range(1f, 15f)]
    public float LFOF_Frecuency = 10;

    [Range(0f, 1f)]
    public float LFOF_Amplitud = 1;


    public int LFOF_index =0;
    public float LFOf_SineWave(int timeindex, float frecuencia, float Amplitud)
    {
        return 1 + Mathf.Sin(2 * Mathf.PI * timeindex * frecuencia / FM) * Amplitud;        
    }

    //para el ADSR de frecuencia
    [Range(0.25f, 4f)]
    public float LFOADSR = 1;
 
}
