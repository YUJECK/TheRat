using UnityEngine;

public class FrogPredictor : MonoBehaviour
{
    public InteractivePanel panel;
    public GameObject NotMuchCheese;
    public DialogueManager dialogueManager;
    private bool SaysYes;
    private int SaysSentences;
    private bool isEndOfExtraSentences = false;
    public int PredictCost;
    [TextArea(3, 10)]
    public string noAnswer;
    [TextArea(3, 10)]
    public string endOfExtraSentences;

    private void Update()
    {
        if (panel.isActive)
        {
            if (!dialogueManager.isStart)
            {
                //���� ����� �� ������� ��� ������, �� ���� ������ ���-�� SayaYes
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (panel.answer)
                    {
                        if (!SaysYes)
                            SayPredict();
                        if (SaysYes & dialogueManager.ExtraSentences == 0)
                            isEndOfExtraSentences = true;

                        if (isEndOfExtraSentences)
                        {
                            dialogueManager.DialogueText.text = endOfExtraSentences;
                            panel.DisablePanel();
                        }
                    }
                    if (!panel.answer) No();
                }
            }
        }
    }
    public void SayPredict()
    {
        if (FindObjectOfType<GameManager>().playerCheese >= 3)
        {
            NotMuchCheese.SetActive(false);
            SaysYes = true;
            //����� ��������� ����� ������������ ����� ��� ��������� ��������
            if (!(SaysSentences >= dialogueManager.Trigger.dialogue.extraSentences.Length))
            {
                FindObjectOfType<GameManager>().CheeseScore(-PredictCost);
                panel.DisablePanel();
                if (dialogueManager.Trigger.dialogue.extraSentences.Length - SaysSentences < 3)
                {
                    dialogueManager.ExtraSentences += dialogueManager.Trigger.dialogue.extraSentences.Length - SaysSentences;
                    dialogueManager.DisplayExtraSentences();
                }
                else
                {
                    dialogueManager.ExtraSentences += 3;
                    SaysSentences += 3;
                    dialogueManager.DisplayExtraSentences();
                }
            }
        }
        else
            NotMuchCheese.SetActive(true);
    }
    public void No()
    {
        dialogueManager.DisplayText(noAnswer);
        NotMuchCheese.SetActive(false);
        panel.DisablePanel();
    }
}