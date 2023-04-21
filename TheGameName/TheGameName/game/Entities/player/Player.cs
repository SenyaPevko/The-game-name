using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheGameName;

public class Player : Unit, IGameEntity
{
    private const int WALK_SPRITE_COUNT = 3;
    private const int ATTACK_SPRITE_COUNT = 9;
    private const int DAMAGED_SPRITE_COUNT = 2;
    private const int SPRITE_HEIGHT = 34;
    private const int SPRITE_WIDTH = 32;
    private const int ATTACK_ANIM_FPS = 10;
    private const int MAGAZINE_SIZE = 10;
    private const double FIRE_RATE = 60;

    private readonly RenderStateController renderStateController = new RenderStateController();

    public float Speed { get; set; } = 2;
    public string CurrentState { get; private set; } = nameof(PlayerTextureContainer.WalkUp);
    public int bulletsCounter = MAGAZINE_SIZE;
    public double shootDelayTimer;
    public Texture2D bulletTexture;


    public Player(TheGameName game, PlayerTextureContainer textureContainer)
    {
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkDown),
            new SpriteAnimation(textureContainer.WalkDown, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkDownLeft),
            new SpriteAnimation(textureContainer.WalkDownLeft, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkDownRight),
            new SpriteAnimation(textureContainer.WalkDownRight, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkUp),
            new SpriteAnimation(textureContainer.WalkUp, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT));
        renderStateController.SetState(nameof(PlayerTextureContainer.WalkUp));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkUpRight),
            new SpriteAnimation(textureContainer.WalkUpRight, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkUpLeft),
            new SpriteAnimation(textureContainer.WalkUpLeft, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkRight),
            new SpriteAnimation(textureContainer.WalkRight, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkLeft),
            new SpriteAnimation(textureContainer.WalkLeft, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT));

        /*        var attackAnimation = new SpriteAnimation(textureContainer.Attack,
                    ATTACK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT)
                { Fps = ATTACK_ANIM_FPS};
                attackAnimation.AnimationCompleted += Attack_AnimationCompleted;
                renderStateController.AddState(nameof(PlayerTextureContainer.Attack), attackAnimation);*/
    }

/*    private void Attack_AnimationCompleted(object sender, AnimationCompletedEventArgs e)
    {
        renderStateController.SetState(nameof(PlayerTextureContainer.Idle));
    }*/

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        renderStateController.Draw(spriteBatch, Position, SpriteEffects.None);
    }

    public void Update(GameTime gameTime)
    {
        renderStateController.Update(gameTime);
    }

    public void Move(Vector2 direction, string state)
    {
        CurrentState = state;
        renderStateController.SetState(state);
        renderStateController.CurrentState.Animation.Play();
        Position += direction * Speed;
    }

    public void Halt()
    {
        renderStateController.SetState(CurrentState);
        renderStateController.CurrentState.Animation.Play();
        renderStateController.CurrentState.Animation.Pause();
    }

    public bool Attack(GameTime gameTime)
    {
        if (bulletsCounter <= 0) return false;
        shootDelayTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (shootDelayTimer > FIRE_RATE) 
        {
            shootDelayTimer = 0;
            //renderStateController.SetState(nameof(PlayerTextureContainer.Attack));
            //renderStateController.CurrentState.Animation.Play();
            Globals.bulletsContoller.AddBulletToFired(this);
            bulletsCounter--;
        }
        return true;
    }

    public void Reload()
    {
        bulletsCounter = MAGAZINE_SIZE;
    }
}
