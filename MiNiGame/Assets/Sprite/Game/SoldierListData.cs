using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierListData : MonoBehaviour
{
    [SerializeField] private UISprite m_SoldierSprite;
    [SerializeField] private UIEventListener m_Click;

    private GameClass.SoldierClass m_SaveSoldierClass = new GameClass.SoldierClass();
    public GameClass.SoldierClass SaveSoldierClass { get { return m_SaveSoldierClass; } }
    public UIEventListener Click
    {
        get
        {
            return m_Click;
        }
        set
        {
            m_Click = value;
        }
    }

    public void DataInit(GameClass.SoldierClass usSoldierClass)
    {
        ShareFunction.SoliderSpriteCheck(usSoldierClass.m_SoldierAtlasName, m_SoldierSprite);
        m_SoldierSprite.spriteName = ShareFunction.SoldierAtlasName(usSoldierClass.m_SoldierAtlasName);
        m_SaveSoldierClass = usSoldierClass;
    }
}
