using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ParadoxEngine;
using ParadoxEngine.Utilities;

public class DCharacter : PEData
{
    [SerializeField]
    private List<TupleSprite> _sprites;
    [SerializeField]
    private List<TupleAnimation> _animations;

    public override void SetIdentificator(string name)
    {
        _name = name;
        this.name = _name;
    }

    [SerializeField]
    public bool UseSprite;
    [SerializeField]
    public bool UseAnimation;

    #region Sprites
    public void SuscribeSprite(string name, Sprite sprite)
    {
        _sprites.Add(new TupleSprite(name, sprite));
    }

    public Sprite GetSprite(string name) => _sprites.GetValue(x => x == name);

    public void UnsuscribeSprite(string name)
    {
        if (!_sprites.Any(x => x.Item1 == name))
            return;

        _sprites.Remove(_sprites.Where(x => x.Item1 == name).First());
    }

    public void UnsuscribeSprite(Sprite sprite)
    {
        if (!_sprites.Any(x => x.Item2 == sprite))
            return;

        _sprites.Remove(_sprites.Where(x => x.Item2 == sprite).First());
    }

    public void UnsuscribeSprite(TupleSprite tuple)
    {
        if (!_sprites.Any(x => x.Item1 == tuple.Item1 && x.Item2 == tuple.Item2))
            return;

        _sprites.Remove(tuple);
    }

    public bool ContainsSprite(string name) { return _sprites.Any(x => x.Item1 == name); }
    public bool ContainsSprite(Sprite sprite) { return _sprites.Any(x => x.Item2 == sprite); }

    public IEnumerable<TupleSprite> SpriteForeach()
    {
        if (_sprites == null)
            _sprites = new List<TupleSprite>();

        foreach (var item in _sprites)
        {
            yield return item;
        }
    }
    #endregion

    #region Animations
    public void SuscribeAnimation(string name, AnimationClip anim)
    {
        _animations.Add(new TupleAnimation(name, anim));
    }

    public AnimationClip GetAnimation(string name) => _animations.GetValue(x => x == name);

    public void UnsuscribeAnimation(string name)
    {
        if (!_animations.Any(x => x.Item1 == name))
            return;

        _animations.Remove(_animations.Where(x => x.Item1 == name).First());
    }

    public void UnsuscribeAnimation(AnimationClip anim)
    {
        if (!_animations.Any(x => x.Item2 == anim))
            return;

        _animations.Remove(_animations.Where(x => x.Item2 == anim).First());
    }

    public void UnsuscribeAnimation(TupleAnimation tuple)
    {
        if (!_animations.Any(x => x.Item1 == tuple.Item1 && x.Item2 == tuple.Item2))
            return;

        _animations.Remove(tuple);
    }

    public bool ContainsAnimation(string name) { return _animations.Any(x => x.Item1 == name); }
    public bool ContainsAnimation(AnimationClip anim) { return _animations.Any(x => x.Item2 == anim); }

    public IEnumerable<TupleAnimation> AnimationForeach()
    {
        if (_animations == null)
            _animations = new List<TupleAnimation>();

        foreach (var item in _animations)
        {
            yield return item;
        }
    }
    #endregion
}
