//Используется csharp
//PLAYER SCRIPT
//RPG TEST NPC BASE CLASS
//Это базовый класс. Сделан для того чтобы из него читались все дефолтные значения для НПЦ 1 уровня.
//В принципе переписать для разных уровней не составляет труда, но увы я уже устал... да и печатная машинка сломалась
//Тут я думаю ничего комментировать не нужно, у кого с английским плохо, как у меня: Google Translate Welcome

using UnityEngine;

public class NPC_BASE
{
        [System.Serializable]
        public struct _main
        {
                const float _HEALTHMAX = 60f;
                const float _MANAMAX = 40f;
                const float _RAGEMAX = 100f;
                const float _ENERGYMAX = 100f;
                
                public float HealthMax { get { return _HEALTHMAX; }}
                public float ManaMax { get { return _MANAMAX; }}
                public float RageMax { get { return _RAGEMAX; }}
                public float EnergyMax { get { return _ENERGYMAX; }}
        }
        [System.Serializable]
        public struct _stats
        {
                const byte _STAMINA = 2;
                const byte _INTELLECT = 3;
                const byte _AGILITY = 3;
                const byte _STRENGH = 4;
                const byte _SPIRIT = 2;
                
                public byte Stamina { get { return _STAMINA; }}
                public byte Intellect { get { return _INTELLECT; }}
                public byte Agility { get { return _AGILITY; }}
                public byte Strengh { get { return _STRENGH; }}
                public byte Spirit { get { return _SPIRIT; }}
        }
        [System.Serializable]
        public struct _protection
        {
                const float _ARMOR = 0f;
                const float _RANGE_ARMOR_PERCENT = 0f;
                const float _MAGIC_ARMOR_PERCENT = 0f;
                
                public float Armor { get { return _ARMOR; } }
                public float RangeArmorPercent { get { return _RANGE_ARMOR_PERCENT; }}
                public float MagicArmorPercent { get { return _MAGIC_ARMOR_PERCENT; }}
        }
        [System.Serializable]
        public struct _level
        {
                const byte _LEVEL = 1;
                public byte Level { get { return _LEVEL; }}
        }
        [System.Serializable]
        public struct _speed
        {
                const float _WALK_SPEED = 1.5f;
                const float _FASTWALK_SPEED = 2.5f;
                const float _RUN_SPEED = 3.5f;
                const float _SWIM_SPEED = 3f;
                const float _FLY_SPEED = 7.5f;

                public float WalkSpeed { get { return _WALK_SPEED; }}
                public float FastWalkSpeed { get { return _FASTWALK_SPEED; }}
                public float RunSpeed { get { return _RUN_SPEED; }}
                public float SwimSpeed { get { return _SWIM_SPEED; }}
                public float FlySpeed { get { return _FLY_SPEED; }}
        }
        [System.Serializable]
        public struct _attack
        {
                const float _ATTACK_SPEED = 1.7f;
                const float _ATTACK_CRITICAL_CHANCE = 4.5f;
                const float _MIN_DAMAGE = 1f;
                const float _MAX_DAMAGE = 4f;
                const float _ATTACK_DISTANCE = 2.5f;
                const float _VISIBLE_DISTANCE = 10f;

                public float AttackSpeed { get { return _ATTACK_SPEED;}}
                public float AttackCriticalChance { get { return _ATTACK_CRITICAL_CHANCE; }}
                public float MinDamage { get { return _MIN_DAMAGE; }}
                public float MaxDamage { get { return _MAX_DAMAGE; }}
                public float AttackDistance { get { return _ATTACK_DISTANCE; }}
                public float VisibleDistance { get { return _VISIBLE_DISTANCE; }}
        }
        [System.Serializable]
        public struct _walking
        {
                const float _MAX_DISTANCE = 5f;
                const float _MAX_FOLLOWING_DISTANCE = 20f;
                const float _STAY_ON_NEW_POSITION = 15f;

                public float MaxWalkingDistance { get { return _MAX_DISTANCE; }}
                public float MaxFollowingDistance { get { return _MAX_FOLLOWING_DISTANCE; }}
                public float StayOnPosition { get { return _STAY_ON_NEW_POSITION; }}
        }
        [System.Serializable]
        public struct _chance
        {
                const float _MISS_CHANCE = 4f;
                const float _BLOCK_CHANCE = 2.5f;
                const float _PARRY_CHANCE = 1.75f;

                public float MissChance { get { return _MISS_CHANCE; }}
                public float BlockChance { get { return _BLOCK_CHANCE; }}
                public float ParryChance { get { return _PARRY_CHANCE; }}
        }

        public static _main Main;
        public static _stats Stats;
        public static _speed Speed;
        public static _attack Attack;
        public static _chance Chance;
        public static _protection Protection;
        public static _level level;
        public static _walking Walking;
}