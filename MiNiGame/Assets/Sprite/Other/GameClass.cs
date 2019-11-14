using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClass
{
    //國家資料Class
    public class CountryClass
    {
        public string m_CountryName;//國家名稱
        public int m_CountryHp; //國家HP(國防等級 * 國家速度 *基本時間),(國家最高血量 + ( 國防等級 * 國家最高血量 * 10% ))
        public int m_CountryMoney;//國家資金(商業等級 * 國家速度 *基本時間)
        public int m_CountryTechnologyPoint;//國家科技點數(科技等級 * 國家速度 *基本時間)
        public int m_CountryBusinessLevel;//商業等級
        public int m_CountryTechnologyLevel;//科技等級
        public int m_CountryNationalDefenseLevel;//國防等級
        public int m_CountrySoldierInfightingLevel;//近戰裝備等級
        public int m_CountrySoldierDistantLevel;//遠戰裝備等級
        public int m_CountrySoldierDefenseLevel;//防禦裝備等級
        public int m_CountryGetMoneyTime;//國家獲得資金時間(秒)
        public int m_CountryGetTechnologyPoint;//國家獲得科技點時間(秒)
        public int m_CountryStartMoney;//國家初始金額
        public int m_CountryStartTechnologyPoint;//國家初始科技點
        public float m_CountrySpeed;//國家速度(資金生產,科技點樹生產)
        public CountryLevelMaxClass m_CountryLevelMaxClass;
    }
    //國家建築最高等級Class
    public class CountryLevelMaxClass
    {
        public int m_CountryBusinessLevelMax;//商業等級MAX
        public int m_CountryTechnologyLevelMAX;//科技等級MAX
        public int m_CountryNationalDefenseLevelMax;//國防等級MAX
        public int m_CountrySoldierInfightingLevelMax;//近戰裝備等級MAX
        public int m_CountrySoldierDistantLevelMAX;//遠戰裝備等級MAX
        public int m_CountrySoldierDefenseLevelMAX;//防禦裝備等級MAX
    }
    //國家建築資料Class
    public class CountryBuildingLevelClass
    {
        public string m_BuildingName;//建築名稱
        public string m_BuildingNameShow;//建築名稱(顯示用)
        public int m_BuildingLevel;//建築等級
        public float m_BuildingLevelUpTime;//建築升級時間
        public int m_BuildingLevelUpMoney;//升級需要金額
        public int m_BuildingLevelUpTechnology;//升級需要資源
    }
    //士兵所有資料Class
    public class SoldierClass
    {
        public string m_SoldierName;//兵種名稱
        public GameEnum.SOLDIER_ENUM m_SoldierType;//type
        public int m_SoldierHp;//血量
        public int m_SoldierAttack;//攻擊力
        public int m_SoldierDefense;//防禦力
        public float m_SoldierMoveSpeed;//移動速度
        public float m_SoldierAttackSpeed;//攻擊速度
        public float m_SoldierAttackRange;//攻擊範圍
        public float m_SoldierHiddenForecs;//潛在能力
        public int m_SoldierMoeny;//費用
        public int m_SoldierTechnology;//資源
        public string m_SoldierAtlasName;//Atlas名稱
        public float m_MakeTime;//製造時間
    }
    
}
