using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateShowWindow : MonoBehaviour
{
    public static DateShowWindow instance;
    [SerializeField] private GameObject m_MainObj;
    [SerializeField] private UISprite m_SoldierSprite;
    [SerializeField] private UISprite m_BuildingSprite;
    [SerializeField] private UILabel m_NameLabel;
    [SerializeField] private UILabel m_NextMoneyLabel;
    [SerializeField] private UILabel m_NextTechnologyLabel;
    [SerializeField] private UILabel m_MaxLevelLabel;
    [SerializeField] private UIEventListener m_CloseClick;

    public bool m_IsPress;
    private string m_OpenName;
    private float m_DelayTime = 0;
    private int m_OpenTime = 1;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        m_CloseClick.onClick = CloseObj;
    }

    private void Update()
    {
        if (m_IsPress)
        {
            if(m_DelayTime < m_OpenTime)
            {
                m_DelayTime += Time.deltaTime;
            }
            else
            {
                m_MainObj.gameObject.SetActive(true);
            }
        }
        else
        {
            m_DelayTime = 0;
        }
    }

    public void MainObjOpen(string usName)
    {
        m_OpenName = usName;
    }

    private void CloseObj(GameObject obj)
    {
        m_MainObj.gameObject.SetActive(false);
    }

    public void DateShow(GameEnum.DATE_SHOW_ENUM usEnum,string name,string usMoney,string usTechnology,string usLevel)
    {
        //TODO 補圖片名稱
        if (m_OpenName != name) return;

        m_SoldierSprite.gameObject.SetActive(false);
        m_BuildingSprite.gameObject.SetActive(false);
        switch(usEnum)
        {
            case GameEnum.DATE_SHOW_ENUM.BUILDING:
                m_BuildingSprite.gameObject.SetActive(true);
                //m_BuildingSprite.spriteName = "";
                break;
            case GameEnum.DATE_SHOW_ENUM.SOLDIER:
                m_SoldierSprite.gameObject.SetActive(true);
                //m_SoldierSprite.spriteName = "";
                break;
        }
        m_NameLabel.text = name;
        m_NextMoneyLabel.text = usMoney;
        m_NextTechnologyLabel.text = usTechnology;
        m_MaxLevelLabel.text = usLevel;
    }
}
