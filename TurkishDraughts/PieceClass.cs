﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurkishDraughts.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing;
using System.Xml.Serialization;
using System.Threading.Tasks.Sources;
using System.CodeDom;

namespace TurkishDraughts
{
    internal class PieceClass
    {
        private int i, j, value;
        public PictureBox pictureBoxButtons;
        protected GameBoard gameBoard;
        protected GameBoardNetwork gameBoardNetwork;

        private void pictureBoxClick(object sender, EventArgs e)
        {
           if(gameBoard!=null)
                gameBoard.pictureBoxClick(sender);
           if(gameBoardNetwork!=null)
                gameBoardNetwork.pictureBoxClick(sender);
        }
        //value=0 spatiu gol
        //value=1 piesa neagra
        //value=2 piesa rosie
        //value=3 rege negru
        //value=4 rege rosu
        public PieceClass(int i, int j, int value, GameBoard gameBoard, GameBoardNetwork gameBoardNetwork)
        {
            this.gameBoard = gameBoard;
            this.gameBoardNetwork = gameBoardNetwork;
            this.i = i;
            this.j = j;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameBoard));
            Image tempRes = ((System.Drawing.Image)(resources.GetObject("EmptySquare"))); //imagine default
            if (i == 1 || i == 2)
            {
                tempRes = Resources.BlackPiece;
                value = 1;
            }
            if (i == 5 || i == 6)
            {
                tempRes = Resources.RedPiece;
                value = 2;
            }
            if (i == 0 || i == 7 || i > 2 && i < 5)
            {
                value = 0;
            }
            this.value = value;
            pictureBoxButtons = new PictureBox
            {
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImage = tempRes,

                BackgroundImageLayout = ImageLayout.Stretch,
                Location = new System.Drawing.Point((29 + j * 79), (83 + i * 77)),
                Name = "pictureBox" + i + j,
                Size = new System.Drawing.Size(68, 68),
                TabIndex = 9,
                TabStop = false,

            };
            pictureBoxButtons.Click += new System.EventHandler(this.pictureBoxClick); //atribuie functia generala de click de piesa locatiei curente
            
        }
        public PictureBox getPictureBox()
        {
            return pictureBoxButtons;
        }
        public int getValue()
        {
            return value;
        }
        public void setValue(int value)
        {
            this.value = value;
        }
    }
}
