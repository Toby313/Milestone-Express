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
        ("Ik was goed in sporten, maar het was niet mijn passie.", 0.0f, 3.0f),
        ("Iets wat mij ook altijd leuk leek was fotographie.", 3.0f, 6.0f),
        ("Maar nadat ik het had geprobeerd was het toch niet echt iets voor mij.", 6.0f, 9.0f),
        ("Daarna ben ik doorgegaan met een miluestudie.", 9.0f, 12.5f),
        ("Daar heb ik heel erg veel van geleerd.", 12.5f, 15.0f),
        ("Maar toen ik stage begon te lopen merkte ik dat dit werk", 15.0f, 17.5f),
        ("eigelijk helemaal niks voor mij was.", 17.5f, 19.5f),
        ("Het was wel heel erg interessant.", 19.5f, 22.5f),
        ("Sterker nog ik heb de hele studie nog afgerond.", 22.5f, 25.0f),
        ("En ook al vond ik de studie interessant, het werk was toch niet iets voor mij.", 25.0f, 30.5f),
        // Add more subtitles for Act 2
    };

    private List<(string Text, float StartTime, float EndTime)> act3Subtitles = new List<(string, float, float)>
    {
        ("Ik begon na te denken, wat vond ik leuk om te doen?", 0.0f, 4.0f),
        ("Ik vind het leuk om te tekenen om glas en lood te maken", 4.0f, 8.5f),
        ("en om met mijn handen bezig te zijn.", 8.5f, 10.5f),
        ("Ik begon te doen wat ik leuk vond en niet wat ik had gestudeerd", 10.5f, 15.0f),
        ("Ik had een paar projectjes hier en daar gemaakt maar.", 15.0f, 18.0f),
        ("mijn grootste project was toen ik een deur compleet had na gemaakt in glas en lood.", 18.0f, 23.0f),
        ("Ik heb daarna geprobeerd om bij een paar bedrijven te gaan werken.", 23.0f, 28.0f),
        ("Maar dat ging niet goed en ik ben na een jaar weer weggegaan.", 28.0f, 30.5f),
        ("Dat is daarna ook nog wel een paar keer vaker gebeurt.", 30.5f, 33.5f),
        // Add more subtitles for Act 3
    };

    private List<(string Text, float StartTime, float EndTime)> act4Subtitles = new List<(string, float, float)>
    {
        ("Ik had veels te veel geleerd dus ik wou doorgaan met glas en lood.", 0.0f, 4.0f),
        ("Dus toen ben ik maar voor mij zelf begonnen.", 4.0f, 6.5f),
        ("2 jaar later had ik al mijn eigen atelier waar ik nog steeds werk.", 6.5f, 10f),
        ("Ik maak nu al meer dan 20 jaar glas en lood voor mensen.", 10f, 14f),
        ("En ik geef een curses hoe jezelf ook glas en lood kan maken.", 14f, 17.50f),
        ("Als ik terug denk heb ik nergens spijt van.", 17.5f, 22f),
        ("Je moet gewoon zoveel mogelijk dingen uitproberen.", 22f, 24f),
        ("En ook al heb je een diploma kan jij nog steed een andere richting op gaan", 24f, 28f),
        ("en doen wat je echt wilt.", 28f, 30f),
        ("A hier is mijn halte alweer.", 31f, 32.5f),
        ("Het was fijn om met je te praten.", 32.5f, 35f),
        ("En onthou je moet doen waarbij jij je goed voelt.", 35f, 38.5f),
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