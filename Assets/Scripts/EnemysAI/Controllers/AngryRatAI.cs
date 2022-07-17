using System.Collections;
using UnityEngine;
using UnityEngine.Events;
namespace EnemysAI
{
    [RequireComponent(typeof(Move))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(EnemyHealth))]
    public class AngryRatAI : MonoBehaviour
    {
        [Header("Параметры скорости")]
        [SerializeField] private float walkSpeed = 2f; //Скорость при ходьбе
        [SerializeField] private float runSpeed = 3.3f; //Скорость при беге
        private bool isStopped = false;
        private EnemyTarget target; //Подвижный ли таргет

        //Всякие приватные поля
        private bool isSleep = false;

        //Ссылки на другие классы
        [Header("Другое")]
        [SerializeField] private UnityEvent onSleep = new UnityEvent();
        [SerializeField] private UnityEvent onWakeUp = new UnityEvent();
        private Animator animator; //Ссылка на аниматор объекта
        [Header("Другие компоненты")]
        [SerializeField] private Combat combat;
        [SerializeField] private TargetSelection targetSelection;
        private Move moving;
        private EnemyHealth health;

        private IEnumerator ChangeSpeed(TargetType moveType) //Плавный переход скорости
        {
            float nextSpeed;
            if (moveType == TargetType.Static) nextSpeed = walkSpeed;
            else nextSpeed = runSpeed;

            float k = (nextSpeed - moving.speed) / 20;
            int n = (int)((nextSpeed - moving.speed) / k);
            if (n < 0f) n *= -1;

            for (int i = 0; i < n; i++)
            {
                yield return new WaitForSeconds(0.25f);
                moving.speed += k;
            }
        }
        private void CheckTarget(EnemyTarget target)
        {
            if (targetSelection.targets.Count == 0 && !isSleep)
            {
                GoSleep();
                this.target = null;
            }
            if (targetSelection.targets.Count > 0 && isSleep)
            {
                WakeUp();
                if (this.target == target) return;
                else if (this.target == null || this.target.targetMoveType != target.targetMoveType) StartCoroutine(ChangeSpeed(target.targetMoveType));
                this.target = target;
            }
        }

        //Оглушение
        public IEnumerator Stun(float duration)
        {
            SetStun(true, true);
            yield return new WaitForSeconds(duration);
            ResetStun(true, true);
        }
        public void GetStunned(float duration) { StartCoroutine(Stun(duration)); }
        public void SetStun(bool stopChange, bool blockChange)
        {
            if (stopChange) moving.SetStop(true);
            if (blockChange) moving.SetBlocking(true);
            combat.SetStop(true);
            isStopped = true;
            if (animator.GetBool("isRun")) animator.SetBool("isRun", false);
            animator.SetBool("isStunned", true);
        }
        public void ResetStun(bool stopChange, bool blockChange)
        {
            if (blockChange) moving.SetBlocking(false);
            if (stopChange) moving.SetStop(false);
            combat.SetStop(false);
            isStopped = false;
            animator.SetBool("isStunned", false);
        }

        //Типо сеттеры и геттеры
        public void GoSleep()
        {
            if (!isSleep)
            {
                isSleep = true;
                onSleep.Invoke();
                if (animator.GetBool("isRun")) animator.SetBool("isRun", false);
                moving.enabled = false;
                health.enabled = false;
                for (int i = 0; i < transform.childCount; i++)
                    transform.GetChild(i).gameObject.SetActive(false);
                targetSelection.gameObject.SetActive(true);
            }
        }
        public void WakeUp()
        {
            if (isSleep)
            {
                isSleep = false;
                onWakeUp.Invoke();
                moving.enabled = true;
                health.enabled = true;

                for (int i = 0; i < transform.childCount; i++)
                    transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        public void SetStop(bool active) { isStopped = active; }
        public bool GetStop() { return isStopped; }

        //Юнитивские методы
        private void Start()
        {
            animator = GetComponent<Animator>();
            moving = GetComponent<Move>();
            health = GetComponent<EnemyHealth>();
            moving.speed = walkSpeed;

            //События
            GetComponent<EnemyHealth>().stun.AddListener(GetStunned);
            targetSelection.onTargetChange.AddListener(CheckTarget);
            targetSelection.onResetTarget.AddListener(CheckTarget);
            GoSleep();
        }
        private void Update() //Основная логика
        {
            if (!isSleep && !isStopped)
            {
                if (animator != null && moving != null)//Анимация и атака
                {
                    //Анимация бега
                    if (moving.isNowWalk && !animator.GetBool("isRun")) animator.SetBool("isRun", true);
                    if ((!moving.isNowWalk || moving.GetStop()) && animator.GetBool("isRun")) animator.SetBool("isRun", false);
                }
            }
        }
    }
}