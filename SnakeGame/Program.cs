using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] posisiX = new int[1624];
            posisiX[0] = 30;
            int[] posisiY = new int[1624];
            posisiY[0] = 15;

            int foodx = 10;
            int foody = 10;
            int foodeaten = 0;

            int speed = 150;

            bool start = true;
            bool hit = false;
            bool hithimself = false;
            bool foodeat = false;

            Random random = new Random();

            Console.CursorVisible = false;

            //ular muncul di layar
            snakecolor(foodeaten, posisiX, posisiY, out posisiX, out posisiY);

            //makanan di layar
            setfoodposition(random, foodeaten, posisiX, posisiY, out foodx, out foody);
            foodcolor(foodx, foody);

            //buat tembok
            Wall();

            //ular bergerak
            ConsoleKey command = Console.ReadKey().Key;
            ConsoleKey commandSebelum = command;
            do
            {
                switch (command)
                {
                    case ConsoleKey.LeftArrow:
                        Console.SetCursorPosition(posisiX[0], posisiY[0]);
                        Console.Write(" ");
                        posisiX[0]--;
                        break;
                    case ConsoleKey.UpArrow:
                        Console.SetCursorPosition(posisiX[0], posisiY[0]);
                        Console.Write(" ");
                        posisiY[0]--;
                        break;
                    case ConsoleKey.RightArrow:
                        Console.SetCursorPosition(posisiX[0], posisiY[0]);
                        Console.Write(" ");
                        posisiX[0]++;
                        break;
                    case ConsoleKey.DownArrow:
                        Console.SetCursorPosition(posisiX[0], posisiY[0]);
                        Console.Write(" ");
                        posisiY[0]++;
                        break;
                }
                
                //apakah ular nabrak
                hit = didsnakehit(posisiX[0], posisiY[0]);
                hithimself = didsnakehithimself(foodeaten, posisiX, posisiY);               
                if (hit || hithimself)
                {
                    start = false;
                    Console.Clear();
                    Console.SetCursorPosition(30, 15);
                    Console.Write("Cie nabrak nie X_X");
                    continue;
                }

                //warna badan ular
                snakecolor(foodeaten, posisiX, posisiY, out posisiX, out posisiY);

                //apakah apel sudah kemakan
                foodeat = hopeapplewaseaten(posisiX[0], posisiY[0], foodx, foody);

                //muncul makanan
                if (foodeat)
                {
                    setfoodposition(random, foodeaten, posisiX, posisiY, out foodx, out foody);
                    foodcolor(foodx, foody);

                    //seberapa banyak makanan yang dimakan
                    //ular tumbuh
                    foodeaten++;
                }

                //cek arah terakhir ular
                if (Console.KeyAvailable)
                {
                    commandSebelum = command;
                    command = Console.ReadKey().Key;
                    if (command == ConsoleKey.LeftArrow && commandSebelum == ConsoleKey.RightArrow) command = commandSebelum;
                    if (command == ConsoleKey.RightArrow && commandSebelum == ConsoleKey.LeftArrow) command = commandSebelum;
                    if (command == ConsoleKey.UpArrow && commandSebelum == ConsoleKey.DownArrow) command = commandSebelum;
                    if (command == ConsoleKey.DownArrow && commandSebelum == ConsoleKey.UpArrow) command = commandSebelum;
                }

                //kecepatan ular
                System.Threading.Thread.Sleep(speed);

            } while (start);
        }   

        private static void snakecolor(int foodeaten, int[] posisiXin, int[] posisiYin, out int[] posisixout, out int[] posisiyout)
        {
            //warna kepala ular
            Console.SetCursorPosition(posisiXin[0], posisiYin[0]);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine((char)214);

            //warna badan
            for (int i = 1; i < foodeaten + 1; i++)
            {
                Console.SetCursorPosition(posisiXin[i], posisiYin[i]);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("O");
            }
            
            //hapus bagian terakhir ular
            Console.SetCursorPosition(posisiXin[foodeaten + 1], posisiYin[foodeaten + 1]);
            Console.WriteLine(" ");

            //menyimpan setiap bagian ular
            for (int i = foodeaten + 1; i > 0; i--)
            {
                posisiXin[i] = posisiXin[i - 1];
                posisiYin[i] = posisiYin[i - 1];
            }

            //array baru
            posisixout = posisiXin;
            posisiyout = posisiYin;

        }

        private static bool didsnakehit(int posisiX, int posisiY)
        {
            if (posisiX == 1 || posisiX == 60 || posisiY == 1 || posisiY == 30) return true; return false;
        }

        private static void Wall()
        {
            for (int i = 1; i < 31; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(1, i);
                Console.Write("█");
                Console.SetCursorPosition(60, i);
                Console.Write("█");
            }
            for (int i = 1; i < 61; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(i, 1);
                Console.Write("█");
                Console.SetCursorPosition(i, 30);
                Console.Write("█");
            }
        }

        private static void setfoodposition(Random random, int foodeaten, int[] posisiX, int[] posisiY, out int foodx, out int foody)
        {
            bool check = false;
            do
            {
                check = false;
                foodx = random.Next(2, 58);
                foody = random.Next(2, 28);
                for (int i = 0; i < foodeaten + 1; i++)
                {
                    if (foodx == posisiX[i] && foody == posisiY[i]) check = true;
                }
            } while (check);
        }

        private static void foodcolor(int foodx, int foody)
        {
            Console.SetCursorPosition(foodx, foody);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("@");
        }

        private static bool hopeapplewaseaten(int posisiX, int posisiY, int foodx, int foody)
        {
            if (posisiX == foodx && posisiY == foody) return true; return false;
        }
        
        private static bool didsnakehithimself(int foodeaten, int[] posisiX, int[] posisiY) 
        {
            for (int i = 1; i < foodeaten+1; i++)
            {
                if (posisiX[0] == posisiX[i] && posisiY[0] == posisiY[i]) return true;
            }
            return false;
        }
    }
}
