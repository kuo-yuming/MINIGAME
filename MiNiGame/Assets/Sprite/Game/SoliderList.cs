using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliderList : MonoBehaviour
{
    public static SoliderList instance;

    [SerializeField] private SoldierListData m_SoldierListData;
    [SerializeField] private UIScrollView m_ScrollView;
    [SerializeField] private UITable m_Table;
    [SerializeField] private TweenPosition m_TweenPosition;

    public bool m_IsSoliderListClick = false;
    private SoldierBarSeat m_SelectSoldierBarSeat;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        m_IsSoliderListClick = false;
        SoldierListStart();
    }

    private void SoldierListStart()
    {
        if (GameDateSetting.instance.m_CountrySoldierAllDate.ContainsKey(GameDateSetting.instance.m_PlayerSelectCountry))
        {
            for (int i = 0; i < GameDateSetting.instance.m_CountrySoldierAllDate[GameDateSetting.instance.m_PlayerSelectCountry].Count; i++)
            {
                SoldierListData m_obj = Instantiate(m_SoldierListData, m_Table.transform);
                m_obj.name = i < 10 ? "0" + i : i.ToString();
                m_obj.DataInit(GameDateSetting.instance.m_CountrySoldierAllDate[GameDateSetting.instance.m_PlayerSelectCountry][i]);
                m_obj.Click.onClick = SelectSoldierClick;
                m_obj.gameObject.SetActive(true);
            }
            
            m_ScrollView.ResetPosition();
            //m_ScrollView.UpdatePosition();
            m_ScrollView.UpdateScrollbars(true);
            m_Table.repositionNow = true;
        }
        else
        {
            Debug.LogError("Select Country Name is Error, Country Name:" + GameDateSetting.instance.m_PlayerSelectCountry);
        }  
    }

    public void ListBarShowCheck(SoldierBarSeat usSoldierBarSeat)
    {
        m_SelectSoldierBarSeat = usSoldierBarSeat;

        if (m_TweenPosition.isActiveAndEnabled) return;
        if (m_SelectSoldierBarSeat.m_IsSoldierHave) return;

        if (m_IsSoliderListClick)
        {
            m_TweenPosition.PlayReverse();
        }
        else
        {
            m_TweenPosition.PlayForward();
        }
    }

    public void SelectSoldierClick(GameObject obj)
    {
        if (m_TweenPosition.isActiveAndEnabled) return;

        m_SelectSoldierBarSeat.MakeSolider(obj.GetComponent<SoldierListData>().SaveSoldierClass);
        m_TweenPosition.PlayReverse();
    }

    public void IsBoolCheck()
    {
        m_IsSoliderListClick = m_IsSoliderListClick ? false : true;
    }
}
