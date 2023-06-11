using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EntitiyOrnek
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DbSinavOgrenciEntities db = new DbSinavOgrenciEntities();
        private void button1_Click(object sender, EventArgs e)
        {

            dataGridView1.DataSource = db.TBLOgrenci.ToList();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void btnDersListele_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=//* DATABASE NAME *//;Initial Catalog=DbSinavOgrenci;Integrated Security=True");
            SqlCommand komut = new SqlCommand("Select * From TBLDersler", conn);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void btnNotListele_Click(object sender, EventArgs e)
        {
            var query = from item in db.TBLNotlar
                        select new
                        {
                            item.NotID,
                            item.Ogr,
                            item.Ders,
                            item.Sinav1,
                            item.Sinav2,
                            item.Sinav3,
                            item.Ortalama,
                            item.Durum
                        };
            dataGridView1.DataSource = query.ToList();
            //dataGridView1.DataSource= db.TBLNotlar.ToList();
        }

        private void btnKayder_Click(object sender, EventArgs e)
        {
            TBLOgrenci t = new TBLOgrenci();
            t.Ad = txtOgrenciAd.Text;
            t.Soyad = txtSoyad.Text;
            string ogrAd = txtOgrenciAd.Text;
            string ogrSoyad = txtSoyad.Text;
            db.TBLOgrenci.Add(t);
            db.SaveChanges();
            MessageBox.Show(ogrAd + " " + ogrSoyad + " Öğrencisi Listeye Eklenmiştir.");

        }

        private void btnDersEkle_Click(object sender, EventArgs e)
        {
            TBLDersler dersekle = new TBLDersler();
            dersekle.DersAd = txtDersAD.Text;
            string dersadi = txtDersAD.Text;
            db.TBLDersler.Add(dersekle);
            db.SaveChanges();
            MessageBox.Show(dersadi + " Dersi başarı ile eklenmiştir");
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtOgrenciID.Text);
            var x = db.TBLOgrenci.Find(id);
            //string silinenOGr = x.Ad;
            Tuple<string, string> silinenOgr = new Tuple<string, string>(x.Ad, x.Soyad);
            db.TBLOgrenci.Remove(x);
            db.SaveChanges();
            MessageBox.Show(silinenOgr + " Öğrenci Sistemden Silindi");

        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtOgrenciID.Text);
            var x = db.TBLOgrenci.Find(id);
            x.Ad = txtOgrenciAd.Text;
            x.Soyad = txtSoyad.Text;
            x.Fotograf = txtFotoLink.Text;
            db.SaveChanges();
            MessageBox.Show("Öğrenci Bilgileri Başarıyla Güncellendi");
        }

        private void btnPrsdr_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.NOTLISTESI();
        }

        private void btnBul_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.TBLOgrenci.Where(x => x.Ad == txtOgrenciAd.Text | x.Soyad == txtSoyad.Text).ToList();
            int ogrenciID = Convert.ToInt32(txtOgrenciID.Text);
            string ogrenciAdi = txtOgrenciAd.Text;
            string ogrenciSoyadi = txtSoyad.Text;

            TBLOgrenci ogrenci = null;

            if(ogrenciID != 0)
            {
                ogrenci = db.TBLOgrenci.Find(ogrenciID);

            }
            else if (!string.IsNullOrEmpty(ogrenciAdi) && !string.IsNullOrEmpty(ogrenciSoyadi))
            {
                ogrenci = db.TBLOgrenci.FirstOrDefault(o => o.Ad == ogrenciAdi && o.Soyad == ogrenciSoyadi);
            }

            if (ogrenci != null)
            {
                if (!string.IsNullOrEmpty(ogrenci.Fotograf))
                {
                    pictureBox1.Image = Image.FromFile(ogrenci.Fotograf);
                    
                }
                else
                {
                    // Öğrencinin resmi yok, varsayılan bir resim göstermek isterseniz:
                    pictureBox1.Image = Image.FromFile("default resim yolu");
                    dataGridView1.DataSource = db.TBLOgrenci.Where(x => x.Ad == txtOgrenciAd.Text || x.Soyad == txtSoyad.Text).ToList();
                }
                dataGridView1.DataSource = new List<TBLOgrenci> { ogrenci };
            }
        }

        private void txtOgrenciAd_TextChanged(object sender, EventArgs e)
        {
            string aranan = txtOgrenciAd.Text;
            var degerler = from item in db.TBLOgrenci
                           where item.Ad.Contains(aranan) || item.Soyad.Contains(aranan)
                           select item;
            dataGridView1.DataSource=degerler.ToList(); ;
        }

        private void btnLinqEntitiy_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                List<TBLOgrenci> liste1 = db.TBLOgrenci.OrderBy(x => x.Ad).ToList();
                dataGridView1.DataSource=liste1;
            }
            if(radioButton2.Checked == true)
            {
                List<TBLOgrenci> liste2 = db.TBLOgrenci.OrderByDescending(x => x.Ad).ToList();
                dataGridView1.DataSource = liste2;
            }
            if(radioButton3.Checked == true)
            {
                List<TBLOgrenci> liste3 = db.TBLOgrenci.OrderBy(x => x.Ad).Take(5).ToList();
                dataGridView1.DataSource =liste3;
            }
            if(radioButton4.Checked == true)
            {
                int ogrenId =int.Parse(txtOgrenciID.Text);
                List<TBLOgrenci> liste4 = db.TBLOgrenci.Where(x => x.Id == ogrenId).ToList();
                dataGridView1.DataSource = liste4;
            }
            if(radioButton5.Checked == true)
            {
                List<TBLOgrenci> liste5 = db.TBLOgrenci.Where(x => x.Ad.StartsWith("a")).ToList();
                dataGridView1.DataSource =liste5;
            }
            if(radioButton6.Checked == true)
            {
                List<TBLOgrenci> liste6 =db.TBLOgrenci.Where(x=> x.Ad.EndsWith("a")).ToList();
                dataGridView1.DataSource =liste6;
            }
            if (radioButton7.Checked == true)
            {
                bool deger = db.TBLOgrenci.Any();
                MessageBox.Show(deger.ToString(), " Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (radioButton8.Checked == true)
            {
                int toplam = db.TBLOgrenci.Count();
                MessageBox.Show(toplam.ToString(), " Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if(radioButton9.Checked == true)
            {
                var toplam = db.TBLNotlar.Sum(x => x.Sinav1);
                MessageBox.Show("1. Sınavın Toplam Puanı: " + toplam.ToString());

            }
            if(radioButton10.Checked == true)
            {
                var ortalama =db.TBLNotlar.Average(x => x.Sinav1);
                MessageBox.Show("1. Sinav Toplam Ortalaması: " + ortalama);
            }
            if(radioButton11.Checked == true)
            {
                var ortalama = db.TBLNotlar.Average(x => x.Sinav1);
                List<TBLNotlar> liste7 = db.TBLNotlar.Where(x => x.Sinav1 > ortalama).ToList();
                dataGridView1.DataSource = liste7;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void btnNotGuncelle_Click(object sender, EventArgs e)
        {
            int dersID = Convert.ToInt32(txtDersID.Text);
            int ogrenciID = Convert.ToInt32(txtOgrenciID.Text);
            var x = db.TBLNotlar.Find(dersID);
            var y = db.TBLNotlar.Find(ogrenciID);
            x.Sinav1 = Convert.ToInt16(txtSinav1.Text);
            x.Sinav2 = Convert.ToInt16(txtSınav2.Text);
            x.Sinav3 = Convert.ToInt16(txtSınav3.Text);

            db.SaveChanges();
            MessageBox.Show("Eklendi");

        }

        private void btnHesapla_Click(object sender, EventArgs e)
        {
            int ogrenciID = Convert.ToInt32(txtOgrenciID.Text);
            int dersID = Convert.ToInt32(txtDersID.Text);

            var notlar = db.TBLNotlar.Where(x => x.Ogr == ogrenciID && x.Ders == dersID).ToList();
            double toplamPuan = 0;

            foreach (var not in notlar)
            {
                toplamPuan += Convert.ToDouble(not.Sinav1) + Convert.ToDouble(not.Sinav2) + Convert.ToDouble(not.Sinav3);
            }
            double ortalama = toplamPuan / (notlar.Count * 3);

            foreach(var not in notlar)
            {
                not.Ortalama = Convert.ToDecimal(ortalama);
            }
            db.SaveChanges();
            
        }
    }
}
