using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class Effect : RefCounted
{

    public AudioStream sound;

    public virtual void Execute(List<Node> _targets)
    {
        
    }

}
