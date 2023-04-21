﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameName;
public struct PlayerTextureContainer
{
    public Texture2D WalkUp { get; set; }
    public Texture2D WalkUpLeft { get; set; }
    public Texture2D WalkUpRight { get; set; }
    public Texture2D WalkDown { get; set; }
    public Texture2D WalkDownLeft { get; set; }
    public Texture2D WalkDownRight { get; set; }
    public Texture2D WalkRight { get; set; }
    public Texture2D WalkLeft { get; set; }
    public Texture2D Damaged { get; set; }
    public Texture2D Attack { get; set; }
    public Texture2D Death { get; set; }
}