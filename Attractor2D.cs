using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor2D : MonoBehaviour
{
    public Rigidbody2D rb;

    public static List<Attractor2D> Attractors;

    const float G = 6.0f;
    

    
    void FixedUpdate(){
        
            foreach (Attractor2D attractor in Attractors)
            {
                if (attractor != this)
                    Attract(attractor);
            } 
    }

    void OnEnable(){

        if (Attractors == null)
            Attractors = new List<Attractor2D>();
            
        Attractors.Add(this);
    }

    void OnDisable(){
        Attractors.Remove(this);
    }

    void Attract (Attractor2D objToAttract){

        Rigidbody2D rbToAttract = objToAttract.rb;

        Vector2 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;

        if (distance == 0f)
            return;

        float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);

        Vector2 force = direction.normalized * forceMagnitude;

        if (Input.GetMouseButtonDown(0))
            forceMagnitude = 0f;

        rbToAttract.AddForce(force);

    }

}
