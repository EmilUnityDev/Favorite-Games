using UnityEngine;
using UnityEngine.UI; // Для работы с UI
using System.Collections; // Для использования корутин

public class TicTacToe : MonoBehaviour
{
    public Cell[] cell; // Префабы крестиков и ноликов
    public GameObject[] crosses = new GameObject[9];
    public GameObject[] naughts = new GameObject[9];

    private int[] boardState = new int[9]; // Состояния ячеек (0 - пусто, 1 - крестик, 2 - нолик)
    public bool isCrossTurn; // Переменная для отслеживания текущего игрока
    public float timeLimit = 10f; // Таймер в секундах
    private float timer; // Переменная для отсчета времени
    public Text timerText; // Текст для отображения таймера
    public int score;
    public Text scoreText;
    public int Round;
    public Text RoundText;
    public GameObject yourTurn;
    public GameObject AITurn;
    public bool IsGame;
    public Animator animatorRound;

    public GameObject WinPanel;
    public GameObject DefeatPanel;
    public GameObject DrawPanel;
    private void Start()
    {
        ClearCell();
        Round = 1;
        RoundText.text = "Round: " + Round.ToString() + "/3";
    }
    public void ClearCell()
    {
        int coin = PlayerPrefs.GetInt("coin1", 0);
        if (coin < score)
        {
            PlayerPrefs.SetInt("coin1", score);
        }
        Round++;
        RoundText.text = "Round: " + Round.ToString() + "/3";
        if (Round > 3)
        {
            PlayerPrefs.SetInt("OpenLabyrinth", 1);
            WinPanel.SetActive(true);
            IsGame = false;
            return;
        }
        // Инициализация массива состояний
        for (int i = 0; i < boardState.Length; i++)
        {
            crosses[i] = cell[i].crosses;
            crosses[i].SetActive(false);
            naughts[i] = cell[i].naughts;
            naughts[i].SetActive(false);
            boardState[i] = 0; // Все ячейки изначально пустые
        }
        

        // Определяем случайным образом, за кого играть
        isCrossTurn = Random.Range(0, 2) == 0; // 50% на крестики или нолики
        timer = timeLimit; // Устанавливаем таймер
         // Если компьютер начинает первым, запускаем его ход
        if (!isCrossTurn)
        {
            StartCoroutine(ComputerTurnCoroutine());
        }
        
    }
    public void AddCount()
    {
        score += Random.Range(50,100) + (int)( 25 * Random.Range(0.1f, 2.0f));
    }
    private void Update()
    {
        if (IsGame)
        {
            timer -= Time.deltaTime;

            // Убедитесь, что таймер не становится отрицательным
            if (timer < 0)
            {
                timer = 0;
            }

            // Рассчитываем минуты и секунды
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);

            // Обновляем текст таймера
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            scoreText.text = "Score: " + score.ToString();
            if (isCrossTurn)
            {
                yourTurn.SetActive(true);
                AITurn.SetActive(false);
            }
            else
            {
                AITurn.SetActive(true);
                yourTurn.SetActive(false);
            }
            // Проверяем нажатие левой кнопки мыши
            if (Input.GetMouseButtonDown(0) && timer > 0 && isCrossTurn)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Проверяем, попал ли луч в какой-либо объект
                if (Physics.Raycast(ray, out hit))
                {
                    // Проверяем, является ли объект ячейкой
                    Cell cell = hit.collider.GetComponent<Cell>();
                    if (cell != null)
                    {
                        int cellIndex = cell.cellIndex; // Получаем индекс ячейки

                        // Проверяем, свободна ли ячейка
                        if (boardState[cellIndex] == 0)
                        {
                            // Активируем символ в нужной ячейке
                            GameObject symbolToShow = isCrossTurn ? crosses[cellIndex] : naughts[cellIndex];
                            symbolToShow.SetActive(true); // Активируем префаб

                            // Запускаем анимацию и звуки
                            this.cell[cellIndex].StartClick();

                            // Обновляем состояние ячейки
                            boardState[cellIndex] = isCrossTurn ? 1 : 2;
                            isCrossTurn = false; // Меняем текущего игрока на компьютер
                            AddCount();
                            // Проверяем на победу
                            if (CheckForWin())
                            {
                                print("победил человек");
                                IsGame = true;
                                if (Round < 3)
                                {
                                    animatorRound.SetBool("NextRound", true);
                                }
                                ClearCell();
                                return; // Завершаем игру
                            }
                            else
                            {
                                animatorRound.SetBool("NextRound", false);
                            }

                            // Проверка на ничью
                            if (CheckForDraw())
                            {
                                Debug.Log("Игра окончена. Ничья!");
                                DrawPanel.SetActive(true);
                                IsGame = false;
                                yourTurn.SetActive(false);
                                AITurn.SetActive(false);
                                return; // Завершаем игру

                            }

                            // Ход компьютера с задержкой
                            StartCoroutine(ComputerTurnCoroutine());
                        }
                    }
                }
            }

