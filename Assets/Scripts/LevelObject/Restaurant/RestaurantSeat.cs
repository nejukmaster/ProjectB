using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantSeat : MonoBehaviour
{
    [SerializeField] Transform sitPos;
    [SerializeField] bool bIsAvailable;

    public bool IsAvailable()
    {
        return bIsAvailable;
    }

    public Transform GetSitPos()
    {
        return sitPos;
    }
}
