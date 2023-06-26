using System.Numerics;
using Raylib_cs;

namespace CellularAutomataSpreading
{
    internal class Program
    {
        static readonly int gridWidth = 750;
        static readonly int gridHeight = 300;
        static readonly int gridScale = 4;

        static readonly int[,] map = new int[gridWidth, gridHeight];
        static readonly int[,] tempMap = new int[gridWidth, gridHeight];

        static readonly float updatesPerSecond = 30;
        static float updateTimer = 1 / updatesPerSecond;

        static bool paused = true;

        static readonly Random rand = new Random();
        static readonly float spreadProbability = 0.5f;

        static bool flipOrder = false;

        static Color[] colors = new Color[6]
        {
            Color.BLACK, //0
            Color.LIME, //1
            Color.DARKPURPLE, //2
            Color.SKYBLUE, //3
            Color.ORANGE, //4
            Color.BLUE, //5
        };
        static KeyboardKey[] keys = new KeyboardKey[6]
        {
            KeyboardKey.KEY_ZERO,
            KeyboardKey.KEY_ONE,
            KeyboardKey.KEY_TWO,
            KeyboardKey.KEY_THREE,
            KeyboardKey.KEY_FOUR,
            KeyboardKey.KEY_FIVE,
        };

        static void Main(string[] args)
        {
            //colors = new Color[32];
            //keys = new KeyboardKey[32];
            //for (int i = 1; i < 32; i++)
            //{
            //    colors[i] = new Color(rand.Next(50, 255), rand.Next(50, 255), rand.Next(50, 255), 255);
            //}
            //colors[0] = Color.BLACK;

            Raylib.InitWindow(gridWidth * gridScale, gridHeight * gridScale, "Spreading");

            while (!Raylib.WindowShouldClose())
            {
                updateTimer -= Raylib.GetFrameTime();

                Vector2 mousePosition = Raylib.GetMousePosition();
                int mouseX = (int)(mousePosition.X / gridScale);
                int mouseY = (int)(mousePosition.Y / gridScale);

                if (!(mouseX < 0 || mouseY < 0 || mouseX >= gridWidth || mouseY >= gridHeight))
                {
                    for (int i = 0; i < colors.Length; i++)
                    {
                        if (Raylib.IsKeyDown(keys[i]))
                        {
                            map[mouseX, mouseY] = i;
                        }
                    }
                }

                if (Raylib.IsKeyDown(KeyboardKey.KEY_F)) updateTimer = 0;

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_R))
                {
                    for (int y = 0; y < gridHeight; y++)
                    {
                        for (int x = 0; x < gridWidth; x++)
                        {
                            map[x, y] = 0;
                        }
                    }
                }

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_A))
                {
                    for (int y = 0; y < gridHeight; y++)
                    {
                        for (int x = 0; x < gridWidth; x++)
                        {
                            map[x, y] = rand.Next(0, colors.Length);
                        }
                    }
                }

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
                {
                    paused = !paused;
                }

                if (!paused && updateTimer <= 0)
                {
                    UpdateGrid();
                    updateTimer += 1 / updatesPerSecond;
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
            //if (rand.NextSingle() < 0.005f)
            //{
            //    map[rand.Next(0, gridWidth), rand.Next(0, gridHeight)] = rand.Next(0, colors.Length);
            //}

            flipOrder = !flipOrder;

            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    int sx = flipOrder ? gridWidth - x - 1 : x;
                    int sy = flipOrder ? gridHeight - y - 1 : y;

                    if (map[sx, sy] != 0)
                    {
                        if (rand.NextSingle() < spreadProbability)
                        {
                            int nx = sx + rand.Next(-1, 2);
                        
                            int ny = sy + rand.Next(-1, 2);

                            if (!(nx < 0 || ny < 0 || nx >= gridWidth || ny >= gridHeight))
                            {
                                map[nx, ny] = map[sx, sy];
                            }
                        }
                    }
                }
            }
        }
    }
}