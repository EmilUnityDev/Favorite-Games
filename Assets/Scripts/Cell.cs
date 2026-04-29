using UnityEngine;

public class Cell : MonoBehaviour
{
    public int cellIndex; // Индекс ячейки
    public GameObject crosses;
    public GameObject naughts;
    public Animator animator;
    private void Start()
    {
        // Установите индекс ячейки на основе позиции в массиве
        cellIndex = transform.GetSiblingIndex(); // Предполагается, что ячейки находятся в одном родителе
    }
    public void StartClick()
    {
        animator.SetTrigger("Click");
        SoundController.Instance.StartClickEffect();
    }
}