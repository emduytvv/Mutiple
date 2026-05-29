using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : SaiMonoBehaviour
{
    private static EnemyFactory _instance;
    public static EnemyFactory Instance => _instance;

    [SerializeField] private List<EnemyCreator> _creators;
    private Dictionary<EnemyName, EnemyCreator> _dictionary;

    protected override void Awake()
    {
        base.Awake();
        _instance = this;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCreators();
        LoadDictionary();
    }

    private void LoadCreators()
    {
        if (_creators.Count > 0) return;
        _creators = new List<EnemyCreator>();
        foreach (Transform child in transform)
            _creators.Add(child.GetComponent<EnemyCreator>());
    }

    private void LoadDictionary()
    {
        if (_dictionary != null) return;
        _dictionary = new Dictionary<EnemyName, EnemyCreator>();
        foreach (EnemyCreator creator in _creators)
            foreach (EnemyName name in creator.EnemyNames)
                _dictionary[name] = creator;
        Debug.Log(transform.name + ": Load Dictionary", gameObject);
    }

    public EnemyType GetEnemyType(EnemyName name)
    {
        if (_dictionary.TryGetValue(name, out EnemyCreator creator))
            return creator.EnemyType;
        Debug.LogWarning("EnemyFactory: không tìm thấy creator cho " + name);
        return default;
    }

    public EnemyCtrl Create(EnemyName name, Vector3 pos, Quaternion rot = default, float multiplier = 1f)
    {
        if (!_dictionary.TryGetValue(name, out EnemyCreator creator))
        {
            Debug.LogWarning("EnemyFactory: không tìm thấy creator cho " + name);
            return null;
        }
        return creator.Create(name, pos, rot, multiplier);
    }
}
