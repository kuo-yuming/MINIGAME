using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDate : MonoBehaviour
{
    [SerializeField] private UISprite m_LevelUpSprite;
    [SerializeField] private UISprite m_BackgroundSprite;
    [SerializeField] private UIEventListener m_LevelUpClick;
    [SerializeField] private UILabel m_NowLevelLabel;

    private bool m_IsLevelUp = false;//是否再升級
    private int m_MaxLevel = 0;//最高等級
    private string m_BuildingName = "";//建築名稱
    private string m_BuildingShowName = "";//建築名稱(顯示用)
    private int m_BuildingTypeInt = 0;//建築TYPE
    private float m_LevelUpTime = 0;//建築升級時間
    private int m_LevelUpShowCount = 0;//表演次數
    private float m_MinusFillAmount = 0;//要減少SCROLLBAR的Amount
    private float m_UpdateTime = 0.2f;//每次刷新時間
    private int m_NextLevelUpMoney = 0;//下一階升級金額
    private int m_MinusMoneyCheck = 0;//升級總金額確認
    private int m_MinusMoeny = 0;//要減少金額
    private int m_NextLevelUpTechnology = 0;//下一階升級資源
    private int m_MinusTechnologyCheck = 0;//升級總資源確認
    private int m_MinusTechnology = 0;//要減少資源

    public void DataInit(int usEnum)
    {
        //TODO 補圖片名稱
        //m_LevelUpSprite.spriteName = "";
        m_BackgroundSprite.fillAmount = 0;
        m_LevelUpClick.onClick = OnLevelUpClick;
        m_LevelUpClick.onPress = DateShowClick;
        EnumDateCheck(usEnum);
        NextUpMoneyCheck(int.Parse(m_NowLevelLabel.text));
    }
    
    private void EnumDateCheck(int usEnum)
    {
        m_BuildingTypeInt = usEnum;
        switch (usEnum)
        {
            case (int)GameEnum.BUILDING_ENUM.BUSINESS:
                m_BuildingName = GameEnum.BUILDING_ENUM.BUSINESS.ToString();
                m_NowLevelLabel.text = GameDateSetting.instance.m_CountryAllDate[GameDateSetting.instance.m_PlayerSelectCountry].m_CountryBusinessLevel.ToString();
                m_MaxLevel = GameDateSetting.instance.m_CountryAllDate[GameDateSetting.instance.m_PlayerSelectCountry].m_CountryLevelMaxClass.m_CountryBusinessLevelMax;
                break;
            case (int)GameEnum.BUILDING_ENUM.TECHNOLOGY:
                m_BuildingName = GameEnum.BUILDING_ENUM.TECHNOLOGY.ToString();
                m_NowLevelLabel.text = GameDateSetting.instance.m_CountryAllDate[GameDateSetting.instance.m_PlayerSelectCountry].m_CountryTechnologyLevel.ToString();
                m_MaxLevel = GameDateSetting.instance.m_CountryAllDate[GameDateSetting.instance.m_PlayerSelectCountry].m_CountryLevelMaxClass.m_CountryTechnologyLevelMAX;
                break;
            case (int)GameEnum.BUILDING_ENUM.NATIONAL_DEFENSE:
                m_BuildingName = GameEnum.BUILDING_ENUM.NATIONAL_DEFENSE.ToString();
                m_NowLevelLabel.text = GameDateSetting.instance.m_CountryAllDate[GameDateSetting.instance.m_PlayerSelectCountry].m_CountryNationalDefenseLevel.ToString();
                m_MaxLevel = GameDateSetting.instance.m_CountryAllDate[GameDateSetting.instance.m_PlayerSelectCountry].m_CountryLevelMaxClass.m_CountryNationalDefenseLevelMax;
                break;
            case (int)GameEnum.BUILDING_ENUM.SOLDIER_INFIGHTIOG:
                m_BuildingName = GameEnum.BUILDING_ENUM.SOLDIER_INFIGHTIOG.ToString();
                m_NowLevelLabel.text = GameDateSetting.instance.m_CountryAllDate[GameDateSetting.instance.m_PlayerSelectCountry].m_CountrySoldierInfightingLevel.ToString();
                m_MaxLevel = GameDateSetting.instance.m_CountryAllDate[GameDateSetting.instance.m_PlayerSelectCountry].m_CountryLevelMaxClass.m_CountrySoldierInfightingLevelMax;
                break;
            case (int)GameEnum.BUILDING_ENUM.SOLDIER_DISTANT:
                m_BuildingName = GameEnum.BUILDING_ENUM.SOLDIER_DISTANT.ToString();
                m_NowLevelLabel.text = GameDateSetting.instance.m_CountryAllDate[GameDateSetting.instance.m_PlayerSelectCountry].m_CountrySoldierDistantLevel.ToString();
                m_MaxLevel = GameDateSetting.instance.m_CountryAllDate[GameDateSetting.instance.m_PlayerSelectCountry].m_CountryLevelMaxClass.m_CountrySoldierDistantLevelMAX;
                break;
            case (int)GameEnum.BUILDING_ENUM.SOLDIER_DEFENSE:
                m_BuildingName = GameEnum.BUILDING_ENUM.SOLDIER_DEFENSE.ToString();
                m_NowLevelLabel.text = GameDateSetting.instance.m_CountryAllDate[GameDateSetting.instance.m_PlayerSelectCountry].m_CountrySoldierDefenseLevel.ToString();
                m_MaxLevel = GameDateSetting.instance.m_CountryAllDate[GameDateSetting.instance.m_PlayerSelectCountry].m_CountryLevelMaxClass.m_CountrySoldierDefenseLevelMAX;
                break;
            default:
                Debug.LogError("GameEnum Building is number error");
                break;
        }
    }

    private void NextUpMoneyCheck(int usLV)
    {
        string m_Key = GameDateSetting.instance.m_PlayerSelectCountry + "/" + m_BuildingName;
        if (GameDateSetting.instance.m_CountryAllLevelDate.ContainsKey(m_Key))
        {
            foreach(var item in GameDateSetting.instance.m_CountryAllLevelDate[m_Key])
            {
                if(item.m_BuildingLevel == usLV)
                {
                    m_BuildingShowName = item.m_BuildingNameShow;
                    m_NextLevelUpMoney = item.m_BuildingLevelUpMoney;
                    m_NextLevelUpTechnology = item.m_BuildingLevelUpTechnology;
                    m_LevelUpTime = item.m_BuildingLevelUpTime;
                }
            }
        }
        else
        {
            Debug.LogError("CountryAllLevelDate key is error. keyName: " + m_Key);
        }
    }

    private void OnLevelUpClick(GameObject obj)
    {
        if (m_NowLevelLabel.text == "MAX") return;
        if (m_IsLevelUp) return;

        m_IsLevelUp = true;
        m_BackgroundSprite.fillAmount = 1;
        m_LevelUpShowCount = (int)(m_LevelUpTime / m_UpdateTime);
        Debug.Log("LevelUpShowCount:" + m_LevelUpShowCount);
        m_MinusFillAmount = 1 / (float)m_LevelUpShowCount;
        m_MinusMoneyCheck = m_NextLevelUpMoney;
        m_MinusMoeny = m_NextLevelUpMoney / m_LevelUpShowCount;
        m_MinusTechnologyCheck = m_NextLevelUpTechnology;
        m_MinusTechnology = m_NextLevelUpTechnology / m_LevelUpShowCount;
        InvokeRepeating("LevelUpShow", 0f, m_UpdateTime);
    }

    private void LevelUpShow()
    {
        if(GameDateSetting.instance.m_NowPlayerTotalMoney - m_MinusMoeny >= 0 
            && GameDateSetting.instance.m_NowPlayerTotalTechnology - m_MinusTechnology >= 0)
        {
            if(m_LevelUpShowCount == 1)
            {
                if (GameDateSetting.instance.m_NowPlayerTotalMoney - m_MinusMoneyCheck < 0) return;
                if (GameDateSetting.instance.m_NowPlayerTotalTechnology - m_MinusTechnologyCheck < 0) return;
            }

            m_BackgroundSprite.fillAmount = m_BackgroundSprite.fillAmount - m_MinusFillAmount;
            MainGame.instance.MinusMoneyShow(m_MinusMoeny);
            MainGame.instance.MinusTechnologyShow(m_MinusTechnology);
            m_MinusMoneyCheck -=  m_MinusMoeny;
            m_MinusTechnologyCheck -= m_MinusTechnology;
            m_LevelUpShowCount--;
            if (m_LevelUpShowCount == 0)
            {
                m_BackgroundSprite.fillAmount = 0;
                MainGame.instance.MinusMoneyShow(m_MinusMoneyCheck);
                MainGame.instance.MinusTechnologyShow(m_MinusTechnology);
                int nextlv = int.Parse(m_NowLevelLabel.text) + 1;
                EnumDateLevelUpCheck(m_BuildingTypeInt, nextlv);
                m_NowLevelLabel.text = nextlv >= m_MaxLevel ? "MAX" : (int.Parse(m_NowLevelLabel.text) + 1).ToString();
                NextUpMoneyCheck(nextlv);
                m_IsLevelUp = false;
                DateShowWindow.instance.DateShow(GameEnum.DATE_SHOW_ENUM.BUILDING, m_BuildingShowName
          , m_NextLevelUpMoney.ToString(), m_NextLevelUpTechnology.ToString(), m_MaxLevel.ToString());
                CancelInvoke("LevelUpShow");
            }
        }
    }
    //資料升級更新
    private void EnumDateLevelUpCheck(int usEnum,int lv)
    {
        switch (usEnum)
        {
            case (int)GameEnum.BUILDING_ENUM.BUSINESS:
                GameDateSetting.instance.m_PlayerCountryClass[GameEnum.PALYER_ENUM.PLAYER].m_CountryBusinessLevel = lv;
                break;
            case (int)GameEnum.BUILDING_ENUM.TECHNOLOGY:
                GameDateSetting.instance.m_PlayerCountryClass[GameEnum.PALYER_ENUM.PLAYER].m_CountryTechnologyLevel = lv;
                break;
            case (int)GameEnum.BUILDING_ENUM.NATIONAL_DEFENSE:
                GameDateSetting.instance.m_PlayerCountryClass[GameEnum.PALYER_ENUM.PLAYER].m_CountryNationalDefenseLevel = lv;
                break;
            case (int)GameEnum.BUILDING_ENUM.SOLDIER_INFIGHTIOG:
                GameDateSetting.instance.m_PlayerCountryClass[GameEnum.PALYER_ENUM.PLAYER].m_CountrySoldierInfightingLevel = lv;
                break;
            case (int)GameEnum.BUILDING_ENUM.SOLDIER_DISTANT:
                GameDateSetting.instance.m_PlayerCountryClass[GameEnum.PALYER_ENUM.PLAYER].m_CountrySoldierDistantLevel = lv;
                break;
            case (int)GameEnum.BUILDING_ENUM.SOLDIER_DEFENSE:
                GameDateSetting.instance.m_PlayerCountryClass[GameEnum.PALYER_ENUM.PLAYER].m_CountrySoldierDefenseLevel = lv;
                break;
            default:
                Debug.LogError("GameEnum Building LevelUp is number error");
                break;
        }
    }
    //顯示資訊
    private void DateShowClick(GameObject obj,bool isPress)
    {
        DateShowWindow.instance.m_IsPress = isPress;
        if (isPress)
        {
            DateShowWindow.instance.MainObjOpen(m_BuildingShowName);
            DateShowWindow.instance.DateShow(GameEnum.DATE_SHOW_ENUM.BUILDING, m_BuildingShowName
                , m_NextLevelUpMoney.ToString(), m_NextLevelUpTechnology.ToString(), m_MaxLevel.ToString());
        }
    }
}
