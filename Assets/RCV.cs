using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Composing;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Standards;
using UnityEngine;

public class RCV : MonoBehaviour
{
    public AudioGen1 Osc0, Osc1, Osc2;
    //public float note;

    // Start is called before the first frame update
    void Start()
    {
        Osc0.audioSource.Play();
        Osc1.audioSource.Play();
        Osc2.audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private float MidiToFrequency(int midiNote){
        float semitoneRatio = Mathf.Pow(2f,1f/12f);
        float semitoneOffset = midiNote-69; //69 es el número MIDI para la nota A440

        return 440f*Mathf.Pow(semitoneRatio,semitoneOffset);
    }

    public void PlayEvents (MidiEvent midiEvent){
        switch(midiEvent){
            case NoteOnEvent noteOnEvent when noteOnEvent.Channel==0:
                float note = MidiToFrequency(noteOnEvent.NoteNumber);
                Osc0.ADSRindex=0;
                Osc0.frecuencia=note;
                break;
            case NoteOnEvent noteOnEvent when noteOnEvent.Channel==1:
                note = MidiToFrequency(noteOnEvent.NoteNumber);
                Osc1.ADSRindex=0;
                Osc1.frecuencia=note;
                break;
            case NoteOnEvent noteOnEvent when noteOnEvent.Channel==2:
                note = MidiToFrequency(noteOnEvent.NoteNumber);
                Osc2.ADSRindex=0;
                Osc2.frecuencia=note;
                break;
            case NoteOffEvent noteOffEvent when noteOffEvent.Channel==0:
                Osc0.frecuencia=0;
                break;
            case NoteOffEvent noteOffEvent when noteOffEvent.Channel==1:
                Osc1.frecuencia=0;
                break;
            case NoteOffEvent noteOffEvent when noteOffEvent.Channel==2:
                Osc2.frecuencia=0;
                break;
        }
    }
}
