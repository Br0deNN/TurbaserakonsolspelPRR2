using System;
using System.Runtime.CompilerServices;

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
                Console.WriteLine("Ny omgång");
                Battle();
                BattleLoop();

            }
            Console.WriteLine("Du dog, spelet är över");
            Console.WriteLine(" ");
            SaveScore(player.Name, player.score);
            ShowScores();
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
                Console.WriteLine("------------------------");
                Console.WriteLine("Välj vad du vill göra!");
                Console.WriteLine("1. Attackera");
                Console.WriteLine("2. Visa hur mycket liv du har");
                Console.WriteLine("------------------------");

                
                int val = int.Parse(Console.ReadLine());
                Console.WriteLine($"Ditt val: {val}");

                switch (val)
                {
                    case 1:
                        attackEnemy();
                        break;
                    case 2:
                        Console.WriteLine("Ditt liv: ");
                        Console.WriteLine($"Du har {player.Health}/{player.MaxHealth} hp");
                        break;
                    default: 
                        Console.WriteLine("Ogiltigt val");
                        break;
                    
                }
                EnemyTurn();
            }

            if (player.isAlive)
            {   
                Console.WriteLine("Bra jobbat!!!!!!!!!!!!!!!!");
                Console.WriteLine("Du besegrade alla fiender!");
                player.AddScore(100);               
            }
        }

        private void attackEnemy()
        {

            Console.Write("Välj fiende att attackera: ");
            Console.WriteLine("");
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].isAlive)
                    Console.WriteLine($"{i + 1}. {enemies[i].Name} ({enemies[i].Health} HP)");
            }

            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= enemies.Count)
            {
                Enemy target = enemies[choice - 1];
                if (!target.isAlive)
                {   
                    Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!");
                    Console.WriteLine("Fienden är redan död!");
                    return;
                }

                int damage = player.Attack();
                target.TakeDamage(damage);
                Console.WriteLine("");
                Console.WriteLine($"Du gjorde {damage} skada på {target.Name}!");
            }
            else
            {
                Console.WriteLine("Ogiltigt val.");
            }
        }

        private void EnemyTurn()
        {
            foreach (var enemy in enemies.Where(e => e.isAlive))
            {
                int damage = enemy.Attack();
                player.TakeDamage(damage);
                Console.WriteLine("");
                Console.WriteLine($"{enemy.Name} går till attack gör {damage} skada på dig");
                
            }
        }

        private void SaveScore(string name, int score)
        {
            using (StreamWriter sw = new StreamWriter("highscores.txt", true)) 
            {
                sw.WriteLine($"{name} {score}");
            }
        }

        private void ShowScores()
        {
            Console.WriteLine("Topplistan");
            if(File.Exists("highscores.txt")) 
            {
                try 
                {
                    using (StreamReader sr = new StreamReader("highscores.txt"))
                    {
                        string content = sr.ReadToEnd();
                        Console.WriteLine(content);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Det gick inte att läsa filen...");
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }
}