using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ParadoxEngine;
using ParadoxEngine.Utilities;

public class DSoundtrack : PEData
{
    [SerializeField]
    private List<TupleSound> _sounds;
    [SerializeField]
    public SoundChannelEnum Channel;

    public AudioClip this[string name]
    {
        get { return _sounds.GetValue(x => x == name); }
    }

    public override void SetIdentificator(string name)
    {
        _name = name;
        this.name = _name;
    }

    public void SuscribeSound(string name, AudioClip clip)
    {
        if (_sounds == null)
            _sounds = new List<TupleSound>();

        _sounds.Add(new TupleSound(name, clip));
    }

    public void UnsuscribeSound(string name)
    {
        if (!_sounds.Any(x => x.Item1 == name))
            return;

        _sounds.Remove(_sounds.Where(x => x.Item1 == name).First());
    }

    public void UnsuscribeSound(AudioClip clip)
    {
        if (_sounds.Any(x => x.Item2 == clip))
            return;

        _sounds.Remove(_sounds.Where(x => x.Item2 == clip).First());
    }

    public void UnsuscribeSound(TupleSound tuple)
    {
        if (!_sounds.Any(x => x == tuple))
            return;

        _sounds.Remove(tuple);
    }

    public bool ContainsSound(string name) { return _sounds.Any(x => x.Item1 == name); }
    public bool ContainsSound(AudioClip clip) { return _sounds.Any(x => x.Item2 == clip); }

    public IEnumerable<TupleSound> Foreach()
    {
        if (_sounds == null)
            _sounds = new List<TupleSound>();

        foreach (var item in _sounds)
        {
            yield return item;
        }
    }
}
