using System;

namespace TheGame
{
    abstract class Character
    {
        public string Name {get; set;}
        public int Health {get; set;}

        public int MaxHealth {get; set;}

        public Character(string name, int health)
        {
            Name = name;
            Health = health;
            MaxHealth = health;
        }

        public void TakeDamage(int amount){
            Health -= amount;
            if (Health < 0 ) Health = 0;
        }

        public bool isAlive => Health > 0 ;

        public abstract int Attack();


    }

    





    class program
    {
        static void Main(string[] args)
        {
            
        }
    }
}