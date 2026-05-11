using UnityEngine;

// ScriptableObject cho item loại tiêu hao (PowerUp)
// Tạo: chuột phải trong Project → Create → SO → PowerUpDataSO
[CreateAssetMenu(fileName = "PowerUpDataSO", menuName = "SO/PowerUpDataSO")]
public class PowerUpDataSO : ItemDataSO
{
    public float _value;
    public float _duration; // 0 = vĩnh viễn
}
