using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class HoldToLoad : MonoBehaviour
    {
        public float holdTime = 2f;
        public Image fillImage;
        private float timeHeld = 0f;
        private Coroutine holdCoroutine;
        private bool hasTriggered = false;
        public AudioSource intro;
        public AudioSource loop;
        public GameObject player;
        public GameObject boss;
        public GameObject NPC;
        public GameObject BossMusic;
        public static event Action onHoldComplete;

        void Start()
        {
            if (fillImage != null)
            {
                fillImage.type = Image.Type.Filled;
                fillImage.fillMethod = Image.FillMethod.Radial360;
                fillImage.fillOrigin = (int)Image.Origin360.Top;
                fillImage.fillClockwise = true;
                fillImage.fillAmount = 0f;
                var color = fillImage.color;
                color.a = 1f;
                fillImage.color = color;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z) && !hasTriggered && holdCoroutine == null)
            {
                holdCoroutine = StartCoroutine(HoldRoutine());
            }

            if (Input.GetKeyUp(KeyCode.Z))
            {
                CancelHold();
            }
        }

        private IEnumerator HoldRoutine()
        {
            timeHeld = 0f;
            UpdateFill(0f);

            while (timeHeld < holdTime)
            {
                if (!Input.GetKey(KeyCode.Z))
                {
                    ResetFillAndTimer();
                    yield break;
                }

                timeHeld += Time.deltaTime;
                UpdateFill(timeHeld / holdTime);
                Debug.Log("Z key held, timeHeld: " + timeHeld);
                yield return null;
            }

            Debug.Log("Hold complete, triggering action");
            TriggerHoldAction();
            ResetFillAndTimer();
            holdCoroutine = null;
        }

        private void TriggerHoldAction()
        {
            if (hasTriggered) return;
            hasTriggered = true;

            if (boss) boss.SetActive(true);
            else Debug.LogWarning("HoldToLoad: boss reference is null.");

            if (NPC) NPC.SetActive(false);
            if (BossMusic) BossMusic.SetActive(true);

            var dungeonMusicObj = GameObject.Find("DungeonMusic");
            if (dungeonMusicObj) Destroy(dungeonMusicObj);

            if (intro != null)
            {
                intro.Play();
            }
            else
            {
                Debug.LogWarning("HoldToLoad: intro AudioSource is null.");
            }

            if (loop != null)
            {
                if (intro != null && intro.clip != null)
                {
                    loop.PlayScheduled(AudioSettings.dspTime + intro.clip.length);
                }
                else
                {
                    loop.Play();
                }
            }

            if (player)
            {
                player.transform.position = new Vector3(26f, 7f, 0f);
            }
            else
            {
                Debug.LogWarning("HoldToLoad: player reference is null.");
            }

            onHoldComplete?.Invoke();
        }

        private void CancelHold()
        {
            if (holdCoroutine != null)
            {
                StopCoroutine(holdCoroutine);
                holdCoroutine = null;
            }
            ResetFillAndTimer();
        }

        private void ResetFillAndTimer()
        {
            timeHeld = 0f;
            UpdateFill(0f);
        }

        private void UpdateFill(float amount)
        {
            if (fillImage != null)
            {
                fillImage.fillAmount = Mathf.Clamp01(amount);
            }
        }
    }
}