            // Проверка на окончание игры
            if (timer <= 0)
            {
                Debug.Log("Время вышло! Игра окончена.");
                DefeatPanel.SetActive(true);
                IsGame = false;
                yourTurn.SetActive(false);
                AITurn.SetActive(false);
                // Здесь вы можете добавить логику для завершения игры
            }
        }
    }

    private IEnumerator ComputerTurnCoroutine()
    {
        yield return new WaitForSeconds(1f); // Задержка 1 секунда перед ходом компьютера

        // Проверка на возможность блокировки или выигрыша
        for (int i = 0; i < boardState.Length; i++)
        {
            if (boardState[i] == 0)
            {
                // Проверка, может ли компьютер выиграть
                boardState[i] = 2; // Предполагаем, что компьютер делает ход
                if (CheckForWin())
                {
                    // Компьютер выигрывает
                    naughts[i].SetActive(true);
                    this.cell[i].StartClick(); // Запускаем анимацию и звуки для компьютера
                    Debug.Log("Компьютер выиграл!");
                    yourTurn.SetActive(false);
                    AITurn.SetActive(false);
                    DefeatPanel.SetActive(true);
                    IsGame = false;
                    yield break;
                }
                boardState[i] = 0; // Сбрасываем состояние ячейки
            }
        }

        // Проверка, может ли игрок выиграть на следующем ходе
        for (int i = 0; i < boardState.Length; i++)
        {
            if (boardState[i] == 0)
            {
                // Проверка, может ли игрок выиграть
                boardState[i] = 1; // Предполагаем, что игрок делает ход
                if (CheckForWin())
                {
                    // Блокируем ход игрока
                    naughts[i].SetActive(true);
                    this.cell[i].StartClick(); // Запускаем анимацию и звуки для блокировки
                    boardState[i] = 2; // Обновляем состояние ячейки
                    isCrossTurn = true; // Возвращаем ход игроку
                    Debug.Log("Компьютер заблокировал ход игрока.");
                    if (CheckForDraw())
                    {
                        Debug.Log("Игра окончена. Ничья!");
                        DrawPanel.SetActive(true);
                        IsGame = false;
                        yourTurn.SetActive(false);
                        AITurn.SetActive(false);
                        yield break;
                    }
                    yield break;
                }
                boardState[i] = 0; // Сбрасываем состояние ячейки
            }
        }

        // Если нет возможности блокировать или выигрывать, делаем случайный ход
        MakeRandomMove();
    }

    private void MakeRandomMove()
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, boardState.Length);
        } while (boardState[randomIndex] != 0); // Ищем случайную свободную ячейку

        naughts[randomIndex].SetActive(true); // Активируем префаб
        this.cell[randomIndex].StartClick(); // Запускаем анимацию и звуки для компьютера
        boardState[randomIndex] = 2; // Обновляем состояние ячейки
        isCrossTurn = true; // Возвращаем ход игроку

        // Проверяем на победу
        if (CheckForWin())
        {
            Debug.Log("Компьютер выиграл!");
            DefeatPanel.SetActive(true);
            yourTurn.SetActive(false);
            AITurn.SetActive(false);
            IsGame = false;
            return; // Завершаем игру
        }

        // Проверка на ничью
        if (CheckForDraw())
        {
            Debug.Log("Игра окончена. Ничья!");
            DrawPanel.SetActive(true);
            yourTurn.SetActive(false);
            AITurn.SetActive(false);
            IsGame = false;
            return; // Завершаем игру
        }
    }

    private bool CheckForWin()
    {
        // Примеры выигрышных комбинаций (индексы ячеек)
        int[,] winPatterns = new int[,]
        {
            { 0, 1, 2 },
            { 3, 4, 5 },
            { 6, 7, 8 },
            { 0, 3, 6 },
            { 1, 4, 7 },
            { 2, 5, 8 },
            { 0, 4, 8 },
            { 2, 4, 6 }
        };

        for (int i = 0; i < winPatterns.GetLength(0); i++)
        {
            int a = winPatterns[i, 0];
            int b = winPatterns[i, 1];
            int c = winPatterns[i, 2];

            if (boardState[a] != 0 && boardState[a] == boardState[b] && boardState[a] == boardState[c])
            {
                // Объявление победителя
                return true; // Возвращаем true, если найден победитель
            }
        }
        return false; // Возвращаем false, если победителя нет
    }

    private bool CheckForDraw()
    {
        // Проверяем, есть ли свободные ячейки
        foreach (int state in boardState)
        {
            if (state == 0)
            {
                return false; // Если есть хотя бы одна свободная ячейка, игра не вничью
            }
        }
        return true; // Если свободных ячеек нет, значит ничья
    }
}
