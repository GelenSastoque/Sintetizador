using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ADSR : MonoBehaviour
{
    [Range(20,20000)]
    public float frecuencia;
    public float FrecuenciaMuestreo = 44100;
    public float TiempoSegundos = 2.0f;
    AudioSource audioSource;
    int timeIndex =0;
    [Range(5,100)]
    public float A=100;

    [Range(20,300)]
    public float D=200;

    [Range(200,1000)]
    public float S=300;

    int tA;
    int tD;
    int tS;
    int ADSRindex=0;
    public TMP_Dropdown waveformDropdown; 
    public Slider attackSlider;
    public Slider decaySlider;
    public Slider sustainSlider;
    [Range(0,1)]
    public float[] Amplitudes={1f,0.9f,0.8f,0.7f,0.6f,0.5f,0.4f,0.3f,0.2f,0.1f};
    public Slider[] amplitudeSliders;
    // Start is called before the first frame update
    void Start()
    { 
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0;
        audioSource.Stop();

        int waveformIndex = (int)waveformType;
        waveformDropdown.value = waveformIndex;

        attackSlider.value = A;
        decaySlider.value = D;
        sustainSlider.value = S;

        for (int i = 0; i < amplitudeSliders.Length; i++)
        {
            amplitudeSliders[i].value = Amplitudes[i];
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        ADSRvalues();
        ADSRuwu();
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(!audioSource.isPlaying)
            {
                frecuencia=261.626f;
                timeIndex = 0;
                ADSRindex=0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(!audioSource.isPlaying)
            {
                frecuencia=277.183f;
                timeIndex = 0;
                ADSRindex=0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(!audioSource.isPlaying)
            {
                frecuencia=293.665f;
                timeIndex = 0;
                ADSRindex=0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!audioSource.isPlaying)
            {
                frecuencia=311.127f;
                timeIndex = 0;
                ADSRindex=0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if(!audioSource.isPlaying)
            {
                frecuencia=329.628f;
                timeIndex = 0;
                ADSRindex=0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(!audioSource.isPlaying)
            {
                frecuencia=349.228f;
                timeIndex = 0;
                ADSRindex=0;
                audioSource.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(!audioSource.isPlaying)
            {
                frecuencia=369.994f;
                timeIndex = 0;
                ADSRindex=0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if(!audioSource.isPlaying)
            {
                frecuencia=391.995f;
                timeIndex = 0;
                ADSRindex=0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if(!audioSource.isPlaying)
            {
                frecuencia=415.305f;
                timeIndex = 0;
                ADSRindex=0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            if(!audioSource.isPlaying)
            {
                frecuencia=440.000f;
                timeIndex = 0;
                ADSRindex=0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            if(!audioSource.isPlaying)
            {
                frecuencia=466.164f;
                timeIndex = 0;
                ADSRindex=0;
                audioSource.Play();
            }
    
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if(!audioSource.isPlaying)
            {
                frecuencia=493.883f;
                timeIndex = 0;
                ADSRindex=0;
                audioSource.Play();
            }
    
        }
        
    
        if (Input.GetKeyUp(KeyCode.A)||Input.GetKeyUp(KeyCode.W)||Input.GetKeyUp(KeyCode.S)||Input.GetKeyUp(KeyCode.E)||Input.GetKeyUp(KeyCode.D)||Input.GetKeyUp(KeyCode.F)||Input.GetKeyUp(KeyCode.T)||Input.GetKeyUp(KeyCode.G)||Input.GetKeyUp(KeyCode.Y)||Input.GetKeyUp(KeyCode.H)||Input.GetKeyUp(KeyCode.U)||Input.GetKeyUp(KeyCode.J))
        {
            audioSource.Stop();
        }
        for (int i = 0; i < amplitudeSliders.Length; i++)
        {
            Amplitudes[i] = amplitudeSliders[i].value;
        }
    }
    void ADSRvalues()
    {
        tA=(int)(A*(FrecuenciaMuestreo/1000));
        tD=(int)(D*(FrecuenciaMuestreo/1000));
        tS=(int)(S*(FrecuenciaMuestreo/1000));
    }
    public float[] env;
    void ADSRuwu()
    {
        float[] As=new float[tA];
        float[] Ds=new float[tD];
        float[] Ss=new float[tS];
    
        //Para obtener el ataque
        for(int i=0;i<tA;i++)
        {
            As[i]=(Mathf.Pow((i/tA),(1.0f/3.0f)));
        }

        //Para obtener el Decay
        float ID=0.5f/tD;
        float VD=1f;

        for(int i=0;i<tD;i++)
        {
            Ds[i]=(Mathf.Pow(VD,(1.0f/3.0f)));
            VD-=ID;
        }

        //Para obtener el Sustain

        float m=-(0.8f/tS);
        for(int i=0;i<tS;i++)
        {
            Ss[i]=m*i+0.8f;
        }

        //Para unir todos los componentes del ADS en un solo arreglo final

        float[] result=new float[As.Length+Ds.Length+Ss.Length];
        As.CopyTo(result,0);
        Ds.CopyTo(result,As.Length);
        Ss.CopyTo(result,As.Length+Ds.Length);
        env=new float[result.Length];
        result.CopyTo(env,0);

    }

    public float CreateSeno(int timeIndex, float frecuencia, float FrecuenciaMuestreo,float A){
        return A*Mathf.Sin(2*Mathf.PI*timeIndex*frecuencia/FrecuenciaMuestreo);
    }

    public float CreateSquare(int timeIndex, float frecuencia, float FrecuenciaMuestreo, float A){
        return Mathf.Sign(Mathf.Sin(2*Mathf.PI*timeIndex*frecuencia/FrecuenciaMuestreo))*A;
    }

    public float CreateTriangle(int timeIndex, float frecuencia, float FrecuenciaMuestreo, int Tm,float A){
        float m1 = 1/((Tm/4.0f));
        float m2 = -2/((Tm*(3/4.0f))-((Tm/4.0f)));
        float m3 = 1/(Tm-((Tm*3)/4.0f));

        float b1 = 1-(m1*(Tm/4));
        float b2 = 1-(m2*(Tm/4));
        float b3 = 0-(m3*Tm);
        int x=timeIndex-((int)(timeIndex/Tm)*Tm);

        if(x<=(Tm/4))
            return A*(m1*x+b1);
        else if (x>(Tm/4) && x<=((Tm*3)/4))
            return A*(m2*x+b2);
        else 
            return A*(m3*x+b3);
    }

    public float CreateSawTooth(int timeIndex, float frecuencia, float FrecuenciaMuestreo, int Tm, float A){
        float m1 = 1/((Tm/2.0f));
        float m2 = 1/(Tm-((Tm)/2.0f));

        float b1 = 1-(m1*(Tm/2));
        float b2 = 0-(m2*Tm);

        int x=timeIndex-((int)(timeIndex/Tm)*Tm);

        if(x<=(Tm/2))
            return A*(m1*x+b1);
        else
            return A*(m2*x+b2);
    }

    public enum WaveformType{
        Sine,
        Square,
        Triangle,
        Sawtooth
    }
    
    public WaveformType waveformType=WaveformType.Square;

    int nA=10;
    float s=0;
    int Tm;
    
    private void OnAudioFilterRead(float[] data, int channels)
    {
        for(int i=0; i<data.Length;i+=channels)
    {
        float x=0;
        for(int n=1; n<=nA;n++)
        {
            Tm=(int)(FrecuenciaMuestreo/(frecuencia*n));
            switch(waveformType){
                case WaveformType.Sine:
                    x+=CreateSeno(timeIndex, frecuencia*n, FrecuenciaMuestreo,Amplitudes[n-1]);
                    break;

                case WaveformType.Square:
                    x+=CreateSquare(timeIndex, frecuencia*n, FrecuenciaMuestreo,Amplitudes[n-1]);
                    break;
                
                case WaveformType.Triangle:
                    x+=CreateTriangle(timeIndex,frecuencia*n,FrecuenciaMuestreo,Tm,Amplitudes[n-1]);
                    break;

                case WaveformType.Sawtooth:
                    x+=CreateSawTooth(timeIndex,frecuencia*n,FrecuenciaMuestreo,Tm,Amplitudes[n-1]);
                    break;
            }
        }
        float E;
        if (ADSRindex<env.Length)
        {
            E=env[ADSRindex];
        }
        else{
            E=0f;
        }

        data[i]=E*x/(float)nA;
        if(channels==2)
        {
            data[i+1]=E*x/(float)nA;

        }
        timeIndex++;
        if(timeIndex>=(FrecuenciaMuestreo*TiempoSegundos)){
            timeIndex=0;
        }
        ADSRindex++;
    }
        
       
    }
    public void ChangeWaveform(int index)
    {
        waveformType = (WaveformType)index;
    }

    public void OnAttackValueChanged(float value)
    {
        A = value; // Actualizar el valor de A con el valor del slider
    }
    public void OnDecayValueChanged(float value)
    {
        D = value; // Actualizar el valor de D con el valor del slider
    }

    public void OnSustainValueChanged(float value)
    {
        S = value; // Actualizar el valor de S con el valor del slider
    }
}
