using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidCalculs : MonoBehaviour
{
    public void dens_step(int N, ref float[] x, ref float[] x0, ref float[] vx, ref float[] vy, float diff, float dt)
    {
        add_source(N,ref x,ref x0,dt);
        SWAP(ref x0, ref x); 
        diffuse(N, 0, ref x, ref x0, diff, dt);
        SWAP(ref x0, ref x);
        advect(N, 0, ref x, ref x0, ref vx, ref vy, dt);
    }
    private void add_source(int N, ref float[] x, ref float[] s, float dt)
    {
        int size = (N + 2) * (N + 2); 
        for (int i = 0; i < size; i++)
        {
            x[i] = dt * s[i];
        }
    }

    private void diffuse(int N, int b, ref float[] x, ref float[] x0, float diff, float dt)
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

    private void advect(int N, int b, ref float[] d, ref float[] d0, ref float[] vx, ref float[] vy, float dt)
    {
        float dt0 = dt * N;
        for (int i = 1; i <= N; i++)
        {
            for (int j = 1; j <= N; j++)
            {
                float x = i - dt0 * vx[IX(i, j, N)];
                float y = j - dt0 * vy[IX(i, j, N)];
                
                if (x < 0.5f) x = 0.5f;
                if (x > N + 0.5f) x = N + 0.5f;
                int i0 = Mathf.FloorToInt(x);
                int i1 = i0 + 1;
                
                if (y < 0.5f) y = 0.5f;
                if (y > N + 0.5f) y = N + 0.5f;
                int j0 = Mathf.FloorToInt(y);
                int j1 = j0 + 1;

                float s1 = x - i0;
                float s0 = 1 - s1;
                float t1 = y - j0;
                float t0 = 1 - t1;

                d[IX(i, j, N)] = s0 * (t0 * d0[IX(i0, j0, N)] + t1 * d0[IX(i0, j1, N)]) +
                                 s1 * (t0 * d0[IX(i1, j0, N)] + t1 * d0[IX(i1, j1, N)]);
            }
        }
    }
    private int IX(int i, int j, int N)
    {
        return i + (N + 2) * j;
    }

    private void SWAP(ref float[] t1, ref float[] t2)
    {
        (t1, t2) = (t2, t1);
    }
}
