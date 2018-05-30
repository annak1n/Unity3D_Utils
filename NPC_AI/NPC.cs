//Это мозг всего НПЦ! он и будет отвечать у вас за бла бла бла.. Ну вы поняли)) за ничего он будет отвечать.))
//Этот скрипт хранит в себе все текущие переменные типа статистик и прочего от вашего НПЦ.

using UnityEngine;

[System.Serializable]
public class NPC_STATS : NPC_BASE       //Наследуем от Базы.
{
        [HideInInspector] // Скрывает Public переменные в инспекторе
        public float currentHealth;
        [HideInInspector]
        public float currentMana;
        [HideInInspector]
        public float currentEnergy;
        [HideInInspector]
        public float currentRage;

        //MAIN
        [Tooltip("Default: 60")] // Показывает подсказки в инспекторе при наведение на название функции
        public float HealthMax;
        [Tooltip("Default: 40")]
        public float ManaMax;
        [Tooltip("Default: 100")]
        public float EnergyMax;
        [Tooltip("Default: 100")]
        public float RageMax;

        //STATS
        [Space(10)]
        [Tooltip("Default: 2")]
        public byte Stamina;
        [Tooltip("Default: 3")]
        public byte Intellect;
        [Tooltip("Default: 3")]
        public byte Agility;
        [Tooltip("Default: 4")]
        public byte Strengh;
        [Tooltip("Default: 2")]
        public byte Spirit;

        //###PROTECTION
        [Space(10)]
        [Tooltip("Default: 0")]
        public float Armor;
        [Tooltip("Default: 0")]
        public float RangeArmorPercent;
        [Tooltip("Default: 0")]
        public float MagicArmorPercent;

        //###LEVEL
        [Space(10)]
        [Tooltip("Default: 1")]
        public byte Level;

        //###SPEED
        [Space(10)]
        [Tooltip("Default: 1.7")]
        public float AttackSpeed;
        [Tooltip("Default: 1.5")]
        public float WalkSpeed;
        [Tooltip("Default: 2.5")]
        public float FastWalkSpeed;
        [Tooltip("Default: 3.5")]
        public float RunSpeed;
        [Tooltip("Default: 3")]
        public float SwimSpeed;
        [Tooltip("Default: 7.5")]
        public float FlySpeed;

        //###ATTACK
        [Space(10)]
        [Tooltip("Default: 1")]
        public float MinDamage;
        [Tooltip("Default: 4")]
        public float MaxDamage;
        [Tooltip("Default: 4.5")]
        public float AttackCriticalChance;
        [Tooltip("Default: 2.5")]
        public float AttackDistance;
        [Tooltip("Default: 7.5")]
        public float VisibleDistance;

        //###CHANCE
        [Space(10)]
        [Tooltip("Default: 4")]
        public float MissChance;
        [Tooltip("Default: 2.5")]
        public float BlockChance;
        [Tooltip("Default: 1.75")]
        public float ParryChance;

        //###WALKING
        [Space(10)]
        public bool isWalking;
        [Tooltip("Default: 5")]
        public float MaxWalkingDistance;
        [Tooltip("Default: 20")]
        public float MaxFollowingDistance;
        [Tooltip("Default: 15")]
        public float StayOnPosition;

        //###ENUMS
        public enum _class { Melee, Range, Magic };
        public enum _friction { Neutral, Agressive, Friend };
        public _class Class;
        public _friction Friction;

        //###OTHER
        [HideInInspector]
        public bool isDead;
        [HideInInspector]
        public bool inCombat;
        [HideInInspector]
        public GameObject target;

