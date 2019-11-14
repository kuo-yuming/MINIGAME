using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GameDateSetting : MonoBehaviour
{
    public static GameDateSetting instance;
    public static bool m_IsPrefabCopy = false;//是否取得資料
    public string m_PlayerSelectCountry = "";
    public int m_NowPlayerTotalMoney = 0;
    public int m_NowPlayerTotalTechnology = 0;
    public SoldierDate m_SelectSoldier = null;

    public Dictionary<GameEnum.PALYER_ENUM, GameClass.CountryClass> m_PlayerCountryClass = new Dictionary<GameEnum.PALYER_ENUM, GameClass.CountryClass>();
    public Dictionary<string, GameClass.CountryClass> m_CountryAllDate = new Dictionary<string, GameClass.CountryClass>();//國家相關資料
    public Dictionary<string, List<GameClass.SoldierClass>> m_CountrySoldierAllDate = new Dictionary<string, List<GameClass.SoldierClass>>();//國家兵種所有相關資料
    public Dictionary<string, List<GameClass.CountryBuildingLevelClass>> m_CountryAllLevelDate = new Dictionary<string, List<GameClass.CountryBuildingLevelClass>>();//國家等級相關資料

    private void Awake()
    {
        if (m_IsPrefabCopy) return;

        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);//複製資料
        m_IsPrefabCopy = true;
        string[] countryClassList = GetResourcesTxtDate("Txt/CountryClass");
        string[] countryLevelMaxClassList = GetResourcesTxtDate("Txt/CountryLevelMaxClass");
        CountryAllListCheck(countryClassList, countryLevelMaxClassList);
        string[] countryBuildingLevelClass = GetResourcesTxtDate("Txt/CountryBuildingLevelClass");
        CountryBuildingAllListCheck(countryBuildingLevelClass);
        string[] soldierClassList = GetResourcesTxtDate("Txt/SoldierClass");
        CountrySoldierAllListCheck(soldierClassList);
        m_PlayerSelectCountry = GameEnum.COUNTRY_ENUM.EMPIRE.ToString();
    }
    //取各項TXT檔
    private string[] GetResourcesTxtDate(string file)
    {
        //讀取csv二進位制檔案
        TextAsset binAsset = (TextAsset)Resources.Load(file);
        //讀取每一行的內容
        string[] usMessagetext = binAsset.text.Split('\n');
        return usMessagetext;
    }
    //取各國初始資料
    #region CountryAllListCheck
    private void CountryAllListCheck(string[] usClass, string[] usClassMax)
    {
        if (usClass.Length != usClassMax.Length)
        {
            Debug.LogError("CountryClass.txt or CountryLevelMaxClass.txt Length is Error");
            return;
        }

        //第一筆資料不取用
        for (int i = 1; i < usClass.Length; i++)
        {
            if (usClass[i] == "") return;

            string[] saveString = usClass[i].Split(',');
            string[] saveStringMax = usClassMax[i].Split(',');

            if (saveString[0] != saveStringMax[0])
            {
                Debug.LogError("CountryClass.txt or CountryLevelMaxClass.txt name list is error");
                return;
            }

            if (m_CountryAllDate.ContainsKey(saveString[0]))
            {
                Debug.LogError("m_CountryAllDate key is error");
                return;
            }

            GameClass.CountryClass m_Class = new GameClass.CountryClass();
            m_Class.m_CountryLevelMaxClass = new GameClass.CountryLevelMaxClass();
            m_Class.m_CountryName = saveString[0];//國家名稱
            m_Class.m_CountryHp = int.Parse(saveString[1]); //國家HP(國防等級 * 國家速度 *基本時間),(國家最高血量 + ( 國防等級 * 國家最高血量 * 10% ))
            m_Class.m_CountryMoney = int.Parse(saveString[2]);//國家資金(商業等級 * 國家速度 *基本時間)
            m_Class.m_CountryTechnologyPoint = int.Parse(saveString[3]);//國家科技點數(科技等級 * 國家速度 *基本時間)
            m_Class.m_CountryBusinessLevel = int.Parse(saveString[4]);//商業等級
            m_Class.m_CountryTechnologyLevel = int.Parse(saveString[5]);//科技等級
            m_Class.m_CountryNationalDefenseLevel = int.Parse(saveString[6]);//國防等級
            m_Class.m_CountrySoldierInfightingLevel = int.Parse(saveString[7]);//近戰裝備等級
            m_Class.m_CountrySoldierDistantLevel = int.Parse(saveString[8]);//遠戰裝備等級
            m_Class.m_CountrySoldierDefenseLevel = int.Parse(saveString[9]);//防禦裝備等級
            m_Class.m_CountryGetMoneyTime = int.Parse(saveString[10]);//國家獲得金額時間
            m_Class.m_CountryGetTechnologyPoint = int.Parse(saveString[11]);//國家獲得科技點時間
            m_Class.m_CountryStartMoney = int.Parse(saveString[12]);//國家初始金額
            m_Class.m_CountryStartTechnologyPoint = int.Parse(saveString[13]);//國家初始科技點
            string[] lastString = saveString[14].Split('\r');//最後一筆資料特別處理
            m_Class.m_CountrySpeed = float.Parse(lastString[0]);//國家速度(資金生產,科技點樹生產)
            m_Class.m_CountryLevelMaxClass.m_CountryBusinessLevelMax = int.Parse(saveStringMax[1]);//商業等級MAX
            m_Class.m_CountryLevelMaxClass.m_CountryTechnologyLevelMAX = int.Parse(saveStringMax[2]);//科技等級MAX
            m_Class.m_CountryLevelMaxClass.m_CountryNationalDefenseLevelMax = int.Parse(saveStringMax[3]);//國防等級MAX
            m_Class.m_CountryLevelMaxClass.m_CountrySoldierInfightingLevelMax = int.Parse(saveStringMax[4]);//近戰裝備等級MAX
            m_Class.m_CountryLevelMaxClass.m_CountrySoldierDistantLevelMAX = int.Parse(saveStringMax[5]);//遠戰裝備等級MAX
            string[] lastStringMax = saveStringMax[6].Split('\r');//最後一筆資料特別處理
            m_Class.m_CountryLevelMaxClass.m_CountrySoldierDefenseLevelMAX = int.Parse(lastStringMax[0]);//防禦裝備等級MAX
            m_CountryAllDate.Add(m_Class.m_CountryName, m_Class);
        }
    }
    #endregion
    //取各國建築資料
    #region CountryBuildingAllListCheck
    private void CountryBuildingAllListCheck(string[] usClass)
    {
        //第一筆資料不取用
        for (int i = 1; i < usClass.Length; i++)
        {
            if (usClass[i] == "") return;

            string[] saveString = usClass[i].Split(',');
            string m_key = saveString[0] + "/" + saveString[1];
            GameClass.CountryBuildingLevelClass m_CountryBuildingLevelClass = new GameClass.CountryBuildingLevelClass();
            m_CountryBuildingLevelClass.m_BuildingName = saveString[1];//建築名稱
            m_CountryBuildingLevelClass.m_BuildingNameShow = saveString[2];//建築名稱(顯示用)
            m_CountryBuildingLevelClass.m_BuildingLevel = int.Parse(saveString[3]);//建築等級
            m_CountryBuildingLevelClass.m_BuildingLevelUpTime = float.Parse(saveString[4]);//建築升級時間
            m_CountryBuildingLevelClass.m_BuildingLevelUpMoney = int.Parse(saveString[5]);//升級需要金額
            string[] lastString = saveString[6].Split('\r');
            m_CountryBuildingLevelClass.m_BuildingLevelUpTechnology = int.Parse(lastString[0]);//升級需要資源

            if (!m_CountryAllLevelDate.ContainsKey(m_key))
            {
                List<GameClass.CountryBuildingLevelClass> m_List = new List<GameClass.CountryBuildingLevelClass>();
                m_List.Add(m_CountryBuildingLevelClass);
                m_CountryAllLevelDate.Add(m_key, m_List);
            }
            else
            {
                m_CountryAllLevelDate[m_key].Add(m_CountryBuildingLevelClass);
            }
        }
    }
    #endregion
    //取各國兵種資料
    #region CountrySoldierAllListCheck
    private void CountrySoldierAllListCheck(string[] usClass)
    {
        for (int i = 1; i < usClass.Length; i++)
        {
            if (usClass[i] == "") return;

            string[] saveString = usClass[i].Split(',');
            GameClass.SoldierClass m_SoldierClass = new GameClass.SoldierClass();
            m_SoldierClass.m_SoldierName = saveString[1];//兵種名稱
            m_SoldierClass.m_SoldierType = SoldierEnumCheck(int.Parse(saveString[2]));//type
            m_SoldierClass.m_SoldierHp = int.Parse(saveString[3]);//血量
            m_SoldierClass.m_SoldierAttack = int.Parse(saveString[4]);//攻擊力
            m_SoldierClass.m_SoldierDefense = int.Parse(saveString[5]);//防禦力
            m_SoldierClass.m_SoldierMoveSpeed = float.Parse(saveString[6]);//移動速度
            m_SoldierClass.m_SoldierAttackSpeed = float.Parse(saveString[7]);//攻擊速度
            m_SoldierClass.m_SoldierAttackRange = float.Parse(saveString[8]);//攻擊範圍
            m_SoldierClass.m_SoldierHiddenForecs = float.Parse(saveString[9]);//潛在能力
            m_SoldierClass.m_SoldierMoeny = int.Parse(saveString[10]);//費用
            m_SoldierClass.m_SoldierTechnology = int.Parse(saveString[11]);//資源
            m_SoldierClass.m_SoldierAtlasName = saveString[12];//Atlas名稱
            string[] lastString = saveString[13].Split('\r');
            m_SoldierClass.m_MakeTime = float.Parse(lastString[0]);//製造時間

            if (!m_CountrySoldierAllDate.ContainsKey(saveString[0]))
            {
                List<GameClass.SoldierClass> m_List = new List<GameClass.SoldierClass>();
                m_List.Add(m_SoldierClass);
                m_CountrySoldierAllDate.Add(saveString[0], m_List);
            }
            else
            {
                m_CountrySoldierAllDate[saveString[0]].Add(m_SoldierClass);
            }
        }
    }
    #endregion
    private GameEnum.SOLDIER_ENUM SoldierEnumCheck(int number)
    {
        GameEnum.SOLDIER_ENUM outEnum = GameEnum.SOLDIER_ENUM.INFIGHTING;
        switch (number)
        {
            case (int)GameEnum.SOLDIER_ENUM.INFIGHTING:
                outEnum = GameEnum.SOLDIER_ENUM.INFIGHTING;
                break;
            case (int)GameEnum.SOLDIER_ENUM.DISTANT:
                outEnum = GameEnum.SOLDIER_ENUM.DISTANT;
                break;
            default:
                Debug.LogError("Soldier enum is error");
                break;
        }

        return outEnum;
    }
}
