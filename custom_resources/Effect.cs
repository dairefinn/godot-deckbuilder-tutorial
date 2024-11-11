using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Effect : RefCounted
{

    public AudioStream sound;

    public virtual void Execute(Array<Node> _targets)
    {
        
    }

}
