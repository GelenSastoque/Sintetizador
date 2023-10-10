using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using Unity.VisualScripting;
using System.Linq;

public class AudioGen1 : MonoBehaviour
{
    public TMP_Dropdown waveformDropdown; 

    public AudioSource audioSource;
    public float sampleRate = 44100;
    public float[] wavetable;
    public int wavetableSize = 2048;
    public float freqDetune = 10.0f;
    public float[] wavetableDetune;

    public enum WaveformType
    {
        Sine,
        Square,
        Triangle,
        Sawtooth,
        PadWave,
        Noise,
        Kick,
        Snare
    }

    public WaveformType waveformType = WaveformType.Square;

    private float CreateSeno(int timeindex, float frecuencia)
    {
        return Mathf.Sin(2f * Mathf.PI * timeindex * frecuencia / sampleRate);
    }

    private float CreateSquare(int timeindex, float frecuencia)
    {
        return Mathf.Sign(Mathf.Sin(2 * Mathf.PI * timeindex * frecuencia / sampleRate));
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

    private float WhiteNoise(){
        System.Random random = new System.Random();

        return (float)(random.NextDouble()*2.0-1.0);
    }

    // public float PadWave(int timeindex, float frecuencia)
    // {
    //     if (timeindex < wavetableSize) return Mathf.Sign(Mathf.Sin(2 * Mathf.PI * timeindex * frecuencia / FM));
    //     else return Mathf.Sin(2 * Mathf.PI * timeindex * frecuencia / FM);
    // }

    private void GenerateWavetable(){
        wavetable = new float[wavetableSize];
        wavetableDetune = new float[wavetableSize];
        float f = sampleRate / wavetableSize;
        for(int i = 0; i<wavetableSize;i++){
            switch(waveformType){
                case WaveformType.Sine:
                    wavetable[i]+=CreateSeno(i,f);
                    wavetableDetune[i]+=CreateSeno(i,f+freqDetune);
                    break;
                case WaveformType.Square:
                    wavetable[i]+=CreateSquare(i,f);
                    wavetableDetune[i]+=CreateSquare(i,f+freqDetune);
                    break;
                case WaveformType.Triangle:
                    wavetable[i]+=CreateTriangle(i,f);
                    wavetableDetune[i]+=CreateTriangle(i,f+freqDetune);
                    break;
                case WaveformType.Sawtooth:
                    wavetable[i]+=CreateSawTooth(i,f);
                    wavetableDetune[i]+=CreateSawTooth(i,f+freqDetune);
                    break;
                case WaveformType.Noise:
                    wavetable[i]+=WhiteNoise();
                    break;
                case WaveformType.Snare:
                    wavetable[i]+=WhiteNoise();
                    break;
                /*case WaveformType.PadWave:
                    wavetable[i]+=CreatePadWave(i,f);
                    wavetableDetune[i]+=CreatePadWave(i,f+freqDetune);
                    break;
                case WaveformType.Kick:
                    wavetable[i]+=CreateKick(i,f);
                    break;*/
            }
        }
    }

    float Amplitud;
    [Range(-60,0)]
    public float dbfsValue = -20f;
    private float DBFSToLinearValue(float dbfs){
        return Mathf.Pow(10f, dbfs/20f);
    }

    [Range(0.01f,0.9f)]
    public float A = 0.1f;

    [Range(0.1f,1.5f)]
    public float D = 0.2f;

    [Range(0.1f,1f)]
    public float S = 0.3f;

    [Range(0.1f,1f)]
    public float R = 0.3f;

    [Range(0.1f,1f)]
    public float SustainLevel = 0.5f;
    public int ADSRindex = 0;

    float[] env;
    float[] ADSR(){
        int totalADSRSize = (int)(sampleRate*(A+D+R+S));
        float[] envelope = new float[totalADSRSize];

        int ASamples = (int)(sampleRate*A);
        int DSamples = (int)(sampleRate*D);
        int SSamples = (int)(sampleRate*S);
        int RSamples = (int)(sampleRate*R);

        for(int i = 0;i<totalADSRSize; i++){
            float value = 0f;
            if(i<ASamples){
                value=Mathf.Lerp(0f,1f,(float)i/ASamples);
            }
            else if(i<ASamples+DSamples){
                value=Mathf.Lerp(1f,SustainLevel,(float)i/DSamples);
            }
            else if(i<ASamples+DSamples+SSamples){
                value=SustainLevel;
            }
            else if (i<ASamples+DSamples+SSamples+RSamples){
                value=Mathf.Lerp(SustainLevel,0f,(float)i/RSamples);
            }
            envelope[i]=value;
        }
        return envelope;
    }

    //Versión Mnonofónica
    public float frecuencia;
    float phaseM;
    private void OnAudioFilterRead(float[] data, int channels){
        for(int i = 0; i<data.Length; i+=channels){
            float E;
            if(ADSRindex<env.Length){
                E=env[ADSRindex];
            }
            else{
                E=0f;
            }

            float currentSample = 0;

            try{
                currentSample+=wavetable[(int)(phaseM*wavetableSize)]*Amplitud*E;
                for(int channel = 0; channel<channels; channel++){
                    data[i+channel]=currentSample;
                }
            }
            catch(System.IndexOutOfRangeException ex){
                print("Exception.");
                print("Error message: "+ ex.Message);
            }

            try{
                phaseM+=frecuencia/sampleRate;
                if(phaseM>1f){
                    phaseM-=1f;
                }
            }
            catch(System.IndexOutOfRangeException ex){
                print("Exception.");
                print("Error message: "+ ex.Message);
            }
            ADSRindex++;
        }
    }

    private void Awake(){
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = true;
        audioSource.spatialBlend = 0;
        GenerateWavetable();
        env = ADSR();
        Application.runInBackground=true;
        audioSource.Play();
    }

    void Start(){
        Amplitud=DBFSToLinearValue(dbfsValue);
        // frecuencias.Add(0f);
    }
}
