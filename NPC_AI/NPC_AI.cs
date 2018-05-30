//А Это наше ТЕЛО! тут вся логика не до логика)).
//В комментариях приведенны строки, они могут не совпадать, т.к возможно я что то до писал и строка сбилась.
//Делать Было нечего вот и решил навалять вот это..
using UnityEngine;

public class NPC_AI : MonoBehaviour 
{
        private NPC NPC;                                                //Наш МОЗГ!!!
        private NavMeshAgent NAV_MESH_AGENT;    //Агент 007

        private Vector3 PLAYER_POSITION;                //Позиция игрока
        private Vector3 START_POSITION;                 //Стартовая позиция

        private float _distanceToPlayer;                //Дистанция до игрока 40строка
        private float _distanceToStartPosition; //Дистанция до позиции старта 42строка

        private PLAYER PLAYER_SCRIPT;                   //Ссылка на скрипт игрока.

        private int _frictionNumber;                    //Локальная переменная под фракцию НПЦ при старте.
        private float _attackSpeed;                             //Локальная переменная под счетчик задержки между ударами.
        private float _stayOnPosition;                  //Локальная переменная под счетчик ожидания на новой позиции.
        private float _timeOutBattle;                   //Название может и не правильное. Это для выхода из боя
        private float _visibleDistance;                 //Нужна для исправления бага.
        
        void Start ()
        {
                PLAYER_SCRIPT = GameObject.FindGameObjectWithTag("Player").GetComponent<PLAYER>();
                NPC = this.GetComponent<NPC>(); //Добавляем скрипт с этого же объекта

                //Если на объекте нет Агента 007 -> Добавляем его и сразу забиваем на него ссылку
                if (gameObject.GetComponent<NavMeshAgent>() == null)
                        NAV_MESH_AGENT = gameObject.AddComponent<NavMeshAgent>();
                //Или же просто забиваем его в ссылку
                else
                        NAV_MESH_AGENT = gameObject.GetComponent<NavMeshAgent>();

                //Указываем что останавливатся нужно на дистанции атаки, а не нюхать модельку игрока
                NAV_MESH_AGENT.stoppingDistance = NPC.Stats.AttackDistance;

                //Забиваем число его фракции. (Нужен для того чтобы вернуть его из агрессивного в нейтральный)
                _frictionNumber = (int)NPC.Stats.Friction; //0-neutral, 1-agressive, 2-friend : friction enum

                //Ну и стартовая позиция записывается при спавне
                START_POSITION = this.transform.position;

                //Сохранение начального значение переменной
                _visibleDistance = NPC.Stats.VisibleDistance;
        }
        void Update ()
        {
                //Записываем в переменную позицию игрока
                PLAYER_POSITION = GameObject.FindGameObjectWithTag("Player").transform.position;

                //Записываем в переменную дистанцию до игрока.
                _distanceToPlayer = Vector3.Distance(this.transform.position, PLAYER_POSITION);

                //Записываем в переменную дистанцию до спавна.
                _distanceToStartPosition = Vector3.Distance(this.transform.position, START_POSITION);

                //Если НПЦ НЕ МЕРТВ!
                if (!NPC.Stats.isDead)
                {
                        //Проверяем статус нпц ( Фракцией называется потому что изначально у него были фракции в дальнейшем убрал)
                        if (NPC.Stats.Friction == NPC_STATS._friction.Agressive && !PLAYER_SCRIPT.isDead)
                        {
                                //Если игрок в зони видимости нпц -> Преследуем игрока
                                if (_distanceToPlayer <= NPC.Stats.VisibleDistance)
                                        FollowingPlayer();
                        }

                        #region ПРОВЕРКИ НА БОЙ
                        //Если НПЦ не в бою
                        if (!NPC.Stats.inCombat)
                        {
                                //Если здоровья меньше максимального -> Восстанавливаем здоровье игроку.
                                if (NPC.Stats.currentHealth < NPC.Stats.HealthMax)
                                        NPC.Stats.currentHealth += (5 * NPC.Stats.Level) * Time.deltaTime;
                        }
                        //Если НПЦ в бою
                        if (NPC.Stats.inCombat)
                        {
                                //Если дистанция до игрока больше видимой, или игрок умер
                                if (_distanceToPlayer > NPC.Stats.VisibleDistance || PLAYER_SCRIPT.isDead)
                                {
                                        //Считаем время до выхода из боя
                                        if (_timeOutBattle > 0)
                                                _timeOutBattle -= Time.deltaTime;
                                        else
                                                //Выходим из боя
                                                NPC.Stats.inCombat = false;
                                }
                        }
                        #endregion
                        #region ПРОВЕРКИ НА ХП
                        //Если здоровье вышло за границы -> Выравниваем его по максимальному
                        if(NPC.Stats.currentHealth > NPC.Stats.HealthMax)
                                NPC.Stats.currentHealth = NPC.Stats.HealthMax;
                        
                        //Если здоровье ушло. -> Включаем смерть
                        if (NPC.Stats.currentHealth <= 0f)
                        {
                                NPC.Stats.isDead = true;
                                NPC.Stats.currentHealth = 0f;
                        }
                        #endregion
                }

                //Если НПЦ может ходить и не в бою и не мертв.
                if (NPC.Stats.isWalking && !NPC.Stats.inCombat && !NPC.Stats.isDead)
                {
                        //Вызываем метод.
                        Walking ();
                }

                //Возвращение обзора для НПЦ, когда он почти добежал до точки старта
                Debug.Log(Vector3.Distance(transform.position, START_POSITION));
                if (Vector3.Distance(transform.position, START_POSITION) < 5f)
                        NPC.Stats.VisibleDistance = _visibleDistance;
        }

