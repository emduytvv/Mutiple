using UnityEngine;

[CreateAssetMenu(fileName = "IntrinsicSkill", menuName = "SO/IntrinsicSkill")]
public class IntrinsicSkillSO : ScriptableObject
{
    public int _id;
    public Sprite _icon;
    public IntrinsicSkillName _name;
    [TextArea] public string _description;
}
