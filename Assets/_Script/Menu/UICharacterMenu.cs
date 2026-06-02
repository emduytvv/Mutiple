using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UICharacterMenu : SaiMonoBehaviour
{
    [Header("Login")]
    public int Number;
    [SerializeField] private Button _leftArrow;
    [SerializeField] private Button _rightArrow;

    [SerializeField] private List<GameObject> listCharacter;
    [SerializeField] private List<GameObject> listPanelStats;
    protected override void Awake()
    {
        base.Awake();
        _leftArrow.onClick.AddListener(() => SwitchCharacter(-1));
        _rightArrow.onClick.AddListener(() => SwitchCharacter(1));

    }
    public string GetCharacterNameSelected()
    {
        return listCharacter[Number].name;
    }
    protected virtual void SwitchCharacter(int Num)
    {
        for (int i = 0; i < listCharacter.Count; i++)
        {
            listCharacter[i].SetActive(false);
            listPanelStats[i].SetActive(false);
        }

        Number += Num;

        if (Number >= listCharacter.Count)
        {
            Number = 0;
        }

        if (Number < 0)
        {
            Number = listCharacter.Count - 1;
        }

        listCharacter[Number].SetActive(true);
        listPanelStats[Number].SetActive(true);
    }
}
