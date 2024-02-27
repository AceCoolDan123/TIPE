using UnityEngine;

public static class Tools
{
    public static bool IsLookingAtMe(Vector3 targetPos, Vector3 targetLook, Vector3 myPos)
    {
        return (Vector3.Dot(Vector3.Normalize(targetPos - myPos), targetLook) < 0.75);
    }
}