        //А вот и он.
        private void Walking ()
        {
                //Если таймер простоя на точке больше 0 -> Считаем таймер
                if (_stayOnPosition > 0)
                        _stayOnPosition -= Time.deltaTime;
                //Или же Выдаем ему новую позицию, отправляем его туда и обнуляем таймер.
                else
                {
                        /*
                        ЭТОТ ВАРИАНТ Я ИСПОЛЬЗОВАЛ РАНЬШЕ.

                        float x = Random.Range(0f, NPC.Stats.MaxWalkingDistance);
                        float z = Random.Range(0f, NPC.Stats.MaxWalkingDistance);
                        Vector3 newPosition = new Vector3(x, transform.position.y, z);
                        NAV_MESH_AGENT.SetDestination(newPosition);
                        */

                        //СЕЙЧАС РЕШИЛ ПОПРОБОВАТЬ ВОТ ТАК.
                        //+ Тут его мы не отпускаем от стартовой позиции. иначе он бы ушел через 30 минут в африку пешком)))
                        NAV_MESH_AGENT.SetDestination(new Vector3( 
                                                                  START_POSITION.x + Random.Range(0f, NPC.Stats.MaxWalkingDistance),    //x
                                                                  transform.position.y,                                                         //y
                                                                  START_POSITION.z + Random.Range(0f, NPC.Stats.MaxWalkingDistance)));  //z

                        //Обнуление таймера
                        _stayOnPosition = NPC.Stats.StayOnPosition;
                }
        }

        //Преследование игрока
        private void FollowingPlayer ()
        {
                //Включаем режим боя.
                NPC.Stats.inCombat = true;
                //Если наш моб убежал за максимальную дистанцию преследования -> заставляем его бежать на респаун, и отключаем режим боя.
                if (_distanceToStartPosition > NPC.Stats.MaxFollowingDistance && NPC.Stats.inCombat)
                {
                        //Вот как раз тут и пригодится нам переменная _frictionNumber
                        //Здесь НПЦ изначально был нейтральным, но когда мы его ударили(стр.205) и он стал агрессивным, и тут мы все возвращаем назад
                        if (NPC.Stats.Friction == NPC_STATS._friction.Agressive && _frictionNumber == 0)
                                NPC.Stats.Friction = (NPC_STATS._friction)_frictionNumber;

                        NAV_MESH_AGENT.SetDestination(START_POSITION);
                        NPC.Stats.inCombat = false;
                        //Обнуляем ему таймер на тупняк.
                        _stayOnPosition = 0f;
                        //Ограничиваем обзор для того чтобы небыло бага, когда моб дрыгается из стороны в сторону, так как SetDestanation пересекаются
                        NPC.Stats.VisibleDistance = 2f;
                }
                //Или же бежим за игроком
                else
                        NAV_MESH_AGENT.SetDestination(PLAYER_POSITION);

                //Если дистанция до игрока уже позволяет бить игрока -> Бъем игрока
                if (_distanceToPlayer <= NPC.Stats.AttackDistance)
                        Attack ();
        }


