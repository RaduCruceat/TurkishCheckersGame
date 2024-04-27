﻿using TurkishDraughts.Properties;

namespace TurkishDraughts
{
    public partial class GameBoard : Form
    {

        PieceClass[][] pictureBoxButtons;
        PlayerClass player1, player2, currentPlayer;
        SpecialProprieties specialProprieties;
        private int i_firstMove, j_firstMove;
        public GameBoard(String player1, String player2)
        {
            MaximizeBox = false;

            initStartState();
            initBoardButtons();//pune pictureboxurile pe tabla
            InitializeComponent();
            initPlayerNames(player1, player2);
            //stare initiala jucator actual
        }

        private void initBoardButtons()
        {
            //atribuim o matrice de clase piesa, fiecare legat de picturebox
            int value = 0;
            pictureBoxButtons = new PieceClass[8][];
            for (int i = 0; i < 8; i++)
            {
                pictureBoxButtons[i] = new PieceClass[8];
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    pictureBoxButtons[i][j] = new PieceClass(i, j, value, this, null, null);
                    Controls.Add(pictureBoxButtons[i][j].getPictureBox());
                }
            }

        }
        private void initPlayerNames(String name1, String name2)
        {
            player1 = new PlayerClass(name1);
            player1TextBox.Text = player1.getName();
            player2 = new PlayerClass(name2);
            player2TextBox.Text = player2.getName();
            currentPlayer = player1;
            currentPlayerTextBox.Text = "Red moves";
            currentPlayerTextBox.ForeColor = Color.Red;
            player2TextBox.BackColor = Color.FromArgb(49, 46, 43);
            player2TextBox.ForeColor = Color.FromArgb(49, 46, 43);

        }
        private void initStartState()
        {
            specialProprieties = new SpecialProprieties(false, false, false, 0, 0, 0, 0);
        }
        private void removeBoardTraces()
        {
            //sterge casutele verzi
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    pictureBoxButtons[i][j].getPictureBox().BackColor = Color.Transparent;
                }
            if (specialProprieties.getMultipleMove())
                pictureBoxButtons[specialProprieties.getCurrentMultipleMoveI()][specialProprieties.getCurrentMultipleMoveJ()].getPictureBox().BackColor = Color.GreenYellow;
        }
        private void blockPictureBox()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    pictureBoxButtons[i][j].getPictureBox().Enabled = false;
                }
            }
            removeBoardTraces();
        }
        private bool checkGameOver(PlayerClass player1, PlayerClass player2)
        {
            //verifica daca nu exista nici o piesa neagra sau rosie pe tabla
            int counterRed = 0, counterBlack = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (pictureBoxButtons[i][j].getValue() % 2 == 0 && pictureBoxButtons[i][j].getValue() != 0)
                        counterRed++;
                    if (pictureBoxButtons[i][j].getValue() % 2 != 0 && pictureBoxButtons[i][j].getValue() != 0)
                        counterBlack++;
                }
            }
            if (counterBlack == 0 || counterRed == 0)
            {
                if (counterBlack == 0)
                {
                    player1TextBox.BackColor = Color.FromArgb(49, 46, 43);
                    player2TextBox.BackColor = Color.FromArgb(49, 46, 43);
                    currentPlayerTextBox.Text = "Game over";
                    currentPlayerTextBox.ForeColor = Color.Blue;
                    MessageBox.Show(player1.getName() + " wins!");
                }
                if (counterRed == 0)
                {
                    player1TextBox.BackColor = Color.FromArgb(49, 46, 43);
                    player2TextBox.BackColor = Color.FromArgb(49, 46, 43);
                    currentPlayerTextBox.Text = "Game over";
                    currentPlayerTextBox.ForeColor = Color.Blue;
                    MessageBox.Show(player2.getName() + " wins!");
                }
                return true;
            }
            return false;
        }
        public void swapCurrentPlayerName()
        {
            if (specialProprieties.getPlayerTurn() == false)
            {
                currentPlayerTextBox.Text = "Red moves";
                currentPlayerTextBox.ForeColor = Color.Red;
                player1TextBox.BackColor = Color.DarkGoldenrod;

                player2TextBox.BackColor = Color.FromArgb(49, 46, 43);
                player2TextBox.ForeColor = Color.FromArgb(49, 46, 43);

            }
            else
            {
                currentPlayerTextBox.Text = "Black moves";
                currentPlayerTextBox.ForeColor = Color.Black;
                player2TextBox.BackColor = Color.DarkGoldenrod;
                player1TextBox.BackColor = Color.FromArgb(49, 46, 43);
                player1TextBox.ForeColor = Color.FromArgb(49, 46, 43);

            }
        }
        public void swapCurrentPlayerTurn(bool turn)
        {
            if (turn == false)
                specialProprieties.setPlayerTurn(true);
            else
                specialProprieties.setPlayerTurn(false);
        }

        public void swapImage(int i_initial, int j_initial, int i_final, int j_final)
        {
            pictureBoxButtons[i_final][j_final].getPictureBox().BackgroundImage = pictureBoxButtons[i_initial][j_initial].getPictureBox().BackgroundImage;
            pictureBoxButtons[i_initial][j_initial].getPictureBox().BackgroundImage = null;
        }
        public void swapValue(int i_initial, int j_initial, int i_final, int j_final)
        {
            pictureBoxButtons[i_final][j_final].setValue(pictureBoxButtons[i_initial][j_initial].getValue());
            pictureBoxButtons[i_initial][j_initial].setValue(0);
        }
        public void resetPictureboxPressed(int i_initial, int j_initial, int i_final, int j_final)
        {
            //se trece la starea initiala daca nu e miscarea legala
            pictureBoxButtons[i_initial][j_initial].getPictureBox().BackColor = Color.Transparent;
            specialProprieties.setPressed(false);
        }
        public void checkInitialMove(int i, int j)
        {
            //retinem pozitia butonului si afisam miscarile posibile lui
            i_firstMove = i;
            j_firstMove = j;
            pictureBoxButtons[i][j].getPictureBox().BackColor = Color.GreenYellow;
            specialProprieties.setPressed(true);
            checkLegalMoves(i, j);
        }
        public void checkFinalMove(int i_initial, int j_initial, int i_final, int j_final)
        {
            //verificam daca locul unde vrem sa mutam e permis, da->muta, nu->reseteaza miscare
            if (specialProprieties.getPressed() == true)
            {

                if (pictureBoxButtons[i_final][j_final].getValue() != 0 ||
                    pictureBoxButtons[i_initial][j_initial].getValue() == 0 ||
                    pictureBoxButtons[i_initial][j_initial].getValue() % 2 != 0 && specialProprieties.getPlayerTurn() == false ||
                    pictureBoxButtons[i_initial][j_initial].getValue() % 2 == 0 && specialProprieties.getPlayerTurn() == true ||
                    pictureBoxButtons[i_final][j_final].getPictureBox().BackColor != Color.GreenYellow ||
                    (specialProprieties.getMultipleMove() == true &&
                    (specialProprieties.getCurrentMultipleMoveI() != i_initial || specialProprieties.getCurrentMultipleMoveJ() != j_initial))
                    )
                {
                    resetPictureboxPressed(i_initial, j_initial, i_final, j_final);
                    removeBoardTraces();
                }
                else
                {
                    movePiece(i_initial, j_initial, i_final, j_final);
                }
                removeBoardTraces();
            }
            if (checkGameOver(player1, player2))
            {
                blockPictureBox();
                removeBoardTraces();
            }
        }
        public void checkLegalMoves(int i, int j)
        {
            if (pictureBoxButtons[i][j].getValue() != 0)
            {
                drawLegalMovesTraces(i, j);
            }
        }
        public bool checkMultipleMovesBlackPiece(int i_intial, int j_initial, int i, int j)
        {
            if (i < 6 && pictureBoxButtons[i + 2][j].getValue() == 0 && pictureBoxButtons[i + 1][j].getValue() % 2 == 0 && pictureBoxButtons[i + 1][j].getValue() != 0)
                return true;
            if (j < 6 && pictureBoxButtons[i][j + 2].getValue() == 0 && pictureBoxButtons[i][j + 1].getValue() % 2 == 0 && pictureBoxButtons[i][j + 1].getValue() != 0)
                return true;
            if (j > 1 && pictureBoxButtons[i][j - 2].getValue() == 0 && pictureBoxButtons[i][j - 1].getValue() % 2 == 0 && pictureBoxButtons[i][j - 1].getValue() != 0)
                return true;
            return false;
        }
        public bool checkMultipleMovesRedPiece(int i_intial, int j_initial, int i, int j)
        {
            if (i > 1 && pictureBoxButtons[i - 2][j].getValue() == 0 && pictureBoxButtons[i - 1][j].getValue() % 2 != 0)
                return true;
            if (j > 1 && pictureBoxButtons[i][j - 2].getValue() == 0 && pictureBoxButtons[i][j - 1].getValue() % 2 != 0)
                return true;
            if (j < 6 && pictureBoxButtons[i][j + 2].getValue() == 0 && pictureBoxButtons[i][j + 1].getValue() % 2 != 0)
                return true;
            return false;
        }
        public bool checkMultipleMovesRedKingLeft(int i_intial, int j_initial, int i, int j)
        {
            int i_search = i, j_search = j;
            bool redPieceInBetween = false;
            bool doubleBlackPiece = false;
            while (j_search > 1 && !redPieceInBetween && !doubleBlackPiece)
            {
                j_search--;
                if (pictureBoxButtons[i][j_search].getValue() % 2 != 0 &&
                 pictureBoxButtons[i][j_search].getValue() != 0 &&
                 pictureBoxButtons[i][j_search - 1].getValue() % 2 != 0 &&
                 pictureBoxButtons[i][j_search - 1].getValue() != 0)
                    doubleBlackPiece = true;
                if (pictureBoxButtons[i][j_search].getValue() % 2 == 0 && pictureBoxButtons[i][j_search].getValue() != 0)
                    redPieceInBetween = true;
                if (pictureBoxButtons[i][j_search].getValue() % 2 != 0 && pictureBoxButtons[i][j_search].getValue() != 0 && pictureBoxButtons[i][j_search - 1].getValue() == 0)
                    return true;
            }
            return false;
        }
        public bool checkMultipleMovesRedKingRight(int i_intial, int j_initial, int i, int j)
        {
            int i_search = i, j_search = j;
            bool redPieceInBetween = false;
            bool doubleBlackPiece = false;
            while (j_search < 6 && !redPieceInBetween && !doubleBlackPiece)
            {
                j_search++;
                if (pictureBoxButtons[i][j_search].getValue() % 2 != 0 &&
                 pictureBoxButtons[i][j_search].getValue() != 0 &&
                 pictureBoxButtons[i][j_search + 1].getValue() % 2 != 0 &&
                 pictureBoxButtons[i][j_search + 1].getValue() != 0)
                    doubleBlackPiece = true;
                if (pictureBoxButtons[i][j_search].getValue() % 2 == 0 && pictureBoxButtons[i][j_search].getValue() != 0)
                    redPieceInBetween = true;
                if (pictureBoxButtons[i][j_search].getValue() % 2 != 0 && pictureBoxButtons[i][j_search].getValue() != 0 && pictureBoxButtons[i][j_search + 1].getValue() == 0)
                    return true;
            }
            return false;
        }
        public bool checkMultipleMovesRedKingUp(int i_intial, int j_initial, int i, int j)
        {
            int i_search = i, j_search = j;
            bool redPieceInBetween = false;
            bool doubleBlackPiece = false;
            while (i_search > 1 && !redPieceInBetween && !doubleBlackPiece)
            {
                i_search--;
                if (pictureBoxButtons[i_search][j].getValue() % 2 != 0 &&
                  pictureBoxButtons[i_search][j].getValue() != 0 &&
                  pictureBoxButtons[i_search - 1][j].getValue() % 2 != 0 &&
                  pictureBoxButtons[i_search - 1][j].getValue() != 0)
                    doubleBlackPiece = true;
                if (pictureBoxButtons[i_search][j].getValue() % 2 == 0 && pictureBoxButtons[i_search][j].getValue() != 0)
                    redPieceInBetween = true;
                if (pictureBoxButtons[i_search][j].getValue() % 2 != 0 && pictureBoxButtons[i_search][j].getValue() != 0 && pictureBoxButtons[i_search - 1][j].getValue() == 0)
                    return true;
            }

            return false;
        }
        public bool checkMultipleMovesRedKingDown(int i_intial, int j_initial, int i, int j)
        {
            int i_search = i, j_search = j;
            bool redPieceInBetween = false;
            bool doubleBlackPiece = false;
            while (i_search < 6 && !redPieceInBetween && !doubleBlackPiece)
            {
                i_search++;
                if (pictureBoxButtons[i_search][j].getValue() % 2 != 0 &&
                   pictureBoxButtons[i_search][j].getValue() != 0 &&
                   pictureBoxButtons[i_search + 1][j].getValue() % 2 != 0 &&
                   pictureBoxButtons[i_search + 1][j].getValue() != 0)
                    doubleBlackPiece = true;
                if (pictureBoxButtons[i_search][j].getValue() % 2 == 0 && pictureBoxButtons[i_search][j].getValue() != 0)
                    redPieceInBetween = true;
                if (pictureBoxButtons[i_search][j].getValue() % 2 != 0 && pictureBoxButtons[i_search][j].getValue() != 0 && pictureBoxButtons[i_search + 1][j].getValue() == 0)
                    return true;
            }

            return false;
        }
        public bool checkMultipleMovesBlackKingLeft(int i_intial, int j_initial, int i, int j)
        {
            int i_search = i, j_search = j;
            bool blackPieceInBetween = false;
            bool doubleRedPiece = false;
            while (j_search > 1 && !blackPieceInBetween && !doubleRedPiece)
            {
                j_search--;
                if (pictureBoxButtons[i][j_search].getValue() % 2 == 0 &&
                  pictureBoxButtons[i][j_search].getValue() != 0 &&
                  pictureBoxButtons[i][j_search - 1].getValue() % 2 == 0 &&
                  pictureBoxButtons[i][j_search - 1].getValue() != 0)
                    doubleRedPiece = true;
                if (pictureBoxButtons[i][j_search].getValue() % 2 != 0 && pictureBoxButtons[i][j_search].getValue() != 0)
                    blackPieceInBetween = true;
                if (pictureBoxButtons[i][j_search].getValue() % 2 == 0 && pictureBoxButtons[i][j_search].getValue() != 0 && pictureBoxButtons[i][j_search - 1].getValue() == 0)
                    return true;
            }
            return false;
        }
        public bool checkMultipleMovesBlackKingRight(int i_intial, int j_initial, int i, int j)
        {
            int i_search = i, j_search = j;
            bool blackPieceInBetween = false;
            bool doubleRedPiece = false;
            while (j_search < 6 && !blackPieceInBetween && !doubleRedPiece)
            {
                j_search++;
                if (pictureBoxButtons[i][j_search].getValue() % 2 == 0 &&
                  pictureBoxButtons[i][j_search].getValue() != 0 &&
                  pictureBoxButtons[i][j_search + 1].getValue() % 2 == 0 &&
                  pictureBoxButtons[i][j_search + 1].getValue() != 0)
                    doubleRedPiece = true;

                if (pictureBoxButtons[i][j_search].getValue() % 2 != 0 && pictureBoxButtons[i][j_search].getValue() != 0)
                    blackPieceInBetween = true;
                if (pictureBoxButtons[i][j_search].getValue() % 2 == 0 && pictureBoxButtons[i][j_search].getValue() != 0 && pictureBoxButtons[i][j_search + 1].getValue() == 0)
                    return true;
            }
            return false;
        }
        public bool checkMultipleMovesBlackKingUp(int i_intial, int j_initial, int i, int j)
        {
            int i_search = i, j_search = j;
            bool blackPieceInBetween = false;
            bool doubleRedPiece = false;
            while (i_search > 1 && !blackPieceInBetween && !doubleRedPiece)
            {
                i_search--;
                if (pictureBoxButtons[i_search][j].getValue() % 2 == 0 &&
                   pictureBoxButtons[i_search][j].getValue() != 0 &&
                   pictureBoxButtons[i_search - 1][j].getValue() % 2 == 0 &&
                   pictureBoxButtons[i_search - 1][j].getValue() != 0)
                    doubleRedPiece = true;

                if (pictureBoxButtons[i_search][j].getValue() % 2 != 0 && pictureBoxButtons[i_search][j].getValue() != 0)
                    blackPieceInBetween = true;
                if (pictureBoxButtons[i_search][j].getValue() % 2 == 0 && pictureBoxButtons[i_search][j].getValue() != 0 && pictureBoxButtons[i_search - 1][j].getValue() == 0)
                    return true;
            }
            return false;

        }
        public bool checkMultipleMovesBlackKingDown(int i_intial, int j_initial, int i, int j)
        {
            int i_search = i, j_search = j;
            bool blackPieceInBetween = false;
            bool doubleRedPiece = false;
            while (i_search < 6 && !blackPieceInBetween && !doubleRedPiece)
            {
                i_search++;
                if (pictureBoxButtons[i_search][j].getValue() % 2 == 0 &&
                    pictureBoxButtons[i_search][j].getValue() != 0 &&
                    pictureBoxButtons[i_search + 1][j].getValue() % 2 == 0 &&
                    pictureBoxButtons[i_search + 1][j].getValue() != 0)
                    doubleRedPiece = true;

                if (pictureBoxButtons[i_search][j].getValue() % 2 != 0 && pictureBoxButtons[i_search][j].getValue() != 0)
                    blackPieceInBetween = true;
                if (pictureBoxButtons[i_search][j].getValue() % 2 == 0 && pictureBoxButtons[i_search][j].getValue() != 0 && pictureBoxButtons[i_search + 1][j].getValue() == 0)
                    return true;
            }
            return false;
        }

        public bool checkMultipleMoves(int i_initial, int j_initial, int i, int j)
        {
            //piesa rosie
            if (pictureBoxButtons[i][j].getValue() == 2)
            {
                if (checkMultipleMovesRedPiece(i_initial, j_initial, i, j))
                    return true;
            }
            //piesa neagra
            if (pictureBoxButtons[i][j].getValue() == 1)
            {
                if (checkMultipleMovesBlackPiece(i_initial, j_initial, i, j))
                    return true;
            }
            //rege rosu
            if (pictureBoxButtons[i][j].getValue() == 4)
            {
                //nu permitem ca regele sa faca o miscare multipla la 180 de grade, zona din care a venit e falsa
                bool i_up = false;
                bool i_down = false;
                bool j_right = false;
                bool j_left = false;
                if (i != i_initial)
                    if (i - i_initial > 0)
                        i_up = true;
                    else
                        i_down = true;

                if (j != j_initial)
                    if (j - j_initial > 0)
                        j_left = true;
                    else
                        j_right = true;

                //verifica miscari legale in toate directiile cu exceptia locului de unde a venit
                if (!j_left)
                    if (checkMultipleMovesRedKingLeft(i_initial, j_initial, i, j))
                        return true;
                if (!j_right)
                    if (checkMultipleMovesRedKingRight(i_initial, j_initial, i, j))
                        return true;
                if (!i_up)
                    if (checkMultipleMovesRedKingUp(i_initial, j_initial, i, j))
                        return true;
                if (!i_down)
                    if (checkMultipleMovesRedKingDown(i_initial, j_initial, i, j))
                        return true;

            }
            //rege negru
            if (pictureBoxButtons[i][j].getValue() == 3)
            {
                //nu permitem ca regele sa faca o miscare multipla la 180 de grade, zona din care a venit e falsa
                bool i_up = false;
                bool i_down = false;
                bool j_right = false;
                bool j_left = false;
                if (i != i_initial)
                    if (i - i_initial > 0)
                        i_up = true;
                    else
                        i_down = true;

                if (j != j_initial)
                    if (j - j_initial > 0)
                        j_left = true;
                    else
                        j_right = true;

                if (!j_left)
                    if (checkMultipleMovesBlackKingLeft(i_initial, j_initial, i, j))
                        return true;

                if (!j_right)
                    if (checkMultipleMovesBlackKingRight(i_initial, j_initial, i, j))
                        return true;
                if (!i_up)
                    if (checkMultipleMovesBlackKingUp(i_initial, j_initial, i, j))
                        return true;
                if (!i_down)
                    if (checkMultipleMovesBlackKingDown(i_initial, j_initial, i, j))
                        return true;
            }
            return false;
        }
        public void drawRedPieceTrace(int i, int j)
        {
            //spatiu gol
            if (specialProprieties.getMultipleMove() == false)
            {
                if (i > 0 && pictureBoxButtons[i - 1][j].getValue() == 0)
                    pictureBoxButtons[i - 1][j].getPictureBox().BackColor = Color.GreenYellow;
                if (j > 0 && pictureBoxButtons[i][j - 1].getValue() == 0)
                    pictureBoxButtons[i][j - 1].getPictureBox().BackColor = Color.GreenYellow;
                if (j < 7 && pictureBoxButtons[i][j + 1].getValue() == 0)
                    pictureBoxButtons[i][j + 1].getPictureBox().BackColor = Color.GreenYellow;
            }
            //piesa langa
            if (i > 1 && pictureBoxButtons[i - 2][j].getValue() == 0 && pictureBoxButtons[i - 1][j].getValue() % 2 != 0)
                pictureBoxButtons[i - 2][j].getPictureBox().BackColor = Color.GreenYellow;
            if (j > 1 && pictureBoxButtons[i][j - 2].getValue() == 0 && pictureBoxButtons[i][j - 1].getValue() % 2 != 0)
                pictureBoxButtons[i][j - 2].getPictureBox().BackColor = Color.GreenYellow;
            if (j < 6 && pictureBoxButtons[i][j + 2].getValue() == 0 && pictureBoxButtons[i][j + 1].getValue() % 2 != 0)
                pictureBoxButtons[i][j + 2].getPictureBox().BackColor = Color.GreenYellow;

            if ((specialProprieties.getCurrentMultipleMoveI() != i ||
           specialProprieties.getCurrentMultipleMoveJ() != j) &&
           specialProprieties.getMultipleMove())
                removeBoardTraces();
        }
        public void drawBlackPieceTrace(int i, int j)
        {
            //spatiu gol
            if (specialProprieties.getMultipleMove() == false)
            {
                if (i < 7 && pictureBoxButtons[i + 1][j].getValue() == 0)
                    pictureBoxButtons[i + 1][j].getPictureBox().BackColor = Color.GreenYellow;
                if (j < 7 && pictureBoxButtons[i][j + 1].getValue() == 0)
                    pictureBoxButtons[i][j + 1].getPictureBox().BackColor = Color.GreenYellow;
                if (j > 0 && pictureBoxButtons[i][j - 1].getValue() == 0)
                    pictureBoxButtons[i][j - 1].getPictureBox().BackColor = Color.GreenYellow;
            }
            //piesa langa
            if (i < 6 && pictureBoxButtons[i + 2][j].getValue() == 0 && pictureBoxButtons[i + 1][j].getValue() % 2 == 0 && pictureBoxButtons[i + 1][j].getValue() != 0)
                pictureBoxButtons[i + 2][j].getPictureBox().BackColor = Color.GreenYellow;
            if (j < 6 && pictureBoxButtons[i][j + 2].getValue() == 0 && pictureBoxButtons[i][j + 1].getValue() % 2 == 0 && pictureBoxButtons[i][j + 1].getValue() != 0)
                pictureBoxButtons[i][j + 2].getPictureBox().BackColor = Color.GreenYellow;
            if (j > 1 && pictureBoxButtons[i][j - 2].getValue() == 0 && pictureBoxButtons[i][j - 1].getValue() % 2 == 0 && pictureBoxButtons[i][j - 1].getValue() != 0)
                pictureBoxButtons[i][j - 2].getPictureBox().BackColor = Color.GreenYellow;

            if ((specialProprieties.getCurrentMultipleMoveI() != i ||
            specialProprieties.getCurrentMultipleMoveJ() != j) &&
            specialProprieties.getMultipleMove())
                removeBoardTraces();
        }
        public void drawRedKingLeftTrace(int i, int j)
        {
            int i_search = i, j_search = j, contor = 0;
            while (j_search > 0 && contor < 2)
            {
                j_search--;
                if (pictureBoxButtons[i][j_search].getValue() % 2 != 0 && pictureBoxButtons[i][j_search].getValue() != 0)
                    contor++;
                if (pictureBoxButtons[i][j_search].getValue() % 2 == 0 && pictureBoxButtons[i][j_search].getValue() != 0)
                    contor = 2;
                if (pictureBoxButtons[i][j_search].getValue() == 0)
                    pictureBoxButtons[i][j_search].getPictureBox().BackColor = Color.GreenYellow;
            }
        }
        public void drawRedKingRightTrace(int i, int j)
        {
            int i_search = i, j_search = j, contor = 0;
            while (j_search < 7 && contor < 2)
            {
                j_search++;
                if (pictureBoxButtons[i][j_search].getValue() % 2 != 0 && pictureBoxButtons[i][j_search].getValue() != 0)
                    contor++;
                if (pictureBoxButtons[i][j_search].getValue() % 2 == 0 && pictureBoxButtons[i][j_search].getValue() != 0)
                    contor = 2;
                if (pictureBoxButtons[i][j_search].getValue() == 0)
                    pictureBoxButtons[i][j_search].getPictureBox().BackColor = Color.GreenYellow;
            }
        }
        public void drawRedKingUpTrace(int i, int j)
        {
            int i_search = i, j_search = j, contor = 0;
            while (i_search > 0 && contor < 2)
            {
                i_search--;
                if (pictureBoxButtons[i_search][j].getValue() % 2 != 0 && pictureBoxButtons[i_search][j].getValue() != 0)
                    contor++;
                if (pictureBoxButtons[i_search][j].getValue() % 2 == 0 && pictureBoxButtons[i_search][j].getValue() != 0)
                    contor = 2;
                if (pictureBoxButtons[i_search][j].getValue() == 0)
                    pictureBoxButtons[i_search][j].getPictureBox().BackColor = Color.GreenYellow;
            }
        }
        public void drawRedKingDownTrace(int i, int j)
        {
            int i_search = i, j_search = j, contor = 0;
            while (i_search < 7 && contor < 2)
            {
                i_search++;
                if (pictureBoxButtons[i_search][j].getValue() % 2 != 0 && pictureBoxButtons[i_search][j].getValue() != 0)
                    contor++;
                if (pictureBoxButtons[i_search][j].getValue() % 2 == 0 && pictureBoxButtons[i_search][j].getValue() != 0)
                    contor = 2;
                if (pictureBoxButtons[i_search][j].getValue() == 0)
                    pictureBoxButtons[i_search][j].getPictureBox().BackColor = Color.GreenYellow;
            }
        }
        public void drawBlackKingLeftTrace(int i, int j)
        {
            int i_search = i, j_search = j, contor = 0;
            while (j_search > 0 && contor < 2)
            {
                j_search--;
                if (pictureBoxButtons[i][j_search].getValue() % 2 == 0 && pictureBoxButtons[i][j_search].getValue() != 0)
                    contor++;
                if (pictureBoxButtons[i][j_search].getValue() % 2 != 0 && pictureBoxButtons[i][j_search].getValue() != 0)
                    contor = 2;
                if (pictureBoxButtons[i][j_search].getValue() == 0)
                    pictureBoxButtons[i][j_search].getPictureBox().BackColor = Color.GreenYellow;
            }
        }
        public void drawBlackKingRightTrace(int i, int j)
        {
            int i_search = i, j_search = j, contor = 0;
            while (j_search < 7 && contor < 2)
            {
                j_search++;
                if (pictureBoxButtons[i][j_search].getValue() % 2 == 0 && pictureBoxButtons[i][j_search].getValue() != 0)
                    contor++;
                if (pictureBoxButtons[i][j_search].getValue() % 2 != 0 && pictureBoxButtons[i][j_search].getValue() != 0)
                    contor = 2;
                if (pictureBoxButtons[i][j_search].getValue() == 0)
                    pictureBoxButtons[i][j_search].getPictureBox().BackColor = Color.GreenYellow;
            }
        }
        public void drawBlackKingUpTrace(int i, int j)
        {
            int i_search = i, j_search = j, contor = 0;
            while (i_search > 0 && contor < 2)
            {
                i_search--;
                if (pictureBoxButtons[i_search][j].getValue() % 2 == 0 && pictureBoxButtons[i_search][j].getValue() != 0)
                    contor++;
                if (pictureBoxButtons[i_search][j].getValue() % 2 != 0 && pictureBoxButtons[i_search][j].getValue() != 0)
                    contor = 2;
                if (pictureBoxButtons[i_search][j].getValue() == 0)
                    pictureBoxButtons[i_search][j].getPictureBox().BackColor = Color.GreenYellow;

            }
        }
        public void drawBlackKingDownTrace(int i, int j)
        {
            int i_search = i, j_search = j, contor = 0;
            while (i_search < 7 && contor < 2)
            {
                i_search++;
                if (pictureBoxButtons[i_search][j].getValue() % 2 == 0 && pictureBoxButtons[i_search][j].getValue() != 0)
                    contor++;
                if (pictureBoxButtons[i_search][j].getValue() % 2 != 0 && pictureBoxButtons[i_search][j].getValue() != 0)
                    contor = 2;
                if (pictureBoxButtons[i_search][j].getValue() == 0)
                    pictureBoxButtons[i_search][j].getPictureBox().BackColor = Color.GreenYellow;
            }
        }
        public void drawLegalMovesTraces(int i, int j)
        {
            //piese rosii
            if (pictureBoxButtons[i][j].getValue() % 2 == 0)
            {
                drawRedPieceTrace(i, j);

                if (pictureBoxButtons[i][j].getValue() == 4)
                {
                    if (specialProprieties.getMultipleMove() == false)
                    {
                        drawRedKingLeftTrace(i, j);
                        drawRedKingRightTrace(i, j);
                        drawRedKingUpTrace(i, j);
                        drawRedKingDownTrace(i, j);
                    }
                    else
                    {
                        int i_initial = specialProprieties.getLastMultipleMoveI();
                        int j_initial = specialProprieties.getLastMultipleMoveJ();
                        bool i_up = false;
                        bool i_down = false;
                        bool j_right = false;
                        bool j_left = false;
                        if (i != i_initial)
                            if (i - i_initial > 0)
                                i_up = true;
                            else
                                i_down = true;

                        if (j != j_initial)
                            if (j - j_initial > 0)
                                j_left = true;
                            else
                                j_right = true;

                        if (!j_left)
                            if (checkMultipleMovesRedKingLeft(i_initial, j_initial, i, j))
                                drawRedKingLeftTrace(i, j);
                        if (!j_right)
                            if (checkMultipleMovesRedKingRight(i_initial, j_initial, i, j))
                                drawRedKingRightTrace(i, j);
                        if (!i_up)
                            if (checkMultipleMovesRedKingUp(i_initial, j_initial, i, j))
                                drawRedKingUpTrace(i, j);
                        if (!i_down)
                            if (checkMultipleMovesRedKingDown(i_initial, j_initial, i, j))
                                drawRedKingDownTrace(i, j);

                        if ((specialProprieties.getCurrentMultipleMoveI() != i ||
                        specialProprieties.getCurrentMultipleMoveJ() != j) &&
                        specialProprieties.getMultipleMove())
                            removeBoardTraces();
                    }
                }
            }
            //piese negre
            if (pictureBoxButtons[i][j].getValue() % 2 != 0)
            {
                drawBlackPieceTrace(i, j);
                if (pictureBoxButtons[i][j].getValue() == 3)
                    if (specialProprieties.getMultipleMove() == false)
                    {
                        drawBlackKingLeftTrace(i, j);
                        drawBlackKingRightTrace(i, j);
                        drawBlackKingUpTrace(i, j);
                        drawBlackKingDownTrace(i, j);
                    }
                    else
                    {
                        int i_initial = specialProprieties.getLastMultipleMoveI();
                        int j_initial = specialProprieties.getLastMultipleMoveJ();
                        bool i_up = false;
                        bool i_down = false;
                        bool j_right = false;
                        bool j_left = false;
                        if (i != i_initial)
                            if (i - i_initial > 0)
                                i_up = true;
                            else
                                i_down = true;

                        if (j != j_initial)
                            if (j - j_initial > 0)
                                j_left = true;
                            else
                                j_right = true;

                        if (!j_left)
                            if (checkMultipleMovesBlackKingLeft(i_initial, j_initial, i, j))
                                drawBlackKingLeftTrace(i, j);
                        if (!j_right)
                            if (checkMultipleMovesBlackKingRight(i_initial, j_initial, i, j))
                                drawBlackKingRightTrace(i, j);
                        if (!i_up)
                            if (checkMultipleMovesBlackKingUp(i_initial, j_initial, i, j))
                                drawBlackKingUpTrace(i, j);
                        if (!i_down)
                            if (checkMultipleMovesBlackKingDown(i_initial, j_initial, i, j))
                                drawBlackKingDownTrace(i, j);

                        if ((specialProprieties.getCurrentMultipleMoveI() != i ||
                        specialProprieties.getCurrentMultipleMoveJ() != j) &&
                        specialProprieties.getMultipleMove())
                            removeBoardTraces();
                    }
            }
        }
        public bool removeCapturedPieces(int i_initial, int j_initial, int i_final, int j_final)
        {
            //vector intre pozitia de unde a plecat -> unde a ajuns o piesa, sterge tot intre
            if (j_initial > j_final)
            {
                int j_temp = j_final; j_final = j_initial; j_initial = j_temp;
            }
            if (i_initial > i_final)
            {
                int i_temp = i_final; i_final = i_initial; i_initial = i_temp;
            }

            if (j_initial == j_final && i_initial != i_final + 1)
                for (int i = i_initial + 1; i < i_final; i++)
                    if (pictureBoxButtons[i][j_final].getValue() != 0)
                    {
                        pictureBoxButtons[i][j_final].setValue(0);
                        pictureBoxButtons[i][j_final].getPictureBox().BackgroundImage = null;
                        return true;
                    }
            if (i_initial == i_final && j_initial != j_final + 1)
                for (int j = j_initial + 1; j < j_final; j++)
                    if (pictureBoxButtons[i_initial][j].getValue() != 0)
                    {
                        pictureBoxButtons[i_initial][j].setValue(0);
                        pictureBoxButtons[i_initial][j].getPictureBox().BackgroundImage = null;
                        return true;
                    }
            return false;
        }
        public void movePiece(int i_initial, int j_initial, int i_final, int j_final)
        {
            pictureBoxButtons[i_initial][j_initial].getPictureBox().BackColor = Color.Transparent;
            specialProprieties.setPressed(false);
            swapImage(i_initial, j_initial, i_final, j_final);
            swapValue(i_initial, j_initial, i_final, j_final);
            if (removeCapturedPieces(i_initial, j_initial, i_final, j_final))
            {
                removeCapturedPieces(i_initial, j_initial, i_final, j_final);
                if (checkMultipleMoves(i_initial, j_initial, i_final, j_final) == false)
                {
                    checkIfPieceIsKing(i_final, j_final);
                    swapCurrentPlayerTurn(specialProprieties.getPlayerTurn());
                    swapCurrentPlayerName();
                    specialProprieties.setMultipleMoves(false);
                }
                else
                {
                    specialProprieties.setMultipleMoves(true);
                    specialProprieties.setLastMultipleMoveI(i_initial);
                    specialProprieties.setLastMultipleMoveJ(j_initial);
                    specialProprieties.setCurrentMultipleMoveI(i_final);
                    specialProprieties.setCurrentMultipleMoveJ(j_final);
                    //daca piesa ajunge la final si inca mai poate sari peste alta piesa, sare pestea ea apoi devine rege
                }
            }
            else
            {
                checkIfPieceIsKing(i_final, j_final);
                swapCurrentPlayerTurn(specialProprieties.getPlayerTurn());
                swapCurrentPlayerName();
            }
        }
        public bool checkIfPieceIsKing(int i, int j)
        {
            if (pictureBoxButtons[i][j].getValue() == 1 && i == 7)
            {
                pictureBoxButtons[i][j].setValue(3);
                pictureBoxButtons[i][j].getPictureBox().BackgroundImage = Resources.BlackKing;
                return true;
            }
            if (pictureBoxButtons[i][j].getValue() == 2 && i == 0)
            {
                pictureBoxButtons[i][j].setValue(4);
                pictureBoxButtons[i][j].getPictureBox().BackgroundImage = Resources.RedKing;
                return true;
            }
            return false;
        }

        public void pictureBoxClick(object sender)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (sender == pictureBoxButtons[i][j].getPictureBox())
                    {
                        if (specialProprieties.getPressed() == false)
                            checkInitialMove(i, j);
                        else
                        {
                            checkFinalMove(i_firstMove, j_firstMove, i, j);
                        }
                    }
                }
            }

        }
        private void player1TextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }

}

