using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//福本和也
public class Base : MonoBehaviour
{
    //最大ステータス
    const float Max_hp = 99999;

    //基本ステータス
    public float hp = 100;
    public bool enemyUnit = false;

    //バフ値
    public float hp_Buff = 0.0f;  //HPアップ
    
    //UI
    public Slider hpSlider;

    public void Damage(float damage)
    {
        hp -= damage;

        hpSlider.value = hp;

        //Debug.Log(hp);
    }

    void BuffSet()
    {
        //マイナスの値がないか確認
        if (hp < 0) hp *= -1;
        if (hp_Buff < 0) hp_Buff *= -1;

        if (hp + hp_Buff >= Max_hp)
        {
            hp = Max_hp;
            Debug.Log("HPが最大の値に設定されました。");
        }
        else
        {
            hp += hp_Buff;
        }

    }

    void Start()
    {
        //ステータス整合性チェック
        BuffSet();

        //HPバー処理用
        hpSlider.minValue = 0;
        hpSlider.maxValue = hp;
        hpSlider.value = hp;

        if (hpSlider == null) {
            Debug.Log("基地のHPバーがNULLです");
        }
    }

}
