using Godot;
using System;
using System.Collections.Generic;

public partial class AudioManager : Node
{
    private AudioStreamPlayer2D audioPlayer;  // Audio player for playback
    private AudioStreamPlayer2D Act2Player;
    private AudioStreamPlayer2D Act3Player;
    private AudioStreamPlayer2D Act4Player;
    private Label subtitleLabel;              // Subtitle display label

    private AudioStreamPlayer2D StartTrain;
    private AudioStreamPlayer2D InsideTrain;
    private AudioStreamPlayer2D StopTrain;

    // Subtitles data for different audio tracks
    private List<(string Text, float StartTime, float EndTime)> mainSubtitles = new List<(string, float, float)>
    {
        ("Hallo, mijn naam is Robert Paul.", 1.0f, 3.0f),
        ("Ik kan mij nog goed herinneren toen ik jouw leeftijd was.", 3.0f, 6.5f),
        ("De hele wereld nog voor mij, maar ik had nog geen idee wat ik wou doen.", 6.5f, 10.5f),
        ("Ik was maar begonnen bij de Sport Acedemy in Amsterdam", 10.5f, 14.0f),
        ("Ik was erg goed in sporten, vooral in schaatsen", 14.0f, 17.5f),
        ("Dus ik dacht de sport acedemy, dat is de plek waar ik moet zijn", 17.5f, 21.8f),
        ("Maar ja, na 1 jaar was ik alweer weg", 21.8f, 25.0f),
    };

    private List<(string Text, float StartTime, float EndTime)> act2Subtitles = new List<(string, float, float)>
    {
        ("Act 2, first line.", 0.0f, 2.0f),
        ("Act 2, second line.", 2.0f, 4.0f),
        ("Act 2, third line.", 4.0f, 6.0f),
        // Add more subtitles for Act 2
    };

    private List<(string Text, float StartTime, float EndTime)> act3Subtitles = new List<(string, float, float)>
    {
        ("Act 3, first line.", 0.0f, 2.0f),
        ("Act 3, second line.", 2.0f, 4.0f),
        ("Act 3, third line.", 4.0f, 6.0f),
        // Add more subtitles for Act 3
    };

    private List<(string Text, float StartTime, float EndTime)> act4Subtitles = new List<(string, float, float)>
    {
        ("Act 4, first line.", 0.0f, 2.0f),
        ("Act 4, second line.", 2.0f, 4.0f),
        ("Act 4, third line.", 4.0f, 6.0f),
        // Add more subtitles for Act 4
    };

    private int currentSubtitleIndex = -1;   // Index of the current subtitle (-1 means none)
    private bool audioFinished = false;

    public override void _Ready()
    {
        // Get references to AudioStreamPlayer and Label
        audioPlayer = GetNode<AudioStreamPlayer2D>("AudioPlayer");
        Act2Player = GetNode<AudioStreamPlayer2D>("Act2Player");
        Act3Player = GetNode<AudioStreamPlayer2D>("Act3Player");
        Act4Player = GetNode<AudioStreamPlayer2D>("Act4Player");
        subtitleLabel = GetNode<Label>("UI/SubtitleLabel"); // Path to subtitle label in UI

        StartTrain = GetNode<AudioStreamPlayer2D>("StartTrain");
        InsideTrain = GetNode<AudioStreamPlayer2D>("InsideTrain");
        StopTrain = GetNode<AudioStreamPlayer2D>("StopTrain");
    }

    public override void _Process(double delta)
    {
        if (audioFinished)
            return;

        // Update subtitles based on the audio playback position
        UpdateSubtitles();
    }

    public void StartAudioWithSubtitles()
    {
        GD.Print("Starting audio and subtitles.");
        audioPlayer.Play(); // Play the audio
        currentSubtitleIndex = -1; // Reset subtitle index
        audioFinished = false;
    }

    public void StartAudioAct2()
    {
        GD.Print("Starting Act 2");
        Act2Player.Play();
        currentSubtitleIndex = -1; // Reset subtitle index
        audioFinished = false;
    }

    public void StartAudioAct3()
    {
        GD.Print("Starting Act 3");
        Act3Player.Play();
        currentSubtitleIndex = -1; // Reset subtitle index
        audioFinished = false;
    }

    public void StartAudioAct4()
    {
        GD.Print("Starting Act 4");
        Act4Player.Play();
        currentSubtitleIndex = -1; // Reset subtitle index
        audioFinished = false;
    }

    public void PlayStartTrain()
    {
        GD.Print("Playing start train");
        StartTrain.Play();
        audioFinished = false;
    }

    public void StopStartTrain()
    {
        GD.Print("Stoped start train");
        StartTrain.Stop();
    }

    public void PlayInsideTrain()
    {
        GD.Print("Playing inside train");
        InsideTrain.Play();
        audioFinished = false;
    }

    public void StopInsideTrain()
    {
        GD.Print("STopping inside train");
        InsideTrain.Stop();
    }

    public void StartStopTrain()
    {
        GD.Print("Starting stop train");
        StopTrain.Play();
        audioFinished = false;
    }

    public void StopStopTrain()
    {
        GD.Print("Stopping stop train");
        StopTrain.Stop();
    }

    private void UpdateSubtitles()
    {
        // Check which audio player is playing and update subtitles accordingly
        if (audioPlayer.Playing)
        {
            UpdateSubtitlesForPlayer(audioPlayer, mainSubtitles);
        }
        else if (Act2Player.Playing)
        {
            UpdateSubtitlesForPlayer(Act2Player, act2Subtitles);
        }
        else if (Act3Player.Playing)
        {
            UpdateSubtitlesForPlayer(Act3Player, act3Subtitles);
        }
        else if (Act4Player.Playing)
        {
            UpdateSubtitlesForPlayer(Act4Player, act4Subtitles);
        }
    }

    private void UpdateSubtitlesForPlayer(AudioStreamPlayer2D player, List<(string Text, float StartTime, float EndTime)> subtitlesList)
    {
        // Get the current playback position of the audio
        float playbackPosition = (float)player.GetPlaybackPosition();

        // Check if the audio has finished (only after it started playing)
        if (!player.Playing && playbackPosition > 0)
        {
            OnAudioFinished();
            return;
        }

        // Find the correct subtitle to display
        for (int i = 0; i < subtitlesList.Count; i++)
        {
            var (text, startTime, endTime) = subtitlesList[i];

            if (playbackPosition >= startTime && playbackPosition < endTime)
            {
                if (currentSubtitleIndex != i)
                {
                    // Update the subtitle only if it's a new one
                    currentSubtitleIndex = i;
                    subtitleLabel.Text = text;
                }
                return;
            }
        }

        // If no subtitle matches, clear the label
        subtitleLabel.Text = "";
        currentSubtitleIndex = -1;
    }

    private void OnAudioFinished()
    {
        GD.Print("Audio finished. Clearing subtitles.");
        subtitleLabel.Text = ""; // Clear the subtitle text
        audioFinished = true;    // Stop further updates
    }

    public void StopAudioAndSubtitles()
    {
        GD.Print("Stopping audio and subtitles.");
        audioPlayer.Stop();      // Stop the audio
        Act2Player.Stop();       // Stop Act 2 audio
        Act3Player.Stop();       // Stop Act 3 audio
        Act4Player.Stop();       // Stop Act 4 audio
        subtitleLabel.Text = ""; // Clear the subtitle text
        audioFinished = true;    // Stop further updates
    }
}