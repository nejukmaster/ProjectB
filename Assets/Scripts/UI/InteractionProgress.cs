using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectB 
{
    public class InteractionProgress : MonoBehaviour
    {
        [SerializeField] GameObject interactionProgressBar;
        [SerializeField] Image ProgressionMask;

        public void StartProgression(bool useProgression, float prgressionTime, float waitTime, Action Callback, Action Preprocess)
        {
            Preprocess();
            interactionProgressBar.SetActive(useProgression);
            StartCoroutine(ProgressCo(useProgression, prgressionTime, waitTime, Callback));
        }
        IEnumerator ProgressCo(bool useProgression, float prgressionTime, float waitTime, Action Callback)
        {
            ProgressionMask.fillAmount = 0;
            while (useProgression && ProgressionMask.fillAmount < 1)
            {
                ProgressionMask.fillAmount += 1.0f/(20* prgressionTime);
                yield return new WaitForSeconds(0.05f);
            }
            if(waitTime > 0)
                yield return new WaitForSeconds(waitTime);
            Callback();
            interactionProgressBar.SetActive(false);
            yield return null;
        }
    }
}
