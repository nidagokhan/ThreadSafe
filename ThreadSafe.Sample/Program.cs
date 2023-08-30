using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ThreadSafe.Sample
{
    /// <summary>
    /// Üç masalı bir lokantada yoğunluktan dolayı yer problemi yaşanmaktadır. Masalarda yer boşaldıkça sıradaki müşteri boşalan yere alınmakta ve bu işlem eş zamanlı olarak tüm masalar için gerçekleştirilmektedir.
    /// </summary>
    internal class Program
    {
        // static Queue siradakiler; //Queue olarak tanımlandığında çakışamlar oluyor ve hata fırlatıyor

        static ConcurrentQueue<string> siradakiler; //yukarıdaki hatadan dolayı concurrenrqueue şeklinde tanımlarız. bunun içinde trydequeue olduğundan çakışma yapmadan atama yapar
        static async Task Main(string[] args)
        {
            siradakiler = new ConcurrentQueue<string>();
            siradakiler.Enqueue("Nida");
            siradakiler.Enqueue("Servet");
            siradakiler.Enqueue("Melike");
            siradakiler.Enqueue("Betül");
            siradakiler.Enqueue("Zeynep");
            siradakiler.Enqueue("Müge");
            siradakiler.Enqueue("Göksel");
            siradakiler.Enqueue("Hızır");
            siradakiler.Enqueue("Doruk");

            await Kontrol();
            Console.ReadKey();
        }

        //Sıradaki kişilerin sayısını kontrol ederek (siradakiler.Count > 0), await Task.Delay(100); satırı ile her kişi arasında 100 milisaniye beklenir. Ardından siradakiler.TryDequeue(out string siradaki); satırı ile sıradan bir kişi çıkarılır ve bu kişi masanın adıyla birlikte ekrana yazdırılır.
        static async Task Masa(string masaAdi)
        {
            while (siradakiler.Count > 0)
            {
                await Task.Delay(100);
                siradakiler.TryDequeue(out string siradaki);
                Console.WriteLine($"{masaAdi}-{siradaki}");
            }
        }

        //Her masa için ayrı bir görev (task1, task2, vb.) oluşturur ve bu görevlerin tamamlanmasını await Task.WhenAll ile bekler. Bu sayede masaların işlemleri eşzamanlı olarak gerçekleştirilir.
        static async Task Kontrol()
        {
            var task1 = Masa("Masa 1");
            var task2 = Masa("Masa 2");
            var task3 = Masa("Masa 3");

            await Task.WhenAll(task1, task2, task3);
            Console.WriteLine("Masa sırası bitmiştir");

        }

        /*
        * Asenkron programlama, paralel işlemleri daha etkili bir şekilde yönetmek için kullanılır ve await anahtar kelimesi, uzun süren işlemlerin tamamlanmasını beklemek yerine kaynakları serbest bırakarak diğer işlemlere geçilmesine olanak tanır.
        * Her masa aynı anda kendi işini yapabilir ve kuyruğa erişimde çakışmaları engellemek için ConcurrentQueue kullanılır.Asenkron programlama, await ve async kullanarak uzun süren işlemlerin daha etkili bir şekilde yönetilmesini sağlar.
        */
    }
}
