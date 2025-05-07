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

    class Player : Character {
        public int score {get; private set;}

        public Player(string name, int health) : base(name, health)
        {
            score = 0;
        }

        public void AddScore(int amount)
        {
            score += amount;
        }

        public override int Attack()
        {
            return new Random().Next(5,15);
        }
    }

    abstract class Enemy : Character
    {
        public Enemy(string name, int health) : base(name, health){}
    }

    class Spider : Enemy 
    {
        
        public Spider() : base("Spider", 20) {}

        public override int Attack()
        {
            return new Random().Next(2,6); //Spider gör mellan 2-5 skada
        }

    }

    class Zombie : Enemy 
    {
        public Zombie() : base("Zombie", 30){}

        public override int Attack()
        {
            return new Random().Next(6,15); //Zombie gör mellan 6-14 skada
        }
    }
    

    class Game //Huvudklass för spelet
    {
        private Player player; //Spelaren
        private List<Enemy> enemies; //Lista med fiender
        private Random random = new Random(); //Genererar slumpade tal

        public void Start()
        {
            Console.WriteLine("Ange ditt namn");
            string name = Console.ReadLine();
            player = new Player(name, 100); //Skapar en spelare med 100 HP
        }
    }





    class program
    {
        static void Main(string[] args)
        {
            
        }
    }
}