

using System;
using System.Threading;

class Program
{
    static int Width = 30;
    static int Height = 30;
    static char[,] buffer = new char[Width, Height];

    static void Main()
    {
        // 立方体的8个顶点
        float[,] cubeVertices = {
            {-1, -1, -1},
            {1, -1, -1},
            {1, 1, -1},
            {-1, 1, -1},
            {-1, -1, 1},
            {1, -1, 1},
            {1, 1, 1},
            {-1, 1, 1}
        };

        // 立方体的12条边，每条边由一对顶点索引定义
        int[,] cubeEdges = {
            {0, 1}, {1, 2}, {2, 3}, {3, 0},
            {4, 5}, {5, 6}, {6, 7}, {7, 4},
            {0, 4}, {1, 5}, {2, 6}, {3, 7}
        };

        float angle = 0.0f;

        while (true)
        {
            ClearBuffer();
            angle += 0.05f;

            // 旋转立方体
            for (int i = 0; i < cubeEdges.GetLength(0); i++)
            {
                // 获取边的两个顶点
                int vi0 = cubeEdges[i, 0];
                int vi1 = cubeEdges[i, 1];
                float[] vertex0 = RotateVertex(cubeVertices[vi0, 0], cubeVertices[vi0, 1], cubeVertices[vi0, 2], angle);
                float[] vertex1 = RotateVertex(cubeVertices[vi1, 0], cubeVertices[vi1, 1], cubeVertices[vi1, 2], angle);

                // 将3D顶点映射到2D屏幕
                int x0 = (int)(Width / 2 + vertex0[0] * Width / 4);
                int y0 = (int)(Height / 2 - vertex0[1] * Height / 4);
                int x1 = (int)(Width / 2 + vertex1[0] * Width / 4);
                int y1 = (int)(Height / 2 - vertex1[1] * Height / 4);

                // 绘制边
                DrawLine(x0, y0, x1, y1);
            }

            // 输出到控制台
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Console.Write(buffer[x, y]);
                }
                Console.WriteLine();
            }

            // 等待一小段时间
            Thread.Sleep(100);

            // 清除控制台
            Console.Clear();
        }
    }

    static float[] RotateVertex(float x, float y, float z, float angle)
    {
        float cos = (float)Math.Cos(angle);
        float sin = (float)Math.Sin(angle);
        float x2 = x * cos - z * sin;
        float z2 = z * cos + x * sin;
        return new float[] { x2, y, z2 };
    }

    static void ClearBuffer()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                buffer[x, y] = ' ';
            }
        }
    }

    static void DrawLine(int x0, int y0, int x1, int y1)
    {
        int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = dx + dy, e2;

        while (true)
        {
            if (x0 >= 0 && x0 < Width && y0 >= 0 && y0 < Height)
                buffer[x0, y0] = '*';

            if (x0 == x1 && y0 == y1) break;
            e2 = 2 * err;
            if (e2 >= dy) { err += dy; x0 += sx; }
            if (e2 <= dx) { err += dx; y0 += sy; }
        }
    }
}
