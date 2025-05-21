using System;
using System.Runtime.CompilerServices;

namespace TheGame
{
    abstract class Character //Detta är en abstrakt basklass som player och enemy ärver av
    {
        public string Name {get; set;} //Detta hämtar och namnger karaktären
        public int Health {get; set;} //Detta visar det aktuella HP:t som karaktären har

        public int MaxHealth {get; set;} //MaxHealth visar karaktärens maximala HP och används för att beräkna det aktuella HP:t

        public Character(string name, int health) //Detta är en konstrukor som sätter namn och hp
        {
            Name = name;
            Health = health;
            MaxHealth = health; //MaxHealth sätts lika med startvärdet på HP
        }

        public void TakeDamage(int amount) //Denna metod är till för att kunna göra damage på en karaktär
        {
            Health -= amount;
            if (Health < 0 ) //Denna if-sats är till för att kontrollera att HP:t inte är lika med 0
            {
                Health = 0;
            } 
        }

        public bool isAlive => Health > 0; //Detta är en property som retunerar värdet true om karaktären fortfarande lever

        public abstract int Attack(); //Detta är en abstrakt metod som definnerar hur Attack metoden ska fungera i subklassera

    }

    class Player : Character //Detta är sjävla spelarens klass, alltså player, som ärver av Character
    {
        public int score {get; private set;} //Detta är spelarens poäng och den kan endast ändras inuti klassen

        
        public Player(string name, int health) : base(name, health) //Denna konstrukor vidarbefodrar  väderna name och health till player
        {
            score = 0; //Denna konstrukor bestämmer att spelaren börjar på 0 poäng
        }

        public void AddScore(int amount) //Denna metod ökar spelarens poäng genom att addera hur mycket poäng den får med score
        {
            score += amount;
        }

        public override int Attack() //Denna skriver över metoden Attack och gör så att player gör mellan 7 och 20 skada
        {
            return new Random().Next(7,21); //Då maxvärdet är 21 så kommer player kunna göra 20 i skada maximalt
        }
    }

    abstract class Enemy : Character //Detta är fiendernas klass, enemy och ärver av Charachter
    {
        public Enemy(string name, int health) : base(name, health){} //Denna konstrukor vidarbefodrar väderna name och health till enemy
    }

    class Spider : Enemy //Spider är en typ av fiende som ärvar av Enemy. Spider är även den svaga fiende typen
    {
        
        public Spider() : base("Spider", 20) {} //Denna konstrukor sätter namn och 20 hp på fienden

        public override int Attack()
        {
            return new Random().Next(2,6); //Spider gör mellan 2-5 skada
        }

    }

    class Zombie : Enemy //Den andra fiendetypen som är den starkare av dem. Zombie ärver också från Enemy
    {
        public Zombie() : base("Zombie", 30){} //Ger fienden namn och 30 hp

        public override int Attack()
        {
            return new Random().Next(6,15); //Zombie gör mellan 6-14 skada
        }
    }
    

    class Game //Huvudklass för spelet
    {
        private Player player; //Referens till spelaren
        private List<Enemy> enemies; //Lista med fiender som används i varje battle
        private Random random = new Random(); //Genererar slumpade tal

        public void Start() //Denna metod är till för att kunna starta spelet
        {
            Console.WriteLine("Ange ditt namn"); //Spelaren ska skriva in sitt namn
            string name = Console.ReadLine();  // Tar emot det spelaren skriver
            player = new Player(name, 100); //Skapar en spelare med 100 HP

            int battleCount = 0; //Sätter ett start värde på vilken runda det är som spelas
            while(player.isAlive) //Denna while loop körs så länge spelaren är vid liv. 
            {
                battleCount++; //Adderas med 1 vid varje ny runda som startas
                Console.WriteLine("Ny omgång"); //Skriver ut ny omgång
                Battle(); //Denna metod skapar nya fiender
                BattleLoop(); //Denna metod startar en ny omgång

            }
            //Om spelaren dör så sker följande så avslutas spelet och följande visas.
            Console.WriteLine("Du dog, spelet är över");
            Console.WriteLine(" ");
            SaveScore(player.Name, player.score); //Detta sparar spelarens poäng till filen highscores.txt
            ShowScores(); //Denna metod skriver ut alla sparade resultat som finns i filen highscores.txt
        }

        private void Battle() //Denna metod startar en ny omgång där fienderna slumpas fram
        {
            enemies = new List<Enemy>(); //En ny lista skapas för varje Battle som genereras
            int enemyCount = random.Next(1,3); //En eller två fiender skapas

            for (int i = 0; i < enemyCount; i++) //Denna loop gör så att antingen Spider eller Zombie läggs till
            {
                enemies.Add(random.Next(0,2) == 0 ? new Spider() : new Zombie()); //Denna kodrad gör så att chansen är 50% på varje fiendes chans att spawna
            }
        }

