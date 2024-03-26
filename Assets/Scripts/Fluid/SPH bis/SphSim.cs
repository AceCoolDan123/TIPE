using UnityEngine;

public class SphSim : MonoBehaviour
{
    public int particleCount = 100;
    // Déclarer les buffers pour les positions, vélocités et densités des particules
    public ComputeBuffer positionsBuffer;
    public ComputeBuffer velocitiesBuffer;
    public ComputeBuffer densityDataBuffer;

    // Autres paramètres nécessaires pour le shader
    public float scale = 1.0f;
    public Color colA = Color.white;
    public Texture2D colourMap;
    public float velocityMax = 10.0f;

    // Le material utilisant le shader
    private Material particleMaterial;

    void Start()
    {
        // Assurez-vous de créer et d'initialiser correctement les buffers
        InitializeBuffers();

        // Obtenez le Material attaché au Renderer de votre objet
        Renderer renderer = GetComponent<Renderer>();
        particleMaterial = renderer.material;
        
        // Assurez-vous que le shader est correctement configuré dans le Material
        particleMaterial.shader = Shader.Find("Instanced/Particle2D");

        // Affectez les buffers et autres paramètres au shader
        particleMaterial.SetBuffer("Positions2D", positionsBuffer);
        particleMaterial.SetBuffer("Velocities", velocitiesBuffer);
        particleMaterial.SetBuffer("DensityData", densityDataBuffer);
        particleMaterial.SetFloat("scale", scale);
        particleMaterial.SetColor("colA", colA);
        particleMaterial.SetTexture("ColourMap", colourMap);
        particleMaterial.SetFloat("velocityMax", velocityMax);
    }

    void Update()
    {
        // Mettez à jour les données des buffers au besoin
        // ...
    }

    void OnDestroy()
    {
        // Assurez-vous de libérer les buffers lorsqu'ils ne sont plus nécessaires
        ReleaseBuffers();
    }

    void InitializeBuffers()
    {
        // Initialisez et remplissez vos buffers avec les données appropriées
        // Assurez-vous que les tailles des buffers correspondent au nombre de particules

        // Exemple : création d'un tableau de positions aléatoires pour chaque particule
        Vector2[] randomPositions = new Vector2[particleCount];
        for (int i = 0; i < particleCount; i++)
        {
            randomPositions[i] = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        }

        // Créez et initialisez les buffers
        positionsBuffer = new ComputeBuffer(particleCount, sizeof(float) * 2);
        positionsBuffer.SetData(randomPositions);

        velocitiesBuffer = new ComputeBuffer(particleCount, sizeof(float) * 2);
        // Initialisez les données des vélocités au besoin

        densityDataBuffer = new ComputeBuffer(particleCount, sizeof(float) * 2);
        // Initialisez les données de densité au besoin
    }

    void ReleaseBuffers()
    {
        // Libérez les buffers lorsqu'ils ne sont plus nécessaires
        if (positionsBuffer != null) positionsBuffer.Release();
        if (velocitiesBuffer != null) velocitiesBuffer.Release();
        if (densityDataBuffer != null) densityDataBuffer.Release();
    }
}
