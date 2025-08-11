using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

//□=0（方格正常）
//●=1（幸运轮盘1：交换位置 2：轰炸对方）
//▲=2（暂停一回合）
//卐=3（快速隧道前进10格）
//※=4（炸弹退6格）

namespace 飞行棋
{
    class Program
    {
        public static int[] Maps = new int[102];
        public static int[] PlayerPos = new int[2];
        public static string[] PlayerName = new string[2];
        static void Main(string[] args)
        {
            //显示emoji==============================
            Console.OutputEncoding = Encoding.UTF8;
            //=======================================

            //预处理地图
            Map();
            #region 用户登录
            GameHead();
            Console.WriteLine("请输入用户A姓名");
            while (true)
            {
                string PlayerNameA = Console.ReadLine();
                if (PlayerNameA.Replace(" ", "") == "")
                {
                    Console.WriteLine("用户名A名称不能为空，请重新输入");
                }
                else
                {
                    PlayerName[0] = PlayerNameA;
                    break;
                }
            }
            Console.WriteLine("请输入用户B姓名");
            while (true)
            {
                string PlayerNameB = Console.ReadLine();
                if (PlayerNameB.Replace(" ", "") == "" || PlayerNameB == PlayerName[0])
                {
                    Console.WriteLine(PlayerNameB == PlayerName[0] ? "用户B名称不能与A相同" : "用户名B名称不能为空，请重新输入");
                }
                else
                {
                    PlayerName[1] = PlayerNameB;
                    break;
                }
            }
            #endregion
            Console.Clear();//清屏
            GameHead();
            Console.WriteLine("玩家A名称为{0}", PlayerName[0]);
            Console.WriteLine("玩家B名称为{0}", PlayerName[1]);
            DrawMap();
            Console.WriteLine();
            while (PlayerPos[0] < 102 && PlayerPos[1] < 102)
            {

                Random random = new Random();
                Console.WriteLine("{0}按任意键开始掷骰子", PlayerName[0]);
                Console.ReadKey(true);
                int result = random.Next(1, 6);
                Console.WriteLine("{0}掷出了{1}", PlayerName[0], result);
                PlayerPos[0] += result;
                Console.WriteLine("{0}按任意键开始行动", PlayerName[0]);
                Console.ReadKey(true);
                Console.Clear();
                DrawMap();
                Console.WriteLine("");
                Console.WriteLine("行动完成"+ PlayerPos[0] + "," + PlayerPos[1]);
                PlayerPosition(0);
                Console.ReadKey(true);
                Console.Clear();
                DrawMap();
                Console.WriteLine("");

                Random random1 = new Random();
                Console.WriteLine("{0}按任意键开始掷骰子", PlayerName[1]);
                Console.ReadKey(true);
                int result1 = random1.Next(1, 6);
                Console.WriteLine("{0}掷出了{1}", PlayerName[1], result1);
                PlayerPos[1] += result1;
                Console.WriteLine("{0}按任意键开始行动", PlayerName[1]);
                Console.ReadKey(true);
                Console.Clear();
                DrawMap();
                Console.WriteLine("");
                Console.WriteLine("行动完成" + PlayerPos[0] + "," + PlayerPos[1]);
                PlayerPosition(1);
                Console.ReadKey(true);
                Console.Clear();
                DrawMap();
                Console.WriteLine("");

            }
            if (PlayerPos[0] == 102)
            {
                Console.WriteLine("恭喜{0}赢了，游戏结束", PlayerName[0]);
                Console.ReadKey();
            }
            else if (PlayerPos[1] == 102)
            {
                Console.WriteLine("恭喜{0}赢了，游戏结束", PlayerName[1]);
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("游戏可能出现了错误");
            }
        }
        /// <summary>
        /// 打印游戏头
        /// </summary>
        public static void GameHead()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("**************************************");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("**************************************");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("**************************************");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("***************飞行棋*****************");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("**************************************");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("**************************************");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("**************************************");
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// 地图初始化
        /// </summary>
        public static void Map()
        {
            //随机向地图中添加道具
            Random rnd = new Random();
            for (int i = 0; i < 6; i++)
            {
                Maps[rnd.Next(3, Maps.Length)] = 1;
                Maps[rnd.Next(3, Maps.Length)] = 2;
                Maps[rnd.Next(3, Maps.Length)] = 3;
                Maps[rnd.Next(3, Maps.Length)] = 4;
                Maps[101] = 0;
                
            }


        }
        /// <summary>
        /// 绘图判断
        /// </summary>
        public static void DrawWhat(int i)
        {
            Console.ForegroundColor = ConsoleColor.White;
            if (PlayerPos[0] == PlayerPos[1] && PlayerPos[0] == i)
            {
                Console.Write("<>");
            }
            else if (PlayerPos[0] == i+1 && PlayerPos[0]>0)
            {
                Console.Write("Ａ");
            }
            else if (PlayerPos[1] == i+1 && PlayerPos[0] > 0)
            {
                Console.Write("Ｂ");
            }
            
            else
            {
                switch (Maps[i])
                {
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("□"); break;
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("●"); break;
                    case 2:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("▲"); break;
                    case 3:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("卐"); break;
                    case 4:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("※"); break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("第{0}点地图绘制出错", i); break;
                }
            }
            
        }
        /// <summary>
        /// 绘图
        /// </summary>
        public static void DrawMap()
        {
            Console.WriteLine("道具图例：●：幸运轮盘  ▲：暂停  卐：快速隧道  ※：炸弹");
            #region 第一横行
            for (int i = 0; i < 30; i++)
            {
                DrawWhat(i);
            }
            #endregion
            #region 第一竖列
            for (int i = 30; i < 36; i++)
            {
                Console.WriteLine("");
                for (int j = 0; j < 29; j++)
                {
                    Console.Write("  ");
                }
                DrawWhat(i);
            }
            #endregion
            #region 第二横行
            Console.WriteLine();
            for (int i = 66 - 1; i >= 36; i--)
            {
                DrawWhat(i);
            }

            #endregion
            #region 第二竖列

            for (int i = 66; i < 72; i++)
            {
                Console.WriteLine("");
                DrawWhat(i);
            }
            Console.WriteLine("");
            #endregion
            #region 第三横行
            for (int i = 72; i < 102; i++)
            {
                DrawWhat(i);
            }
            #endregion
        }
        /// <summary>
        /// 判断玩家应奖励/惩罚的步数
        /// </summary>
        /// <param name="NameIndex">玩家数字代号</param>
        public static void PlayerPosition(int NameIndex)
        {
            for (int i = 0; i < Maps.Length; i++)
            {
                
                int num=Maps[i];
                //玩家踩到玩家
                if (PlayerPos[0] > 0 && PlayerPos[1] > 0 && PlayerPos[0] == PlayerPos[1])
                {
                    PlayerPos[NameIndex==0?1:0] -= 6;
                    Console.WriteLine("玩家{0}踩到了玩家{1}，玩家{1}退6格", PlayerName[NameIndex], PlayerName[NameIndex == 0 ? 1 : 0]);
                    break;
                }
                //玩家踩到幸运轮盘
                else if (num == 1 && i == PlayerPos[NameIndex] - 1)
                {
                    while (true)
                    {
                        Console.WriteLine("玩家{0}踩到了幸运轮盘  按1--轰炸玩家{1}  按2--与玩家{1}交换位置", PlayerName[NameIndex], PlayerName[NameIndex == 0 ? 1 : 0]);
                        string input = Console.ReadLine();
                        if (input == "1")
                        {
                            PlayerPos[NameIndex == 0 ? 1 : 0] -= 6;
                            Console.WriteLine("玩家{0}遭到了轰炸，玩家{0}退6格", PlayerName[NameIndex == 0 ? 1 : 0]);
                            break;
                        }
                        else if (input == "2")
                        {
                            int temp;
                            temp = PlayerPos[NameIndex];
                            PlayerPos[NameIndex] = PlayerPos[NameIndex == 0 ? 1 : 0];
                            PlayerPos[NameIndex == 0 ? 1 : 0] = temp;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("输入有误，请重新输入");
                            continue;
                        }
                    }


                    break;
                }
                //玩家踩到地雷
                else if (num == 4 && i == PlayerPos[NameIndex] - 1)
                {
                    PlayerPos[NameIndex] -= 6;
                    Console.WriteLine("玩家{0}踩到了地雷，玩家{0}退6格", PlayerName[NameIndex]);
                    break;
                }
                //玩家踩到暂停
                else if (num == 2 && i == PlayerPos[NameIndex] - 1)
                {
                    Console.WriteLine("玩家{0}踩到了暂停，玩家{0}暂停一局", PlayerName[NameIndex]);
                    break;
                }
                //玩家踩到时空隧道
                else if (num == 3 && i == PlayerPos[NameIndex] - 1)
                {
                    PlayerPos[NameIndex] += 10;
                    Console.WriteLine("玩家{0}踩到了时空隧道，玩家{0}前进10格", PlayerName[NameIndex]);
                    break;
                }
                else if (num == 0 && i == PlayerPos[NameIndex] - 1)
                {
                    PlayerPos[NameIndex] += 0;
                    Console.WriteLine("玩家{0}踩到了方格，神马也没有发生", PlayerName[NameIndex]);
                    break;
                }
                else
                {
                    
                }
                
                
            }
            return;
        }
    }
}
