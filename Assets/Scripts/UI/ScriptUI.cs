using ProjectB;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectB
{
    //플레이어와 NPC의 대화를 표시하는 UI입니다.
    public class ScriptUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField, Range(1f, 100f)] float textSpeed;
        [SerializeField] TextMeshProUGUI tmp;
        [SerializeField] GameObject npcNameTag;
        [SerializeField] GameObject playerNameTag;

        Consumer currentConsumer;
        Coroutine coroutine;

        IEnumerator<Script> scriptEnumerator;

        public void SetScriptGroup(ScriptGroup group)
        {
            scriptEnumerator = group.GetEnumerator();
        }

        public void SetCurrentConsumer(Consumer currentConsumer)
        {
            this.currentConsumer = currentConsumer;
        }

        public void OutputScript(Script script)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            tmp.text = "";
            coroutine = StartCoroutine(OutputScriptCo(DeserializeScript(script.messages)));
        }

        public void OutputNextScript()
        {
            if (scriptEnumerator.MoveNext())
            {
                if(scriptEnumerator.Current.speaker == "Player")
                {
                    playerNameTag.SetActive(true);
                    npcNameTag.SetActive(false);
                    playerNameTag.GetComponentInChildren<TextMeshProUGUI>().SetText("Player");
                }
                else
                {
                    npcNameTag.SetActive(true);
                    playerNameTag.SetActive(false);
                    npcNameTag.GetComponentInChildren<TextMeshProUGUI>().SetText(scriptEnumerator.Current.speaker);
                }
                OutputScript(scriptEnumerator.Current);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }

        IEnumerator OutputScriptCo(string msg)
        {
            for (int i = 0; i < msg.Length; i++)
            {
                tmp.SetText(msg.Substring(0, i));
                yield return new WaitForSeconds(1 / textSpeed);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OutputNextScript();
            }
        }

        public string DeserializeScript(string script)
        {
            string s;
            s = script.Replace("*menu", currentConsumer.GetCurrentMenu().GetName());
            return s;
        }
    }
}
