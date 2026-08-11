using UnityEngine;
using UnityEngine.Animations;

public class Marker_track : MonoBehaviour
{
    public Transform target;

    public Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        transform.position = target.position;
    }
}
