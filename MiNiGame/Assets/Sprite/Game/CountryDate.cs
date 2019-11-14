using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryDate : MonoBehaviour
{
    public static CountryDate instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    //金額增加公式
    public int MoneyUpFormula(GameEnum.PALYER_ENUM usEnum)
    {
        //(國家資金 * 商業等級 * 國家速度 * 國家獲得資金時間)
        int outMoney = (int)(GameDateSetting.instance.m_PlayerCountryClass[usEnum].m_CountryMoney
            * GameDateSetting.instance.m_PlayerCountryClass[usEnum].m_CountryBusinessLevel
            * GameDateSetting.instance.m_PlayerCountryClass[usEnum].m_CountrySpeed
            * GameDateSetting.instance.m_PlayerCountryClass[usEnum].m_CountryGetMoneyTime);
        return outMoney;
    }
    //資源增加公式
    public int TechnologyUpFormula(GameEnum.PALYER_ENUM usEnum)
    {
        //(國家資金 * 科技等級 * 國家速度 * 國家獲得資金時間)
        int outMoney = (int)(GameDateSetting.instance.m_PlayerCountryClass[usEnum].m_CountryTechnologyPoint
            * GameDateSetting.instance.m_PlayerCountryClass[usEnum].m_CountryTechnologyLevel
            * GameDateSetting.instance.m_PlayerCountryClass[usEnum].m_CountrySpeed
            * GameDateSetting.instance.m_PlayerCountryClass[usEnum].m_CountryGetMoneyTime);
        return outMoney;
    }
}
