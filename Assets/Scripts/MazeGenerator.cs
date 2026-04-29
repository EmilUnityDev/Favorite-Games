using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MazeGenerator : MonoBehaviour
{
    public GameObject columnPrefab; // Префаб столба
    public GameObject wallPrefab;    // Префаб стены
    public GameObject coinPrefab;    // Префаб монетки
    public int width = 10;           // Ширина сетки
    public int height = 10;          // Высота сетки
    public float spacing = 1.0f;     // Расстояние между объектами
    [Range(0, 1)] public float generationFill = 1.0f; // Заполненность (0 - не генерировать, 1 - всегда генерировать)
    [Range(0, 1)] public float coinSpawnChance = 0.1f; // Вероятность спавна монетки

    private List<GameObject> columns = new List<GameObject>(); // Список столбов
    private List<GameObject> walls = new List<GameObject>();   // Список стен
    private List<GameObject> coins = new List<GameObject>();   // Список монеток

    void Start()
    {
        SpawnGrid();
    }

    public void SpawnGrid()
    {
        // Уничтожение предыдущих объектов
        DestroyExistingObjects();

        // Создание столбов и стен
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Генерируем столбы с учетом заполненности
                if (Random.value <= generationFill)
                {
                    Vector3 columnPosition = new Vector3(x * spacing, 0, y * spacing);
                    GameObject column = Instantiate(columnPrefab, columnPosition, Quaternion.identity, transform);
                    columns.Add(column); // Добавляем столб в список

                    // Создание стен между столбами с учетом заполненности
                    if (x < width - 1 && Random.value <= generationFill) // Если не последний столбец
                    {
                        CreateWallBetweenColumns(columnPosition, new Vector3((x + 1) * spacing, 0, y * spacing));
                    }
                    if (y < height - 1 && Random.value <= generationFill) // Если не последняя строка
                    {
                        CreateWallBetweenColumns(columnPosition, new Vector3(x * spacing, 0, (y + 1) * spacing));
                    }
                }
                else
                {
                    // Спавн монетки в случайной позиции
                    if (Random.value <= coinSpawnChance)
                    {
                        Vector3 coinPosition = new Vector3(x * spacing, 1.2f, y * spacing);
                        GameObject coin = Instantiate(coinPrefab, coinPosition, Quaternion.identity, transform);
                        coins.Add(coin); // Добавляем монетку в список
                    }
                }
            }
        }
    }

    private void CreateWallBetweenColumns(Vector3 columnPos1, Vector3 columnPos2)
    {
        // Находим середину между двумя столбами
        Vector3 wallPosition = (columnPos1 + columnPos2) / 2;

        // Случайный выбор поворота (0 или 90 градусов)
        int randomRotation = Random.Range(0, 2) * 90;

        // Создаем стену
        GameObject wall = Instantiate(wallPrefab, wallPosition, Quaternion.Euler(0, randomRotation, 0), transform);
        walls.Add(wall); // Добавляем стену в список
    }

    private void DestroyExistingObjects()
    {
        // Уничтожаем все существующие столбы
        foreach (var column in columns)
        {
            DestroyImmediate(column);
        }
        columns.Clear(); // Очищаем список столбов

        // Уничтожаем все существующие стены
        foreach (var wall in walls)
        {
            DestroyImmediate(wall);
        }
        walls.Clear(); // Очищаем список стен

        // Уничтожаем все существующие монетки
        foreach (var coin in coins)
        {
            DestroyImmediate(coin);
        }
        coins.Clear(); // Очищаем список монеток
    }

    private void UpdateCoinSpawnChance(float value)
    {
        coinSpawnChance = value;
        SpawnGrid();
    }
}
