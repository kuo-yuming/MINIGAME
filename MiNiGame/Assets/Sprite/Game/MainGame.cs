using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    public static MainGame instance;

    [SerializeField] private UILabel m_MoneyLabel;
    [SerializeField] private UILabel m_TechnologyLabel;
    [SerializeField] private UIEventListener m_GameStartClick;
    [SerializeField] private UIEventListener m_GameMapMoveClick;
    [SerializeField] private UIScrollView m_BuildingDateScrollView;
    [SerializeField] private UITable m_BuildingDateTable;
    [SerializeField] private BuildingDate m_BuildingDate;
    [SerializeField] private Camera m_Camera;
    [SerializeField] private UISprite m_BackgroundSpite;
    [SerializeField] private UIRoot m_CameraRoot;
    //測試
    [SerializeField] private SoldierDate m_SoldierDate;
    [SerializeField] private SoldierDate m_EnemySoldierDate;

    private bool m_IsGameStart = false;
    private int m_SaveUpMoney = 0;//要增加的金額
    private int m_ShowUpMoneyCount = 10;//要表演的COUNT(增加)
    private int m_SaveUpTechnology = 0;//要增加的金額
    private int m_NowUpShowCount = 10;//現在要表演的COUNT(增加)
    private int m_MapLeftMaxSeat = 0;//地圖左邊最大值
    private int m_MapRightMaxSeat = 0;//地圖右邊最大值
    private int m_MapMovePoint = 50;//地圖移動值
    private int m_PlusMoney = 0;
    private int m_PlusTechnology = 0;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        m_GameStartClick.onClick = OnGameStartClick;
        m_GameMapMoveClick.onDrag = CameraMoveClick;
        m_GameMapMoveClick.onClick = OnCameraClick;
        m_MapLeftMaxSeat = (int)((m_BackgroundSpite.localSize.y / 2) - (m_CameraRoot.manualWidth / 2)) * -1;
        m_MapRightMaxSeat = (int)((m_BackgroundSpite.localSize.y / 2) - (m_CameraRoot.manualWidth / 2)) * 1;
        m_SoldierDate.gameObject.SetActive(false);
        m_EnemySoldierDate.gameObject.SetActive(false);
    }

    private void GameStart()
    {
        int ran = (int)Random.Range(0, (int)GameEnum.SEAT_ENUM.MAX);
        int seat = ran == 0 ? -1 : 1;
        int seatX = (int)((m_BackgroundSpite.localSize.y / 2) - (m_CameraRoot.manualWidth / 2)) * seat;
        m_Camera.gameObject.transform.localPosition = new Vector3(seatX, 0, 0);
        InstantiateCountryDate();
        GameDateSetting.instance.m_PlayerCountryClass.Add(GameEnum.PALYER_ENUM.PLAYER, GameDateSetting.instance.m_CountryAllDate[GameDateSetting.instance.m_PlayerSelectCountry]);
        m_MoneyLabel.text = GameDateSetting.instance.m_PlayerCountryClass[GameEnum.PALYER_ENUM.PLAYER].m_CountryStartMoney.ToString();
        m_TechnologyLabel.text = GameDateSetting.instance.m_PlayerCountryClass[GameEnum.PALYER_ENUM.PLAYER].m_CountryStartTechnologyPoint.ToString();
        //測試
        m_SoldierDate.gameObject.SetActive(true);
        m_EnemySoldierDate.gameObject.SetActive(true);
        m_SoldierDate.SolderDateInit(GameEnum.PALYER_ENUM.PLAYER
            , GameDateSetting.instance.m_CountrySoldierAllDate[GameDateSetting.instance.m_PlayerSelectCountry][5]
            , GameDateSetting.instance.m_PlayerCountryClass[GameEnum.PALYER_ENUM.PLAYER].m_CountrySoldierInfightingLevel
            , GameDateSetting.instance.m_PlayerCountryClass[GameEnum.PALYER_ENUM.PLAYER].m_CountrySoldierDistantLevel
            , GameDateSetting.instance.m_PlayerCountryClass[GameEnum.PALYER_ENUM.PLAYER].m_CountrySoldierDefenseLevel
            ,1);
        m_EnemySoldierDate.SolderDateInit(GameEnum.PALYER_ENUM.ENEMY
            , GameDateSetting.instance.m_CountrySoldierAllDate[GameDateSetting.instance.m_PlayerSelectCountry][6]
            , GameDateSetting.instance.m_PlayerCountryClass[GameEnum.PALYER_ENUM.PLAYER].m_CountrySoldierInfightingLevel
            , GameDateSetting.instance.m_PlayerCountryClass[GameEnum.PALYER_ENUM.PLAYER].m_CountrySoldierDistantLevel
            , GameDateSetting.instance.m_PlayerCountryClass[GameEnum.PALYER_ENUM.PLAYER].m_CountrySoldierDefenseLevel
            ,2);
        GameDateSetting.instance.m_SelectSoldier = m_SoldierDate;
    }

    private void OnGameStartClick(GameObject obj)
    {
        m_IsGameStart = true;
        GameStart();
        StartCoroutine(GetMoney());
        m_GameStartClick.gameObject.SetActive(false);
    }
    //建築資料生成
    private void InstantiateCountryDate()
    {
        for (int i = 0; i < (int)GameEnum.BUILDING_ENUM.MAX; i++)
        {
            BuildingDate m_obj = Instantiate(m_BuildingDate, m_BuildingDateTable.transform);
            m_obj.DataInit(i);
            m_obj.name = i.ToString();
            m_obj.gameObject.SetActive(true);
        }

        m_BuildingDateTable.repositionNow = true;
        m_BuildingDateScrollView.ResetPosition();
        m_BuildingDateScrollView.UpdatePosition();
        m_BuildingDateScrollView.UpdateScrollbars(true);
    }
    //獲取金額
    IEnumerator GetMoney()
    {
        yield return new WaitForSeconds(GameDateSetting.instance.m_PlayerCountryClass[GameEnum.PALYER_ENUM.PLAYER].m_CountryGetMoneyTime);
        m_SaveUpMoney = CountryDate.instance.MoneyUpFormula(GameEnum.PALYER_ENUM.PLAYER);
        m_PlusMoney = m_SaveUpMoney / m_ShowUpMoneyCount;
        m_SaveUpTechnology = CountryDate.instance.MoneyUpFormula(GameEnum.PALYER_ENUM.PLAYER);
        m_PlusTechnology = m_SaveUpTechnology / m_ShowUpMoneyCount;
        InvokeRepeating("PlusMoneyShow", 0f, 0.05f);
        StartCoroutine(GetMoney());
    }
    //金錢增加表演
    private void PlusMoneyShow()
    {
        m_SaveUpMoney -= m_PlusMoney;
        m_SaveUpTechnology -= m_PlusTechnology;
        GameDateSetting.instance.m_NowPlayerTotalMoney = int.Parse(m_MoneyLabel.text) + m_PlusMoney;
        m_MoneyLabel.text = (int.Parse(m_MoneyLabel.text) + m_PlusMoney).ToString();
        GameDateSetting.instance.m_NowPlayerTotalTechnology = int.Parse(m_TechnologyLabel.text) + m_PlusTechnology;
        m_TechnologyLabel.text = (int.Parse(m_TechnologyLabel.text) + m_PlusTechnology).ToString();
        m_NowUpShowCount--;
        if (m_NowUpShowCount == 0)
        {
            m_MoneyLabel.text = (int.Parse(m_MoneyLabel.text) + m_SaveUpMoney).ToString();
            m_TechnologyLabel.text = (int.Parse(m_TechnologyLabel.text) + m_PlusTechnology).ToString();
            m_SaveUpMoney = 0;
            m_SaveUpTechnology = 0;
            m_NowUpShowCount = m_ShowUpMoneyCount;
            CancelInvoke("PlusMoneyShow");
        }
    }
    //金錢減少表演
    public void MinusMoneyShow(int usMinusMoney)
    {
        GameDateSetting.instance.m_NowPlayerTotalMoney = int.Parse(m_MoneyLabel.text) - usMinusMoney;
        m_MoneyLabel.text = (int.Parse(m_MoneyLabel.text) - usMinusMoney).ToString();
    }
    //資源減少表演
    public void MinusTechnologyShow(int usMinusTechnology)
    {
        GameDateSetting.instance.m_NowPlayerTotalTechnology = int.Parse(m_TechnologyLabel.text) - usMinusTechnology;
        m_TechnologyLabel.text = (int.Parse(m_TechnologyLabel.text) - usMinusTechnology).ToString();
    }
    //Camera移動
    private void CameraMoveClick(GameObject obj,Vector2 vec)
    {
        if(vec.x > 0)
        {
            if(m_MapLeftMaxSeat < m_Camera.gameObject.transform.localPosition.x - m_MapMovePoint)
            {
                m_Camera.gameObject.transform.localPosition = new Vector3(m_Camera.gameObject.transform.localPosition.x - m_MapMovePoint, 0, 0);
            }
        }
        else if(vec.x < 0)
        {
            if (m_MapRightMaxSeat > m_Camera.gameObject.transform.localPosition.x + m_MapMovePoint)
            {
                m_Camera.gameObject.transform.localPosition = new Vector3(m_Camera.gameObject.transform.localPosition.x + m_MapMovePoint, 0, 0);
            }
        }
    }
    //螢幕點選
    private void OnCameraClick(GameObject obj)
    {
        float nowMouseX = Input.mousePosition.x - (m_CameraRoot.manualWidth / 2);
        float nowMouseY = Input.mousePosition.y - (m_CameraRoot.manualHeight / 2);
        Vector2 v2 = new Vector2(m_Camera.transform.localPosition.x + nowMouseX, m_Camera.transform.localPosition.y + nowMouseY);
        if(GameDateSetting.instance.m_SelectSoldier != null)
        {
            GameDateSetting.instance.m_SelectSoldier.MoveCheck(v2.x,v2.y,true);
        }
    }
}
