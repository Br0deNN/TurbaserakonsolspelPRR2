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

            int battleCount = 0;
            while(player.isAlive)
            {
                battleCount++;
                Console.WriteLine("New battle");
                //InitBattle();
                //BatteLoop();

            }

            Console.WriteLine("Du dog, spelet är över");
            
        }

        private void Battle()
        {
            enemies = new List<Enemy>();
            int enemyCount = random.Next(1,3);

            for (int i = 0; i < enemyCount; i++)
            {
                enemies.Add(random.Next(0,2) == 0 ? new Spider() : new Zombie());
            }
        }

        private void BattleLoop()
        {
            while (player.isAlive && enemies.Any(e => e.isAlive))
            {
                Console.WriteLine("Välj vad du vill göra!");
                Console.WriteLine("1. Attackera");
                Console.WriteLine("2. Visa hur mycket liv du har");

                int val = int.Parse(Console.ReadLine());

                switch (val)
                {
                    case 1:
                        attackEnemy();
                        break;
                    case 2:
                        Console.WriteLine($"Du har {player.Health}/{player.MaxHealth} hp");
                        break;
                    default: 
                        Console.WriteLine("Ogiltigt val");
                        break;
                    
                }
                
                
            }

            if (player.isAlive)
            {
                Console.WriteLine("Du besegrade alla fiender!");
                player.AddScore(100);               
            }
        }

        private void attackEnemy()
        {

        }
    }





    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}