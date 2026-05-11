using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : SaiMonoBehaviour
{
    private static EnemyFactory _instance;
    public static EnemyFactory Instance => _instance;

    [SerializeField] private List<EnemyCreator> _creators;
    private Dictionary<EnemyType, EnemyCreator> _dictionary;

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
        foreach (Transform creator in transform)
            _creators.Add(creator.GetComponent<EnemyCreator>());
    }
    private void LoadDictionary()
    {
        if (_dictionary != null) return;
        _dictionary = new Dictionary<EnemyType, EnemyCreator>();
        foreach (EnemyCreator creator in _creators)
            _dictionary[creator.EnemyType] = creator;
        Debug.Log(transform.name + ": Load Dictionary", gameObject);
    }

    public EnemyCtrl Create(EnemyType type, Vector3 pos, Quaternion rot = default)
    {
        if (!_dictionary.TryGetValue(type, out EnemyCreator creator))
        {
            Debug.LogWarning("EnemyFactory: không tìm thấy creator cho " + type);
            return null;
        }
        return creator.Create(pos, rot);
    }
}
