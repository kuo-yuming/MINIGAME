using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBarSeat : MonoBehaviour
{
    [SerializeField] private UIEventListener m_BoxClick;
    [SerializeField] private UISprite m_BlackBackground;
    [SerializeField] private UISprite m_SoldierSprite;
    [SerializeField] private GameObject m_SeachObject;

    public bool m_IsSoldierHave = false;
    private float m_UpdateTime = 0.2f;//每次刷新時間
    private int m_LevelUpShowCount = 0;//表演次數

    private void Awake()
    {
        m_BoxClick.onClick = OnBoxClick;
        m_SeachObject.SetActive(false);
        m_BlackBackground.enabled = false;
    }

    private void OnBoxClick(GameObject obj)
    {
        if (m_IsSoldierHave) return;

        SoliderList.instance.ListBarShowCheck(this);
    }
    //製造士兵
    public void MakeSolider(GameClass.SoldierClass usClass)
    {
        m_IsSoldierHave = true;
        m_BlackBackground.enabled = true;
        m_BlackBackground.fillAmount = 1;
        ShareFunction.SoliderSpriteCheck(usClass.m_SoldierAtlasName, m_SoldierSprite);
        m_SoldierSprite.spriteName = ShareFunction.SoldierAtlasName(usClass.m_SoldierAtlasName);

    }

    
}
