using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace OgrenciDersYonetim
{
    // Base Class
    public abstract class Kisi
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }

        public abstract void BilgiGoster();
    }

    // Interface
    public interface ILogin
    {
        bool Login(string kullaniciAdi, string sifre);
    }

    // Ogrenci Sınıfı
    public class Ogrenci : Kisi, ILogin
    {
        public int Numara { get; set; }
        public override void BilgiGoster()
        {
            Console.WriteLine($"Öğrenci: {Ad} {Soyad}, Numara: {Numara}");
        }

        public bool Login(string kullaniciAdi, string sifre)
        {
            return kullaniciAdi == "ogrenci" && sifre == "1234";
        }
    }

    // OgretimGorevlisi Sınıfı
    public class OgretimGorevlisi : Kisi, ILogin
    {
        public string Unvan { get; set; }

        public override void BilgiGoster()
        {
            Console.WriteLine($"Öğretim Görevlisi: {Unvan} {Ad} {Soyad}");
        }

        public bool Login(string kullaniciAdi, string sifre)
        {
            return kullaniciAdi == "ogretim" && sifre == "1234";
        }
    }

    // Ders Sınıfı
    public class Ders
    {
        public string DersAdi { get; set; }
        public int Kredi { get; set; }
        public OgretimGorevlisi OgretimGorevlisi { get; set; }
        public List<Ogrenci> KayitliOgrenciler { get; set; } = new List<Ogrenci>();

        public void DersBilgisiGoster()
        {
            Console.WriteLine($"Ders: {DersAdi}, Kredi: {Kredi}, Öğretim Görevlisi: {OgretimGorevlisi.Unvan} {OgretimGorevlisi.Ad} {OgretimGorevlisi.Soyad}");
            Console.WriteLine("Kayıtlı Öğrenciler:");
            foreach (var ogrenci in KayitliOgrenciler)
            {
                Console.WriteLine($" - {ogrenci.Ad} {ogrenci.Soyad}");
            }
        }
    }

    public static class DataHelper
    {
        public static void SaveToText<T>(string filePath, List<T> data)
        {
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var item in data)
                {
                    writer.WriteLine(item);
                }
            }
        }

        public static List<string> LoadFromText(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<string>();

            return new List<string>(File.ReadAllLines(filePath));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var ogrencilerFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ogrenciler.txt");
            var ogretimGorevlileriFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ogretimGorevlileri.txt");


            var ogrenciler = DataHelper.LoadFromText(ogrencilerFile);
            var ogretimGorevlileri = DataHelper.LoadFromText(ogretimGorevlileriFile);

            // Menü
            while (true)
            {
                Console.WriteLine("1. Öğrenci Ekle");
                Console.WriteLine("2. Öğretim Görevlisi Ekle");
                Console.WriteLine("3. Listele");
                Console.WriteLine("4. Çıkış");
                Console.Write("Seçiminiz: ");

                var secim = Console.ReadLine();

                switch (secim)
                {
                    case "1":
                        Console.Write("Ad: ");
                        var ogrenciAd = Console.ReadLine();
                        Console.Write("Soyad: ");
                        var ogrenciSoyad = Console.ReadLine();
                        Console.Write("Numara: ");
                        var ogrenciNumara = Console.ReadLine();
                        ogrenciler.Add($"Ad: {ogrenciAd}, Soyad: {ogrenciSoyad}, Numara: {ogrenciNumara}");
                        DataHelper.SaveToText(ogrencilerFile, ogrenciler);
                        break;
                    case "2":
                        Console.Write("Ad: ");
                        var ogretimAd = Console.ReadLine();
                        Console.Write("Soyad: ");
                        var ogretimSoyad = Console.ReadLine();
                        Console.Write("Unvan: ");
                        var ogretimUnvan = Console.ReadLine();
                        ogretimGorevlileri.Add($"Ad: {ogretimAd}, Soyad: {ogretimSoyad}, Unvan: {ogretimUnvan}");
                        DataHelper.SaveToText(ogretimGorevlileriFile, ogretimGorevlileri);
                        break;
                    case "3":
                        Console.WriteLine("Öğrenciler:");
                        foreach (var ogrenci in ogrenciler)
                        {
                            Console.WriteLine(ogrenci);
                        }

                        Console.WriteLine("\nÖğretim Görevlileri:");
                        foreach (var ogretimGorevlisi in ogretimGorevlileri)
                        {
                            Console.WriteLine(ogretimGorevlisi);
                        }
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim.");
                        break;
                }
            }
        }
    }
}
