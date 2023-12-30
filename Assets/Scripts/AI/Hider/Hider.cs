using UnityEngine;

public class Hider : MonoBehaviour
{
    #region Variables
    private HiderWorld _worldSeen;
    public HiderWorld WorldSeen { get {return _worldSeen;} }
    #endregion

    #region Unity Methods
    private void Start() 
    {    
    }
    private void Update() 
    {    
    }
    #endregion

    #region Other Methods
    public void SenseAround()
    {
        // sight, hearing
    } 
    #endregion
}