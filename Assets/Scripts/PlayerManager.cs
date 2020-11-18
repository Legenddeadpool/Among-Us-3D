using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float health;
    public float maxHealth;
    public MeshRenderer model;
    public bool isDead;
    public GameObject player;
    public PlayerController _controller;
    public int colourID;

    public bool meetingHost = false;
    public bool wasVoted = false;

    private Vector3 fromPos = Vector3.zero;
    private Vector3 toPos = Vector3.zero;
    private float lastTime;

    private void Awake()
    {
        if (_controller != null)
        {
            _controller.playerCamera = gameObject.GetComponentInChildren<Camera>();
            _controller.playerCamera.cullingMask = _controller.aliveMask;
        }
    }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;
    }

    public void SetPosition(Vector3 position)
    {
        fromPos = toPos;
        toPos = position;
        lastTime = Time.time;
    }

    private void Update()
    {
        this.transform.position = Vector3.Lerp(fromPos, toPos, (Time.time - lastTime) / (1.0f / 30));
    }

    public void SetHealth(float _health)
    {
        health = _health;
    }

    public void Die()
    {
        if (!wasVoted)
        {
            Debug.Log($"Player {id} has died");
            GameManager.instance.SpawnCorpse(gameObject.transform.position, gameObject.transform.rotation, colourID);
            Instantiate(GameManager.instance.killAnimationPrefab, gameObject.transform.position, gameObject.transform.rotation);
        }
        
        player.layer = 6;
        for (int i = 0; i < player.transform.childCount; i++)
        {
            player.transform.GetChild(i).gameObject.layer = 6;
        }
        isDead = true;
        if(_controller != null)
        {
            _controller.playerCamera.cullingMask = _controller.deadMask;
        }
        GameManager.instance.ghostLight.enabled = true;
        GameManager.instance.map.SetActive(false);
    }
}
