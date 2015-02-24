using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    public class Piece
    {
        private Random random;

        /// <summary>
        /// Peças do jogo
        /// </summary>
        private byte[][,] models = { new byte[,] { { 0, 1, 0 }, 
                                                   { 1, 1, 1 } },

                                     new byte[,] { { 2, 2, 2 }, 
                                                   { 2, 0, 0 } },

                                     new byte[,] { { 3, 3 }, 
                                                   { 3, 3 } },

                                     new byte[,] { { 4, 4, 4 }, 
                                                   { 0, 0, 4 } },

                                     new byte[,] { { 0, 5, 5 }, 
                                                   { 5, 5, 0 } },

                                     new byte[,] { { 6, 6, 0 }, 
                                                   { 0, 6, 6 } },

                                     new byte[,] { { 7, 7, 7, 7 } }
                                   };
        
        private int selectedPiece;

        private int width;

        /// <summary>
        /// Largura da peça
        /// </summary>
        public int Width
        {
            get { return width; }
        }

        private int height;

        /// <summary>
        /// Altura da peça
        /// </summary>
        public int Height
        {
            get { return height; }
        }

        byte[,] instance;
        
        /// <summary>
        /// Construtor de peça
        /// </summary>
        /// <param name="peca">Matriz da peça</param>
        public Piece(){
            random = new Random();
            selectedPiece = random.Next(models.Count());
            instance = models[selectedPiece];
            this.width = instance.GetLength(1);
            this.height = instance.GetLength(0);
        }

        /// <summary>
        /// Devolve informação sobre o interior da peça
        /// </summary>
        /// <param name="y">Coordenada Y</param>
        /// <param name="x">Coordenada X</param>
        /// <returns>0 ou 1</returns>
        public byte getBlock(int y, int x)
        {
            return instance[y, x];
        }

        public void rotate()
        {
            byte[,] rotated = new byte[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    rotated[x, height - y - 1] = instance[y, x];
                }
            }
            instance = rotated;
            width = instance.GetLength(1);
            height = instance.GetLength(0);
        }
    }

}
