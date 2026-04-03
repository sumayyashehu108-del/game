using System;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int health = 100;
    public int damage = 10;
    public enum EnemyState { Idle, Chasing, Attacking }
    private EnemyState currentState = EnemyState.Idle;

    void Update() {
        switch (currentState) {
            case EnemyState.Idle:
                // Logic for idle state
                break;
            case EnemyState.Chasing:
                // Logic for chasing player
                ChasePlayer();
                break;
            case EnemyState.Attacking:
                // Logic for attacking player
                AttackPlayer();
                break;
        }
    }

    private void ChasePlayer() {
        // Implement chase logic here
    }

    private void AttackPlayer() {
        // Implement attack logic here
    }

    public void TakeDamage(int amount) {
        health -= amount;
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
        // Logic for death
        Destroy(gameObject);
    }
}