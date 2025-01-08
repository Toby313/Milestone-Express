using Godot;
using System;
using System.Collections.Generic;

public partial class AudioManager : Node
{
    private AudioStreamPlayer2D audioPlayer;  // Audio player for playback
    private Label subtitleLabel;              // Subtitle display label

    // Subtitles data: List of tuples with text and display time (in seconds)
    private List<(string Text, float StartTime, float EndTime)> subtitles = new List<(string, float, float)>
    {
        ("This is the first line.", 0.0f, 1.5f),
        ("Here comes the second line.", 1.5f, 3.5f),
        ("And this is the third line.", 3.5f, 5.5f),
        ("This is the fourth line", 5.5f, 7.5f),
        ("This is the 5e line", 7.5f, 9.5f),
        ("This is the 6 lined", 9.5f, 11.5f),
        ("This is the 7th line", 11.5f, 13.5f),
    };

    private int currentSubtitleIndex = -1;   // Index of the current subtitle (-1 means none)
    private bool audioFinished = false;

    public override void _Ready()
    {
        // Get references to AudioStreamPlayer and Label
        audioPlayer = GetNode<AudioStreamPlayer2D>("AudioPlayer");
        subtitleLabel = GetNode<Label>("UI/SubtitleLabel"); // Path to subtitle label in UI

        // Play the audio and start subtitles
        StartAudioWithSubtitles();
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

    private void UpdateSubtitles()
    {
        // Get the current playback position of the audio
        float playbackPosition = (float)audioPlayer.GetPlaybackPosition();

        // Check if the audio has finished
        if (!audioPlayer.Playing)
        {
            OnAudioFinished();
            return;
        }

        // Find the correct subtitle to display
        for (int i = 0; i < subtitles.Count; i++)
        {
            var (text, startTime, endTime) = subtitles[i];

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
        subtitleLabel.Text = ""; // Clear the subtitle text
        audioFinished = true;    // Stop further updates
    }
}