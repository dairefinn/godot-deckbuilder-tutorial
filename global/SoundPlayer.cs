namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class SoundPlayer : Node
{

    public static Dictionary<string, SoundPlayer> Instances = new Dictionary<string, SoundPlayer>();

    public override void _EnterTree()
    {
        base._EnterTree();
        Instances[Name] = this;
    }

    public static void TryPlayOnInstance(string instanceName, AudioStream audio, bool single = false)
    {
        if (Instances.TryGetValue(instanceName, out SoundPlayer instance))
        {
            instance.Play(audio, single);
        }
        else
        {
            GD.PrintErr($"SoundPlayer instance '{instanceName}' not found.");
        }
    }

    public void Play(AudioStream audio, bool single = false)
    {
        if (audio == null) return;

        if (single)
        {
            Stop();
        }

        foreach(Node node in GetChildren())
        {
            if (node is not AudioStreamPlayer audioPlayer) continue;
            if (audioPlayer.Playing) continue;
            audioPlayer.Stream = audio;
            audioPlayer.Play();
            break;
        }
    }

    public void Stop()
    {
        foreach(Node node in GetChildren())
        {
            if (node is not AudioStreamPlayer audioPlayer) continue;
            audioPlayer.Stop();
        }
    }

}
