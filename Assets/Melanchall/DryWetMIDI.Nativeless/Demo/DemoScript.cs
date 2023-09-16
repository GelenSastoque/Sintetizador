using System;
using System.Linq;
using System.Text;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Composing;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Standards;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DemoScript : MonoBehaviour
{
	public class LogOutputDevice : MonoBehaviour, IOutputDevice
	{
		public event EventHandler<MidiEventSentEventArgs> EventSent;
        public RCV rcv;
		public void PrepareForEventsSending()
		{
            LoadSynths();
		}

		public void SendEvent(MidiEvent midiEvent)
		{
			rcv.PlayEvents(midiEvent);
		}

		public void Dispose()
		{
		}

        public void LoadSynths(){
            rcv = GameObject.Find("Pruebita").GetComponent<RCV>();
        }
	}

    private IOutputDevice _outputDevice;
    private Playback _playback;

    public TMP_Dropdown musicDropdown; // Asigna el Dropdown desde el Inspector
    public string[] midiFilePaths = {Application.dataPath + "/MIDIS/MIDI-3.mid",Application.dataPath + "/MIDIS/mario.mid",Application.dataPath + "/MIDIS/MIDI-2.mid"};

    // string midiFilePath = Application.dataPath + "/MIDIS/MIDI-3.mid";
    // string midiFilePath2 = Application.dataPath + "/MIDIS/mario.mid";
    // string midiFilePath3 = Application.dataPath + "/MIDIS/MIDI-2.mid";

    private void Start()
    {
        InitializeOutputDevice();
        var midiFile = MidiFile.Read(midiFilePaths[0]);

        InitializeFilePlayback(midiFile);

        // Suscribe al evento de cambio de valor del Dropdown
        // musicDropdown.onValueChanged.AddListener(ChangeMusic);
    }

    public void ButtonStart(){
        StartPlayback();
    }

    public void ButtonStop(){
        StopPlayback();
    }

    public void ButtonMidi1(){
            StopPlayback();
            var midiFile = MidiFile.Read(midiFilePaths[0]);
            InitializeFilePlayback(midiFile);
            StartPlayback();
    }

    public void ButtonMidi2(){
            StopPlayback();
            var midiFile = MidiFile.Read(midiFilePaths[1]);
            InitializeFilePlayback(midiFile);
            StartPlayback();
    }

    public void ButtonMidi3(){
            StopPlayback();
            var midiFile = MidiFile.Read(midiFilePaths[2]);
            InitializeFilePlayback(midiFile);
            StartPlayback();
    }

    // public void ChangeMusic(int selectedIndex)
    // {
    //     // Detiene la reproducción actual
    //     StopPlayback();

    //     // Carga y reproduce el archivo MIDI seleccionado
    //     InitializeFilePlayback(midiFilePaths[selectedIndex]);
    //     StartPlayback();
    // }


    void Update(){
        // var midiFile;
        // if (Input.GetKeyDown(KeyCode.P)){
        //     StartPlayback();
        // }
        // if (Input.GetKeyDown(KeyCode.O)){
        //     StopPlayback();
        // }

        // if(Input.GetKeyDown(KeyCode.A)){
        //     StopPlayback();
        //     var midiFile = MidiFile.Read(midiFilePath);
        //     InitializeFilePlayback(midiFile);
        //     StartPlayback();
        // }

        // if(Input.GetKeyDown(KeyCode.S)){
        //     StopPlayback();
        //     var midiFile = MidiFile.Read(midiFilePath2);
        //     InitializeFilePlayback(midiFile);
        //     StartPlayback();
        // }

        // if(Input.GetKeyDown(KeyCode.D)){
        //     StopPlayback();
        //     var midiFile = MidiFile.Read(midiFilePath3);
        //     InitializeFilePlayback(midiFile);
        //     StartPlayback();
        // }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Releasing playback and device...");

        if (_playback != null)
        {
            _playback.NotesPlaybackStarted -= OnNotesPlaybackStarted;
            _playback.NotesPlaybackFinished -= OnNotesPlaybackFinished;
            _playback.Dispose();
        }

        if (_outputDevice != null)
            _outputDevice.Dispose();

        Debug.Log("Playback and device released.");
    }

    private void InitializeOutputDevice()
    {
        Debug.Log($"Initializing output device...");
        _outputDevice = new LogOutputDevice();
        Debug.Log($"Output device initialized.");
    }

    private MidiFile CreateTestFile()
    {
        Debug.Log("Creating test MIDI file...");

        var patternBuilder = new PatternBuilder()
            .SetNoteLength(MusicalTimeSpan.Eighth)
            .SetVelocity(SevenBitNumber.MaxValue)
            .ProgramChange(GeneralMidiProgram.Harpsichord);

        foreach (var noteNumber in SevenBitNumber.Values)
        {
            patternBuilder.Note(Melanchall.DryWetMidi.MusicTheory.Note.Get(noteNumber));
        }

        var midiFile = patternBuilder.Build().ToFile(TempoMap.Default);
        Debug.Log("Test MIDI file created.");

        return midiFile;
    }

    private void InitializeFilePlayback(MidiFile midiFile)
    {
        Debug.Log("Initializing playback...");

        _playback = midiFile.GetPlayback(_outputDevice);
        _playback.Loop = true;
        _playback.NotesPlaybackStarted += OnNotesPlaybackStarted;
        _playback.NotesPlaybackFinished += OnNotesPlaybackFinished;
       
        Debug.Log("Playback initialized.");
    }

    private void StartPlayback()
    {
        Debug.Log("Starting playback...");
        _playback.Start();
    }

    private void StopPlayback(){
        Debug.Log("Stopping playback...");
        _playback.Stop();
    }

    private void OnNotesPlaybackFinished(object sender, NotesEventArgs e)
    {
        LogNotes("Notes finished:", e);
    }

    private void OnNotesPlaybackStarted(object sender, NotesEventArgs e)
    {
        LogNotes("Notes started:", e);
    }

    private void LogNotes(string title, NotesEventArgs e)
    {
        var message = new StringBuilder()
            .AppendLine(title)
            .AppendLine(string.Join(Environment.NewLine, e.Notes.Select(n => $"  {n}")))
            .ToString();
        Debug.Log(message.Trim());
    }
}