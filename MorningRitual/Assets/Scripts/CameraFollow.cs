﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    private Transform followTransform;

    [Header("Configure")]
    public float applySpeed = 1;
    public float ySnap = 2.0f;

    private Camera camera;
    private GameManager manager;
    private int yHeight = 0;

    // Use this for initialization
    void Awake()
    {
        camera = GetComponent<Camera>();
        manager = GameObject.FindObjectOfType<GameManager>();
        followTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (followTransform)
        {
            Vector3 pos = followTransform.position;
            pos.z = transform.position.z;
            transform.position = pos;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(transform.position.x, followTransform.position.x, Time.fixedDeltaTime * applySpeed);
        pos.y = Mathf.Lerp(transform.position.y, followTransform.position.y, Time.fixedDeltaTime * applySpeed);
        transform.position = pos;
        LockToBounds();
    }

    private void LockToBounds()
    {
        if (manager == null) return;
        Vector2 min = manager.min;
        Vector2 max = manager.max;
        Vector3 pos = transform.position;
        float width = camera.orthographicSize * camera.aspect;
        if(min.x + width > max.x - width)
        {
            pos.x = (max.x - min.x) / 2.0f;
        } else {
            pos.x = Mathf.Clamp(pos.x, min.x + width, max.x - width);
        }
        if(min.y + camera.orthographicSize > max.y - camera.orthographicSize)
        {
            pos.y = (min.y - max.y) / 2.0f;
        } else {
            pos.y = Mathf.Clamp(pos.y, min.y + camera.orthographicSize, max.y - camera.orthographicSize);
        }
        transform.position = pos;
    }
}
