using System.Numerics;
using Raylib_cs;

namespace CellularAutomataSpreading
{
    internal class Program
    {
        static readonly int gridWidth = 50;
        static readonly int gridHeight = 50;
        static readonly int gridScale = 15;

        static readonly int[,] map = new int[gridWidth, gridHeight];
        static readonly int[,] tempMap = new int[gridWidth, gridHeight];

        static readonly float updatesPerSecond = 50;
        static float updateTimer = 1 / updatesPerSecond;

        static bool paused = true;

        static readonly Random rand = new Random();
        static readonly float spreadProbability = 1f;

        static readonly Color[] colors = new Color[3]
        {
            Color.BLACK,
            Color.LIME,
            Color.VIOLET,
        };

        static void Main(string[] args)
        {
            Raylib.InitWindow(gridWidth * gridScale, gridHeight * gridScale, "Spreading");
            Raylib.SetTargetFPS(120);

            while (!Raylib.WindowShouldClose())
            {
                updateTimer -= Raylib.GetFrameTime();

                Vector2 mousePosition = Raylib.GetMousePosition();
                int mouseX = (int)(mousePosition.X / gridScale);
                int mouseY = (int)(mousePosition.Y / gridScale);

                if (!(mouseX < 0 || mouseY < 0 || mouseX >= gridWidth || mouseY >= gridHeight))
                {
                    if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
                    {
                        map[mouseX, mouseY] = 1;
                    }
                    if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_RIGHT))
                    {
                        map[mouseX, mouseY] = 2;
                    }
                }

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
                {
                    paused = !paused;
                }

                if (!paused && updateTimer <= 0)
                {
                    UpdateGrid();
                    updateTimer = 1 / updatesPerSecond;
                }

                Raylib.BeginDrawing();
                DrawMap();
                if (paused) Raylib.DrawText("paused", 10, 10, 20, Color.WHITE);
                Raylib.EndDrawing();
            }    

            Raylib.CloseWindow();
        }

        static void DrawMap()
        {
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    Raylib.DrawRectangle(x * gridScale, y * gridScale, gridScale, gridScale, colors[map[x, y]]);
                }
            }
        }

        static void UpdateGrid()
        {
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    tempMap[x, y] = map[x, y];
                }
            }

            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    if (map[x, y] != 0)
                    {
                        if (rand.NextSingle() < spreadProbability)
                        {
                            int nx = x + rand.Next(-1, 2);
                        
                            int ny = y + rand.Next(-1, 2);

                            if (!(nx < 0 || ny < 0 || nx >= gridWidth || ny >= gridHeight))
                            {
                                tempMap[nx, ny] = map[x, y];
                            }
                        }
                    }
                }
            }

            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    map[x, y] = tempMap[x, y];
                }
            }
        }
    }
}