using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace City_Racing
{
    class SeparateMethods
    {
        Random rand;
        Building[,] buildings;

        public SeparateMethods()
        {
            rand = new Random();
            // = new Building[100, 100];
        }

        public Building[,] CreateBuildings()
        {
            buildings = new Building[100, 100];
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    //BORDER
                    if (i == 0 || i == 99 || j == 0 || j == 99)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //1
                    else if (i >= 2 && i <= 11 && j >= 2 && j <= 11)
                    {
                        GiveBuildingValues(i, j);
                    }

                         //2
                    else if (i >= 2 && i <= 11 && j >= 13 && j <= 22)
                    {
                        GiveBuildingValues(i, j);
                    }

                         //3
                    else if (i >= 13 && i <= 22 && j >= 2 && j <= 11)
                    {
                        GiveBuildingValues(i, j);
                    }

                         //4
                    else if (i >= 13 && i <= 22 && j >= 13 && j <= 22)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //5
                    else if (i >= 24 && i <= 36 && j >= 2 && j <= 22)
                    {
                        GiveBuildingValues(i, j);
                    }

                       //6
                    else if (i >= 38 && i <= 42 && j >= 2 && j <= 22)
                    {
                        GiveBuildingValues(i, j);
                    }

                       //7
                    else if (i >= 45 && i <= 69 && j >= 1 && j <= 22)
                    {
                        GiveBuildingValues(i, j);
                    }

                       //8
                    else if (i >= 71 && i <= 72 && j >= 2 && j <= 22)
                    {
                        GiveBuildingValues(i, j);
                    }

                         //9
                    else if (i >= 75 && i <= 85 && j >= 2 && j <= 22)
                    {
                        GiveBuildingValues(i, j);
                    }

                         //10
                    else if (i >= 87 && i <= 97 && j >= 12 && j <= 22)
                    {
                        GiveBuildingValues(i, j);
                    }

                         //11
                    else if (i >= 87 && i <= 97 && j >= 2 && j <= 10)
                    {
                        GiveBuildingValues(i, j);
                    }

                         //12
                    else if (i >= 2 && i <= 22 && j >= 25 && j <= 34)
                    {
                        GiveBuildingValues(i, j);
                    }
                  
                        //13
                    else if (i >= 24 && i <= 42 && j >= 25 && j <= 34)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //14
                    else if (i >= 45 && i <= 63 && j >= 25 && j <= 34)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //15
                    else if (i >= 65 && i <= 83 && j >= 25 && j <= 34)
                    {
                        GiveBuildingValues(i, j);
                    }

                       //16
                    else if (i >= 85 && i <= 96 && j >= 25 && j <= 34)
                    {
                        GiveBuildingValues(i, j);
                    }


                       //17
                    else if (i >= 1 && i <= 11 && j >= 36 && j <= 41)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //18
                    else if (i >= 13 && i <= 29 && j >= 36 && j <= 41)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //19
                    else if (i >= 31 && i <= 35 && j >= 36 && j <= 41)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //20
                    else if (i >= 38 && i <= 42 && j >= 36 && j <= 64)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //21
                    else if (i >= 45 && i <= 56 && j >= 36 && j <= 64)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //22
                    else if (i >= 59 && i <= 76 && j >= 36 && j <= 47)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //23
                    else if (i >= 79 && i <= 83 && j >= 36 && j <= 47)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //24
                    else if (i >= 85 && i <= 87 && j >= 36 && j <= 47)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //25
                    else if (i >= 89 && i <= 96 && j >= 36 && j <= 55)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //26
                    else if (i >= 2 && i <= 18 && j >= 43 && j <= 53)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //27
                    else if (i >= 21 && i <= 29 && j >= 43 && j <= 53)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //28
                    else if (i >= 31 && i <= 35 && j >= 43 && j <= 47)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //29
                    else if (i >= 30 && i <= 35 && j >= 49 && j <= 53)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //30
                    else if (i >= 57 && i <= 74 && j >= 49 && j <= 53)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //31
                    else if (i >= 76 && i <= 87 && j >= 49 && j <= 53)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //32
                    else if (i >= 2 && i <= 18 && j >= 55 && j <= 68)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //33
                    else if (i >= 21 && i <= 35 && j >= 55 && j <= 64)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //34
                    else if (i >= 59 && i <= 70 && j >= 56 && j <= 64)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //35
                    else if (i >= 72 && i <= 74 && j >= 56 && j <= 64)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //36
                    else if (i >= 76 && i <= 86 && j >= 56 && j <= 64)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //37
                    else if (i >= 89 && i <= 96 && j >= 57 && j <= 64)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //38
                    else if (i >= 2 && i <= 18 && j >= 70 && j <= 73)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //39
                    else if (i >= 20 && i <= 22 && j >= 67 && j <= 73)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //40
                    else if (i >= 24 && i <= 28 && j >= 67 && j <= 73)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //41
                    else if (i >= 31 && i <= 42 && j >= 67 && j <= 72)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //42
                    else if (i >= 45 && i <= 60 && j >= 67 && j <= 84)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //43
                    else if (i >= 63 && i <= 75 && j >= 67 && j <= 84)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //44
                    else if (i >= 78 && i <= 86 && j >= 66 && j <= 74)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //45
                    else if (i >= 89 && i <= 98 && j >= 66 && j <= 74)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //46
                    else if (i >= 2 && i <= 28 && j >= 75 && j <= 97)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //47
                    else if (i >= 31 && i <= 42 && j >= 74 && j <= 75)
                    {
                        GiveBuildingValues(i, j);
                    }

                         //48
                    else if (i >= 31 && i <= 42 && j >= 77 && j <= 88)
                    {
                        GiveBuildingValues(i, j);
                    }

                         //49
                    else if (i >= 31 && i <= 42 && j >= 90 && j <= 98)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //50
                    else if (i >= 45 && i <= 57 && j >= 86 && j <= 98)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //51
                    else if (i >= 59 && i <= 75 && j >= 86 && j <= 90)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //52
                    else if (i >= 59 && i <= 90 && j >= 92 && j <= 97)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //53
                    else if (i >= 78 && i <= 90 && j >= 89 && j <= 90)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //54
                    else if (i >= 78 && i <= 90 && j >= 86 && j <= 87)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //55
                    else if (i >= 78 && i <= 84 && j >= 77 && j <= 84)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //56
                    else if (i >= 87 && i <= 96 && j >= 77 && j <= 82)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //57
                    else if (i >= 87 && i <= 96 && j >= 84 && j <= 84)
                    {
                        GiveBuildingValues(i, j);
                    }

                        //58
                    else if (i >= 92 && i <= 96 && j >= 86 && j <= 97)
                    {
                        GiveBuildingValues(i, j);
                    }

                    else
                    {
                        buildings[i, j] = new Building(false);
                    }
                }
            }

            return buildings;
        }

        private void GiveBuildingValues(int i, int j)
        {
            int type = rand.Next(1, 7);
            int height = type;
       //     Texture2D tex = buildingTex1;
            buildings[i, j] = new Building(height, true);
        }
    }
}
