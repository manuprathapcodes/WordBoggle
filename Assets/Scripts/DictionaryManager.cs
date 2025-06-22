using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manager class for maintaining a dictionary.
/// </summary>
public class DictionaryManager : MonoBehaviour
{
    public HashSet<string> words = new HashSet<string>();

    void Awake()
    {
        TextAsset ta = Resources.Load<TextAsset>("wordlist");
        foreach (var w in ta.text.Split('\n'))
            if (w.Length >= 3) words.Add(w.Trim().ToUpper());
    }

    public bool IsValid(string word) => words.Contains(word.ToUpper());
}
