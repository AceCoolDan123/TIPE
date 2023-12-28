using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidCalculs : MonoBehaviour
{
    public void add_source(int N, ref float[] x, ref float[] s, float dt)
    {
        int size = (N + 2) * (N + 2); 
        for (int i = 0; i < size; i++)
        {
            x[i] = dt * s[i];
        }
    }

    public void diffuse(int N, int b, ref float[] x, ref float[] x0, float diff, float dt)
    {
        float a = dt * diff * N * N;
        for (int k = 0; k < 20; k++)
        {
            for (int i = 1; i <= N; i++)
            {
                for (int j = 1; j <= N; j++)
                {
                    x[IX(i, j, N)] = (x0[IX(i, j, N)] + a * (x[IX(i - 1, j, N)] + x[IX(i + 1, j, N)] + x[IX(i, j - 1, N)] + x[IX(i, j + 1, N)])) / (1 + 4 * a);
                }
            }
        }
    }

    private int IX(int i, int j, int N)
    {
        return i + (N + 2) * j;
    }
}
