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

    public void vel_step(int N, ref float[] vx, ref float[] vy, ref float[] vx0, ref float[] vy0, float visc, float dt)
    {
        add_source(N,ref vx, ref vx0, dt);
        add_source(N,ref vy, ref vy0, dt);
        SWAP(ref vx0, ref vx); diffuse(N, 1, ref vx, ref vx0, visc, dt);
        SWAP(ref vy0, ref vy); diffuse(N, 2, ref vy, ref vy0, visc, dt);
        project(N,ref vx,ref vy,ref vx0, ref vy0);
        SWAP(ref vx0, ref vx); SWAP(ref vy0, ref vy);
        advect(N,1,ref vx,ref vx0, ref vx0, ref vy0, dt); advect(N,2,ref vy,ref vy0, ref vx0, ref vy0, dt);
        project(N,ref vx,ref vy,ref vx0, ref vy0);
    }
    
    /******************** FONCTIONS PRIVEES ******************************/
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
            set_bound(N, b, ref x);
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
        set_bound(N, b, ref d);
    }
    /* Unique a la vitesse */
    private void project(int N, ref float[] u, ref float[] v, ref float[] p, ref float[] div)
    {
        float h = 1f / N;
        for (int i = 1; i <= N; i++)
        {
            for (int j = 1; j <= N; j++)
            {
                div[IX(i, j, N)] = -0.5f * h * (u[(IX(i + 1, j, N))] - u[IX(i - 1, j, N)] + v[IX(i, j + 1, N)] - v[IX(i, j - 1, N)]);
                p[IX(i, j, N)] = 0;
            }
        }
        set_bound(N, 0, ref div); set_bound(N, 0, ref p);

        for (int k = 0; k < 20; k++)
        {
            for (int i = 1; i <= N; i++)
            {
                for (int j = 1; j <= N; j++)
                {
                    p[IX(i, j, N)] = (div[IX(i, j, N)] + p[IX(i + 1, j, N)] + p[IX(i - 1, j, N)] + 
                                      p[IX(i, j + 1, N)] + p[IX(i, j - 1, N)]) / 4;
                }
            }
            set_bound(N, 0, ref p);
        }

        for (int i = 1; i <= N; i++)
        {
            for (int j = 1; j <= N; j++)
            {
                u[IX(i, j, N)] -= 0.5f * (p[IX(i + 1, j, N)] - p[IX(i - 1, j, N)]) / h;
                v[IX(i, j, N)] -= 0.5f * (p[IX(i, j + 1, N)] - p[IX(i, j - 1, N)]) / h;
            }
        }
        set_bound(N, 1, ref u);set_bound(N, 2, ref v);
    }
    /* LIMITES */
    private void set_bound(int N, int b, ref float[] x)
    {
        for (int i = 0; i < N; i++)
        {
            x[IX(0    , i  , N)] = b == 1 ? -x[IX(1, i, N)] : x[IX(1, i, N)];
            x[IX(N + 1,i   , N)] = b == 1 ? -x[IX(N, i, N)] : x[IX(N, i, N)];
            x[IX(i     , 0  , N)] = b == 2 ? -x[IX(i, 1, N)] : x[IX(i, 1, N)];
            x[IX(i     , N+1, N)] = b == 2 ? -x[IX(i, N, N)] : x[IX(i, N, N)];
        }

        x[IX(0    , 0    , N)] = 0.5f * (x[IX(1, 0, N)] + x[IX(0, 1, N)]);
        x[IX(0    , N + 1, N)] = 0.5f * (x[IX(1, N + 1, N)] + x[IX(0, N, N)]);
        x[IX(N + 1, 0    , N)] = 0.5f * (x[IX(N, 0, N)] + x[IX(N + 1, 0, N)]);
        x[IX(N + 1, N + 1, N)] = 0.5f * (x[IX(N, N + 1, N)] + x[IX(N + 1, N, N)]);
    }
    
    /******************** MACROS ******************************/
    private int IX(int i, int j, int N)
    {
        return i + (N + 2) * j;
    }

    private void SWAP(ref float[] t1, ref float[] t2)
    {
        (t1, t2) = (t2, t1);
    }
}
