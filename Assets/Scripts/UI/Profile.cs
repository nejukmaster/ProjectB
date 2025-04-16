using ProjectB;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectB
{
    //좌측 상단에 위치해 플레이어의 돈, 피로도, 프로필 이미지등을 표시하는 UI입니다.
    public enum ProfileFace
    {
        SMILE = 0,
        HAPPY = 1,
        SAD = 2
    }
    public class Profile : MonoBehaviour
    {
        [SerializeField] GameObject profilePanel;
        [SerializeField] Sprite[] profileImages;
        [SerializeField] ProfileFace defaultFace;

        [SerializeField] GameObject moneyPanel;
        [SerializeField] GameObject fatigueBar;

        Coroutine currentCo;

        private void Update()
        {
            if(GameInstanceSystem.instance.GetLocalPlayer() != null)
            {
                PlayerInfo info = GameInstanceSystem.instance.GetLocalPlayer().playerInfo;

                Image fatigueProgress = fatigueBar.GetComponentsInChildren<Image>()[1];
                fatigueProgress.fillAmount = info.fatigue;

                moneyPanel.GetComponentInChildren<TextMeshProUGUI>().SetText(info.money.ToString());
            }
        }

        public void FaceChange(ProfileFace to, float duration)
        {
            if(currentCo != null)
                StopCoroutine(currentCo);
            currentCo = StartCoroutine(FaceChangeCo(to,duration));
        }

        public void SetProfile(ProfileFace face)
        {
            profilePanel.GetComponent<Image>().sprite = profileImages[(int)face];
        }

        IEnumerator FaceChangeCo(ProfileFace to, float duration)
        {
            SetProfile(to);
            yield return new WaitForSeconds(duration);
            SetProfile(defaultFace);
        }
    }
}
