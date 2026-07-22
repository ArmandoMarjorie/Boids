using UnityEngine;
using System;

public class BoidArrayModel
{
    /* ------------- ATTRIBUTES ------------- */
    private BoidModel[] boids = null;
    private int nbBoids;

    /* ------------- GETTERS AND SETTERS ------------- */
    public int NbBoids { get => nbBoids; }
    public BoidModel getBoid(int i)
    {
        if (boids != null && i >=0 && i < NbBoids)
            return boids[i];
        else
            return null;
    }

    /* ------------- CONSTRUCTOR ------------- */
    public BoidArrayModel(int nbBoids, 
        float widthCanvas, 
        float heightCanvas,
        float depthCanvas,
        int historyMaxSize)
    {
        // Default array size
        if (nbBoids <= 0)
            nbBoids = 100;
        // Default history max size
        if (historyMaxSize <= 0)
            historyMaxSize = 100;
        // Default canvas size
        if (widthCanvas <= 0)
            widthCanvas = 150;
        if (heightCanvas <= 0)
            heightCanvas = 150;
        if (depthCanvas <= 0)
            depthCanvas = 150;

        this.nbBoids = nbBoids;
        boids = new BoidModel[this.nbBoids];
        float x, y, z = 0;
        float dx, dy, dz = 0;

        System.Random random = new System.Random();

        for (int i = 0; i < this.nbBoids; i++)
        {

            x = (float)random.NextDouble() * widthCanvas;
            y = (float)random.NextDouble() * heightCanvas;
            z = (float)random.NextDouble() * depthCanvas;

            dx = (float)random.NextDouble() * 10f - 5f;
            dy = (float)random.NextDouble() * 10f - 5f;
            dz = (float)random.NextDouble() * 10f - 5f;

            boids[i] = new BoidModel(new Vector3(x, y, z), 
                new Vector3(dx, dy, dz),
                historyMaxSize
            );

        }
    }

}