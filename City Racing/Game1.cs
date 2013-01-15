using System;
using System.Collections.Generic;
using System.Linq;
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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        enum CollisionType { Building, Vehicle, None };
        GraphicsDeviceManager graphics;
        GraphicsDevice device;

        Effect effects;
        Matrix viewMatrix;
        Matrix projectionMatrix;

        Vector3 lightDirection = new Vector3(3, -2, 5);
        Quaternion cameraRotation;

        Vehicle playerVehicle;
        Building[,] buildings;
        SeparateMethods sMethods;
        RaceEnd raceEnd;
        AI[] AIs;
   //     Map map;
        Texture2D streetTexture, buildingTex1, buildingTex2, buildingTex3, buildingTex4, buildingTex5, buildingTex6, roofTex1, mapTex, yellowSq, redSq, lamTex, ferTex, retTex;// combBuildTex;

        VertexBuffer texVertexBuffer;
        VertexBuffer[] buildingVertexBuffer;
        VertexDeclaration texVertexDeclaration;
        VertexDeclaration[] buildingVertexDeclaration;
        Random rand;

        int screenWidth, screenHeight;

        Model lamModel;

        BoundingBox[] buildingBoundingBoxes, aiBoundingBoxes;
        BoundingBox playerBoundingBox;

        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";            
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 500;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "City Racing";

            lightDirection.Normalize();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            device = graphics.GraphicsDevice;
            rand = new Random();
            sMethods = new SeparateMethods();
            effects = Content.Load<Effect>("Effects");
            lamModel = LoadModel("Lamborghini");
            lamTex = Content.Load<Texture2D>("Lamburghini");
            ferTex = Content.Load<Texture2D>("FerrariPIC");
            retTex = Content.Load<Texture2D>("txRFCar1");
            buildingTex1 = Content.Load<Texture2D>("buildingTex1");
            buildingTex2 = Content.Load<Texture2D>("buildingTex2");
            buildingTex3 = Content.Load<Texture2D>("buildingTex3");
            buildingTex4 = Content.Load<Texture2D>("buildingTex4");
            buildingTex5 = Content.Load<Texture2D>("buildingTex5");
            buildingTex6 = Content.Load<Texture2D>("brickTex");
     //       combBuildTex = Content.Load<Texture2D>("combBuildTex");
            roofTex1 = Content.Load<Texture2D>("roofTex1");
            mapTex = Content.Load<Texture2D>("map");
            yellowSq = Content.Load<Texture2D>("yellowSquare");
            redSq = Content.Load<Texture2D>("redSquare");
            streetTexture = Content.Load<Texture2D>("streetTex");

            Quaternion rotation = Quaternion.Identity;
            rotation = rotation * Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), -MathHelper.Pi);
          
            playerVehicle = new Vehicle(lamModel, lamTex, new Vector3(1.5f, 0.04f, -2.5f), rotation, 0.0002f, 0.001f, 0.012f, 0.1f);
            AIs = new AI[2];
            AIs[0] = new AI(new Vehicle(lamModel, ferTex, new Vector3(1.1f, 0.04f, -3.0f), rotation, 0.00017f, 0.001f, 0.0065f, 0.076f), 0);
            AIs[1] = new AI(new Vehicle(lamModel, retTex, new Vector3(1.1f, 0.04f, -2.5f), rotation, 0.000165f, 0.001f, 0.0065f, 0.076f), 0);

            buildingVertexBuffer = new VertexBuffer[6];
            buildingVertexDeclaration = new VertexDeclaration[6];
            SetUpVertices();
            buildings = sMethods.CreateBuildings();
            SetUpBuildings();

    //        map = new Map();
     //       map.SetUpMap(playerVehicle.GetPosition(), device);

            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;

            GenerateRaceEndLocation();
            SetUpBoundingBoxes();
            UpdateBoundingBoxes();

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        private Model LoadModel(string modelName)
        {
            Model newModel = Content.Load<Model>(modelName);

            foreach (ModelMesh mesh in newModel.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = effects.Clone(device);
                }
            }

            return newModel;
        }

        private void SetUpVertices()
        {
            List<VertexPositionNormalTexture> verticesList = new List<VertexPositionNormalTexture>();

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, 0, -j - 1), new Vector3(0, 1, 0), new Vector2(0, 0)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, 0, -j), new Vector3(0, 1, 0), new Vector2(1, 1)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, 0, -j), new Vector3(0, 1, 0), new Vector2(0, 1)));

                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, 0, -j), new Vector3(0, 1, 0), new Vector2(1, 1)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, 0, -j - 1), new Vector3(0, 1, 0), new Vector2(0, 0)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, 0, -j - 1), new Vector3(0, 1, 0), new Vector2(1, 0)));

         /*           verticesList.Add(new VertexPositionNormalTexture(new Vector3((i * 2) - 0.25f, 0, -j), new Vector3(0, 1, 0), new Vector2(0, 0)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(((i + 1) * 2) - 0.25f, 0, 0), new Vector3(0, 1, 0), new Vector2(1, 1)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3((i * 2) - 0.25f, 0, 0), new Vector3(0, 1, 0), new Vector2(0, 1)));

                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(((i + 1) * 2) - 0.25f, 0, 0), new Vector3(0, 1, 0), new Vector2(1, 1)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3((i * 2) - 0.25f, 0, -j), new Vector3(0, 1, 0), new Vector2(0, 0)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(((i + 1) * 2) - 0.25f, 0, -j), new Vector3(0, 1, 0), new Vector2(1, 0))); */




      //              verticesList.Add(new VertexPositionNormalTexture(new Vector3(i, 0, -j), new Vector3(0, 1, 0), new Vector2(0, 0)));
      //              verticesList.Add(new VertexPositionNormalTexture(new Vector3((i + 1) * 2, 0, -j - 1), new Vector3(0, 1, 0), new Vector2(1, 1)));
      //              verticesList.Add(new VertexPositionNormalTexture(new Vector3(i * 2, 0, -j), new Vector3(0, 1, 0), new Vector2(0 , 1)));

    //                verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, buildingHeights[currentbuilding], -z - 1), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 0)));
     //               verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, buildingHeights[currentbuilding], -z - 1), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2 + 1) / imagesInTexture, 0)));
     //               verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, buildingHeights[currentbuilding], -z), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2 + 1) / imagesInTexture, 1)));
                }
            }

            

            texVertexBuffer = new VertexBuffer(device, verticesList.Count * VertexPositionNormalTexture.SizeInBytes, BufferUsage.WriteOnly);

            texVertexBuffer.SetData<VertexPositionNormalTexture>(verticesList.ToArray());
            texVertexDeclaration = new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);
        }

        private void SetUpBuildings()
        {
            for (int h = 0; h < 6; h++)
            {
                List<VertexPositionNormalTexture> verticesList = new List<VertexPositionNormalTexture>();
                int height;// n;

                for (int i = 0; i < 100; i++)
                {
                    for (int j = 0; j < 100; j++)
                    {
                        //Border Buildings
                        if (buildings[i, j].exists())
                        {
                            height = buildings[i, j].getHeight();
                            //          height = 5;
                            // height = type
                            //           n = 5;
                            //n is the number of building textures


                            if (h == height - 1)
                            {
                                //FRONT
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, height, -j), new Vector3(0, 0, -1), new Vector2(0, 0)));
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, 0, -j), new Vector3(0, 0, -1), new Vector2(1, 1)));
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, 0, -j), new Vector3(0, 0, -1), new Vector2(0, 1)));

                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, 0, -j), new Vector3(0, 0, -1), new Vector2(1, 1)));
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, height, -j), new Vector3(0, 0, -1), new Vector2(0, 0)));
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, height, -j), new Vector3(0, 0, -1), new Vector2(1, 0)));

                                //BACK
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, 0, -j - 1), new Vector3(0, 0, 1), new Vector2(1, 1)));
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, height, -j - 1), new Vector3(0, 0, 1), new Vector2(1, 0)));
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, height, -j - 1), new Vector3(0, 0, 1), new Vector2(0, 0)));

                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, height, -j - 1), new Vector3(0, 0, 1), new Vector2(0, 0)));
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, 0, -j - 1), new Vector3(0, 0, 1), new Vector2(0, 1)));
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, 0, -j - 1), new Vector3(0, 0, 1), new Vector2(1, 1)));

                                //RIGHT
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, height, -j), new Vector3(1, 0, 0), new Vector2(0, 0)));
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, 0, -j - 1), new Vector3(1, 0, 0), new Vector2(1, 1)));
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, 0, -j), new Vector3(1, 0, 0), new Vector2(0, 1)));

                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, 0, -j - 1), new Vector3(1, 0, 0), new Vector2(1, 1)));
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, height, -j), new Vector3(1, 0, 0), new Vector2(0, 0)));
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, height, -j - 1), new Vector3(1, 0, 0), new Vector2(1, 0)));

                                //LEFT
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, 0, -j), new Vector3(-1, 0, 0), new Vector2(1, 1)));
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, 0, -j - 1), new Vector3(-1, 0, 0), new Vector2(0, 1)));
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, height, -j - 1), new Vector3(-1, 0, 0), new Vector2(0, 0)));

                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, height, -j - 1), new Vector3(-1, 0, 0), new Vector2(0, 0)));
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, height, -j), new Vector3(-1, 0, 0), new Vector2(1, 0)));
                                verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, 0, -j), new Vector3(-1, 0, 0), new Vector2(1, 1)));
                            }


                            /*             //FRONT
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, height, -j), new Vector3(0, 0, -1), new Vector2((height - 1) / n, 0)));
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, 0, -j), new Vector3(0, 0, -1), new Vector2((height) / n, 1)));
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, 0, -j), new Vector3(0, 0, -1), new Vector2((height) / n, 0)));

                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, 0, -j), new Vector3(0, 0, -1), new Vector2((height) / n, 1)));
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, height, -j), new Vector3(0, 0, -1), new Vector2((height - 1) / n, 0)));
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, height, -j), new Vector3(0, 0, -1), new Vector2((height - 1) / n, 1))); 

                                         //BACK
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, 0, -j - 1), new Vector3(0, 0, 1), new Vector2((height) / n, 1)));
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, height, -j - 1), new Vector3(0, 0, 1), new Vector2((height - 1) / n, 1)));
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, height, -j - 1), new Vector3(0, 0, 1), new Vector2((height - 1) / n, 0)));

                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, height, -j - 1), new Vector3(0, 0, 1), new Vector2((height - 1) / n, 0)));
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, 0, -j - 1), new Vector3(0, 0, 1), new Vector2((height) / n, 0)));
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, 0, -j - 1), new Vector3(0, 0, 1), new Vector2((height) / n, 1)));

                                          //RIGHT
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, height, -j), new Vector3(1, 0, 0), new Vector2((height - 1) / n, 0)));
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, 0, -j - 1), new Vector3(1, 0, 0), new Vector2((height) / n, 1)));
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, 0, -j), new Vector3(1, 0, 0), new Vector2((height) / n, 0)));

                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, 0, -j - 1), new Vector3(1, 0, 0), new Vector2((height) / n, 1)));
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, height, -j), new Vector3(1, 0, 0), new Vector2((height - 1) / n, 0)));
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i + 1 - 0.25f, height, -j - 1), new Vector3(1, 0, 0), new Vector2((height - 1) / n, 1))); 

                                         //LEFT
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, 0, -j), new Vector3(-1, 0, 0), new Vector2((height) / n, 1)));
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, 0, -j - 1), new Vector3(-1, 0, 0), new Vector2((height - 1) / n, 1)));
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, height, -j - 1), new Vector3(-1, 0, 0), new Vector2((height - 1) / n, 0)));

                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, height, -j - 1), new Vector3(-1, 0, 0), new Vector2((height - 1) / n, 0)));
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, height, -j), new Vector3(-1, 0, 0), new Vector2((height) / n, 0)));
                                         verticesList.Add(new VertexPositionNormalTexture(new Vector3(i - 0.25f, 0, -j), new Vector3(-1, 0, 0), new Vector2((height) / n, 1))); */
                        }
                    }
                }

                buildingVertexBuffer[h] = new VertexBuffer(device, verticesList.Count * VertexPositionNormalTexture.SizeInBytes, BufferUsage.WriteOnly);

                buildingVertexBuffer[h].SetData<VertexPositionNormalTexture>(verticesList.ToArray());
                buildingVertexDeclaration[h] = new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);
            }
        }

        private void GenerateRaceEndLocation()
        {
            int x, z;
            while (true)
            {
                x = rand.Next(50, 100);
                z = rand.Next(50, 100);
       //         x = 12;
       //         z = 12;
                if (buildings[x, z].exists() == false)
                {
                    break;
                }
            }

            raceEnd = new RaceEnd(new Vector2(x, z), screenHeight);
        }

        private void SetUpBoundingBoxes()
        {
            List<BoundingBox> boundingList = new List<BoundingBox>();

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    if (buildings[i, j].exists())
                    {
                        Vector3[] buildingPoints = new Vector3[2];
                        buildingPoints[0] = new Vector3(i * 1.25f, 0, -j * 1.25f);
                        buildingPoints[1] = new Vector3((i * 1.25f) + 1.25f, buildings[i, j].getHeight(), (-j * 1.25f) - 1.25f);
                        BoundingBox buildingBox = BoundingBox.CreateFromPoints(buildingPoints);
                        boundingList.Add(buildingBox);
                    }
                }
            }

            buildingBoundingBoxes = boundingList.ToArray();
        }

        private CollisionType CheckCollision(int playerOrAI, BoundingBox box)
        {
            for (int i = 0; i < buildingBoundingBoxes.Length; i++)
            {
                if (buildingBoundingBoxes[i].Contains(box) != ContainmentType.Disjoint)
                {
                    return CollisionType.Building;
                }
            }

            //player
            if (playerOrAI == 0)
            {
                for (int i = 0; i < AIs.Length; i++)
                {
                    if (aiBoundingBoxes[i].Contains(box) != ContainmentType.Disjoint)
                    {
                        AIs[i].GetVehicle().SetVelocity(0.012f);
                        AIs[i].GetVehicle().SetTempBackForce(15);
                        AIs[i].GetVehicle().MoveOpp(playerVehicle.GetRotation());

                     /*  if (CheckCollision(i + 1, aiBoundingBoxes[i]) == CollisionType.Building)
                        {
                            AIs[i].GetVehicle().MoveOpp(playerVehicle.GetRotation() * Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), -3.14f));
                        } */
                        
                        return CollisionType.Vehicle;
                    }
                }
            }

                //AI - all playerOrAi values will be 1 higher than index of array of AIs
            else if (playerOrAI > 0)
            {
                if (playerBoundingBox.Contains(box) != ContainmentType.Disjoint)
                {
                    playerVehicle.SetVelocity(0.012f);
                    playerVehicle.SetTempBackForce(15);
                    playerVehicle.MoveOpp(playerVehicle.GetRotation());
                    return CollisionType.Vehicle;
                }

                for (int i = 0; i < AIs.Length; i++)
                {
                    if (i != playerOrAI - 1)
                    {
                        if (aiBoundingBoxes[i].Contains(box) != ContainmentType.Disjoint)
                        {
                            AIs[i].GetVehicle().SetVelocity(0.012f);
                            AIs[i].GetVehicle().SetTempBackForce(15);
                            AIs[i].GetVehicle().MoveOpp(playerVehicle.GetRotation());
                            return CollisionType.Vehicle;
                        }
                    }
                }
            }
    //        Console.WriteLine(buildingBoundingBoxes.Length.ToString());
            return CollisionType.None;
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            CheckInput();

       /*     for (int i = 0; i < AIs.Length; i++)
            {
                AIs[i].DecideAction(raceEnd, buildings, gameTime);
            }  */

            AIs[0].DecideAction(raceEnd, buildings, gameTime);

            Move();
            UpdateBoundingBoxes();
            UpdateCamera();
            CheckVictor();
            
  //          map.SetUpMap(playerVehicle.GetPosition(), device);

            base.Update(gameTime);
        }

        private void UpdateCamera()
        {
            cameraRotation = Quaternion.Lerp(cameraRotation, playerVehicle.rotation, 0.1f);
            
            Vector3 campos = new Vector3(0, 0.1f, -0.6f);
            campos = Vector3.Transform(campos, Matrix.CreateFromQuaternion(cameraRotation));
            campos += playerVehicle.GetPosition();

            Vector3 camup = new Vector3(0, 1, 0);
            camup = Vector3.Transform(camup, Matrix.CreateFromQuaternion(cameraRotation));

            viewMatrix = Matrix.CreateLookAt(campos, playerVehicle.GetPosition(),camup);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.2f, 500.0f);
        }

        private void CheckInput()
        {
            KeyboardState keys = Keyboard.GetState();

            if (keys.IsKeyDown(Keys.Up))
            {
                playerVehicle.Accelerate();
            }

            else if (keys.IsKeyDown(Keys.Down))
            {
                playerVehicle.Brake();
            }

            else
            {
                playerVehicle.Coast();
            }

            if (keys.IsKeyDown(Keys.Right))
            {
            /*    Quaternion rotation = playerVehicle.GetRotation();
                rotation = rotation * Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), -3.14f * playerVehicle.GetTurningSpeed());
                playerVehicle.SetRotation(rotation); */
                playerVehicle.Turn("RIGHT");
            }
                
            else if(keys.IsKeyDown(Keys.Left))
            {
          /*      Quaternion rotation = playerVehicle.GetRotation();
                rotation = rotation * Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), 3.14f * playerVehicle.GetTurningSpeed());
                playerVehicle.SetRotation(rotation); */
                playerVehicle.Turn("LEFT");
            }

      /*      if(keys.IsKeyDown(Keys.R))
            {
                playerVehicle.SetPosition(playerVehicle.GetPosition() + new Vector3(0, 0, -1.25f));
            } */
        }

        private void Move()
        {
            playerVehicle.Move();

            if (CheckCollision(0, playerBoundingBox) == CollisionType.Building)
            {
                if (playerVehicle.GetTempBackForce() == 0)
                {
                    if (playerVehicle.GetVelocity() > 0)
                    {
                        playerVehicle.SetVelocity(-0.008f);
                        playerVehicle.SetTempBackForce(15);
                    }

                    else
                    {
                        playerVehicle.SetVelocity(0.008f);
                        playerVehicle.SetTempBackForce(15);
                    }
                }
           //     else
          //      {
         //           playerVehicle.SetTempBackForce(playerVehicle.GetTempBackForce() - 1);
            //    }
            //    playerVehicle.NegMove();
            }

            else if (CheckCollision(0, playerBoundingBox) == CollisionType.Vehicle)
            {
                if (playerVehicle.GetTempBackForce() == 0)
                {
                    if (playerVehicle.GetVelocity() > 0)
                    {
                        playerVehicle.SetVelocity(-0.008f);
                        playerVehicle.SetTempBackForce(15);
                    }

                    else
                    {
                        playerVehicle.SetVelocity(0.008f);
                        playerVehicle.SetTempBackForce(15);
                    }
                }
            }

            if (playerVehicle.GetTempBackForce() > 0)
            {
                playerVehicle.SetTempBackForce(playerVehicle.GetTempBackForce() - 1);
            }

            for (int i = 0; i < AIs.Length; i++)
            {

                    AIs[i].GetVehicle().Move();

                if (CheckCollision(i + 1, aiBoundingBoxes[i]) == CollisionType.Building || CheckCollision(i + 1, aiBoundingBoxes[i]) == CollisionType.Vehicle)
                {
                    if (AIs[i].GetVehicle().GetTempBackForce() == 0)
                    {
                        if (AIs[i].GetVehicle().GetVelocity() > 0)
                        {
                            AIs[i].GetVehicle().SetVelocity(-0.01f);
                            AIs[i].GetVehicle().SetTempBackForce(25);
                        //    AIs[i].GetVehicle().SetAngle(AIs[i].GetVehicle().GetAngle() - 1f);
                        }

                        else
                        {
                            AIs[i].GetVehicle().SetVelocity(0.01f);
                            AIs[i].GetVehicle().SetTempBackForce(25);
                        }
                    }
                //    AIs[i].GetVehicle().NegMove();
                }

                if (AIs[i].GetVehicle().GetTempBackForce() > 0)
                {
                    AIs[i].GetVehicle().SetTempBackForce(AIs[i].GetVehicle().GetTempBackForce() - 1);
                }
            }
        }

        private void CheckVictor()
        {
            if (playerVehicle.GetPosition().X >= raceEnd.GetPosition().X && playerVehicle.GetPosition().X <= raceEnd.GetPosition().X + 1 && -playerVehicle.GetPosition().Z >= raceEnd.GetPosition().Z && -playerVehicle.GetPosition().Z <= raceEnd.GetPosition().Z + 1)
            {
                playerVehicle.SetRotation(new Quaternion(5, 5, 5, 5));
            }
        }

        private void UpdateBoundingBoxes()
        {
     //       BoundingSphere xwingSphere = new BoundingSphere(xwingPosition, 0.04f);
    //       playerBoundingBox = new BoundingBox(playerVehicle.GetPosition() + new Vector3(-0.045f, -playerVehicle.GetPosition().Y, 0.3999999999999999999999f), playerVehicle.GetPosition() + new Vector3(-0.028f, 0.02f, 0.4f));
     //       playerBoundingBox = new BoundingBox(playerVehicle.GetPosition() + new Vector3(-0.045f, -playerVehicle.GetPosition().Y, 0.0004f), playerVehicle.GetPosition() + new Vector3(-0.028f, 0.02f, 0.0003999999999999999999999f));
            playerBoundingBox = new BoundingBox(playerVehicle.GetPosition() + new Vector3(0.2213f, -playerVehicle.GetPosition().Y, -0.13f), playerVehicle.GetPosition() + new Vector3(0.378f, 0.02f, 0.1f));
        //    Console.WriteLine(playerVehicle.GetPosition().Z);
     //       playerBoundingBox = new BoundingSphere(playerVehicle.GetPosition(), 0.02f);
            aiBoundingBoxes = new BoundingBox[AIs.Length];
            for (int i = 0; i < AIs.Length; i++)
            {
                aiBoundingBoxes[i] = new BoundingBox(AIs[i].GetVehicle().GetPosition() + new Vector3(0.2213f, -AIs[i].GetVehicle().GetPosition().Y, -0.13f), AIs[i].GetVehicle().GetPosition() + new Vector3(0.378f, 0.02f, 0.1f));
            }

            //OLD VALUES BEFORE AI ADDITION
            //aiBoundingBoxes[i] = new BoundingBox(AIs[i].GetVehicle().GetPosition() + new Vector3(0.17f, -AIs[i].GetVehicle().GetPosition().Y, -0.13f), AIs[i].GetVehicle().GetPosition() + new Vector3(0.47f, 0.02f, 0.1f));
        }

        protected override void Draw(GameTime gameTime)
        {
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.LightBlue, 1.0f, 0);

            GraphicsDevice.RenderState.DepthBufferEnable = true;
            GraphicsDevice.RenderState.AlphaBlendEnable = false;
            GraphicsDevice.RenderState.AlphaTestEnable = false;

            GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;

            DrawStreets();
            DrawBuildings();
            DrawModel(playerVehicle);

            for (int i = 0; i < AIs.Length; i++)
            {
                DrawModel(AIs[i].GetVehicle());
            }

            spriteBatch.Begin();
            DrawMap();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawStreets()
        {
            Matrix WorldMatrix = Matrix.CreateScale(1.25f, 1.25f, 1.25f);
            effects.CurrentTechnique = effects.Techniques["Textured"];
            effects.Parameters["World"].SetValue(WorldMatrix);
            effects.Parameters["View"].SetValue(viewMatrix);
            effects.Parameters["Projection"].SetValue(projectionMatrix);
            effects.Parameters["xTexture"].SetValue(streetTexture);
            effects.Begin();
            foreach (EffectPass pass in effects.CurrentTechnique.Passes)
            {
                pass.Begin();
                device.VertexDeclaration = texVertexDeclaration;
                device.Vertices[0].SetSource(texVertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes);
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, texVertexBuffer.SizeInBytes / VertexPositionNormalTexture.SizeInBytes / 3);
                pass.End();
            }
            effects.End();
        }

        private void DrawBuildings()
        {
            for (int i = 0; i < 6; i++)
            {
                Matrix WorldMatrix = Matrix.CreateScale(1.25f, 1.25f, 1.25f);
                effects.CurrentTechnique = effects.Techniques["Textured"];
                effects.Parameters["World"].SetValue(WorldMatrix);
                effects.Parameters["View"].SetValue(viewMatrix);
                effects.Parameters["Projection"].SetValue(projectionMatrix);

                if (i == 0)
                {
                    effects.Parameters["xTexture"].SetValue(buildingTex1);
                }

                else if (i == 1)
                {
                    effects.Parameters["xTexture"].SetValue(buildingTex2);
                }

                else if (i == 2)
                {
                    effects.Parameters["xTexture"].SetValue(buildingTex3);
                }

                else if (i == 3)
                {
                    effects.Parameters["xTexture"].SetValue(buildingTex4);
                }

                else if (i == 4)
                {
                    effects.Parameters["xTexture"].SetValue(buildingTex5);
                }

                else if (i == 5)
                {
                    effects.Parameters["xTexture"].SetValue(buildingTex6);
                }

                effects.Begin();

          //      int x = 0;

                foreach (EffectPass pass in effects.CurrentTechnique.Passes)
                {
                    pass.Begin();

                    //           Matrix WorldMatrix = Matrix.CreateScale(1f, 1f, 1f);
                    /*            effects.CurrentTechnique = effects.Techniques["Textured"];
                                effects.Parameters["World"].SetValue(WorldMatrix);
                                effects.Parameters["View"].SetValue(viewMatrix);
                                effects.Parameters["Projection"].SetValue(projectionMatrix);
                                x = rand.Next(0, 2);
                                if (x == 0)
                                {
                                    effects.Parameters["xTexture"].SetValue(buildingTex1);
                                }

                                else if (x == 1)
                                {
                                    effects.Parameters["xTexture"].SetValue(buildingTex2);
                                }
                                effects.Begin(); */

                    device.VertexDeclaration = buildingVertexDeclaration[i];
                    device.Vertices[0].SetSource(buildingVertexBuffer[i], 0, VertexPositionNormalTexture.SizeInBytes);
                    device.DrawPrimitives(PrimitiveType.TriangleList, 0, buildingVertexBuffer[i].SizeInBytes / VertexPositionNormalTexture.SizeInBytes / 3);
                    pass.End();
                    //         effects.End();
                }
                effects.End();
            }
        }

        private void DrawModel(Vehicle vehicle)
        {
            Model vModel = vehicle.GetModel();
            Texture2D vTex = vehicle.GetTexture();
            Vector3 vPos = vehicle.GetPosition();
            Quaternion vRot = vehicle.GetRotation();

            Matrix worldMatrix = Matrix.CreateScale(0.05f, 0.05f, 0.05f) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateFromQuaternion(vRot) * Matrix.CreateTranslation(vPos);
            Matrix[] xwingTransforms = new Matrix[vModel.Bones.Count];
            vModel.CopyAbsoluteBoneTransformsTo(xwingTransforms);

            foreach (ModelMesh mesh in vModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
                    currentEffect.Parameters["World"].SetValue(xwingTransforms[mesh.ParentBone.Index] * worldMatrix);
                    currentEffect.Parameters["View"].SetValue(viewMatrix);
                    currentEffect.Parameters["Projection"].SetValue(projectionMatrix);
                    currentEffect.Parameters["EnableLighting"].SetValue(true);
                    currentEffect.Parameters["LightDirection"].SetValue(lightDirection);
                    currentEffect.Parameters["Ambient"].SetValue(0.5f);
                    currentEffect.Parameters["xTexture"].SetValue(vTex);
                }

                mesh.Draw();
            }
        }

        private void DrawMap()
        {
    /*        Matrix WorldMatrix = Matrix.CreateScale(1f, 1f, 1f);
            effects.CurrentTechnique = effects.Techniques["Textured"];
            effects.Parameters["World"].SetValue(WorldMatrix);
            effects.Parameters["View"].SetValue(viewMatrix);
            effects.Parameters["Projection"].SetValue(projectionMatrix);
            effects.Parameters["xTexture"].SetValue(mapTex);

            effects.Begin();
            foreach (EffectPass pass in effects.CurrentTechnique.Passes)
            {
                pass.Begin();
                device.VertexDeclaration = map.GetVertexDeclaration();
                device.Vertices[0].SetSource(map.GetVertexBuffer(), 0, VertexPositionNormalTexture.SizeInBytes);
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, map.GetVertexBuffer().SizeInBytes / VertexPositionNormalTexture.SizeInBytes / 3);
                pass.End();
            }
            effects.End(); */

            Rectangle rect = new Rectangle(0, screenHeight - 150, 150, 150);
   //         spriteBatch.Begin();
            spriteBatch.Draw(mapTex, rect, Color.White);//spriteBatch.Draw(carriageTexture, player.Position, null, player.Color, 0, new Vector2(0, carriageTexture.Height), playerScaling, SpriteEffects.None, 0);
     //       spriteBatch.Draw(mapTex, player.Position, null, player.Color, 0, new Vector2(0, carriageTexture.Height), playerScaling, SpriteEffects.None, 0);
   //         spriteBatch.Draw(mapTex, new Vector2(0, screenHeight - 100), null, Color.White, 0, new Vector2(50, screenHeight - 50), 0, SpriteEffects.None, 0); 
    //        spriteBatch.End();
            Vector3 pos = playerVehicle.GetPosition();
            double x = (double)pos.X;
            double z = (double)pos.Z;

            double xPos, zPos;
            xPos = ((x / (99.75f * 1.25f)) * 150);// -0.25f;
            zPos = (z / (99 * 1.25f)) * 150;
            xPos = (float)(Math.Round(xPos, 0));
            zPos = (float)(Math.Round(zPos, 0));
            Rectangle rect2 = new Rectangle((int)xPos - 2, screenHeight - ((int)zPos * -1) - 2, 4, 4);
     //       Console.WriteLine(xPos.ToString());
            spriteBatch.Draw(yellowSq, rect2, Color.White);
            spriteBatch.Draw(redSq, raceEnd.GetMapLoc(), Color.White);
        }
    }
}
