using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParadoxEngine;
using ParadoxEngine.Utilities;
using System.Linq;

public class DLocation : PEData
{
    [SerializeField]
    private List<TupleSprite> _sprites;

    public Sprite this[string name]
    {
        get { return _sprites.GetValue(x => x == name); }
    }

    public override void SetIdentificator(string name)
    {
        _name = name;
        this.name = _name;
    }

    public void SuscribeBackground(string name, Sprite sprite)
    {
        if (_sprites == null)
            _sprites = new List<TupleSprite>();

        _sprites.Add(new TupleSprite(name, sprite));
    }

    public void UnsuscribeBackground(string name)
    {
        if (!_sprites.Any(x => x.Item1 == name))
            return;

        _sprites.Remove(_sprites.Where(x => x.Item1 == name).First());
    }

    public void UnsuscribeBackground(Sprite sprite)
    {
        if (!_sprites.Any(x => x.Item2 == sprite))
            return;

        _sprites.Remove(_sprites.Where(x => x.Item2 == sprite).First());
    }

    public void UnsuscribeBackground(TupleSprite tuple)
    {
        if (!_sprites.Any(x => x == tuple))
            return;

        _sprites.Remove(tuple);
    }

    public bool ContainsBackground(string name) { return _sprites.Any(x => x.Item1 == name); }
    public bool ContainsBackground(Sprite sprite) { return _sprites.Any(x => x.Item2 == sprite); }

    public IEnumerable<TupleSprite> Foreach()
    {
        if (_sprites == null)
            _sprites = new List<TupleSprite>();

        foreach (var item in _sprites)
        {
            yield return item;
        }
    }
}
