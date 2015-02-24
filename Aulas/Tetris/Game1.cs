#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Tetris
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Box que forma as peças
        Texture2D box;
        Vector2 posicaoBox;
        byte[,] board = new byte[22, 10];
        Piece piece;
        int pX = 4, pY = 2;
        float lastAutomaticMove = 0f;
        float lastHumanMove = 0f;
        KeyboardState teclado;
        bool spacePressed, controlPressed;
        Color[] colors = { Color.Wheat, Color.YellowGreen, Color.DarkBlue, Color.Pink, Color.Sienna, Color.LimeGreen, Color.Orchid };

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferMultiSampling = true;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.PreferredBackBufferWidth = 300;
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            posicaoBox = new Vector2(0, 0);
            teclado = new KeyboardState();
            spacePressed = false;
            controlPressed = false;

            piece = new Piece();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            box = Content.Load<Texture2D>("sprites/box");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            box.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            lastAutomaticMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            lastHumanMove += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Atulizar teclado
            teclado = Keyboard.GetState();
            if (lastHumanMove >= 1/20f)
            {
                lastHumanMove = 0;
                //Esquerda
                if (teclado.IsKeyDown(Keys.Left) && pX > 0 && canGoLeft())
                {
                   pX--;
                }
                //Direita
                if (teclado.IsKeyDown(Keys.Right) && pX < 10 - piece.Width && canGoRight())
                {
                    pX++; 
                }
                //Baixo
                if (teclado.IsKeyDown(Keys.Down) && canGoDown())
                {
                    pY++;
                }
                if (teclado.IsKeyUp(Keys.Space))
                {
                    spacePressed = false;
                }
                //Mandar peça diretamente para baixo
                if (!spacePressed && teclado.IsKeyDown(Keys.Space))
                {
                    while (canGoDown())
                    {
                        pY++;
                    }
                    newPiece();
                    spacePressed = true;
                }
                //rodar a peça
                if (!controlPressed && teclado.IsKeyDown(Keys.LeftControl))
                {
                    piece.rotate();
                    controlPressed = true;
                }
                if (teclado.IsKeyUp(Keys.LeftControl))
                {
                    controlPressed = false;
                }
            }
            
            //Atualizar contador da queda de peças
            if(lastAutomaticMove>=0.3f){
                if (canGoDown())
                {
                    pY++;
                    lastAutomaticMove = 0;
                }
                else newPiece();
                
            }

            destruirLinha();

            base.Update(gameTime);
        }

        /// <summary>
        /// Gera uma nova peça
        /// </summary>
        private void newPiece()
        {
            freeze();
            piece = new Piece();
            pX = (10 - piece.Width) / 2;
            pY = 0;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            for (int x = 0; x < 10; x++)
            {
                for (int y = 2; y < 22; y++)
                {
                    if (board[y, x] != 0)
                    {
                        spriteBatch.Draw(box, new Vector2(x * 30, (y - 2) * 30), Color.Orange);
                    }
                    if (y >= pY && x >= pX 
                        && y < pY + piece.Height
                        && x < pX + piece.Width
                        && piece.getBlock(y-pY, x-pX) != 0)
                    {
                        spriteBatch.Draw(box, new Vector2(x * 30, (y - 2) * 30), colors[piece.getBlock(y - pY, x - pX)-1]);
                    }
                }
            }

                
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Indica se uma peça pode descer mais ou não
        /// </summary>
        /// <returns>Bool</returns>
        private bool canGoDown()
        {
            if (pY + piece.Height >=22)
            {
                return false;
            }
            else
            {
                return canGo(pX, pY + 1);
            }
        }

        /// <summary>
        /// Verifica se é possível mover uma peça para a esquerda
        /// </summary>
        /// <returns>bool</returns>
        private bool canGoLeft()
        {
            if (pX == 0) return false;
            return canGo(pX-1, pY);
        }

        /// <summary>
        /// Verifica se é possível mover uma peça para a direita
        /// </summary>
        /// <returns>bool</returns>
        private bool canGoRight()
        {
            if (pX + piece.Width == 10) return false;
            return canGo(pX + 1, pY);
        }

        private bool canGo(int dX, int dY){
            for (int x = 0; x < piece.Width; x++)
            {
                for (int y = 0; y < piece.Height; y++)
                {
                    if (piece.getBlock(y, x) != 0 && board[dY + y, dX + x] != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Copia uma peça que já não pode mexer para o tabuleiro
        /// </summary>
        private void freeze()
        {
            for (int x = 0; x < piece.Width; x++)
            {
                for (int y = 0; y < piece.Height; y++)
                {
                    if (piece.getBlock(y, x) != 0)
                    {
                        board[pY + y, pX + x] = piece.getBlock(y, x);
                    }
                }
            }
        }

        /// <summary>
        /// Verifica todas as linhas e destroi as linhas que estiverem cheias
        /// </summary>
        private void destruirLinha()
        {
            bool linhaCheia;
            for (int y = 0; y < board.GetLength(0); y++)
            {
                linhaCheia = true;
                for (int x = 0; x < board.GetLength(1); x++)
                {
                    if (board[y, x] == 0)
                    {
                        linhaCheia = false;
                    }
                }
                if (linhaCheia)
                {
                    for (int x = 0; x < board.GetLength(1); x++)
                    {
                        board[y, x] = 0;
                    }
                    //Puxar todas as linhas que estão acima para baixo
                    for (int z = y; z >= 0; z--)
                    {
                        for (int x = 0; x < board.GetLength(1); x++)
                        {
                            if(z+1 < 22) board[z + 1, x] = board[z, x];
                            //board[z, x] = 0;
                        }
                        
                    }
                }
            }
        }
    }

}
