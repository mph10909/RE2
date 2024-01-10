using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ResidentEvilClone
{
    public class DeathCulling : MonoBehaviour
    {
        [SerializeField]Camera cam;
        [SerializeField]LayerMask cullOut;
        [SerializeField]GameObject pointMarker; 
        [SerializeField]float distance = 10;
        [SerializeField]float speed = 20;
        [SerializeField]float cullingDistance;

        void OnEnable()
        {
            Actions.OnPlayerKilled += DeathCull;
        }

        void OnDisable()
        {
            Actions.OnPlayerKilled -= DeathCull;
        }


        void DeathCull(bool playerDead, GameObject player)
        {
                cam.cullingMask = cullOut;
                cam.farClipPlane = cullingDistance;
                cam.transform.LookAt(player.transform);
                pointMarker.SetActive(false);
                StartCoroutine(LookAt(player));
        }

        private IEnumerator LookAt(GameObject player)
        {
            float timer = 0;
            while (timer < 30)
            {
                this.transform.LookAt(player.transform);
                float newSpeed = speed * Time.deltaTime;
                float dist = Vector3.Distance(transform.position, player.transform.position);

                if (dist > distance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, newSpeed);
                }
                timer += Time.deltaTime;
                yield return null;
                transform.RotateAround(player.transform.position, Vector3.up, newSpeed * 2);
            }
            Fader.Instance.FadeOut(5, false);
            yield return new WaitForSeconds(4.99f);
            SceneManager.LoadScene("Start", LoadSceneMode.Single);

        }

    }
}
