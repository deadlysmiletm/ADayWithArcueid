using System.Collections.Generic;
using UnityEngine;
using ParadoxEngine;


[CreateAssetMenu(menuName = "Paradox Engine/Paradox Setting")]
public class DSetting : PEData
{
    private List<KeyCode> _keys = new List<KeyCode>() { KeyCode.Return, KeyCode.Space, KeyCode.Mouse0, KeyCode.KeypadEnter };
    private float _timeForChar = .05f;

    public float TimeForChar { get => _timeForChar; set => _timeForChar = value; }
    public int Count => _keys.Count;


    public KeyCode this[int index]
    {
        get => _keys[index];
        set => _keys[index] = value;
    }

    public override void SetIdentificator(string name)
    {
        _name = name;
        this.name = name;
    }

    public void Add(KeyCode key) => _keys.Add(key);
    public void Remove(KeyCode key) => _keys.Remove(key);
    public void RemoveAt(int index) => _keys.RemoveAt(index);

    public bool InputGetKeyDown()
    {
        for (int i = 0; i < _keys.Count; i++)
        {
            if (Input.GetKeyDown(_keys[i]))
            {
                if (!DialogueDatabase.activeGraphPlayer.IsPlaying)
                    return false;

                return true;
            }
        }

        return false;
    }

    public bool InputGetKeyDown(System.Action OnTrue)
    {
        for (int i = 0; i < _keys.Count; i++)
        {
            if (Input.GetKeyDown(_keys[i]))
            {
                if (!DialogueDatabase.activeGraphPlayer.IsPlaying)
                    return false;

                OnTrue();
                return true;
            }
        }

        return false;
    }

    public bool Contains(KeyCode key, int skipIndex)
    {
        for (int i = 0; i < _keys.Count; i++)
        {
            if (i == skipIndex)
                continue;

            if (key == _keys[i])
                return true;
        }

        return false;
    }
}