        #region CheckOnEmptyAttributes
        //Метод сравнивающий значения, и если из тела метода он находит переменную которая равна нулю, заменяет это значение
        //на значение из NPC_BASE
        public void CheckOnEmptyAttributes ()
        {
                //###MAIN
                switch (Class)
                {
                case _class.Melee:
                        if (HealthMax == 0)
                                HealthMax = NPC_STATS.Main.HealthMax;
                        if (RageMax == 0)
                                RageMax = NPC_STATS.Main.RageMax;
                        break;

                case _class.Range:
                        if (HealthMax == 0)
                                HealthMax = NPC_STATS.Main.HealthMax;
                        if (EnergyMax == 0)
                                EnergyMax = NPC_STATS.Main.EnergyMax;
                        break;

                case _class.Magic:
                        if (HealthMax == 0)
                                HealthMax = NPC_STATS.Main.HealthMax;
                        if (ManaMax == 0)
                                ManaMax = NPC_STATS.Main.ManaMax;
                        break;
                }

                //###STATS
                if (Stamina == 0)
                        Stamina = NPC_STATS.Stats.Stamina;
                if (Intellect == 0)
                        Intellect = NPC_STATS.Stats.Intellect;
                if (Agility == 0)
                        Agility = NPC_STATS.Stats.Agility;
                if (Strengh == 0)
                        Strengh = NPC_STATS.Stats.Strengh;
                if (Spirit == 0)
                        Spirit = NPC_STATS.Stats.Spirit;

                //###PROTECTION
                //Все статы брони равны нулю в NPC_BASE скрипте, по этому тут нет присвоения значений
                //Если в NPC_BASE вы ввели значения не равные нулю, то расскоментируйте ниже написаное.
                /*
                if (Armor == 0)
                        Armor = NPC_STATS.Protection.Armor;
                if (RangeArmorPercent == 0)
                        RangeArmorPercent = NPC_STATS.Protection.RangeArmorPercent;
                if (MagicArmorPercent == 0)
                        MagicArmorPercent = NPC_STATS.Protection.MagicArmorPercent;
                 */

                //###LEVEL
                if (Level == 0)
                        Level = NPC_STATS.level.Level;

                //###SPEED
                if (WalkSpeed == 0)
                        WalkSpeed = NPC_STATS.Speed.WalkSpeed;
                if (FastWalkSpeed == 0)
                        FastWalkSpeed = NPC_STATS.Speed.FastWalkSpeed;
                if (RunSpeed == 0)
                        RunSpeed = NPC_STATS.Speed.RunSpeed;
                if (SwimSpeed == 0)
                        SwimSpeed = NPC_STATS.Speed.SwimSpeed;
                if (FlySpeed == 0)
                        FlySpeed = NPC_STATS.Speed.FlySpeed;


                //###ATTACK
                if (AttackSpeed == 0)
                        AttackSpeed = NPC_STATS.Attack.AttackSpeed;
                if (MinDamage == 0)
                        MinDamage = NPC_STATS.Attack.MinDamage;
                if (MaxDamage == 0)
                        MaxDamage = NPC_STATS.Attack.MaxDamage;
                if (AttackCriticalChance == 0)
                        AttackCriticalChance = NPC_STATS.Attack.AttackCriticalChance;
                if (AttackDistance == 0)
                        AttackDistance = NPC_STATS.Attack.AttackDistance;
                if (VisibleDistance == 0)
                        VisibleDistance = NPC_STATS.Attack.VisibleDistance;

                //###CHANCE
                if (MissChance == 0)
                        MissChance = NPC_STATS.Chance.MissChance;
                if (BlockChance == 0)
                        BlockChance = NPC_STATS.Chance.BlockChance;
                if (ParryChance == 0)
                        ParryChance = NPC_STATS.Chance.ParryChance;

                //###WALKING
                if (isWalking)
                {
                        if (MaxWalkingDistance == 0)
                                MaxWalkingDistance = NPC_STATS.Walking.MaxWalkingDistance;
                        if (MaxFollowingDistance == 0)
                                MaxFollowingDistance = NPC_STATS.Walking.MaxFollowingDistance;
                        if (StayOnPosition == 0)
                                StayOnPosition = NPC_STATS.Walking.StayOnPosition;
                }

                AssignCurrentVariables();
        }

        private void AssignCurrentVariables ()
        {
                currentHealth = HealthMax;
                currentMana = ManaMax;
                currentEnergy = EnergyMax;
                currentRage = RageMax;
        }
        #endregion
}
[RequireComponent(typeof(NPC_AI))]
public class NPC : MonoBehaviour 
{
        public NPC_STATS Stats;

        void Start ()
        {       
                Stats.CheckOnEmptyAttributes ();
        }
}
