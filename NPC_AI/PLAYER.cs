//Простенький скрипт игрока. Чисто для проверок
using UnityEngine;

public class PLAYER : MonoBehaviour 
{
        private float _currentHealth;           //Текущее ХП
        public float MaxHealth;                         //Максимальное ХП

        public bool isDead;                                     //Мертв или нет
        public bool blaAttack;                          //Атакуем ли врага (bla ничего не значит, просто чтобы не совподало с методом название)

        public GameObject Target;                       //Цель наша

        float _attackDelay;                                     //Задержка при атаки

        void Awake ()   //http://unity3d.com/learn/tutorials/modules/beginner/scripting/awake-and-start
        {
                tag = "Player"; name = "Player";        //Присваение имени и тэга
                _currentHealth = MaxHealth;                     //Присвоение текущему ХП максимального ХП
        }

        void Update ()  //http://unity3d.com/learn/tutorials/modules/beginner/scripting/update-and-fixedupdate
        {
                if (!isDead)
                {
                        
                        if (Target && Input.GetButtonDown("Cancel"))    //Проверка на наличие цели и нажатие на ESCAPE (Esc)
                        {
                                Target = null;          //Убираем таргет
                                blaAttack = false;      //Убираем атаку
                                _attackDelay = 2f;      //Обнуляем таймер
                        }
                        if (Target && blaAttack && !Target.GetComponent<NPC>().Stats.isDead)                    //Если есть цель, если атакуем, если НПЦ не мертв
                                Attack();                       //Вызов метода атаки (почему то всегда когда я пишу метод, мне на голову приходит "МЕТАДОН!!!" (Я НЕ НАРКОМАН!!!) <img src="./images/smilies/4.gif" alt=":D" title="Гы" />

                }
        }

        //Метод получения урона ничего сложного. Есть входящая переменная типа Float которая записывает в себя урон
        public void TAKE_DAMAGE (float Damage)
        {
                _currentHealth -= Damage;
        }

        //Атака
        private void Attack ()
        {
                if (Target.GetComponent<NPC>().Stats.Friction != NPC_STATS._friction.Friend)
                {
                        //Таймер
                        if (_attackDelay > 0)
                                _attackDelay -= Time.deltaTime;
                        else
                        {
                                Target.GetComponent<NPC_AI>().TakeDamage (Random.Range(1f, 10f)); //Наносим урон от 1 до 10
                                _attackDelay = 2f; //Обнуляем таймер
                        }
                }
                else
                {
                        //Так как Атака у нас включается из НПЦ, придется добавить отключение атаки.
                        //Или там сделать проверку, но мне и так норм <img src="./images/smilies/4.gif" alt=":D" title="Гы" />
                        Debug.Log("Извини, я немогу бить друзей!");
                        blaAttack = false;
                }
        }
}
 