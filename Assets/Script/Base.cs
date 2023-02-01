using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//���{�a��
public class Base : MonoBehaviour
{
    //�ő�X�e�[�^�X
    const float Max_hp = 99999;

    //��{�X�e�[�^�X
    public float hp = 100;
    public bool enemyUnit = false;

    //�o�t�l
    public float hp_Buff = 0.0f;  //HP�A�b�v
    
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
        //�}�C�i�X�̒l���Ȃ����m�F
        if (hp < 0) hp *= -1;
        if (hp_Buff < 0) hp_Buff *= -1;

        if (hp + hp_Buff >= Max_hp)
        {
            hp = Max_hp;
            Debug.Log("HP���ő�̒l�ɐݒ肳��܂����B");
        }
        else
        {
            hp += hp_Buff;
        }

    }

    void Start()
    {
        //�X�e�[�^�X�������`�F�b�N
        BuffSet();

        //HP�o�[�����p
        hpSlider.minValue = 0;
        hpSlider.maxValue = hp;
        hpSlider.value = hp;

        if (hpSlider == null) {
            Debug.Log("��n��HP�o�[��NULL�ł�");
        }
    }

}