        private void BattleLoop() //Denna metod skapar/startar en ny omgång
        {
            while (player.isAlive && enemies.Any(e => e.isAlive)) //Denna while loop körs så länge spelaren och minst en fiende är vid liv
            {   //Uttrycket .Any(e => e.isAlive) returnerar värdet true om minst en fiende i listan har liv kvar, annars retunerar den false
                Console.WriteLine("------------------------");
                Console.WriteLine("Välj vad du vill göra!");
                Console.WriteLine("1. Attackera");
                Console.WriteLine("2. Visa hur mycket liv du har");
                Console.WriteLine("------------------------");

                
                int val = int.Parse(Console.ReadLine());
                Console.WriteLine($"Ditt val: {val}");

                switch (val) //Denna switch metod körs baserat på det spelaren skriver in.
                {
                    case 1: //Om val = 1
                        attackEnemy(); //Spelaren attackerar
                        break;
                    case 2: //Om val = 2
                        Console.WriteLine("Ditt liv: ");
                        Console.WriteLine($"Du har {player.Health}/{player.MaxHealth} hp"); //Denna kodrad visar spelarens aktuella HP
                        break;
                    default: 
                        Console.WriteLine("Ogiltigt val"); //Denna kodrad visas om spelaren skriver något ogiltigt
                        break;
                    
                }
                EnemyTurn(); //Efter att spelaren valt vad han vill göra så attackerar alla fiender
            }

            if (player.isAlive) //Denna if-sats kontrollerar om spelaren överlevde striden
            {   
                Console.WriteLine(" ");
                Console.WriteLine("Bra jobbat!!!!!!!!!!!!!!!!");
                Console.WriteLine("Du besegrade alla fiender!");
                player.AddScore(100); //100 poäng adderas till score vid vunnen runda              
            }
        }

        private void attackEnemy() //Metod för spelarens attack
        {
            Console.Write("Välj fiende att attackera: ");
            Console.WriteLine("");
            for (int i = 0; i < enemies.Count; i++) //Denna loop skriver ut de fiender som fortfarande lever samt hur mycket HP de har
            {
                if (enemies[i].isAlive)
                    Console.WriteLine($"{i + 1}. {enemies[i].Name} ({enemies[i].Health} HP)");
            }

            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= enemies.Count) //Denna rad läser in värdet från spelaren och försöker göra det till datatypen int och om det lyckas så sparas värdet i variablen choice.
            {   //Det minsta giltiga talet är 1 och max är 2 eftersom att det finns två fiendetyper

                Enemy target = enemies[choice - 1]; //Hämtar den fiende som spelaren valt att attackera 
                if (!target.isAlive) //Denna if-sats kontrollerar om fienden redan är död
                {   
                    Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!");
                    Console.WriteLine("Fienden är redan död!");
                    return;
                }

                int damage = player.Attack(); //Denna kodrad anropar spelarens attack metod
                target.TakeDamage(damage); //Target är fienden som spelaren valt att attackera
                Console.WriteLine("");
                Console.WriteLine($"Du gjorde {damage} skada på {target.Name}!"); //Visar hur mycket skada du gjorde på fienden du valde
            }
            else
            {
                Console.WriteLine("Ogiltigt val.");
            }
        }

        private void EnemyTurn() //Fiendernas tur att attackera
        {
            foreach (var enemy in enemies.Where(e => e.isAlive)) //Foreach loopen går igenom alla fiender som fortfarande lever 
            {
                int damage = enemy.Attack(); //Fiendens går till attack
                player.TakeDamage(damage); //Skadan fiendens gör dras från spelarens hp
                Console.WriteLine("");
                Console.WriteLine($"{enemy.Name} går till attack gör {damage} skada på dig");
                
            }
        }

        private void SaveScore(string name, int score) //Metod för att spara spelarens namn och poäng till en fil
        {
            using (StreamWriter sw = new StreamWriter("highscores.txt", true)) //om true, lägg till namn och poäng längst ner i filen utan att skriva över tidigare resultat
            {
                sw.WriteLine($"{name} {score}"); //Skriver in namn och score
            }
        }

        private void ShowScores() //Metod för att skriva ut det som finns i filen
        {
            Console.WriteLine("Topplistan");
            if(File.Exists("highscores.txt")) //Om filen existerar så gör den följande
            {
                try 
                {
                    using (StreamReader sr = new StreamReader("highscores.txt")) //Läser in filen den ska skriva ut
                    {
                        string content = sr.ReadToEnd(); //Läser igenom hela filen
                        Console.WriteLine(content); //Skriver ut allt i filen
                    }
                }
                catch (Exception e) //Om något fel skulle uppstå 
                {
                    Console.WriteLine("Det gick inte att läsa filen...");
                }
            }
        }
    }

    class Program //Denna klass är det som startar programmet
    {
        static void Main(string[] args)
        {
            Game game = new Game(); //Här skapar jag ett nytt objekt av klassen game som innehåller själva spelet
            game.Start(); //Kör spelet
        }
    }
}