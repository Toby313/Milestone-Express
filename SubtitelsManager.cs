using Godot;
using System;
using System.Collections.Generic;


public partial class SubtitelsManager : Node
{
    private AudioStreamPlayer audioPlayer;
    private Label subtitleLabel;
    private Timer subtitleTimer;

    // Subtitles data: List of tuples with text and display time (in seconds)
    private List<(string Text, float Time)> subtitles = new List<(string, float)>
    {
        ("This is the first line.", 0.5f),
        ("Here comes the second line.", 2.0f),
        ("And this is the final line.", 4.5f)
    };

    private int currentSubtitleIndex = 0;

    public override void _Ready()
    {
        // Get references to the AudioStreamPlayer and Label
        audioPlayer = GetNode<AudioStreamPlayer>("AudioPlayer");
        subtitleLabel = GetNode<Label>("UI/SubtitleLabel");

        // Timer for managing subtitle updates
        subtitleTimer = new Timer
        {
            OneShot = true
        };
        AddChild(subtitleTimer);
        subtitleTimer.Timeout += UpdateSubtitle;

        // Start the audio and subtitles
        PlayAudioWithSubtitles();
    }

    private void PlayAudioWithSubtitles()
    {
        if (subtitles.Count == 0)
        {
            GD.PrintErr("No subtitles to display.");
            return;
        }

        // Play the audio
        audioPlayer.Play();

        // Start the first subtitle
        currentSubtitleIndex = 0;
        ShowSubtitle();
    }

    private void ShowSubtitle()
    {
        if (currentSubtitleIndex >= subtitles.Count)
        {
            // All subtitles displayed
            subtitleLabel.Text = "";
            return;
        }

        // Get the current subtitle text and time
        var (text, time) = subtitles[currentSubtitleIndex];
        subtitleLabel.Text = text;

        // Schedule the next subtitle
        subtitleTimer.Start(time);

        // Advance to the next subtitle
        currentSubtitleIndex++;
    }

    private void UpdateSubtitle()
    {
        ShowSubtitle();
    }
}

