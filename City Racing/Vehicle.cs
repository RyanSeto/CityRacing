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
    class Vehicle
    {
        public Model vehicleModel;
        public Texture2D vehicleTexture;
        public Vector3 position;
        public Quaternion rotation;
        public float velocity;
        public float acceleration;// = 0.0002f;
        public float brakingSpeed;// = 0.001f;
        public float friction = 0.00008f;
        public float turningSpeed;// = 0.012f;
        public float angle;
        public float maxSpeed;
        public int tempBackForce;

        public Vehicle(Model model, Texture2D texture, Vector3 position, Quaternion rotation, float Acceleration, float brakeSpeed, float turnSpeed, float maxV)
        {
            vehicleModel = model;
            vehicleTexture = texture;
            this.position = position;
            this.rotation = rotation;

            acceleration = Acceleration;
            brakingSpeed = brakeSpeed;
            turningSpeed = turnSpeed;
            maxSpeed = maxV;

            velocity = 0;
            angle = -MathHelper.Pi;
            tempBackForce = 0;
        }

        public void SetVelocity(float speed)
        {
            velocity = speed;
        }

        public void SetPosition(Vector3 pos)
        {
            position = pos;
        }

        public Model GetModel()
        {
            return vehicleModel;
        }

        public Texture2D GetTexture()
        {
            return vehicleTexture;
        }

        public float GetVelocity()
        {
            return velocity;
        }

        public Vector3 GetPosition()
        {
            return position;
        }

        public void SetRotation(Quaternion rot)
        {
            rotation = rot;
        } 

        public Quaternion GetRotation()
        {
            return rotation;
        }

        public float GetTurningSpeed()
        {
            return turningSpeed;
        }

        public void Accelerate()
        {
            if (velocity + acceleration - friction > maxSpeed)
            {
                velocity = maxSpeed;
            }

            else
            {
                velocity = velocity + acceleration - friction;
            }
        }

        public void Brake()
        {
      /*      if (velocity - brakingSpeed < 0)
            {
                velocity = 0;
            }

            else
            {
                velocity = velocity - brakingSpeed - friction;
            } */

            if (velocity > 0)
            {
                velocity = velocity - brakingSpeed - friction;
            }

            else
            {
                if (velocity - acceleration < -maxSpeed)
                {
                    velocity = -maxSpeed;
                }

                else
                {
                    velocity = velocity - acceleration;
                }
            }
        }

        public void Coast()
        {
        /*    if (velocity - friction < 0)
            {
                velocity = 0;
            }

            else
            {
                velocity = velocity - friction;
            } */
            if (velocity > 0)
            {
                velocity = velocity - friction;
            }

            else
            {
                velocity = velocity + friction;
            }
        }

        public void Turn(String direction)
        {
            if (direction == "RIGHT")
            {
                rotation = rotation * Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), -3.14f * turningSpeed);
                angle = angle + (-3.14f * turningSpeed);
            }

            else if (direction == "LEFT")
            {
                rotation = rotation * Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), 3.14f * turningSpeed);
                angle = angle + (3.14f * turningSpeed);
            }
        }

        public float GetAngle()
        {
            return angle;
        }

        public void SetAngle(float ang)
        {
            angle = ang;
            rotation = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), angle);
        }

        public void Move()
        {
            Vector3 changeVector = Vector3.Transform(new Vector3(0, 0, velocity), rotation);
            position = position + changeVector;
        }

     /*   public void NegMove()
        {
            Vector3 changeVector = Vector3.Transform(new Vector3(0, 0, -velocity), rotation);
            position = position + changeVector;
        } */
     //   public void BackMove(Vector3 

        public void MoveOpp(Quaternion changeRot)
        {
            Vector3 changeVector = Vector3.Transform(new Vector3(0, 0, velocity), changeRot);
            position = position + changeVector;
        }

        public int GetTempBackForce()
        {
            return tempBackForce;
        }

        public void SetTempBackForce(int time)
        {
            tempBackForce = time;
        }
    }
}
