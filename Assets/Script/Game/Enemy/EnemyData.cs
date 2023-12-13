using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/EnemyData")]
public class EnemyData : ScriptableObject   //ScriptableObjectを継承する
{
    public int HP; //体力
    public int ATK; //攻撃力
    public float Speed; //素早さ
}
