using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierDate : MonoBehaviour
{
    [SerializeField] private UISprite m_SoldierSprite;
    [SerializeField] private UISprite m_HPSprite;
    [SerializeField] private BoxCollider2D m_SoldierAttackRange;
    [SerializeField] private UIEventListener m_SelectedClick;
    [SerializeField] private UISpriteAnimation m_SoldierSpriteAnimation;
    
    private GameClass.SoldierClass m_SoldierClassDate = new GameClass.SoldierClass();
    private int m_SoldierTotalAttack = 0;//士兵總攻擊力
    private int m_SoldierTotalDefense = 0;//士兵總防禦力
    private int m_SoldierNowHp = 0;//士兵現在總血量
    private Vector2 m_NextMovePoint = new Vector2();//士兵預定要移動的地點(地圖)
    private int m_MoveX;//移動方向
    private int m_MoveY;//移動方向
    private SoldierDate m_SelectEnemySoldier = null;//被選擇的敵方士兵
    private bool m_IsMove = false;//是否在移動(攻擊時停止移動)
    private bool m_IsAttack = false;//是否在攻擊()
    private bool m_IsDeadShow = false;//是否死亡表演
    private string m_AtlasName = "";
    private bool m_IsReloading = false;
    public int m_SoliderNumber;//士兵編號(場上最多只有六隻兵,所以會是1-6)

    private void Awake()
    {
        m_SelectedClick.onClick = SelectedOnClick;
    }

    private void Update()
    {
        if (m_IsMove)
        {
            if (m_SelectEnemySoldier != null)
            {
                if ((int)transform.localPosition.x != (int)m_SelectEnemySoldier.transform.localPosition.x
                    || (int)transform.localPosition.y != (int)m_SelectEnemySoldier.transform.localPosition.y)
                {
                    SoldierMoveUpdate(m_SelectEnemySoldier.transform.localPosition.x, m_SelectEnemySoldier.transform.localPosition.y);
                }
                else
                {
                    AtlasCheck(GameEnum.CHARA_MODE_ENUM.STOP);
                    m_IsMove = false;
                }
            }
            else
            {
                if ((int)transform.localPosition.x != (int)m_NextMovePoint.x || (int)transform.localPosition.y != (int)m_NextMovePoint.y)
                {
                    SoldierMoveUpdate(m_NextMovePoint.x, m_NextMovePoint.y);
                }
                else
                {
                    AtlasCheck(GameEnum.CHARA_MODE_ENUM.STOP);
                    m_IsMove = false;
                }
            }
        }

        if (m_IsDeadShow)
        {
            if (!m_SoldierSpriteAnimation.isPlaying)
            {
                Destroy(this.gameObject);
            }
        }
    }
    //取得資料(Prefab生成(玩家ENUM,Soldier資料,近戰裝備等級,遠戰裝備等級,防禦裝備等級))
    public void SolderDateInit(GameEnum.PALYER_ENUM usEnum, GameClass.SoldierClass date, int countryInfightingAttackLevel, int countryDistantAttackLevel, int countryDefenseLevel,int souldierNumber)
    {
        gameObject.name = usEnum.ToString();
        m_SoldierClassDate = date;
        m_AtlasName = m_SoldierClassDate.m_SoldierAtlasName;
        ShareFunction.SoliderSpriteCheck(m_AtlasName, m_SoldierSprite);
        AtlasCheck(GameEnum.CHARA_MODE_ENUM.STOP);
        //SoliderSpriteCheck(m_AtlasName);
        Vector2 vector2 = new Vector2();
        vector2.x = m_SoldierAttackRange.size.x + m_SoldierAttackRange.size.x * m_SoldierClassDate.m_SoldierAttackRange;
        vector2.y = m_SoldierAttackRange.size.y + m_SoldierAttackRange.size.y * m_SoldierClassDate.m_SoldierAttackRange;
        m_SoldierAttackRange.size = new Vector2(vector2.x, vector2.y);
        m_SoldierNowHp = date.m_SoldierHp;
        m_HPSprite.fillAmount = 1;
        m_SoldierTotalAttack = SoldierTypeTotalAttackCheck(m_SoldierClassDate.m_SoldierType, countryInfightingAttackLevel, countryDistantAttackLevel);//總攻擊力
        m_SoldierTotalDefense = date.m_SoldierDefense + (int)((float)countryDefenseLevel * date.m_SoldierHiddenForecs);//總防禦力
        m_SoliderNumber = souldierNumber;
        m_IsReloading = true;
    }
    //士兵的圖像
    //private void SoliderSpriteCheck(string name)
    //{
    //    m_SoldierSprite.atlas = (NGUIAtlas)Resources.Load("Atlas/" + name);
    //    if(m_SoldierSprite.atlas == null)
    //    {
    //        Debug.LogError("Atlas Name is Error,Atlas Name:" + name);
    //    }
    //    AtlasCheck(GameEnum.CHARA_MODE_ENUM.STOP);
    //}

    //攻擊力資料更新(近,遠戰強化時只更新總攻擊)
    public void TotalAttackUpdate(int countryInfightingAttackLevel, int countryDistantAttackLevel)
    {
        m_SoldierTotalAttack = SoldierTypeTotalAttackCheck(m_SoldierClassDate.m_SoldierType, countryInfightingAttackLevel, countryDistantAttackLevel);//總攻擊力
    }
    //防禦力資料更新(防禦強化時只更新總防禦)
    public void TotalDefenseUpdate(int countryDefenseLevel)
    {
        m_SoldierTotalDefense = m_SoldierClassDate.m_SoldierDefense + (int)((float)countryDefenseLevel * m_SoldierClassDate.m_SoldierHiddenForecs);//總防禦力
    }
    //確認TYPE
    private int SoldierTypeTotalAttackCheck(GameEnum.SOLDIER_ENUM type, int countryInfightingAttackLevel, int countryDistantAttackLevel)
    {
        int usCountryAttackLevel = type == GameEnum.SOLDIER_ENUM.INFIGHTING ? countryInfightingAttackLevel : countryDistantAttackLevel;
        int m_SoldierTotalAttack = m_SoldierClassDate.m_SoldierAttack + (int)((float)usCountryAttackLevel * m_SoldierClassDate.m_SoldierHiddenForecs);//總攻擊力
        return m_SoldierTotalAttack;
    }
    //士兵移動確認(地圖)
    public void MoveCheck(float transformX, float transformY,bool isStop)
    {
        m_NextMovePoint.x = transformX;
        m_NextMovePoint.y = transformY;
        float SoldierSpriteCheck = m_NextMovePoint.x - transform.localPosition.x >= 0 ? 180 : 0;
        m_MoveX = m_NextMovePoint.x - transform.localPosition.x >= 0 ? 1 : -1;
        m_MoveY = m_NextMovePoint.y - transform.localPosition.y >= 0 ? 1 : -1;
        m_SoldierSprite.transform.eulerAngles = new Vector3(0, SoldierSpriteCheck, 0);
        if (isStop) AttackStop();
        AtlasCheck(GameEnum.CHARA_MODE_ENUM.RAN);
        m_IsMove = true;
    }
    //士兵移動
    private void SoldierMoveUpdate(float transformX, float transformY)
    {
        bool isSeatXOk = Mathf.Abs((int)transform.localPosition.x - (int)transformX) <= 5 ? true : false;
        bool isSeatYOk = Mathf.Abs((int)transform.localPosition.y - (int)transformY) <= 5 ? true : false;
        float seatX = isSeatXOk ? transformX : transform.localPosition.x + (Time.deltaTime * m_SoldierClassDate.m_SoldierMoveSpeed * m_MoveX);
        float seatY = isSeatYOk ? transformY : transform.localPosition.y + (Time.deltaTime * m_SoldierClassDate.m_SoldierMoveSpeed * m_MoveY);
        transform.localPosition = new Vector3(seatX, seatY, 0);
    }
    //點擊事件(被選擇)
    private void SelectedOnClick(GameObject obj)
    {
        Debug.Log("點選確認");
        if (gameObject.name == GameEnum.PALYER_ENUM.PLAYER.ToString()) return;
        GameDateSetting.instance.m_SelectSoldier.m_SelectEnemySoldier = this;
        GameDateSetting.instance.m_SelectSoldier.SelectedCheck();
    }
    //處理點選後事件
    private void SelectedCheck()
    {
        MoveCheck(m_SelectEnemySoldier.transform.localPosition.x, m_SelectEnemySoldier.transform.localPosition.y,false);
    }
    //士兵發生觸碰
    private void OnTriggerStay2D(Collider2D collider)
    {
        Debug.Log("觸碰角色:" + gameObject.name);
        if (collider.gameObject.name == this.gameObject.name) return;//自己人不攻擊
        if (m_SelectEnemySoldier == null) return;//沒有選擇攻擊目標
        if (m_SelectEnemySoldier.m_SoliderNumber != collider.gameObject.GetComponent<SoldierDate>().m_SoliderNumber) return;//不是被指定士兵的編號
        if (!IsAttackRange()) return;
        if (m_IsAttack) return;
        m_IsMove = false;
        AtlasCheck(GameEnum.CHARA_MODE_ENUM.STOP);
        if (!m_IsReloading) return;
        m_IsAttack = true;
        InvokeRepeating("AttackStart", 0f, m_SoldierClassDate.m_SoldierAttackSpeed);//攻擊開始
    }
    //是否在攻擊距離
    private bool IsAttackRange()
    {
        bool isbool = false;
        int usXR = Mathf.Abs((int)transform.localPosition.x - (int)(m_SelectEnemySoldier.transform.localPosition.x + m_SelectEnemySoldier.m_SoldierSprite.width / 2));
        int usXL = Mathf.Abs((int)transform.localPosition.x - (int)(m_SelectEnemySoldier.transform.localPosition.x - m_SelectEnemySoldier.m_SoldierSprite.width / 2));
        int usYR = Mathf.Abs((int)transform.localPosition.y - (int)(m_SelectEnemySoldier.transform.localPosition.y + m_SelectEnemySoldier.m_SoldierSprite.height / 2));
        int usYL = Mathf.Abs((int)transform.localPosition.y - (int)(m_SelectEnemySoldier.transform.localPosition.y - m_SelectEnemySoldier.m_SoldierSprite.height / 2));
        int range = (int)(m_SoldierAttackRange.size.x / 2) + 5;//5為可差距值
        if ((usXR <= range || usXL <= range) && (usYR <= range || usYL <= range))
        {
            isbool = true;
        }
        return isbool;
    }
    //填裝時間
    private void AttackReLoad()
    {
        m_IsReloading = true;
        CancelInvoke("AttackReLoad");
    }
    //開始攻擊
    private void AttackStart()
    {
        if (!m_IsAttack) return;
        if (!m_IsReloading) return;
        if (m_SelectEnemySoldier == null)
        {
            //對方死亡停止攻擊
            AttackStop();
            return;
        }
        m_IsReloading = false;
        InvokeRepeating("AttackReLoad", m_SoldierClassDate.m_SoldierAttackSpeed, 0);//重新填裝
        AtlasCheck(GameEnum.CHARA_MODE_ENUM.ATTACK);
        m_SelectEnemySoldier.HpCheck(m_SoldierTotalAttack);
    }
    //停止攻擊
    private void AttackStop()
    {
        if (m_IsAttack) CancelInvoke("AttackStart");
        m_IsAttack = false;
        m_SelectEnemySoldier = null;
    }
    //扣血確認
    private void HpCheck(int attack)
    {
        int hurt = attack - m_SoldierTotalDefense <= 0 ? 1 : attack - m_SoldierTotalDefense;//傷害計算
        m_SoldierNowHp = m_SoldierNowHp - hurt > 0 ? m_SoldierNowHp - hurt : 0;//剩餘血量計算
        m_HPSprite.fillAmount = m_SoldierNowHp > 0 ? (float)m_SoldierNowHp / (float)m_SoldierClassDate.m_SoldierHp : 0;//Bar的顯示
        if(m_SoldierNowHp == 0)
        {
            AttackStop();
            SoldierDeadShow();
        }
    }
    //死亡表演
    private void SoldierDeadShow()
    {
        AtlasCheck(GameEnum.CHARA_MODE_ENUM.DEAD);
        m_IsDeadShow = true;
    }
    //圖片確認
    private void AtlasCheck(GameEnum.CHARA_MODE_ENUM usEnum)
    {
        string modeString = "";
        m_SoldierSpriteAnimation.loop = false;
        switch (usEnum)
        {
            case GameEnum.CHARA_MODE_ENUM.STOP:
                modeString = "Stop";
                break;
            case GameEnum.CHARA_MODE_ENUM.ATTACK:
                modeString = "Attack";
                break;
            case GameEnum.CHARA_MODE_ENUM.RAN:
                modeString = "Ran";
                if (m_SoldierSpriteAnimation.loop) return;
                m_SoldierSpriteAnimation.loop = true;
                break;
            case GameEnum.CHARA_MODE_ENUM.DEAD:
                modeString = "Dead";
                break;
            default:
                Debug.LogError("CHARA_MODE_ENUM IS ERROR");
                break;
        }

        m_SoldierSprite.spriteName = m_AtlasName + "_" + modeString + "_" + "01";//圖片初始化
        m_SoldierSpriteAnimation.namePrefix = m_AtlasName + "_" + modeString + "_";
        m_SoldierSpriteAnimation.frameIndex = 1;
        m_SoldierSpriteAnimation.Play();
    }
}
