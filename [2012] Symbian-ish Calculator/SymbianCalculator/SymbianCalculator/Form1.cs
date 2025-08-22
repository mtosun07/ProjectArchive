using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SymbianCalculator
{
    public enum BasilanButon : byte
    { None, Toplama, Cikarma, Carpma, Bolme }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Dir = @"C:\CalculaTSN";
            if (!Directory.Exists(Dir))
                Directory.CreateDirectory(Dir);
            islemler = new List<string>();
            bos1 = true;
            bos2 = true;
            sonuc = 0;
            sonButon = BasilanButon.None;
            yukariSayac = 0;
            timer1.Start();
        }



        private void LabelAyarla(int labelSayac = 0)
        {
            string[] texts = new string[13];
            for (int i = 0; i < texts.Length; i++)
                texts[i] = String.Empty;
            if (islemler.Count > 13)
            {
                int sayac = 0;
                for (int i = islemler.Count - 13 - labelSayac; sayac < 13; i++)
                {
                    texts[sayac] = islemler[i];
                    sayac++;
                }
            }
            else
                for (int i = 0; i < islemler.Count; i++)
                    texts[i] = islemler[i];
            lblSonuc1.Text = texts[0];
            lblSonuc2.Text = texts[1];
            lblSonuc3.Text = texts[2];
            lblSonuc4.Text = texts[3];
            lblSonuc5.Text = texts[4];
            lblSonuc6.Text = texts[5];
            lblSonuc7.Text = texts[6];
            lblSonuc8.Text = texts[7];
            lblSonuc9.Text = texts[8];
            lblSonuc10.Text = texts[9];
            lblSonuc11.Text = texts[10];
            lblSonuc12.Text = texts[11];
            lblSonuc13.Text = texts[12];
        }
        private void YeniIslemeGec()
        {
            txtbSayi.Text = String.Empty;
            bos1 = true;
            sonButon = BasilanButon.None;
        }
        private void Islem()
        {
            switch (sonButon)
            {
                case BasilanButon.None:
                    sonuc = sayi;
                    islemler.Add(sayi.ToString());
                    islemler.TrimExcess();
                    break;
                case BasilanButon.Toplama:
                    islemler[islemler.Count - 1] += " + " + sayi.ToString();
                    sonuc += sayi;
                    break;
                case BasilanButon.Cikarma:
                    islemler[islemler.Count - 1] += " − " + sayi.ToString();
                    sonuc -= sayi;
                    break;
                case BasilanButon.Carpma:
                    islemler[islemler.Count - 1] = "(" + islemler[islemler.Count - 1] + ") × " + sayi.ToString();
                    sonuc *= sayi;
                    break;
                case BasilanButon.Bolme:
                    islemler[islemler.Count - 1] = "(" + islemler[islemler.Count - 1] + ") ÷ " + sayi.ToString();
                    sonuc /= sayi;
                    break;
            }
            LabelAyarla();

            bos1 = true;
            txtbSayi.Text = String.Empty;
        }



        string Dir;
        
        BasilanButon sonButon;
        
        decimal sayi;
        bool bos1;

        decimal sonuc;

        decimal sonSonuc;
        bool bos2;

        List<string> islemler;

        int yukariSayac;


        
        private void Form1_Load(object sender, EventArgs e)
        {
            string path = Dir + "\\Variables1.tsn";
            if (!File.Exists(path))
                return;

            while (true)
            {
                using (BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read)))
                {
                    try
                    {
                        sonSonuc = br.ReadDecimal();
                        bos2 = false;
                        br.Close();
                    }
                    catch (EndOfStreamException ex)
                    {
                        MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        br.Close();
                        bos2 = true;
                    }
                    catch (Exception ex)
                    {
                        DialogResult dr = MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                        br.Close();
                        if (dr == System.Windows.Forms.DialogResult.Cancel)
                            Application.Exit();
                        else
                            continue;
                    }
                }
                break;
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bos2)
                return;
            string path = Dir + "\\Variables1.tsn";
            if (File.Exists(path))
                File.Delete(path);

            while (true)
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open(path, FileMode.Append, FileAccess.Write)))
                {
                    try
                    {
                        bw.Write(sonSonuc);
                        bw.Flush();
                        bw.Close();
                    }
                    catch (Exception ex)
                    {
                        DialogResult dr = MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                        bw.Close();
                        if (dr == System.Windows.Forms.DialogResult.Cancel)
                            break;
                        else
                            continue;
                    }
                }
                break;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            YeniIslemeGec();
        }

        private void sonSonuçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bos2)
                return;
            txtbSayi.Text = sonSonuc.ToString();
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            islemler = new List<string>();
            LabelAyarla();
            btnYukari.Enabled = false;
            btnAsagi.Enabled = false;
            YeniIslemeGec();
            timer1.Start();
        }

        private void txtbSayi_TextChanged(object sender, EventArgs e)
        {
            while (!String.IsNullOrEmpty(txtbSayi.Text))
            {
                string str = "";
                for (int i = 0; i < txtbSayi.Text.Length; i++)
                    if (txtbSayi.Text[i] == '.')
                        str += ',';
                    else
                        str += txtbSayi.Text[i];
                txtbSayi.Text = str;
                if (txtbSayi.Text.Length > 0 && txtbSayi.Text[txtbSayi.Text.Length - 1] == ',')
                    break;

                str = "";
                for (int i = 0; i < txtbSayi.Text.Length; i++)
                    if (i > 0 && txtbSayi.Text[i] == (char)8)
                        str = str.Substring(0, i);
                    else
                        str += txtbSayi.Text[i];
                txtbSayi.Text = str;
                
                byte islem = 0;
                if(txtbSayi.Text.Length > 1)
                    for (int i = 0; i < txtbSayi.Text.Length; i++)
                    {
                        switch (txtbSayi.Text[i])
                        {
                            case '+':
                                islem = 1;
                                break;
                            case '-':
                                islem = 2;
                                break;
                            case '*':
                                islem = 3;
                                break;
                            case '/':
                                islem = 4;
                                break;
                            case '=':
                            case (char)13:
                                islem = 5;
                                break;
                            default:
                                break;
                        }
                        if (islem > 0)
                        {
                            txtbSayi.Text = txtbSayi.Text.Remove(i, 1);
                            break;
                        }
                    }


                if (Decimal.TryParse(txtbSayi.Text, out sayi))
                {
                    bos1 = false;
                    txtbSayi.Text = (sayi == 0) ? String.Empty : sayi.ToString();

                    switch (islem)
                    {
                        case 1:
                            btnArti_Click(sender, e);
                            break;
                        case 2:
                            btnEksi_Click(sender, e);
                            break;
                        case 3:
                            btnCarpi_Click(sender, e);
                            break;
                        case 4:
                            btnBolu_Click(sender, e);
                            break;
                        case 5:
                            btnEsittir_Click(sender, e);
                            break;
                        default:
                            break;
                    }

                    break;
                }
                else
                {
                    bos1 = true;
                    if (txtbSayi.Text.Length > 1)
                    {
                        str = "";
                        for (int i = 0; i < txtbSayi.Text.Length; i++)
                        {
                            decimal d;
                            if (decimal.TryParse(txtbSayi.Text[i].ToString(), out d))
                                str += txtbSayi.Text[i];
                        }
                        txtbSayi.Text = str;
                        continue;
                    }
                    else
                        txtbSayi.Text = String.Empty;
                }
            }

            if (!bos1 && sonButon != BasilanButon.None)
                btnYuzde.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (islemler.Count > 13)
            {
                btnAsagi.Enabled = false;
                btnYukari.Enabled = true;
                timer1.Stop();
            }
        }
        private void btnYukari_Click(object sender, EventArgs e)
        {
            btnAsagi.Enabled = true;

            yukariSayac++;
            LabelAyarla(yukariSayac);

            if (islemler.Count - 13 - yukariSayac == 0)
                btnYukari.Enabled = false;
        }
        private void btnAsagi_Click(object sender, EventArgs e)
        {
            btnYukari.Enabled = true;

            yukariSayac--;
            LabelAyarla(yukariSayac);

            if (yukariSayac == 0)
                btnAsagi.Enabled = false;
        }

        private void sonSonuçToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            sonSonuçToolStripMenuItem.ForeColor = Color.IndianRed;
        }
        private void hafızaToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            hafızaToolStripMenuItem.ForeColor = Color.IndianRed;
        }
        private void kaydetToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            kaydetToolStripMenuItem.ForeColor = Color.IndianRed;
        }
        private void hafızadanAlToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            hafızadanAlToolStripMenuItem.ForeColor = Color.IndianRed;
        }
        private void temizleToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            temizleToolStripMenuItem.ForeColor = Color.IndianRed;
        }
        private void xToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            xToolStripMenuItem.ForeColor = Color.Red;
        }

        private void xToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            xToolStripMenuItem.ForeColor = Color.DarkOrange;
        }
        private void sonSonuçToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            sonSonuçToolStripMenuItem.ForeColor = Color.White;
        }
        private void hafızaToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            hafızaToolStripMenuItem.ForeColor = Color.White;
        }
        private void temizleToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            temizleToolStripMenuItem.ForeColor = Color.White;
        }
        private void hafızadanAlToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            hafızadanAlToolStripMenuItem.ForeColor = Color.White;
        }
        private void kaydetToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            kaydetToolStripMenuItem.ForeColor = Color.White;
        }
		
        private void btnArti_Click(object sender, EventArgs e)
        {
            if (sayi == 0 || bos1)
                return;
            Islem();
            sonButon = BasilanButon.Toplama;
        }
        private void btnEksi_Click(object sender, EventArgs e)
        {
            if (sayi == 0 || bos1)
                return;
            Islem();
            sonButon = BasilanButon.Cikarma;
        }
        private void btnCarpi_Click(object sender, EventArgs e)
        {
            if (sayi == 0 || bos1)
                return;
            Islem();
            sonButon = BasilanButon.Carpma;
        }
        private void btnBolu_Click(object sender, EventArgs e)
        {
            if (sayi == 0 || bos1)
                return;
            Islem();
            sonButon = BasilanButon.Bolme;
        }
        private void btnKareKok_Click(object sender, EventArgs e)
        {
            if (sayi == 0 || bos1)
                return;

            double dSayi;
            checked
            {
                dSayi = (double)sayi;
            }

            txtbSayi.Text = Math.Sqrt(dSayi).ToString();
        }
        private void btnArtiEksi_Click(object sender, EventArgs e)
        {
            if (sayi == 0 || bos1)
                return;

            sayi *= -1;
            txtbSayi.Text = sayi.ToString();
        }
        private void btnEsittir_Click(object sender, EventArgs e)
        {
            switch (sonButon)
            {
                case BasilanButon.None:
                    sonuc = sayi;
                    islemler.Add(sonuc.ToString());
                    islemler.TrimExcess();
                    islemler[islemler.Count - 1] = sonuc.ToString() + " = " + sonuc.ToString();
                    break;
                case BasilanButon.Toplama:
                    sonuc += sayi;
                    islemler[islemler.Count - 1] += " + " + sayi.ToString() + " = " + sonuc.ToString();
                    break;
                case BasilanButon.Cikarma:
                    sonuc -= sayi;
                    islemler[islemler.Count - 1] += " − " + sayi.ToString() + " = " + sonuc.ToString();
                    break;
                case BasilanButon.Carpma:
                    sonuc *= sayi;
                    islemler[islemler.Count - 1] = "(" + islemler[islemler.Count - 1] + ") × " + sayi.ToString() + " = " + sonuc.ToString();
                    break;
                case BasilanButon.Bolme:
                    sonuc /= sayi;
                    islemler[islemler.Count - 1] = "(" + islemler[islemler.Count - 1] + ") ÷ " + sayi.ToString() + " = " + sonuc.ToString();
                    break;
            }

            LabelAyarla();
            txtbSayi.Text = sonuc.ToString();
            sonSonuc = sonuc;
            bos2 = false;
            sonButon = BasilanButon.None;
            btnYuzde.Enabled = false;
        }
        private void btnYuzde_Click(object sender, EventArgs e)
        {
            btnYuzde.Enabled = false;

            decimal yuzdesi = sonuc * sayi / 100;
            string str = String.Empty;

            switch (sonButon)
            {
                case BasilanButon.Toplama:
                    str = " +% " + sayi.ToString();
                    sonuc += yuzdesi;
                    break;
                case BasilanButon.Cikarma:
                    str = " −% " + sayi.ToString();
                    sonuc -= yuzdesi;
                    break;
                case BasilanButon.Carpma:
                    str = " ×% " + sayi.ToString();
                    sonuc *= yuzdesi;
                    break;
                case BasilanButon.Bolme:
                    str = " ÷% " + sayi.ToString();
                    sonuc /= yuzdesi;
                    break;
            }

            islemler[islemler.Count - 1] = "(" + islemler[islemler.Count - 1] + ")" + str;
            LabelAyarla();

            txtbSayi.Text = String.Empty;
            bos1 = true;
        }

        private void geçerliDeğerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Dir + @"\Variables2.tsn";
            if (File.Exists(path))
                File.Delete(path);

            while (true)
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open(path, FileMode.Append, FileAccess.Write)))
                {
                    try
                    {
                        bw.Write(sayi);
                        bw.Flush();
                        bw.Close();
                    }
                    catch (Exception ex)
                    {
                        DialogResult dr = MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                        bw.Close();
                        if (dr == System.Windows.Forms.DialogResult.Cancel)
                            break;
                        else
                            continue;
                    }
                }
                break;
            }
        }
        private void geçerliDeğerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string path = Dir + "\\Variables2.tsn";
            if (!File.Exists(path))
            {
                MessageBox.Show("Hafıza boş.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            while (true)
            {
                using (BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read)))
                {
                    try
                    {
                        sayi = br.ReadDecimal();
                        bos1 = false;
                        br.Close();
                        txtbSayi.Text = sayi.ToString();
                    }
                    catch (EndOfStreamException ex)
                    {
                        MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        br.Close();
                        File.Delete(path);
                        bos2 = true;
                    }
                    catch (Exception ex)
                    {
                        DialogResult dr = MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                        br.Close();
                        if (dr == System.Windows.Forms.DialogResult.Cancel)
                            Application.Exit();
                        else
                            continue;
                    }
                }
                break;
            }
        }
        private void geçerliDeğerToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string path = Dir + "\\Variables2.tsn";
            if (!File.Exists(path))
            {
                MessageBox.Show("Hafıza boş.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            File.Delete(path);
        }
        private void işlemListesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (islemler.Count == 0)
            {
                MessageBox.Show("İşlem listesi boş.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            DialogResult dr = saveFileDialog1.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
                return;
            string path = saveFileDialog1.FileName;

            while (true)
            {
                using (StreamWriter sw = new StreamWriter(File.Open(path, FileMode.Create, FileAccess.Write)))
                {
                    try
                    {
                        for (int i = 0; i < islemler.Count; i++)
                            sw.WriteLine(islemler[i]);
                        sw.Flush();
                        sw.Close();
                    }
                    catch (Exception ex)
                    {
                        DialogResult d = MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                        sw.Close();
                        File.Delete(path);
                        if (d != System.Windows.Forms.DialogResult.Cancel)
                            continue;
                    }
                }
                break;
            }
        }
        private void işlemListesiToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
                return;
            string path = openFileDialog1.FileName;

            if (!File.Exists(path))
            {
                MessageBox.Show("Dosya bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<string> okunanlar = new List<string>();
            while (true)
            {
                using (StreamReader sr = new StreamReader(File.Open(path, FileMode.Open, FileAccess.Read)))
                {
                    try
                    {
                        string okunan = String.Empty;
                        while (true)
                        {
                            okunan = sr.ReadLine();
                            if (String.IsNullOrEmpty(okunan))
                                break;
                            okunanlar.Add(okunan);
                        }
                        sr.Close();
                    }
                    catch (EndOfStreamException ex)
                    {
                        DialogResult d = MessageBox.Show("Dosya boş ya da yanlış biçimde!\n" + ex.Message, "Hata", 
                            MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                        sr.Close();
                    }
                    catch (Exception ex)
                    {
                        DialogResult d = MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                        sr.Close();
                        if (d != System.Windows.Forms.DialogResult.Cancel)
                            continue;
                    }
                }
                break;
            }

            okunanlar.TrimExcess();
            if (okunanlar.Count == 0)
            {
                MessageBox.Show("Dosya boş ya da yanlış biçimde!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (islemler.Count > 0)
            {
                DialogResult d2 = MessageBox.Show("Seçilen işlem listesi, şimdiki işlem listesinin üzerine mi eklensin?", "İşlem Listeleri",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (d2 == System.Windows.Forms.DialogResult.No)
                    islemler = new List<string>();
            }
            for (int i = 0; i < okunanlar.Count; i++)
                islemler.Add(okunanlar[i]);
            islemler.TrimExcess();
            LabelAyarla();
        }
    }
}