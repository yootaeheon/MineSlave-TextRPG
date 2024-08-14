﻿using MineSlave.Inventorys;
using MineSlave.Items;
using MineSlave.Monsters;
using MineSlave.Players;
using MineSlave.Scenes;
using System.ComponentModel.Design;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace MineSlave
{
    public class Game
    {
        private bool isRunning;

        private Scene[] scenes;
        private Scene curScene;
        public Scene scene { get { return curScene; } }

        public Player player;
        public Player Player { get { return player; } set { player = value; } }

        public void Run()
        {
            Start();
            while (isRunning)
            {
                Render();
                Input();
                Update();
            }
            End();
        }

        public void ChangeScene(SceneType sceneType)
        {
            curScene.Exit();
            curScene = scenes[(int)sceneType];
            curScene.Enter();
        }

        public void Over()
        {
            isRunning = false;
        }

        private void Start()
        {
           
            isRunning = true;

            player = new();
            player.Day = 0;
            player.Duty = 500;
            player.DeadLine = 5;
            player.Level = 1;
            player.MaxExp = 100;
            player.Exp = 0;
            player.MaxHP = 100;
            player.CurHP = player.MaxHP;
            player.MaxExhaustion = 6;
            player.Exhaustion = player.MaxExhaustion; 
            player.Str = 5;
            Player.Defense = 5;
            player.Gold = 30;

            scenes = new Scene[(int)SceneType.Size];
            scenes[(int)SceneType.Title] = new TitleScene(this);
            scenes[(int)SceneType.Select] = new SelectScene(this);
            scenes[(int)SceneType.Town] = new TownScene(this);
            scenes[(int)SceneType.Camp] = new CampScene(this);
            scenes[(int)SceneType.Gambling] = new GamblingScene(this);

            curScene = scenes[(int)SceneType.Title];
            curScene.Enter();

            // [Feat] 추가된 기능들
            Inventory inventory = new Inventory();
            Item[] items = new Item[10];

            TownMap.data.map = new bool[,]
            {            //숙소        //상점        //도박장
                { false,  true, false,  true, false,  true, false},
                { false,  true,  true,  true,  true,  true, false},
                { false,  true,  true,  true,  true,  true, false},
                { false,  true,  true,  true,  true,  true, false},
                { false,  true,  true,  true,  true,  true,  true}, //광산
                { false,  true,  true,  true,  true,  true, false},
                { false, false, false, false, false, false, false},
              
            };

            TownMap.data.playerPos = new TownMap.Point() { x = 6, y = 3 };
            TownMap.data.minePos = new TownMap.Point() { x = 4, y = 7 };
            TownMap.data.shopPos = new TownMap.Point() { x = 0, y = 4 };
            TownMap.data.campPos = new TownMap.Point() { x = 0, y = 1 };
            TownMap.data.gamblingPos = new TownMap.Point() { x = 0, y = 6 };
        }

        private void End()
        {
            curScene.Exit();
        }

        private void Render()
        {
            curScene.Render();
        }

        private void Input()
        {
            curScene.Input();
        }

        private void Update()
        {
            if (player.Exp >= player.MaxExp)
            {
                player.Level += 1;
                player.Exp = player.Exp - player.MaxExp;

                player.MaxHP += 100;
                player.CurHP = player.MaxHP;
                player.Str += 5;
            }

            if (player.CurHP >= player.MaxHP)
            {
                player.CurHP = player.MaxHP;
            }

            player.Day += 1;
            player.DeadLine -= 1;
            player.Exhaustion -= 1;

            if (player.DeadLine == 0)
            {
                if (player.Gold >= 500)
                {   
                    player.Gold -= 500;
                    player.DeadLine = 5;
                    Console.WriteLine("세금 납부일 입니다.");
                    Console.WriteLine("-500 G");
                }
                else
                {
                    Console.WriteLine("세금 납부일 입니다..");
                    Console.WriteLine("세금을 내지 못하여 처형 당하였습니다..");
                    Console.WriteLine("Game Over");
                }
            }
            if (player.Exhaustion == 0)
            {
                Console.WriteLine("탈진하여 사망하였습니다..");
                Console.WriteLine("Game Over");
            }



        }
    }
}
