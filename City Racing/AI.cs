using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace City_Racing
{
    class AI
    {
        Random rand;
        public Vehicle vehicle;
        public int level;
        public Boolean IsTurning;
        GameTime turnStartTime;
        String turnDir, dir;
        Boolean interDone, prevXZ;
        int prevInterX, prevInterZ;

        public AI(Vehicle vehicle, int theLevel)
        {
            this.vehicle = vehicle;
            level = theLevel;

            rand = new Random();

            IsTurning = false;
            dir = "NORTH";
            interDone = true;

            prevInterX = 0;
            prevInterZ = 0;
            prevXZ = false;
        }

        public Vehicle GetVehicle()
        {
            return vehicle;
        }

        public int GetLevel()
        {
            return level;
        }

        public void DecideAction(RaceEnd raceEnd, Building[,] buildings, GameTime gameTime)
        {
            //-3.14 is straight
            Console.WriteLine(dir);
            Console.WriteLine(vehicle.GetAngle().ToString());
            Console.WriteLine(vehicle.GetVelocity().ToString() + " m/s");
            double DgridX, DgridZ;
            DgridX = vehicle.GetPosition().X / 1.25;
            DgridZ = vehicle.GetPosition().Z / 1.25 * -1;

            if (DgridX < 1)
            {
                DgridX = 1;
            }

            if (DgridZ < 1)
            {
                DgridZ = 1;
            }

            int gridX, gridZ;
            gridX = (int)DgridX;
            gridZ = (int)DgridZ;

            double angle = Math.Round(vehicle.GetAngle(), 2);

            if (dir == "NORTH")
            {
                if (RoadEnds(gridX, gridZ++, buildings))
                {
                    vehicle.Brake();
                    //       Console.WriteLine(gridX.ToString());
                    //       Console.WriteLine(gridZ.ToString());
                }

                else
                {
                    if (IsTurning)
                    {
                        vehicle.Turn(turnDir);
                        if (vehicle.GetVelocity() < 0.008)
                        {
                            vehicle.Accelerate();
                            vehicle.Accelerate();
                        }

                        else
                        {
                            vehicle.Coast();
                        }

                        /*     if ((gameTime.TotalGameTime.TotalMilliseconds - turnStartTime.TotalGameTime.TotalMilliseconds) >= 300)
                             {
                                 IsTurning = false;
                                 Console.WriteLine("END");
                             } */

                        angle = Math.Round(vehicle.GetAngle(), 2);

                        if (angle == -4.71 || (angle >= -4.73 && angle <= -4.69))
                        {
                            IsTurning = false;
                            dir = "EAST";
                            vehicle.SetAngle(-MathHelper.Pi - (MathHelper.Pi / 2));
                        }

                        else if (angle == -1.57 || (angle >= -1.59 && angle <= -1.55))
                        {
                            IsTurning = false;
                            dir = "WEST";
                            vehicle.SetAngle(-MathHelper.Pi + (MathHelper.Pi / 2));
                        }

                        //         Console.WriteLine("turnin");
                    }

                    else
                    {
                        turnDir = InIntersection(gridX, gridZ, buildings, dir, raceEnd);

                        if (interDone == false)
                        {
                            if ((gridX != prevInterX || gridZ != prevInterZ) && !checkTwoLane(gridX, gridZ, buildings, dir))
                            {
                                interDone = true;
                                prevXZ = false;
                            }
                        }

                        if (turnDir == "STRAIGHT")
                        {
                            vehicle.Accelerate();
                            interDone = checkTwoLane(gridX, gridZ, buildings, dir);

                            if (interDone == false && prevXZ == false)
                            {
                                prevInterX = gridX;
                                prevInterZ = gridZ;
                                prevXZ = true;
                            }
                      //                  Console.WriteLine("STRRR");
                        }

                        else if (turnDir != null)
                        {
                            if (interDone == true)
                            {
                                IsTurning = true;
                                turnStartTime = gameTime;
                                vehicle.Turn(turnDir);
                                    vehicle.Coast();
                                //              Console.WriteLine("TURN");
                            }
                            //               Console.WriteLine("gea");
                        }

                        else
                        {
                            vehicle.Accelerate();
                            interDone = true;
                        }
                    }
                }
            }

            if (dir == "EAST")
            {
                //      Console.WriteLine("now heading eastbound");
                if (RoadEnds(gridX++, gridZ, buildings))
                {
                    vehicle.Brake();
                    //       Console.WriteLine(gridX.ToString());
                    //       Console.WriteLine(gridZ.ToString());
                }

                else
                {
                    if (IsTurning)
                    {
                        //         Console.WriteLine("turnin after EAST");
                        vehicle.Turn(turnDir);
                        if (vehicle.GetVelocity() < 0.008)
                        {
                            vehicle.Accelerate();
                            vehicle.Accelerate();
                        }

                        else
                        {
                            vehicle.Coast();
                        }

                        /*     if ((gameTime.TotalGameTime.TotalMilliseconds - turnStartTime.TotalGameTime.TotalMilliseconds) >= 300)
                             {
                                 IsTurning = false;
                                 Console.WriteLine("END");
                             } */

                        //      Console.WriteLine("turnin after EAST");
                        angle = Math.Round(vehicle.GetAngle(), 2);

                        if (angle == -6.28 || (angle >= -6.30 && angle <= -6.26))
                        {
                            IsTurning = false;
                            vehicle.SetAngle(0);
                            dir = "SOUTH";
                        }

                        else if (angle == -3.14 || (angle >= -3.16 && angle <= -3.12))
                        {
                            IsTurning = false;
                            dir = "NORTH";
                            vehicle.SetAngle(-MathHelper.Pi);
                        }

                        //        Console.WriteLine("turnin");
                    }

                    else
                    {
                        turnDir = InIntersection(gridX, gridZ, buildings, dir, raceEnd);

                        if (interDone == false)
                        {
                            if ((gridX != prevInterX || gridZ != prevInterZ) && !checkTwoLane(gridX, gridZ, buildings, dir))
                            {
                                interDone = true;
                                prevXZ = false;
                            }
                        }

                        if (turnDir == "STRAIGHT")
                        {
                            vehicle.Accelerate();
                            interDone = checkTwoLane(gridX, gridZ, buildings, dir);

                            if (interDone == false && prevXZ == false)
                            {
                                prevInterX = gridX;
                                prevInterZ = gridZ;
                                prevXZ = true;
                            }

                            //            Console.WriteLine("STRRR");
                        }

                        else if (turnDir != null)
                        {
                            if (interDone == true)
                            {
                                IsTurning = true;
                                turnStartTime = gameTime;
                                vehicle.Turn(turnDir);
                                    vehicle.Coast();
                            }
                        }

                        else
                        {
                            vehicle.Accelerate();
                            interDone = true;
                        }
                    }
                }
            }

            if (dir == "SOUTH")
            {
                if (RoadEnds(gridX, gridZ--, buildings))
                {
                    vehicle.Brake();
                }

                else
                {
                    if (IsTurning)
                    {
                        vehicle.Turn(turnDir);
                       
                        if (vehicle.GetVelocity() < 0.008)
                        {
                            vehicle.Accelerate();
                            vehicle.Accelerate();
                        }

                        else
                        {
                            vehicle.Coast();
                        }

                        angle = Math.Round(vehicle.GetAngle(), 2);

                        if (angle == -1.57 || (angle >= -1.59 && angle <= -1.55))
                        {
                            IsTurning = false;
                            dir = "WEST";
                            vehicle.SetAngle(-MathHelper.Pi + (MathHelper.Pi / 2));
                        }

                        else if (angle == 1.57f || (angle >= 1.55 && angle <= 1.59))
                        {
                            IsTurning = false;
                            dir = "EAST";
                            vehicle.SetAngle(-MathHelper.Pi - (MathHelper.Pi / 2));
                        }
                    }

                    else
                    {
                        turnDir = InIntersection(gridX, gridZ, buildings, dir, raceEnd);

                        if (interDone == false)
                        {
                            if ((gridX != prevInterX || gridZ != prevInterZ) && !checkTwoLane(gridX, gridZ, buildings, dir))
                            {
                                interDone = true;
                                prevXZ = false;
                            }
                        }

                        if (turnDir == "STRAIGHT")
                        {
                            vehicle.Accelerate();
                            interDone = checkTwoLane(gridX, gridZ, buildings, dir);

                            if (interDone == false && prevXZ == false)
                            {
                                prevInterX = gridX;
                                prevInterZ = gridZ;
                                prevXZ = true;
                            }
                        }

                        else if (turnDir != null)
                        {
                            if (interDone == true)
                            {
                                IsTurning = true;
                                turnStartTime = gameTime;
                                vehicle.Turn(turnDir);
                                    vehicle.Coast();
                            }                           
                        }

                        else
                        {
                            vehicle.Accelerate();
                            interDone = true;
                        }
                    }
                }
            }

            if (dir == "WEST")
            {
                //      Console.WriteLine("now heading eastbound");
                if (RoadEnds(gridX--, gridZ, buildings))
                {
                    vehicle.Brake();
                    //       Console.WriteLine(gridX.ToString());
                    //       Console.WriteLine(gridZ.ToString());
                }

                else
                {
                    if (IsTurning)
                    {
                        //         Console.WriteLine("turnin after EAST");
                        vehicle.Turn(turnDir);
                      
                        if (vehicle.GetVelocity() < 0.008)
                        {
                            vehicle.Accelerate();
                            vehicle.Accelerate();
                        }

                        else
                        {
                            vehicle.Coast();
                        }

                        /*     if ((gameTime.TotalGameTime.TotalMilliseconds - turnStartTime.TotalGameTime.TotalMilliseconds) >= 300)
                             {
                                 IsTurning = false;
                                 Console.WriteLine("END");
                             } */

                        angle = Math.Round(vehicle.GetAngle(), 2);

                        if (angle == -6.28 || (angle >= -6.30 && angle <= -6.26))
                        {
                            IsTurning = false;
                            vehicle.SetAngle(0);
                            dir = "SOUTH";
                        }

                        else if (angle == -3.14 || (angle >= -3.16 && angle <= -3.12))
                        {
                            IsTurning = false;
                            dir = "NORTH";
                            vehicle.SetAngle(-MathHelper.Pi);
                        }

                        //        Console.WriteLine("turnin");
                    }

                    else
                    {
                        turnDir = InIntersection(gridX, gridZ, buildings, dir, raceEnd);

                        if (interDone == false)
                        {
                            if ((gridX != prevInterX || gridZ != prevInterZ) && !checkTwoLane(gridX, gridZ, buildings, dir))
                            {
                                interDone = true;
                                prevXZ = false;
                            }
                        }

                        if (turnDir == "STRAIGHT")
                        {
                            vehicle.Accelerate();
                            interDone = checkTwoLane(gridX, gridZ, buildings, dir);

                            if (interDone == false && prevXZ == false)
                            {
                                prevInterX = gridX;
                                prevInterZ = gridZ;
                                prevXZ = true;
                            }
                        }

                        else if (turnDir != null)
                        {
                            if (interDone == true)
                            {
                                IsTurning = true;
                                turnStartTime = gameTime;
                                vehicle.Turn(turnDir);
                                    vehicle.Coast();
                            }
                        }

                        else
                        {
                            vehicle.Accelerate();
                            interDone = true;
                        }
                    }
                }
            }
        }


        public Boolean RoadEnds(int x, int z, Building[,] buildings)
        {
            if (buildings[x, z].exists())
            {
                return true;
            }

            return false;
        }

        public String InIntersection(double xCoord, double zCoord, Building[,] buildings, string dir, RaceEnd raceEnd)
        {
            int x = (int)xCoord;
            int z = (int)zCoord;
            Boolean[] canGo = new Boolean[3];
            Boolean[] shouldGo = new Boolean[3];
            canGo[0] = false;
            canGo[1] = false;
            canGo[2] = false;
            shouldGo[0] = false;
            shouldGo[1] = false;
            shouldGo[2] = false;
            int randInt;
            //0 straight, 1 left, 2 right
            if (dir == "NORTH")
            {
                /*       if (!buildings[x - 1, z].exists() && !buildings[x + 1, z].exists())
                       {
                           return "RIGHT";
                       }

                       else if (!buildings[x - 1, z].exists())
                       {
                           return "LEFT";
                       }

                       else if (!buildings[x + 1, z].exists())
                       {
                           return "RIGHT";
                       } */

                if (!buildings[x, z + 1].exists())
                {
                    canGo[0] = true;
                }

                if (!buildings[x - 1, z].exists())
                {
                    canGo[1] = true;
                }

                if (!buildings[x + 1, z].exists())
                {
                    canGo[2] = true;
                }

                int count = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (canGo[i])
                    {
                        count++;
                    }
                }

                if (count == 0)
                {
                    return "RIGHT";
                }

                else if (count == 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (canGo[i])
                        {
                            if (i == 0)
                            {
                                return null;
                            }

                            else if (i == 1)
                            {
                                return "LEFT";
                            }

                            else if (i == 2)
                            {
                                return "RIGHT";
                            }
                        }
                    }
                }

                else
                {
                    //       Console.WriteLine("should");
             //       Boolean[] shouldGo = new Boolean[3];
           //         shouldGo[0] = false;
         //           shouldGo[1] = false;
       //             shouldGo[2] = false;

                    if (z + 1 <= raceEnd.GetPosition().Z && canGo[0])
                    {
                        shouldGo[0] = true;
                    }

                    if (x - 1 >= raceEnd.GetPosition().X && canGo[1])
                    {
                        shouldGo[1] = true;
                    }

                    if (x + 1 <= raceEnd.GetPosition().X && canGo[2])
                    {
                        shouldGo[2] = true;
                    }

                    count = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        if (shouldGo[i])
                        {
                            count++;
                        }
                    }
                    //        Console.WriteLine(count.ToString());
                    if (count == 0)
                    {
                        randInt = rand.Next(0, 3);

                        while (canGo[randInt] == false)
                        {
                            randInt = rand.Next(0, 3);
                        }

                        if (randInt == 0)
                        {
                            return "STRAIGHT";
                        }

                        else if (randInt == 1)
                        {
                            return "LEFT";
                        }

                        else if (randInt == 2)
                        {
                            return "RIGHT";
                        }
                    }

                    else if (count == 1)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (shouldGo[i])
                            {
                                if (i == 0)
                                {
                                    return "STRAIGHT";
                                }

                                else if (i == 1)
                                {
                                    return "LEFT";
                                }

                                else if (i == 2)
                                {
                                    return "RIGHT";
                                }
                            }
                        }
                    }

                    else
                    {
                        randInt = rand.Next(0, 3);

                        while (shouldGo[randInt] == false)
                        {
                            randInt = rand.Next(0, 3);
                        }
                        //        randInt = 0;
                        //         Console.WriteLine(randInt.ToString());

                        if (randInt == 0)
                        {
                            return "STRAIGHT";
                        }

                        else if (randInt == 1)
                        {
                            return "LEFT";
                        }

                        else if (randInt == 2)
                        {
                            return "RIGHT";
                        }
                    }
                }
            }

            if (dir == "EAST")
            {
                /*       if (!buildings[x, z + 1].exists() && !buildings[x, z - 1].exists())
                       {
                           return "RIGHT";
                       }

                       else if (!buildings[x, z + 1].exists())
                       {
                           return "LEFT";
                       }

                       else if (!buildings[x, z - 1].exists())
                       {
                           return "RIGHT";
                       } */

                if (!buildings[x + 1, z].exists())
                {
                    canGo[0] = true;
                }

                if (!buildings[x, z + 1].exists())
                {
                    canGo[1] = true;
                }

                if (!buildings[x, z - 1].exists())
                {
                    canGo[2] = true;
                }

                int count = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (canGo[i])
                    {
                        count++;
                    }
                }

                if (count == 0)
                {
                    return "RIGHT";
                }

                else  if (count == 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (canGo[i])
                        {
                            if (i == 0)
                            {
                                return null;
                            }

                            else if (i == 1)
                            {
                                return "LEFT";
                            }

                            else if (i == 2)
                            {
                                return "RIGHT";
                            }
                        }
                    }
                }

                else
                {
                    //       Console.WriteLine("should");
                    //        Boolean[] shouldGo = new Boolean[3];
                    //         shouldGo[0] = false;
                    //         shouldGo[1] = false;
                    //          shouldGo[2] = false;

                    if (x + 1 <= raceEnd.GetPosition().X && canGo[0])
                    {
                        shouldGo[0] = true;
                    }

                    if (z + 1 <= raceEnd.GetPosition().Z && canGo[1])
                    {
                        shouldGo[1] = true;
                    }

                    if (z - 1 >= raceEnd.GetPosition().Z && canGo[2])
                    {
                        shouldGo[2] = true;
                    }

                    count = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        if (shouldGo[i])
                        {
                            count++;
                        }
                    }
                    //        Console.WriteLine(count.ToString());
                    if (count == 0)
                    {
                        randInt = rand.Next(0, 3);

                        while (canGo[randInt] == false)
                        {
                            randInt = rand.Next(0, 3);
                        }

                        if (randInt == 0)
                        {
                            return "STRAIGHT";
                        }

                        else if (randInt == 1)
                        {
                            return "LEFT";
                        }

                        else if (randInt == 2)
                        {
                            return "RIGHT";
                        }
                    }

                    else if (count == 1)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (shouldGo[i])
                            {
                                if (i == 0)
                                {
                                    return "STRAIGHT";
                                }

                                else if (i == 1)
                                {
                                    return "LEFT";
                                }

                                else if (i == 2)
                                {
                                    return "RIGHT";
                                }
                            }
                        }
                    }

                    else
                    {
                        randInt = rand.Next(0, 3);

                        while (shouldGo[randInt] == false)
                        {
                            randInt = rand.Next(0, 3);
                        }
                        //        randInt = 0;
                        //            Console.WriteLine(randInt.ToString());

                        if (randInt == 0)
                        {
                            return "STRAIGHT";
                        }

                        else if (randInt == 1)
                        {
                            return "LEFT";
                        }

                        else if (randInt == 2)
                        {
                            return "RIGHT";
                        }
                    }
                }
            }

            if (dir == "SOUTH")
            {
                if (!buildings[x, z - 1].exists())
                {
                    canGo[0] = true;
                }

                if (!buildings[x + 1, z].exists())
                {
                    canGo[1] = true;
                }

                if (!buildings[x - 1, z].exists())
                {
                    canGo[2] = true;
                }

                int count = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (canGo[i])
                    {
                        count++;
                    }
                }

                if (count == 0)
                {
                    return "RIGHT";
                }

                else if (count == 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (canGo[i])
                        {
                            if (i == 0)
                            {
                                return null;
                            }

                            else if (i == 1)
                            {
                                return "LEFT";
                            }

                            else if (i == 2)
                            {
                                return "RIGHT";
                            }
                        }
                    }
                }

                else
                {
      //              Boolean[] shouldGo = new Boolean[3];
      //              shouldGo[0] = false;
       //             shouldGo[1] = false;
      //              shouldGo[2] = false;

                    if (z - 1 >= raceEnd.GetPosition().Z && canGo[0])
                    {
                        shouldGo[0] = true;
                    }

                    if (x + 1 <= raceEnd.GetPosition().X && canGo[1])
                    {
                        shouldGo[1] = true;
                    }

                    if (x - 1 >= raceEnd.GetPosition().X && canGo[2])
                    {
                        shouldGo[2] = true;
                    }

                    count = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        if (shouldGo[i])
                        {
                            count++;
                        }
                    }
                    //        Console.WriteLine(count.ToString());
                    if (count == 0)
                    {
                        randInt = rand.Next(0, 3);

                        while (canGo[randInt] == false)
                        {
                            randInt = rand.Next(0, 3);
                        }

                        if (randInt == 0)
                        {
                            return "STRAIGHT";
                        }

                        else if (randInt == 1)
                        {
                            return "LEFT";
                        }

                        else if (randInt == 2)
                        {
                            return "RIGHT";
                        }
                    }

                    else if (count == 1)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (shouldGo[i])
                            {
                                if (i == 0)
                                {
                                    return "STRAIGHT";
                                }

                                else if (i == 1)
                                {
                                    return "LEFT";
                                }

                                else if (i == 2)
                                {
                                    return "RIGHT";
                                }
                            }
                        }
                    }

                    else
                    {
                        randInt = rand.Next(0, 3);

                        while (shouldGo[randInt] == false)
                        {
                            randInt = rand.Next(0, 3);
                        }
                        //          Console.WriteLine(randInt.ToString());

                        if (randInt == 0)
                        {
                            return "STRAIGHT";
                        }

                        else if (randInt == 1)
                        {
                            return "LEFT";
                        }

                        else if (randInt == 2)
                        {
                            return "RIGHT";
                        }
                    }
                }
            }

            if (dir == "WEST")
            {
                if (!buildings[x - 1, z].exists())
                {
                    canGo[0] = true;
                }

                if (!buildings[x, z - 1].exists())
                {
                    canGo[1] = true;
                }

                if (!buildings[x, z + 1].exists())
                {
                    canGo[2] = true;
                }

                int count = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (canGo[i])
                    {
                        count++;
                    }
                }

                if (count == 0)
                {
                    return "RIGHT";
                }

                else if (count == 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (canGo[i])
                        {
                            if (i == 0)
                            {
                                return null;
                            }

                            else if (i == 1)
                            {
                                return "LEFT";
                            }

                            else if (i == 2)
                            {
                                return "RIGHT";
                            }
                        }
                    }
                }

                else
                {
                    //       Console.WriteLine("should");
      //              Boolean[] shouldGo = new Boolean[3];
      //              shouldGo[0] = false;
      //              shouldGo[1] = false;
      //              shouldGo[2] = false;

                    if (x - 1 >= raceEnd.GetPosition().X && canGo[0])
                    {
                        shouldGo[0] = true;
                    }

                    if (z - 1 >= raceEnd.GetPosition().Z && canGo[1])
                    {
                        shouldGo[1] = true;
                    }

                    if (z + 1 <= raceEnd.GetPosition().Z && canGo[2])
                    {
                        shouldGo[2] = true;
                    }

                    count = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        if (shouldGo[i])
                        {
                            count++;
                        }
                    }
                    //        Console.WriteLine(count.ToString());
                    if (count == 0)
                    {
                        randInt = rand.Next(0, 3);

                        while (canGo[randInt] == false)
                        {
                            randInt = rand.Next(0, 3);
                        }

                        if (randInt == 0)
                        {
                            return "STRAIGHT";
                        }

                        else if (randInt == 1)
                        {
                            return "LEFT";
                        }

                        else if (randInt == 2)
                        {
                            return "RIGHT";
                        }
                    }

                    else if (count == 1)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (shouldGo[i])
                            {
                                if (i == 0)
                                {
                                    return "STRAIGHT";
                                }

                                else if (i == 1)
                                {
                                    return "LEFT";
                                }

                                else if (i == 2)
                                {
                                    return "RIGHT";
                                }
                            }
                        }
                    }

                    else
                    {
                        randInt = rand.Next(0, 3);

                        while (shouldGo[randInt] == false)
                        {
                            randInt = rand.Next(0, 3);
                        }
                        //        randInt = 0;
                        //                 Console.WriteLine(randInt.ToString());

                        if (randInt == 0)
                        {
                            return "STRAIGHT";
                        }

                        else if (randInt == 1)
                        {
                            return "LEFT";
                        }

                        else if (randInt == 2)
                        {
                            return "RIGHT";
                        }
                    }
                }
            }

            return null;
        }

        private Boolean checkTwoLane(int x, int z, Building[,] buildings, string dir)
        {
            if (dir == "NORTH" || dir == "SOUTH")
            {
                if (z + 2 < 100)
                {
                    if (!buildings[x + 1, z].exists() && !buildings[x + 1, z + 1].exists() && !buildings[x + 1, z + 2].exists())
                    {
                        return true;
                    }

                    if (!buildings[x - 1, z].exists() && !buildings[x - 1, z + 1].exists() && !buildings[x - 1, z + 2].exists())
                    {
                        return true;
                    }
                }

                else
                {
                    if (!buildings[x + 1, z].exists() && !buildings[x + 1, z - 1].exists() && !buildings[x + 1, z - 2].exists())
                    {
                        return true;
                    }

                    if (!buildings[x - 1, z].exists() && !buildings[x - 1, z - 1].exists() && !buildings[x - 1, z - 2].exists())
                    {
                        return true;
                    }
                }
            }

            if (dir == "EAST" || dir == "WEST")
            {
                if (x + 2 < 100)
                {
                    if (!buildings[x, z - 1].exists() && !buildings[x + 1, z - 1].exists() && !buildings[x + 2, z - 1].exists())
                    {
                        return true;
                    }

                    if (!buildings[x, z + 1].exists() && !buildings[x + 1, z + 1].exists() && !buildings[x + 2, z + 1].exists())
                    {
                        return true;
                    }
                }

                else
                {
                    if (!buildings[x, z - 1].exists() && !buildings[x - 1, z - 1].exists() && !buildings[x - 2, z - 1].exists())
                    {
                        return true;
                    }

                    if (!buildings[x, z + 1].exists() && !buildings[x - 1, z + 1].exists() && !buildings[x - 2, z + 1].exists())
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
