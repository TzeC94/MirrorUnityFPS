using System;

public class PlayerHealth : BaseHealth, IHitable {

    public Action CallbackDeath;

    public void Hit(int damage) {
        
        if(isServer){

            currentHealth -= damage;

            CheckDeath();

        }

    }

    void CheckDeath(){

        if(currentHealth <= 0){

            CallbackDeath?.Invoke();

        }

    }

}
