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
                                     new byte[,] { { 1, 1, 1 }, 
                                                   { 1, 0, 0 } },
                                     new byte[,] { { 1, 1 }, 
                                                   { 1, 1 } },
                                     new byte[,] { { 1, 1, 1 }, 
                                                   { 0, 0, 1 } },
                                     new byte[,] { { 0, 1, 1 }, 
                                                   { 1, 1, 0 } },
                                     new byte[,] { { 1, 1, 0 }, 
                                                   { 0, 1, 1 } },
                                     new byte[,] { { 1, 1, 1, 1 } }
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
        
        /// <summary>
        /// Construtor de peça
        /// </summary>
        /// <param name="peca">Matriz da peça</param>
        public Piece(){
            random = new Random();
            selectedPiece = random.Next(models.Count());
            this.width = models[selectedPiece].GetLength(1);
            this.height = models[selectedPiece].GetLength(0);
        }

        /// <summary>
        /// Devolve informação sobre o interior da peça
        /// </summary>
        /// <param name="y">Coordenada Y</param>
        /// <param name="x">Coordenada X</param>
        /// <returns>0 ou 1</returns>
        public byte getBlock(int y, int x)
        {
            return models[selectedPiece][y, x];
        }
    }

}
