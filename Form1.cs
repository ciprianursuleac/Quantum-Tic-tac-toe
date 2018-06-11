using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Tic_tac_toe
{
    public partial class Form1 : Form
    {
        //unsafe public struct CNod { public CNod st,dr,sus, jos;public int X; };
        public class CNod { public CNod st, dr; public int X; }
        CNod Cprim, Cultim;

        unsafe void Initializare_lista_inlantuitaC()
        {
            int i = 1;
            CNod parc = new CNod();
            Cprim = parc;
            Cprim.X = 0;
            while (i != 9)
            { CNod c = new CNod();
                i++;

                parc.dr = c; c.st = parc;
                parc.X = 0;
                parc = c;
            }
            Cultim = parc;
        }

        public Form1()
        {
            InitializeComponent();
        }

        int Cturn = 0, Mturn = 0, Qturn = 0;
        bool qtc = false; //quantum turn count, fiecare jucator are 2 mutari
        int qpc = 0; //quantum pannel check
        int nodQ1, nodQ2;
        bool Click_for_colapse = false;
        int[,] MA = new int[10, 10];
        int[] here_the_cycle = new int[10];
        int[] arbore = new int[10];
        int[] stiva = new int[10];
        int min_lenght_of_cycle = 11;

        private void Reinitialization()
        {
            panel1.Visible = false;
            panel11.Visible = false;
            panelQ.Visible = false;

            Cturn = 0;
            Mturn = 0;
            Qturn = 0;
            qtc = false;
            qpc = 0; //quantum pannel check
            nodQ1 = 0; nodQ2=0;
            Click_for_colapse = false;
            min_lenght_of_cycle = 11;

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    MA[i, j] = 0;
                    here_the_cycle[i] = 0;
                    arbore[i] = 0;
                }

            Initializare_lista_inlantuitaC();
            foreach (Control c in panel1.Controls)
                if (c.GetType() == typeof(Button))
                {
                    Button b = (Button)c;
                    c.Enabled = true;
                    c.Text = "";
                }
            foreach (Control c in panel11.Controls)
                if (c.GetType() == typeof(Button))
                {
                    Button b = (Button)c;
                    c.Enabled = true;
                    c.Text = "";
                }
            foreach (Control v in panelQ.Controls)
                if(v is Panel)
                foreach(Control c in v.Controls)
                if (c.GetType() == typeof(Button))
                {
                    Button b = (Button)c;
                    c.Enabled = true;
                    c.Text = "";
                    b.ForeColor = Color.Black;
                    b.Font = new Font(b.Font,FontStyle.Regular);
                }
        }

        private void classicToolStripMenuItem_Click(object sender, EventArgs e)
        {   //aici se va face o reinitializare a panoului
            Reinitialization();
            panel1.Visible = true;

        }

        private void quantumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reinitialization();
            panelQ.Visible = true;
        }

        private void misèreToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Reinitialization();
            panel11.Visible = true; //of of of
        }

        unsafe private void Cbutton_click(object sender, EventArgs e)
        {
            Button B = (Button)sender;
            if (Cturn % 2 == 0) B.Text = "X";
            else B.Text = "0";
            Cturn = Cturn + 1;
            B.Enabled = false;

            //modificam lista inlantuita
            int possition_in_grid = (int)(B.Name[2]) - 48;

            CNod parcurgere;
            parcurgere = Cprim;
            int ind = 1;

            while (ind != possition_in_grid)
            { parcurgere = parcurgere.dr; ind++; }
            parcurgere.X = (Cturn-1) % 2 +1; //1 pt X si 2 pt 0

            CheckForCWinner();


        }

        unsafe private void CheckForCWinner()
        {
            bool whw = false;

            if ((Cprim.X != 0 && Cprim.X == Cprim.dr.X && Cprim.X == Cprim.dr.dr.X)
             || (Cprim.X !=0 && Cprim.dr.dr.dr.X == Cprim.X && Cprim.X == Cprim.dr.dr.dr.dr.dr.dr.X )
             || (Cprim.X != 0 && Cprim.dr.dr.dr.dr.X == Cprim.X && Cprim.X==Cultim.X )
             )
            { if (Cprim.X == 1) MessageBox.Show("X wins!");
                else MessageBox.Show("0 wins!");
                whw = true;

            }

            if((Cprim.dr.dr.dr.X != 0 && Cprim.dr.dr.dr.X == Cprim.dr.dr.dr.dr.X &&
             Cprim.dr.dr.dr.X == Cprim.dr.dr.dr.dr.dr.X)
             ||(Cprim.dr.dr.dr.dr.X != 0 && Cprim.dr.dr.dr.dr.X == Cprim.dr.X && Cprim.dr.X == Cultim.st.X))
            {
                if (Cprim.dr.dr.dr.dr.X == 1) MessageBox.Show("X wins!");
                else MessageBox.Show("0 wins!");
                whw = true;

            }

            if((Cprim.dr.dr.X != 0 &&Cprim.dr.dr.X == Cprim.dr.dr.dr.dr.X && Cprim.dr.dr.X == Cultim.st.st.X)
                ||(Cprim.dr.dr.X!=0 && Cprim.dr.dr.X == Cultim.X && Cultim.X == Cultim.st.st.st.X))
            {
                if (Cprim.dr.dr.X == 1) MessageBox.Show("X wins!");
                else MessageBox.Show("0 wins!");
                whw = true;

            }

            if(Cultim.X != 0 && Cultim.X == Cultim.st.X && Cultim.X == Cultim.st.st.X)
            {
                if (Cultim.X == 1) MessageBox.Show("X wins!");
                else MessageBox.Show("0 wins!");
                whw = true;
            }
            if(whw)
                foreach (Control c in panel1.Controls)
                    if (c.GetType() == typeof(Button))
                    {
                        Button b = (Button)c;

                        b.Enabled = false;
                    }

            bool verifica_draw = true;
            if (whw) verifica_draw = false;
            CNod cc = Cprim;
            while(cc!=Cultim && verifica_draw)
            { if (cc.X == 0) verifica_draw = false; cc = cc.dr; }
            if (verifica_draw == true) MessageBox.Show("Draw!");

        }

        private void quantum2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("	Quantum tic-tac-toe adds the following rules to the ones from the classical game: \n\t1. { (X1, X1), (O2, O2), (X3, X3), (O4, O4)...} is the valid set of mooves. (This is the rule of superpositions. The marks are called 'spooky marks'.)\n\t2. Whenever a cyclic entanglement (a measurement of the system made by placing Xs and 0s) occurs by someone's moove, the next player gets to choose the collapse.\n\n\tNote: Because after a collapse both X and O can get a row, the player with the 'earlier' row (indicated by the index) gets the win.\n\tExample of a cyclic entanglement: if X0 reffers to (is in the same box with) 02, 02 reffers to 03, 03 reffers to X0.");


    
        }

        private void panelQ_Paint(object sender, PaintEventArgs e)
        {   /*
            Pen blackpen = new Pen(Color.Black, 1);

            Graphics g = e.Graphics;

            g.DrawLine(blackpen, 255, 30, 255, 900);
            g.DrawLine(blackpen, 265, 30, 265, 900);

            g.Dispose();*/
        }

        private void Mbutton_click(object sender, EventArgs e)
        {
            Button B = (Button)sender;
            B.Text = "X";
            Mturn = Mturn + 1;
            B.Enabled = false;

            //modificam lista inlantuita
            int possition_in_grid = (int)(B.Name[2]) - 48;

            CNod parcurgere;
            parcurgere = Cprim;
            int ind = 1;

            while (ind != possition_in_grid)
            { parcurgere = parcurgere.dr; ind++; }
            parcurgere.X =  1; //1 pt X

            CheckForMWinner();
        }

        unsafe private void CheckForMWinner()
        {
            bool whw = false;

            if 
                (((Cprim.X != 0 && Cprim.X == Cprim.dr.X && Cprim.X == Cprim.dr.dr.X)
             || (Cprim.X != 0 && Cprim.dr.dr.dr.X == Cprim.X && Cprim.X == Cprim.dr.dr.dr.dr.dr.dr.X)
             || (Cprim.X != 0 && Cprim.dr.dr.dr.dr.X == Cprim.X && Cprim.X == Cultim.X))
             || ((Cprim.dr.dr.dr.X != 0 && Cprim.dr.dr.dr.X == Cprim.dr.dr.dr.dr.X && Cprim.dr.dr.dr.X == Cprim.dr.dr.dr.dr.dr.X)
             || (Cprim.dr.dr.dr.dr.X != 0 && Cprim.dr.dr.dr.dr.X == Cprim.dr.X && Cprim.dr.X == Cultim.st.X))
             || ((Cprim.dr.dr.X != 0 && Cprim.dr.dr.X == Cprim.dr.dr.dr.dr.X && Cprim.dr.dr.X == Cultim.st.st.X)
             || (Cprim.dr.dr.X != 0 && Cprim.dr.dr.X == Cultim.X && Cultim.X == Cultim.st.st.st.X))
             || (Cultim.X != 0 && Cultim.X == Cultim.st.X && Cultim.X == Cultim.st.st.X))
            {
                if (Mturn%2==1) MessageBox.Show("Second player wins!");
                else MessageBox.Show("First player wins!");
                whw = true;

            }

            if (whw)
                foreach (Control c in panel11.Controls)
                    if (c.GetType() == typeof(Button))
                    {
                        Button b = (Button)c;

                        b.Enabled = false;
                    }
        }

        private void Qbutton_click(object sender, EventArgs e)
        {   //panourile sunt pannel2 ... pannel10

            //if (not valid move) do nothing. else:
            //mark the moove. repeat that for the scnd moove of the player.
            //if cyclic entanglement do what is needed. else continue setting mooves.

            Button B = (Button)sender;
            if (!qtc && !Click_for_colapse && B.ForeColor != Color.Red)
            {
                if (Qturn % 2 == 0) B.Text = "X" + Qturn;
                else B.Text = "0" + Qturn;
                qtc=!qtc;
                B.Enabled = false;
                qpc = what_Qpanel_is_on(B);
                nodQ1 = qpc;

            }
            else if(what_Qpanel_is_on(B)!=qpc && !Click_for_colapse && B.ForeColor != Color.Red)

            {
                nodQ2 = what_Qpanel_is_on(B);
                if (Qturn % 2 == 0) B.Text = "X" + Qturn;
                else B.Text = "0" + Qturn;
                Qturn = Qturn + 1;
                qtc = !qtc;
                B.Enabled = false;
                check_for_CE(); //si pune-le in matricea de adiacenta
            }

            else if(B.Font.Underline == true && Click_for_colapse && B.ForeColor != Color.Red)
            {
                B.ForeColor = Color.Red;
                Click_for_colapse = false;
                //MessageBox.Show("" + what_Qpanel_is_on(B));
                

                foreach (Control p in panelQ.Controls)
                    if (p is Panel)
                        foreach (Control b1 in p.Controls)
                            if (b1 is Button && b1.ForeColor != Color.Red && b1.Text == B.Text)
                                b1.Enabled = false;
                //acum trebuie dezactivate butoanele de pe acelasi panou cu B

                foreach (Control p in panelQ.Controls)
                    if (p is Panel)
                        foreach (Control b1 in p.Controls)
                            if (b1 is Button && b1 == B)
                                foreach (Control b2 in p.Controls)
                                    if (b2 is Button && b2 != B)
                                        b2.Enabled = false;


                                Collapse_all();
                Check_for_QWinner();
                
            }
            
        }

        int what_Qpanel_is_on(Button B)
        {
            //bool sw = true;

            int nrButton;
            if (B.Name.Length==7)
            {
                nrButton = (int)(B.Name[6]) - 48;
            }
            else nrButton = (int)(B.Name[6] - 48) * 10 + (int)(B.Name[7]) - 48;

            nrButton = (nrButton-1) / 9 + 1;
            //MessageBox.Show("" + nrButton);
            return nrButton;
        }

        void check_for_CE()
        {
           // bool cycle = false;
           
            if (MA[nodQ1, nodQ2] == 1)
            {
                here_the_cycle[nodQ1] = 1;
                here_the_cycle[nodQ2] = 1;
                do_cycle_stuff();
            }
            else
            {
                MA[nodQ1, nodQ2] = 1; MA[nodQ2, nodQ1] = 1;
                //cautam un posibil ciclu
                Search_for_cycle2(nodQ1);
            }

            
        }

        void Search_for_cycle()
        {
            int i, j, minimum_lenght_of_cycle = 11, index_of_mlof = 0;
            bool cycle = false;
            for (i = 1; i < 10; i++)
            {
                for (j = 1; j < 10; j++)
                    arbore[j] = 0;

                if (Parcurgere_in_adancime(i, 0))
                {
                    cycle = true;
                    int how_many = 0;
                    for (j = 1; j < 10; j++)
                        if (arbore[j] == 1) how_many++;
                    if (how_many < minimum_lenght_of_cycle)
                    {
                        minimum_lenght_of_cycle = how_many;
                        index_of_mlof = i;
                    }
                }
                for (j = 1; j < 10; j++)
                    arbore[j] = 0;
            }



            if (cycle)
            {
                Parcurgere_in_adancime(index_of_mlof, 0);
                for (i = 1; i < 10; i++)
                {
                    here_the_cycle[i] = arbore[i];
                   
                    for (j = 1; j < 10; j++)
                        MA[i, j] = 0;
                }
                do_cycle_stuff();
            }
        }

        void Search_for_cycle2(int nod)
        {
            bool cycle = false;

            

        
            
                min_lenght_of_cycle = 11;
                for (int j = 0; j < 10; j++)
                { stiva[j] = 0; here_the_cycle[j] = 0; }
                stiva[1] = nod;
                Parcurgere_in_adancime2(2,nod, 0);
            
            if (min_lenght_of_cycle != 11)
            {
                //do_cycle_stuff();
                for (int i = 1; i < 10; i++)
                    if (here_the_cycle[i] == 1)
                        for (int j = 1; j < 10; j++)
                        { MA[i, j] = 0; MA[j, i] = 0; }
                do_cycle_stuff();
            }
        }

        void Parcurgere_in_adancime2(int k,int nod, int precedent_nod)
        {
            for(int i=1;i<10;i++)
                if(i!=precedent_nod && i!=nod &&MA[i,nod]==1)
                {
                    bool sw = false;
                    stiva[k]=i;
                    for (int j = 1; j < k; j++)
                        if (stiva[j] == i) sw = true;
                    if (sw && k-1<min_lenght_of_cycle)
                    {
                        for(int j=1;j<k;j++)

                        here_the_cycle[stiva[j]] = 1;
                        min_lenght_of_cycle = k - 1;
                        //MessageBox.Show("" + stiva[3]);
                    }
                        else if(!sw) Parcurgere_in_adancime2(k + 1, i,nod);
                }
        }

        bool Parcurgere_in_adancime(int nod, int precedent_nod)
        {
            bool cycle_found=false;
            if (precedent_nod == 0) arbore[nod] = 1;
            for (int i = 1; i < 10; i++)
                if (i != nod && i != precedent_nod && MA[nod, i] == 1)
                {
                    if (arbore[i] == 1)
                    {
                        cycle_found = true;
                        //return true;
                    }
                    else arbore[i] = 1;
                }
             for (int i = 1; i < 10; i++)
                if (i != nod && i != precedent_nod && MA[nod, i] == 1)
                    {
                        cycle_found = cycle_found || Parcurgere_in_adancime(i, nod);
                    }

              return cycle_found;
        }

        void do_cycle_stuff()
        {
            //se modifica toate casutele scrise din ciclu conform articolului, se subliniaza
            int i;

            for (i = 1; i < 10; i++)
            {
                if (here_the_cycle[i] == 1)
                {
                    Panel PANEL = panel3;
                    if (i == 1) PANEL = panel2;
                    if (i == 2) PANEL = panel3;
                    if (i == 3) PANEL = panel4;
                    if (i == 4) PANEL = panel5;
                    if (i == 5) PANEL = panel6;
                    if (i == 6) PANEL = panel7;
                    if (i == 7) PANEL = panel8;
                    if (i == 8) PANEL = panel9;
                    if (i == 9) PANEL = panel10;

                    foreach (Control c in PANEL.Controls)
                        if (c.GetType() == typeof(Button))
                        {
                            Button b = (Button)c;
                            if (b.Text.Length > 1)
                            {
                                b.Enabled = true;
                                b.Font = new Font(b.Font.Name, 8, FontStyle.Underline);
                            }
                            else { b.Enabled = false;
                                //MessageBox.Show(""+i);
                            }
                        }
                }
                //lasam jucatorul urmator sa aleaga colapsul
                Click_for_colapse = true;
                //Precollapse();
                
              /*  foreach (Control p in panelQ.Controls)
                    if(p is Panel)
                    foreach(Control b in p.Controls)
                    { if (b is Button)
                            if(b.Font.Underline == false) b.Enabled = false; } */

            }
        

            for (i = 1; i < 10; i++)
            {
                here_the_cycle[i] = 0;
                arbore[i] = 0;
            }
            if(Click_for_colapse) Precollapse();
        }

        void Precollapse()
        {
            foreach (Control p1 in panelQ.Controls)
                if (p1 is Panel)
                    foreach (Control b1 in p1.Controls)
                        if (b1 is Button)
                            foreach (Control p2 in panelQ.Controls)
                                if (p2 is Panel)
                                    foreach (Control b2 in p2.Controls)
                                        if(b2 is Button)
                                        {
                                            if( b1.Text == b2.Text && b1.Text!="" &&
                                                b1.Enabled == true && b2.Enabled == false && b1.ForeColor != Color.Red)
                                            {
                                                //MessageBox.Show("" + b1.Enabled);
                                                b1.Font = new Font(b1.Font, FontStyle.Regular);
                                                b1.Enabled = false;
                                                b2.Enabled = true;
                                                b2.ForeColor = Color.Red;
                                                //b2.Enabled = false;
                                                foreach (Control b3 in p2.Controls)
                                                    if (b3 is Button && b3.Text != b2.Text) b3.Enabled = false;
                                                Precollapse();
                                            }
                                        }
        }

        void Collapse_all()
        {

            foreach (Control p in panelQ.Controls)
                if (p is Panel)
                    foreach (Control b1 in p.Controls)
                        if (b1 is Button && b1.Enabled == false)
                            foreach (Control p2 in panelQ.Controls)
                                if (p2 is Panel)
                                    foreach (Control b2 in p2.Controls)
                                        if (b2 is Button && b2.Text == b1.Text&& b2.Text !="" && b2.Enabled == true && b2.ForeColor != Color.Red)
                                        {
                                            b2.ForeColor = Color.Red;
                                            foreach (Control c in p2.Controls)
                                                if (c is Button && c != b2)
                                                    c.Enabled = false;
                                            Collapse_all();
                                        }
                            
                                   
            ;
        }

        void Check_for_QWinner()
        {
            ;
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The application is made by Ciprian Ursuleac (ciprian.ursuleac98@e-uvt.ro).");
        }


        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {

        }

        private void classic2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\tThose rules are so simple we are pretty shure you will learn them with ease from trying to play the actual game.\n\tGood luck and have fun!") ;
        }

        private void misère2ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\tThe rules are the same ones from the classical game, except that both players are playing with the same symbol and that the one who makes a row looses.");
        }
    }
}