        //Бъем игрока здесь
        private void Attack ()
        {
                //Если есть задержка перед ударом -> считаем ее.
                if (_attackSpeed > 0)
                        _attackSpeed -= Time.deltaTime;
                //Или же наносим игрока повреждения
                else
                {
                        //Кидаем монетку от 0 до 100 на промах
                        float randChance = Random.Range (0f, 100f);
                        //Если выпало больше, то бьем (если <= то промах)
                        if (randChance > NPC.Stats.MissChance)
                        {
                                //Снова выбрасываем монетку, только теперь на крит.
                                randChance = Random.Range (0f, 100f);
                                //Если выпало больше, то бьем обычный удар
                                if (randChance > NPC.Stats.AttackCriticalChance)
                                        //атака во врага обычная 
                                        PLAYER_SCRIPT.TAKE_DAMAGE (Random.Range(NPC.Stats.MinDamage, NPC.Stats.MaxDamage));
                                else
                                        //атака во врага критом 
                                        PLAYER_SCRIPT.TAKE_DAMAGE (Random.Range(NPC.Stats.MinDamage * 2, NPC.Stats.MaxDamage * 2));
                        }
                        else
                                //Можно вывести на экран, что атака не удалась
                                Debug.Log("NPC MISS");

                        //Обнуляем задержку между атаками.
                        _attackSpeed = NPC.Stats.AttackSpeed;
                }
        }

        //Публичная переменная т.к получаем урон из другого скрипта.
        public void TakeDamage (float Damage)
        {
                //Если наш тупоголовый нейтрал, и мы его ударяем то он становиться агрессивным
                //151-152 строка возвращает положение на изначальное.
                if (NPC.Stats.Friction == NPC_STATS._friction.Neutral)
                        NPC.Stats.Friction = NPC_STATS._friction.Agressive;

                //Вводим НПЦ в режим боя
                NPC.Stats.inCombat = true;
                //Устанавливаем Таймер для выхода из боя, реализация его здесь нужна для постоянного обновления при получении урона
                _timeOutBattle = 5f;

                //И снова тут мы будем бросать монетку на паррирование удара.
                float randChance = Random.Range (0f, 100f);
                //Если монетка выкинула больше, то нпц не с паррировал удар
                if (randChance > NPC.Stats.ParryChance)
                {
                        //Снова монетка, теперь уже на блокировку удара
                        randChance = Random.Range (0f, 100f);
                        //Если больше то не блокировали...
                        if (randChance > NPC.Stats.BlockChance)
                        {
                                //Заводим локальную переменную под логику деления урона на броню
                                //пример: 44 урон, 100 брони.
                                //damageDivider = ((100 брони + 100 = 200) / 100) = 2
                                //HP -= 44 / (2) = 22.
                                
                                //пример: 44 урон, 75 брони.
                                //Делитель = ((75 брони + 100 = 200) / 100) = 1.75
                                //ХП -= 44 / (1.75) = 25
                                
                                //ХП ИГРОКА                                      УРОН                                       ДЕЛИТЕЛЬ
                                NPC.Stats.currentHealth -=      Damage  /       ((NPC.Stats.Armor + 100) / 100);
                        }
                        else
                                Debug.Log("Attack Blocked");
                }
                else
                        Debug.Log("Parry Attack");
        }

        //Можете сами заменить на что хотите, мне подходит и такой вариант.
        //Если у нас мышка находится на объекте и мы нажали ЛКМ. Добавляем в таргет этот объект
        //Если нажимаем ПКМ то добавляем в таргет объект и включаем атаку.
        void OnMouseOver ()
        {
                if (Input.GetButtonDown("Fire1"))
                        PLAYER_SCRIPT.Target = this.gameObject;
                if (Input.GetButtonDown("Fire2"))
                {
                        PLAYER_SCRIPT.blaAttack = true;
                        PLAYER_SCRIPT.Target = this.gameObject;
                }
        }
}